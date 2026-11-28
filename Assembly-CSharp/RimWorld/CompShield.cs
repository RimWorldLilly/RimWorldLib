using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001970 RID: 6512
	[StaticConstructorOnStartup]
	public class CompShield : ThingComp
	{
		// Token: 0x17001C4F RID: 7247
		// (get) Token: 0x0600A363 RID: 41827 RVA: 0x003A5401 File Offset: 0x003A3601
		public CompProperties_Shield Props
		{
			get
			{
				return (CompProperties_Shield)this.props;
			}
		}

		// Token: 0x17001C50 RID: 7248
		// (get) Token: 0x0600A364 RID: 41828 RVA: 0x003A540E File Offset: 0x003A360E
		private float EnergyMax
		{
			get
			{
				return this.parent.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true, -1);
			}
		}

		// Token: 0x17001C51 RID: 7249
		// (get) Token: 0x0600A365 RID: 41829 RVA: 0x003A5422 File Offset: 0x003A3622
		private float EnergyGainPerTick
		{
			get
			{
				return this.parent.GetStatValue(StatDefOf.EnergyShieldRechargeRate, true, -1) / 60f;
			}
		}

		// Token: 0x17001C52 RID: 7250
		// (get) Token: 0x0600A366 RID: 41830 RVA: 0x003A543C File Offset: 0x003A363C
		public float Energy
		{
			get
			{
				return this.energy;
			}
		}

		// Token: 0x17001C53 RID: 7251
		// (get) Token: 0x0600A367 RID: 41831 RVA: 0x003A5444 File Offset: 0x003A3644
		public ShieldState ShieldState
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && (pawn.IsCharging() || pawn.IsSelfShutdown()))
				{
					return ShieldState.Disabled;
				}
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				if (comp != null && !comp.Awake)
				{
					return ShieldState.Disabled;
				}
				if (this.ticksToReset <= 0)
				{
					return ShieldState.Active;
				}
				return ShieldState.Resetting;
			}
		}

		// Token: 0x17001C54 RID: 7252
		// (get) Token: 0x0600A368 RID: 41832 RVA: 0x003A5498 File Offset: 0x003A3698
		protected bool ShouldDisplay
		{
			get
			{
				Pawn pawnOwner = this.PawnOwner;
				return pawnOwner.Spawned && !pawnOwner.Dead && !pawnOwner.Downed && (pawnOwner.InAggroMentalState || pawnOwner.Drafted || (pawnOwner.Faction.HostileTo(Faction.OfPlayer) && !pawnOwner.IsPrisoner) || Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks || (ModsConfig.BiotechActive && pawnOwner.IsColonyMech && Find.Selector.SingleSelectedThing == pawnOwner));
			}
		}

		// Token: 0x17001C55 RID: 7253
		// (get) Token: 0x0600A369 RID: 41833 RVA: 0x003A5530 File Offset: 0x003A3730
		protected Pawn PawnOwner
		{
			get
			{
				Apparel apparel = this.parent as Apparel;
				if (apparel != null)
				{
					return apparel.Wearer;
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
				return null;
			}
		}

		// Token: 0x17001C56 RID: 7254
		// (get) Token: 0x0600A36A RID: 41834 RVA: 0x003A5565 File Offset: 0x003A3765
		public bool IsApparel
		{
			get
			{
				return this.parent is Apparel;
			}
		}

		// Token: 0x17001C57 RID: 7255
		// (get) Token: 0x0600A36B RID: 41835 RVA: 0x003A5575 File Offset: 0x003A3775
		private bool IsBuiltIn
		{
			get
			{
				return !this.IsApparel;
			}
		}

		// Token: 0x0600A36C RID: 41836 RVA: 0x003A5580 File Offset: 0x003A3780
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.energy, "energy", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksToReset, "ticksToReset", -1, false);
			Scribe_Values.Look<int>(ref this.lastKeepDisplayTick, "lastKeepDisplayTick", 0, false);
		}

		// Token: 0x0600A36D RID: 41837 RVA: 0x003A55CD File Offset: 0x003A37CD
		public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			CompShield.<CompGetWornGizmosExtra>d__29 <CompGetWornGizmosExtra>d__ = new CompShield.<CompGetWornGizmosExtra>d__29(-2);
			<CompGetWornGizmosExtra>d__.<>4__this = this;
			return <CompGetWornGizmosExtra>d__;
		}

		// Token: 0x0600A36E RID: 41838 RVA: 0x003A55DD File Offset: 0x003A37DD
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompShield.<CompGetGizmosExtra>d__30 <CompGetGizmosExtra>d__ = new CompShield.<CompGetGizmosExtra>d__30(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A36F RID: 41839 RVA: 0x003A55ED File Offset: 0x003A37ED
		private IEnumerable<Gizmo> GetGizmos()
		{
			CompShield.<GetGizmos>d__31 <GetGizmos>d__ = new CompShield.<GetGizmos>d__31(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A370 RID: 41840 RVA: 0x003A55FD File Offset: 0x003A37FD
		public override float CompGetSpecialApparelScoreOffset()
		{
			return this.EnergyMax * this.ApparelScorePerEnergyMax;
		}

		// Token: 0x0600A371 RID: 41841 RVA: 0x003A560C File Offset: 0x003A380C
		public override void CompTick()
		{
			base.CompTick();
			if (this.PawnOwner == null)
			{
				this.energy = 0f;
				return;
			}
			if (this.ShieldState == ShieldState.Resetting)
			{
				this.ticksToReset--;
				if (this.ticksToReset <= 0)
				{
					this.Reset();
					return;
				}
			}
			else if (this.ShieldState == ShieldState.Active)
			{
				this.energy += this.EnergyGainPerTick;
				if (this.energy > this.EnergyMax)
				{
					this.energy = this.EnergyMax;
				}
			}
		}

		// Token: 0x0600A372 RID: 41842 RVA: 0x003A5690 File Offset: 0x003A3890
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
			if (this.ShieldState != ShieldState.Active || this.PawnOwner == null)
			{
				return;
			}
			if (dinfo.Def == DamageDefOf.EMP)
			{
				this.energy = 0f;
				this.Break();
				return;
			}
			if (dinfo.Def.ignoreShields)
			{
				return;
			}
			if (dinfo.Def.isRanged || dinfo.Def.isExplosive)
			{
				this.energy -= dinfo.Amount * this.Props.energyLossPerDamage;
				if (this.energy < 0f)
				{
					this.Break();
				}
				else
				{
					this.AbsorbedDamage(dinfo);
				}
				absorbed = true;
				return;
			}
		}

		// Token: 0x0600A373 RID: 41843 RVA: 0x003A573C File Offset: 0x003A393C
		public void KeepDisplaying()
		{
			this.lastKeepDisplayTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600A374 RID: 41844 RVA: 0x003A5750 File Offset: 0x003A3950
		private void AbsorbedDamage(DamageInfo dinfo)
		{
			SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(this.PawnOwner.Position, this.PawnOwner.Map, false));
			this.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
			Vector3 loc = this.PawnOwner.TrueCenter() + this.impactAngleVect.RotatedBy(180f) * 0.5f;
			float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
			FleckMaker.Static(loc, this.PawnOwner.Map, FleckDefOf.ExplosionFlash, num);
			int num2 = (int)num;
			for (int i = 0; i < num2; i++)
			{
				FleckMaker.ThrowDustPuff(loc, this.PawnOwner.Map, Rand.Range(0.8f, 1.2f));
			}
			this.lastAbsorbDamageTick = Find.TickManager.TicksGame;
			this.KeepDisplaying();
		}

		// Token: 0x0600A375 RID: 41845 RVA: 0x003A5840 File Offset: 0x003A3A40
		private void Break()
		{
			if (this.parent.Spawned)
			{
				float scale = Mathf.Lerp(this.Props.minDrawSize, this.Props.maxDrawSize, this.energy);
				EffecterDefOf.Shield_Break.SpawnAttached(this.parent, this.parent.MapHeld, scale);
				FleckMaker.Static(this.PawnOwner.TrueCenter(), this.PawnOwner.Map, FleckDefOf.ExplosionFlash, 12f);
				for (int i = 0; i < 6; i++)
				{
					FleckMaker.ThrowDustPuff(this.PawnOwner.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), this.PawnOwner.Map, Rand.Range(0.8f, 1.2f));
				}
			}
			this.energy = 0f;
			this.ticksToReset = this.Props.startingTicksToReset;
		}

		// Token: 0x0600A376 RID: 41846 RVA: 0x003A5940 File Offset: 0x003A3B40
		private void Reset()
		{
			if (this.PawnOwner.Spawned)
			{
				SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(this.PawnOwner.Position, this.PawnOwner.Map, false));
				FleckMaker.ThrowLightningGlow(this.PawnOwner.TrueCenter(), this.PawnOwner.Map, 3f);
			}
			this.ticksToReset = -1;
			this.energy = this.Props.energyOnReset;
		}

		// Token: 0x0600A377 RID: 41847 RVA: 0x003A59BD File Offset: 0x003A3BBD
		public override void CompDrawWornExtras()
		{
			base.CompDrawWornExtras();
			if (this.IsApparel)
			{
				this.Draw();
			}
		}

		// Token: 0x0600A378 RID: 41848 RVA: 0x003A59D3 File Offset: 0x003A3BD3
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.IsBuiltIn)
			{
				this.Draw();
			}
		}

		// Token: 0x0600A379 RID: 41849 RVA: 0x003A59EC File Offset: 0x003A3BEC
		private void Draw()
		{
			if (this.ShieldState == ShieldState.Active && this.ShouldDisplay)
			{
				float num = Mathf.Lerp(this.Props.minDrawSize, this.Props.maxDrawSize, this.energy);
				Vector3 vector = this.PawnOwner.Drawer.DrawPos;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				int num2 = Find.TickManager.TicksGame - this.lastAbsorbDamageTick;
				if (num2 < 8)
				{
					float num3 = (float)(8 - num2) / 8f * 0.05f;
					vector += this.impactAngleVect * num3;
					num -= num3;
				}
				float angle = (float)Rand.Range(0, 360);
				Vector3 s = new Vector3(num, 1f, num);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, CompShield.BubbleMat, 0);
			}
		}

		// Token: 0x0600A37A RID: 41850 RVA: 0x003A5ADB File Offset: 0x003A3CDB
		public override bool CompAllowVerbCast(Verb verb)
		{
			return !this.Props.blocksRangedWeapons || !(verb is Verb_LaunchProjectile);
		}

		// Token: 0x04006082 RID: 24706
		protected float energy;

		// Token: 0x04006083 RID: 24707
		protected int ticksToReset = -1;

		// Token: 0x04006084 RID: 24708
		protected int lastKeepDisplayTick = -9999;

		// Token: 0x04006085 RID: 24709
		private Vector3 impactAngleVect;

		// Token: 0x04006086 RID: 24710
		private int lastAbsorbDamageTick = -9999;

		// Token: 0x04006087 RID: 24711
		private const float MaxDamagedJitterDist = 0.05f;

		// Token: 0x04006088 RID: 24712
		private const int JitterDurationTicks = 8;

		// Token: 0x04006089 RID: 24713
		private int KeepDisplayingTicks = 1000;

		// Token: 0x0400608A RID: 24714
		private float ApparelScorePerEnergyMax = 0.25f;

		// Token: 0x0400608B RID: 24715
		private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
	}
}
