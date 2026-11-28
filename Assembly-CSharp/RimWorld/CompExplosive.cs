using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001878 RID: 6264
	public class CompExplosive : ThingComp
	{
		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x06009CC6 RID: 40134 RVA: 0x0038677E File Offset: 0x0038497E
		public CompProperties_Explosive Props
		{
			get
			{
				return (CompProperties_Explosive)this.props;
			}
		}

		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x06009CC7 RID: 40135 RVA: 0x0038678B File Offset: 0x0038498B
		protected int StartWickThreshold
		{
			get
			{
				return Mathf.RoundToInt(this.Props.startWickHitPointsPercent * (float)this.parent.MaxHitPoints);
			}
		}

		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x06009CC8 RID: 40136 RVA: 0x003867AC File Offset: 0x003849AC
		protected virtual bool CanEverExplodeFromDamage
		{
			get
			{
				if (this.Props.chanceNeverExplodeFromDamage < 1E-05f)
				{
					return true;
				}
				Rand.PushState();
				Rand.Seed = this.parent.thingIDNumber.GetHashCode();
				bool result = Rand.Value > this.Props.chanceNeverExplodeFromDamage;
				Rand.PopState();
				return result;
			}
		}

		// Token: 0x06009CC9 RID: 40137 RVA: 0x003867FE File Offset: 0x003849FE
		public void AddThingsIgnoredByExplosion(List<Thing> things)
		{
			if (this.thingsIgnoredByExplosion == null)
			{
				this.thingsIgnoredByExplosion = new List<Thing>();
			}
			this.thingsIgnoredByExplosion.AddRange(things);
		}

		// Token: 0x06009CCA RID: 40138 RVA: 0x00386820 File Offset: 0x00384A20
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Collections.Look<Thing>(ref this.thingsIgnoredByExplosion, "thingsIgnoredByExplosion", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.wickStarted, "wickStarted", false, false);
			Scribe_Values.Look<int>(ref this.wickTicksLeft, "wickTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.destroyedThroughDetonation, "destroyedThroughDetonation", false, false);
			Scribe_Values.Look<int>(ref this.countdownTicksLeft, "countdownTicksLeft", 0, false);
			Scribe_Values.Look<float?>(ref this.customExplosiveRadius, "explosiveRadius", null, false);
		}

		// Token: 0x06009CCB RID: 40139 RVA: 0x003868BC File Offset: 0x00384ABC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.Props.countdownTicks != null)
			{
				this.countdownTicksLeft = this.Props.countdownTicks.Value.RandomInRange;
			}
			this.UpdateOverlays();
		}

		// Token: 0x06009CCC RID: 40140 RVA: 0x00386900 File Offset: 0x00384B00
		public override void CompTick()
		{
			if (this.countdownTicksLeft > 0)
			{
				this.countdownTicksLeft--;
				if (this.countdownTicksLeft == 0)
				{
					this.StartWick(null);
					this.countdownTicksLeft = -1;
				}
			}
			if (this.wickStarted)
			{
				if (this.wickSoundSustainer == null)
				{
					this.StartWickSustainer();
				}
				else
				{
					this.wickSoundSustainer.Maintain();
				}
				if (this.Props.wickMessages != null)
				{
					foreach (WickMessage wickMessage in this.Props.wickMessages)
					{
						if (wickMessage.ticksLeft == this.wickTicksLeft && wickMessage.wickMessagekey != null)
						{
							Messages.Message(wickMessage.wickMessagekey.Translate(this.parent.GetCustomLabelNoCount(false), this.wickTicksLeft.ToStringSecondsFromTicks()), this.parent, wickMessage.messageType ?? MessageTypeDefOf.NeutralEvent, false);
						}
					}
				}
				this.wickTicksLeft--;
				if (this.wickTicksLeft <= 0)
				{
					this.Detonate(this.parent.MapHeld, false);
				}
			}
		}

		// Token: 0x06009CCD RID: 40141 RVA: 0x00386A44 File Offset: 0x00384C44
		private void StartWickSustainer()
		{
			SoundDefOf.MetalHitImportant.PlayOneShot(new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false));
			SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.PerTick);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x06009CCE RID: 40142 RVA: 0x00386A9F File Offset: 0x00384C9F
		private void EndWickSustainer()
		{
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
		}

		// Token: 0x06009CCF RID: 40143 RVA: 0x00386ABC File Offset: 0x00384CBC
		private void UpdateOverlays()
		{
			if (!this.parent.Spawned || !this.Props.drawWick)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayBurningWick);
			if (this.wickStarted)
			{
				this.overlayBurningWick = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.BurningWick));
			}
		}

		// Token: 0x06009CD0 RID: 40144 RVA: 0x00386B34 File Offset: 0x00384D34
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (!this.destroyedThroughDetonation && ((mode == DestroyMode.KillFinalize && this.Props.explodeOnKilled) || this.Props.explodeOnDestroyed))
			{
				this.Detonate(previousMap, true);
			}
		}

		// Token: 0x06009CD1 RID: 40145 RVA: 0x00386B64 File Offset: 0x00384D64
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.EndWickSustainer();
			if (this.parent.BeingTransportedOnGravship)
			{
				return;
			}
			this.StopWick();
		}

		// Token: 0x06009CD2 RID: 40146 RVA: 0x00386B88 File Offset: 0x00384D88
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			if (this.CanEverExplodeFromDamage)
			{
				if (dinfo.Def.ExternalViolenceFor(this.parent) && dinfo.Amount >= (float)this.parent.HitPoints && this.CanExplodeFromDamageType(dinfo.Def))
				{
					if (this.parent.MapHeld != null)
					{
						this.instigator = dinfo.Instigator;
						this.Detonate(this.parent.MapHeld, false);
						if (this.parent.Destroyed)
						{
							absorbed = true;
							return;
						}
					}
				}
				else if (!this.wickStarted && this.Props.startWickOnDamageTaken != null && this.Props.startWickOnDamageTaken.Contains(dinfo.Def))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		// Token: 0x06009CD3 RID: 40147 RVA: 0x00386C50 File Offset: 0x00384E50
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!this.CanEverExplodeFromDamage)
			{
				return;
			}
			if (!this.CanExplodeFromDamageType(dinfo.Def))
			{
				return;
			}
			if (!this.parent.Destroyed)
			{
				if (this.wickStarted && dinfo.Def == DamageDefOf.Stun)
				{
					this.StopWick();
					return;
				}
				if (!this.wickStarted && this.parent.HitPoints <= this.StartWickThreshold && (dinfo.Def.ExternalViolenceFor(this.parent) || (!this.Props.startWickOnInternalDamageTaken.NullOrEmpty<DamageDef>() && this.Props.startWickOnInternalDamageTaken.Contains(dinfo.Def))))
				{
					this.StartWick(dinfo.Instigator);
				}
			}
		}

		// Token: 0x06009CD4 RID: 40148 RVA: 0x00386D0C File Offset: 0x00384F0C
		public void StartWick(Thing instigator = null)
		{
			if (this.wickStarted)
			{
				return;
			}
			if (this.ExplosiveRadius() <= 0f)
			{
				return;
			}
			this.instigator = instigator;
			this.wickStarted = true;
			this.wickTicksLeft = this.Props.wickTicks.RandomInRange;
			this.StartWickSustainer();
			GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this.parent, this.Props.explosiveDamageType, null, instigator);
			this.UpdateOverlays();
		}

		// Token: 0x06009CD5 RID: 40149 RVA: 0x00386D78 File Offset: 0x00384F78
		public void StopWick()
		{
			this.wickStarted = false;
			this.instigator = null;
			this.UpdateOverlays();
			this.EndWickSustainer();
		}

		// Token: 0x06009CD6 RID: 40150 RVA: 0x00386D94 File Offset: 0x00384F94
		public float ExplosiveRadius()
		{
			CompProperties_Explosive props = this.Props;
			float num = this.customExplosiveRadius ?? this.Props.explosiveRadius;
			if (this.parent.stackCount > 1 && props.explosiveExpandPerStackcount > 0f)
			{
				num += Mathf.Sqrt((float)(this.parent.stackCount - 1) * props.explosiveExpandPerStackcount);
			}
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				num += Mathf.Sqrt(this.parent.GetComp<CompRefuelable>().Fuel * props.explosiveExpandPerFuel);
			}
			return num;
		}

		// Token: 0x06009CD7 RID: 40151 RVA: 0x00386E40 File Offset: 0x00385040
		protected void Detonate(Map map, bool ignoreUnspawned = false)
		{
			if (!ignoreUnspawned && !this.parent.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			CompProperties_Explosive props = this.Props;
			float num = this.ExplosiveRadius();
			if (num <= 0f)
			{
				return;
			}
			Thing parent;
			if (this.instigator != null && (!this.instigator.HostileTo(this.parent.Faction) || this.parent.Faction == Faction.OfPlayer))
			{
				parent = this.instigator;
			}
			else
			{
				parent = this.parent;
			}
			if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
			{
				this.parent.GetComp<CompRefuelable>().ConsumeFuel(this.parent.GetComp<CompRefuelable>().Fuel);
			}
			if (props.destroyThingOnExplosionSize <= num && !this.parent.Destroyed)
			{
				this.destroyedThroughDetonation = true;
				this.parent.Kill(null, null);
			}
			this.EndWickSustainer();
			this.wickStarted = false;
			this.UpdateOverlays();
			if (map == null)
			{
				Log.Warning("Tried to detonate CompExplosive in a null map.");
				return;
			}
			if (props.explosionEffect != null)
			{
				Effecter effecter = props.explosionEffect.Spawn();
				effecter.Trigger(new TargetInfo(this.parent.PositionHeld, map, false), new TargetInfo(this.parent.PositionHeld, map, false), -1);
				effecter.Cleanup();
			}
			IntVec3 positionHeld = this.parent.PositionHeld;
			float radius = num;
			DamageDef explosiveDamageType = props.explosiveDamageType;
			Thing thing = parent;
			int damageAmountBase = props.damageAmountBase;
			float armorPenetrationBase = props.armorPenetrationBase;
			SoundDef explosionSound = props.explosionSound;
			ThingDef weapon = null;
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef postExplosionSpawnThingDef = props.postExplosionSpawnThingDef;
			float postExplosionSpawnChance = props.postExplosionSpawnChance;
			int postExplosionSpawnThingCount = props.postExplosionSpawnThingCount;
			GasType? postExplosionGasType = this.Props.postExplosionGasType;
			float? postExplosionGasRadiusOverride = this.Props.postExplosionGasRadiusOverride;
			int postExplosionGasAmount = this.Props.postExplosionGasAmount;
			bool applyDamageToExplosionCellsNeighbors = props.applyDamageToExplosionCellsNeighbors;
			ThingDef preExplosionSpawnThingDef = props.preExplosionSpawnThingDef;
			float preExplosionSpawnChance = props.preExplosionSpawnChance;
			int preExplosionSpawnThingCount = props.preExplosionSpawnThingCount;
			float chanceToStartFire = props.chanceToStartFire;
			bool damageFalloff = props.damageFalloff;
			List<Thing> ignoredThings = this.thingsIgnoredByExplosion;
			bool doVisualEffects = props.doVisualEffects;
			bool doSoundEffects = props.doSoundEffects;
			float propagationSpeed = props.propagationSpeed;
			ThingDef preExplosionSpawnSingleThingDef = props.preExplosionSpawnSingleThingDef;
			ThingDef postExplosionSpawnSingleThingDef = props.postExplosionSpawnSingleThingDef;
			GenExplosion.DoExplosion(positionHeld, map, radius, explosiveDamageType, thing, damageAmountBase, armorPenetrationBase, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, postExplosionGasType, postExplosionGasRadiusOverride, postExplosionGasAmount, applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, preExplosionSpawnChance, preExplosionSpawnThingCount, chanceToStartFire, damageFalloff, null, ignoredThings, null, doVisualEffects, propagationSpeed, 0f, doSoundEffects, null, 1f, null, null, postExplosionSpawnSingleThingDef, preExplosionSpawnSingleThingDef);
		}

		// Token: 0x06009CD8 RID: 40152 RVA: 0x0038706A File Offset: 0x0038526A
		private bool CanExplodeFromDamageType(DamageDef damage)
		{
			return this.Props.requiredDamageTypeToExplode == null || this.Props.requiredDamageTypeToExplode == damage;
		}

		// Token: 0x06009CD9 RID: 40153 RVA: 0x0038708C File Offset: 0x0038528C
		public override string CompInspectStringExtra()
		{
			string text = "";
			if (this.countdownTicksLeft != -1)
			{
				text += "DetonationCountdown".Translate(this.countdownTicksLeft.TicksToDays().ToString("0.0"));
			}
			if (this.Props.extraInspectStringKey != null)
			{
				text += ((text != "") ? "\n" : "") + this.Props.extraInspectStringKey.Translate();
			}
			return text;
		}

		// Token: 0x06009CDA RID: 40154 RVA: 0x00387123 File Offset: 0x00385323
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompExplosive.<CompGetGizmosExtra>d__32 <CompGetGizmosExtra>d__ = new CompExplosive.<CompGetGizmosExtra>d__32(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005D03 RID: 23811
		public bool wickStarted;

		// Token: 0x04005D04 RID: 23812
		public int wickTicksLeft;

		// Token: 0x04005D05 RID: 23813
		private Thing instigator;

		// Token: 0x04005D06 RID: 23814
		private int countdownTicksLeft = -1;

		// Token: 0x04005D07 RID: 23815
		public bool destroyedThroughDetonation;

		// Token: 0x04005D08 RID: 23816
		private List<Thing> thingsIgnoredByExplosion;

		// Token: 0x04005D09 RID: 23817
		public float? customExplosiveRadius;

		// Token: 0x04005D0A RID: 23818
		protected Sustainer wickSoundSustainer;

		// Token: 0x04005D0B RID: 23819
		private OverlayHandle? overlayBurningWick;
	}
}
