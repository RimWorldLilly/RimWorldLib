using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A72 RID: 6770
	[StaticConstructorOnStartup]
	public class Building_TurretGun : Building_Turret
	{
		// Token: 0x17001DFA RID: 7674
		// (get) Token: 0x0600AA32 RID: 43570 RVA: 0x003C4704 File Offset: 0x003C2904
		public bool Active
		{
			get
			{
				return (this.powerComp == null || this.powerComp.PowerOn) && (this.dormantComp == null || this.dormantComp.Awake) && (this.initiatableComp == null || this.initiatableComp.Initiated) && (this.interactableComp == null || this.burstActivated) && (this.powerCellComp == null || !this.powerCellComp.depleted) && (this.hackableComp == null || !this.hackableComp.IsHacked);
			}
		}

		// Token: 0x17001DFB RID: 7675
		// (get) Token: 0x0600AA33 RID: 43571 RVA: 0x003C478F File Offset: 0x003C298F
		public CompEquippable GunCompEq
		{
			get
			{
				return this.gun.TryGetComp<CompEquippable>();
			}
		}

		// Token: 0x17001DFC RID: 7676
		// (get) Token: 0x0600AA34 RID: 43572 RVA: 0x003C479C File Offset: 0x003C299C
		public override LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTargetInt;
			}
		}

		// Token: 0x17001DFD RID: 7677
		// (get) Token: 0x0600AA35 RID: 43573 RVA: 0x003C47A4 File Offset: 0x003C29A4
		private bool WarmingUp
		{
			get
			{
				return this.burstWarmupTicksLeft > 0;
			}
		}

		// Token: 0x17001DFE RID: 7678
		// (get) Token: 0x0600AA36 RID: 43574 RVA: 0x003C47AF File Offset: 0x003C29AF
		public override Verb AttackVerb
		{
			get
			{
				return this.GunCompEq.PrimaryVerb;
			}
		}

		// Token: 0x17001DFF RID: 7679
		// (get) Token: 0x0600AA37 RID: 43575 RVA: 0x003C47BC File Offset: 0x003C29BC
		public bool IsMannable
		{
			get
			{
				return this.mannableComp != null;
			}
		}

		// Token: 0x17001E00 RID: 7680
		// (get) Token: 0x0600AA38 RID: 43576 RVA: 0x003C47C7 File Offset: 0x003C29C7
		private bool PlayerControlled
		{
			get
			{
				return (base.Faction == Faction.OfPlayer || this.MannedByColonist) && !this.MannedByNonColonist && !this.IsActivable;
			}
		}

		// Token: 0x17001E01 RID: 7681
		// (get) Token: 0x0600AA39 RID: 43577 RVA: 0x003C47F1 File Offset: 0x003C29F1
		protected virtual bool CanSetForcedTarget
		{
			get
			{
				return this.mannableComp != null && this.PlayerControlled;
			}
		}

		// Token: 0x17001E02 RID: 7682
		// (get) Token: 0x0600AA3A RID: 43578 RVA: 0x003C4803 File Offset: 0x003C2A03
		private bool CanToggleHoldFire
		{
			get
			{
				return this.PlayerControlled;
			}
		}

		// Token: 0x17001E03 RID: 7683
		// (get) Token: 0x0600AA3B RID: 43579 RVA: 0x003C480B File Offset: 0x003C2A0B
		private bool IsMortar
		{
			get
			{
				return this.def.building.IsMortar;
			}
		}

		// Token: 0x17001E04 RID: 7684
		// (get) Token: 0x0600AA3C RID: 43580 RVA: 0x003C481D File Offset: 0x003C2A1D
		private bool IsMortarOrProjectileFliesOverhead
		{
			get
			{
				return this.AttackVerb.ProjectileFliesOverhead() || this.IsMortar;
			}
		}

		// Token: 0x17001E05 RID: 7685
		// (get) Token: 0x0600AA3D RID: 43581 RVA: 0x003C4834 File Offset: 0x003C2A34
		private bool IsActivable
		{
			get
			{
				return this.interactableComp != null;
			}
		}

		// Token: 0x17001E06 RID: 7686
		// (get) Token: 0x0600AA3E RID: 43582 RVA: 0x003C483F File Offset: 0x003C2A3F
		protected virtual bool HideForceTargetGizmo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001E07 RID: 7687
		// (get) Token: 0x0600AA3F RID: 43583 RVA: 0x003C4842 File Offset: 0x003C2A42
		public TurretTop Top
		{
			get
			{
				return this.top;
			}
		}

		// Token: 0x17001E08 RID: 7688
		// (get) Token: 0x0600AA40 RID: 43584 RVA: 0x003C484C File Offset: 0x003C2A4C
		private bool CanExtractShell
		{
			get
			{
				if (!this.PlayerControlled)
				{
					return false;
				}
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				return compChangeableProjectile != null && compChangeableProjectile.Loaded;
			}
		}

		// Token: 0x17001E09 RID: 7689
		// (get) Token: 0x0600AA41 RID: 43585 RVA: 0x003C487A File Offset: 0x003C2A7A
		private bool MannedByColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x17001E0A RID: 7690
		// (get) Token: 0x0600AA42 RID: 43586 RVA: 0x003C48AA File Offset: 0x003C2AAA
		private bool MannedByNonColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x0600AA43 RID: 43587 RVA: 0x003C48DD File Offset: 0x003C2ADD
		public Building_TurretGun()
		{
			this.top = new TurretTop(this);
		}

		// Token: 0x0600AA44 RID: 43588 RVA: 0x003C48FC File Offset: 0x003C2AFC
		public override void PostMake()
		{
			base.PostMake();
			this.burstCooldownTicksLeft = this.def.building.turretInitialCooldownTime.SecondsToTicks();
			this.MakeGun();
		}

		// Token: 0x0600AA45 RID: 43589 RVA: 0x003C4928 File Offset: 0x003C2B28
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.dormantComp = base.GetComp<CompCanBeDormant>();
			this.initiatableComp = base.GetComp<CompInitiatable>();
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.mannableComp = base.GetComp<CompMannable>();
			this.interactableComp = base.GetComp<CompInteractable>();
			this.refuelableComp = base.GetComp<CompRefuelable>();
			this.powerCellComp = base.GetComp<CompMechPowerCell>();
			this.hackableComp = base.GetComp<CompHackable>();
			if (!respawningAfterLoad)
			{
				this.top.SetRotationFromOrientation();
			}
		}

		// Token: 0x0600AA46 RID: 43590 RVA: 0x003C49AB File Offset: 0x003C2BAB
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.ResetCurrentTarget();
			Effecter effecter = this.progressBarEffecter;
			if (effecter == null)
			{
				return;
			}
			effecter.Cleanup();
		}

		// Token: 0x0600AA47 RID: 43591 RVA: 0x003C49CC File Offset: 0x003C2BCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.burstCooldownTicksLeft, "burstCooldownTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.burstWarmupTicksLeft, "burstWarmupTicksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.currentTargetInt, "currentTarget");
			Scribe_Values.Look<bool>(ref this.holdFire, "holdFire", false, false);
			Scribe_Values.Look<bool>(ref this.burstActivated, "burstActivated", false, false);
			Scribe_Deep.Look<Thing>(ref this.gun, "gun", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.gun == null)
				{
					Log.Error("Turret had null gun after loading. Recreating.");
					this.MakeGun();
					return;
				}
				this.UpdateGunVerbs();
			}
		}

		// Token: 0x0600AA48 RID: 43592 RVA: 0x003C4A7C File Offset: 0x003C2C7C
		public override AcceptanceReport ClaimableBy(Faction by)
		{
			AcceptanceReport result = base.ClaimableBy(by);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.mannableComp != null && this.mannableComp.ManningPawn != null)
			{
				return false;
			}
			if (this.Active && this.mannableComp == null)
			{
				return false;
			}
			if (((this.dormantComp != null && !this.dormantComp.Awake) || (this.initiatableComp != null && !this.initiatableComp.Initiated)) && (this.powerComp == null || this.powerComp.PowerOn))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600AA49 RID: 43593 RVA: 0x003C4B1C File Offset: 0x003C2D1C
		public override void OrderAttack(LocalTargetInfo targ)
		{
			if (!targ.IsValid)
			{
				if (this.forcedTarget.IsValid)
				{
					this.ResetForcedTarget();
				}
				return;
			}
			if ((targ.Cell - base.Position).LengthHorizontal < this.AttackVerb.verbProps.EffectiveMinRange(targ, this))
			{
				Messages.Message("MessageTargetBelowMinimumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if ((targ.Cell - base.Position).LengthHorizontal > this.AttackVerb.EffectiveRange)
			{
				Messages.Message("MessageTargetBeyondMaximumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.forcedTarget != targ)
			{
				this.forcedTarget = targ;
				if (this.burstCooldownTicksLeft <= 0)
				{
					this.TryStartShootSomething(false);
				}
			}
			if (this.holdFire)
			{
				Messages.Message("MessageTurretWontFireBecauseHoldFire".Translate(this.def.label), this, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x0600AA4A RID: 43594 RVA: 0x003C4C3C File Offset: 0x003C2E3C
		protected override void Tick()
		{
			base.Tick();
			if (this.CanExtractShell && this.MannedByColonist)
			{
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				if (!compChangeableProjectile.allowedShellsSettings.AllowedToAccept(compChangeableProjectile.LoadedShell))
				{
					this.ExtractShell();
				}
			}
			if (this.forcedTarget.IsValid && !this.CanSetForcedTarget)
			{
				this.ResetForcedTarget();
			}
			if (!this.CanToggleHoldFire)
			{
				this.holdFire = false;
			}
			if (this.forcedTarget.ThingDestroyed)
			{
				this.ResetForcedTarget();
			}
			if (this.Active && (this.mannableComp == null || this.mannableComp.MannedNow) && !base.IsStunned && base.Spawned)
			{
				this.GunCompEq.verbTracker.VerbsTick();
				if (this.AttackVerb.state != VerbState.Bursting)
				{
					this.burstActivated = false;
					if (this.WarmingUp)
					{
						this.burstWarmupTicksLeft--;
						if (this.burstWarmupTicksLeft <= 0)
						{
							this.BeginBurst();
						}
					}
					else
					{
						if (this.burstCooldownTicksLeft > 0)
						{
							this.burstCooldownTicksLeft--;
							if (this.IsMortar)
							{
								if (this.progressBarEffecter == null)
								{
									this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
								}
								this.progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
								MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
								mote.progress = 1f - (float)Math.Max(this.burstCooldownTicksLeft, 0) / (float)this.BurstCooldownTime().SecondsToTicks();
								mote.offsetZ = -0.8f;
							}
						}
						if (this.burstCooldownTicksLeft <= 0 && this.IsHashIntervalTick(15))
						{
							this.TryStartShootSomething(true);
						}
					}
					this.top.TurretTopTick();
					return;
				}
			}
			else
			{
				this.ResetCurrentTarget();
			}
		}

		// Token: 0x0600AA4B RID: 43595 RVA: 0x003C4E16 File Offset: 0x003C3016
		public void TryActivateBurst()
		{
			this.burstActivated = true;
			this.TryStartShootSomething(true);
		}

		// Token: 0x0600AA4C RID: 43596 RVA: 0x003C4E28 File Offset: 0x003C3028
		public void TryStartShootSomething(bool canBeginBurstImmediately)
		{
			if (this.progressBarEffecter != null)
			{
				this.progressBarEffecter.Cleanup();
				this.progressBarEffecter = null;
			}
			if (!base.Spawned || (this.holdFire && this.CanToggleHoldFire) || (this.AttackVerb.ProjectileFliesOverhead() && base.Map.roofGrid.Roofed(base.Position)) || !this.AttackVerb.Available())
			{
				this.ResetCurrentTarget();
				return;
			}
			bool isValid = this.currentTargetInt.IsValid;
			if (this.forcedTarget.IsValid)
			{
				this.currentTargetInt = this.forcedTarget;
			}
			else
			{
				this.currentTargetInt = this.TryFindNewTarget();
			}
			if (!isValid && this.currentTargetInt.IsValid && this.def.building.playTargetAcquiredSound)
			{
				SoundDefOf.TurretAcquireTarget.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			if (!this.currentTargetInt.IsValid)
			{
				this.ResetCurrentTarget();
				return;
			}
			float randomInRange = this.def.building.turretBurstWarmupTime.RandomInRange;
			if (randomInRange > 0f)
			{
				this.burstWarmupTicksLeft = randomInRange.SecondsToTicks();
				return;
			}
			if (canBeginBurstImmediately)
			{
				this.BeginBurst();
				return;
			}
			this.burstWarmupTicksLeft = 1;
		}

		// Token: 0x0600AA4D RID: 43597 RVA: 0x003C4F64 File Offset: 0x003C3164
		public virtual LocalTargetInfo TryFindNewTarget()
		{
			IAttackTargetSearcher attackTargetSearcher = this.TargSearcher();
			Faction faction = attackTargetSearcher.Thing.Faction;
			float range = this.AttackVerb.EffectiveRange;
			Building t;
			if (Rand.Value < 0.5f && this.AttackVerb.ProjectileFliesOverhead() && faction.HostileTo(Faction.OfPlayer) && base.Map.listerBuildings.allBuildingsColonist.Where(delegate(Building x)
			{
				float num = this.AttackVerb.verbProps.EffectiveMinRange(x, this);
				float num2 = (float)x.Position.DistanceToSquared(this.Position);
				return num2 > num * num && num2 < range * range;
			}).TryRandomElement(out t))
			{
				return t;
			}
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			if (!this.AttackVerb.ProjectileFliesOverhead())
			{
				targetScanFlags |= TargetScanFlags.NeedLOSToAll;
				targetScanFlags |= TargetScanFlags.LOSBlockableByGas;
			}
			if (this.AttackVerb.IsIncendiary_Ranged())
			{
				targetScanFlags |= TargetScanFlags.NeedNonBurning;
			}
			if (this.IsMortar)
			{
				targetScanFlags |= TargetScanFlags.NeedNotUnderThickRoof;
			}
			return (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(attackTargetSearcher, targetScanFlags, new Predicate<Thing>(this.IsValidTarget), 0f, 9999f);
		}

		// Token: 0x0600AA4E RID: 43598 RVA: 0x003C5062 File Offset: 0x003C3262
		private IAttackTargetSearcher TargSearcher()
		{
			if (this.mannableComp != null && this.mannableComp.MannedNow)
			{
				return this.mannableComp.ManningPawn;
			}
			return this;
		}

		// Token: 0x0600AA4F RID: 43599 RVA: 0x003C5088 File Offset: 0x003C3288
		private bool IsValidTarget(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (base.Faction == Faction.OfPlayer && pawn.IsPrisoner)
				{
					return false;
				}
				if (this.AttackVerb.ProjectileFliesOverhead())
				{
					RoofDef roofDef = base.Map.roofGrid.RoofAt(t.Position);
					if (roofDef != null && roofDef.isThickRoof)
					{
						return false;
					}
				}
				if (this.mannableComp == null)
				{
					return !GenAI.MachinesLike(base.Faction, pawn);
				}
				if (pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AA50 RID: 43600 RVA: 0x003C511C File Offset: 0x003C331C
		protected virtual void BeginBurst()
		{
			this.AttackVerb.TryStartCastOn(this.CurrentTarget, false, true, false, false);
			base.OnAttackedTarget(this.CurrentTarget);
		}

		// Token: 0x0600AA51 RID: 43601 RVA: 0x003C5140 File Offset: 0x003C3340
		protected virtual void BurstComplete()
		{
			this.burstCooldownTicksLeft = this.BurstCooldownTime().SecondsToTicks();
		}

		// Token: 0x0600AA52 RID: 43602 RVA: 0x003C5153 File Offset: 0x003C3353
		protected virtual float BurstCooldownTime()
		{
			if (this.def.building.turretBurstCooldownTime >= 0f)
			{
				return this.def.building.turretBurstCooldownTime;
			}
			return this.AttackVerb.verbProps.defaultCooldownTime;
		}

		// Token: 0x0600AA53 RID: 43603 RVA: 0x003C5190 File Offset: 0x003C3390
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.AttackVerb.verbProps.minRange > 0f)
			{
				stringBuilder.AppendLine("MinimumRange".Translate() + ": " + this.AttackVerb.verbProps.minRange.ToString("F0"));
			}
			if (base.Spawned && this.IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
			{
				stringBuilder.AppendLine("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
			}
			else if (base.Spawned && this.burstCooldownTicksLeft > 0 && this.BurstCooldownTime() > 5f)
			{
				stringBuilder.AppendLine("CanFireIn".Translate() + ": " + this.burstCooldownTicksLeft.ToStringSecondsFromTicks());
			}
			CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
			if (compChangeableProjectile != null)
			{
				if (compChangeableProjectile.Loaded)
				{
					stringBuilder.AppendLine("ShellLoaded".Translate(compChangeableProjectile.LoadedShell.LabelCap, compChangeableProjectile.LoadedShell));
				}
				else
				{
					stringBuilder.AppendLine("ShellNotLoaded".Translate());
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600AA54 RID: 43604 RVA: 0x003C5328 File Offset: 0x003C3528
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			Vector3 zero = Vector3.zero;
			float recoilAngleOffset = 0f;
			if (this.IsMortar)
			{
				EquipmentUtility.Recoil(this.def.building.turretGunDef, (Verb_LaunchProjectile)this.AttackVerb, out zero, out recoilAngleOffset, this.top.CurRotation);
			}
			this.top.DrawTurret(drawLoc, zero, recoilAngleOffset);
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x0600AA55 RID: 43605 RVA: 0x003C5390 File Offset: 0x003C3590
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			float effectiveRange = this.AttackVerb.EffectiveRange;
			float num = this.AttackVerb.verbProps.EffectiveMinRange(true);
			if (num < effectiveRange)
			{
				if (effectiveRange < 90f)
				{
					GenDraw.DrawRadiusRing(base.Position, effectiveRange);
				}
				if (num < 90f && num > 0.1f)
				{
					GenDraw.DrawRadiusRing(base.Position, num);
				}
			}
			if (this.WarmingUp)
			{
				int degreesWide = (int)((float)this.burstWarmupTicksLeft * 0.5f);
				GenDraw.DrawAimPie(this, this.CurrentTarget, degreesWide, (float)this.def.size.x * 0.5f);
			}
			if (this.forcedTarget.IsValid && (!this.forcedTarget.HasThing || this.forcedTarget.Thing.Spawned))
			{
				Vector3 vector;
				if (this.forcedTarget.HasThing)
				{
					vector = this.forcedTarget.Thing.TrueCenter();
				}
				else
				{
					vector = this.forcedTarget.Cell.ToVector3Shifted();
				}
				Vector3 a = this.TrueCenter();
				vector.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				a.y = vector.y;
				GenDraw.DrawLineBetween(a, vector, Building_TurretGun.ForcedTargetLineMat, 0.2f);
			}
		}

		// Token: 0x0600AA56 RID: 43606 RVA: 0x003C54C9 File Offset: 0x003C36C9
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_TurretGun.<GetGizmos>d__71 <GetGizmos>d__ = new Building_TurretGun.<GetGizmos>d__71(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AA57 RID: 43607 RVA: 0x003C54DC File Offset: 0x003C36DC
		private void ExtractShell()
		{
			GenPlace.TryPlaceThing(this.gun.TryGetComp<CompChangeableProjectile>().RemoveShell(), base.Position, base.Map, ThingPlaceMode.Near, null, null, null, 1);
		}

		// Token: 0x0600AA58 RID: 43608 RVA: 0x003C5518 File Offset: 0x003C3718
		private void ResetForcedTarget()
		{
			this.forcedTarget = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
			if (this.burstCooldownTicksLeft <= 0)
			{
				this.TryStartShootSomething(false);
			}
		}

		// Token: 0x0600AA59 RID: 43609 RVA: 0x003C553C File Offset: 0x003C373C
		private void ResetCurrentTarget()
		{
			this.currentTargetInt = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
		}

		// Token: 0x0600AA5A RID: 43610 RVA: 0x003C5550 File Offset: 0x003C3750
		public void MakeGun()
		{
			this.gun = ThingMaker.MakeThing(this.def.building.turretGunDef, null);
			this.UpdateGunVerbs();
		}

		// Token: 0x0600AA5B RID: 43611 RVA: 0x003C5574 File Offset: 0x003C3774
		private void UpdateGunVerbs()
		{
			List<Verb> allVerbs = this.gun.TryGetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				verb.caster = this;
				verb.castCompleteCallback = new Action(this.BurstComplete);
			}
		}

		// Token: 0x0400637E RID: 25470
		protected int burstCooldownTicksLeft;

		// Token: 0x0400637F RID: 25471
		protected int burstWarmupTicksLeft;

		// Token: 0x04006380 RID: 25472
		protected LocalTargetInfo currentTargetInt = LocalTargetInfo.Invalid;

		// Token: 0x04006381 RID: 25473
		private bool holdFire;

		// Token: 0x04006382 RID: 25474
		private bool burstActivated;

		// Token: 0x04006383 RID: 25475
		public Thing gun;

		// Token: 0x04006384 RID: 25476
		protected TurretTop top;

		// Token: 0x04006385 RID: 25477
		protected CompPowerTrader powerComp;

		// Token: 0x04006386 RID: 25478
		protected CompCanBeDormant dormantComp;

		// Token: 0x04006387 RID: 25479
		protected CompInitiatable initiatableComp;

		// Token: 0x04006388 RID: 25480
		protected CompMannable mannableComp;

		// Token: 0x04006389 RID: 25481
		protected CompInteractable interactableComp;

		// Token: 0x0400638A RID: 25482
		public CompRefuelable refuelableComp;

		// Token: 0x0400638B RID: 25483
		protected Effecter progressBarEffecter;

		// Token: 0x0400638C RID: 25484
		protected CompMechPowerCell powerCellComp;

		// Token: 0x0400638D RID: 25485
		protected CompHackable hackableComp;

		// Token: 0x0400638E RID: 25486
		private const int TryStartShootSomethingIntervalTicks = 15;

		// Token: 0x0400638F RID: 25487
		public static Material ForcedTargetLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));
	}
}
