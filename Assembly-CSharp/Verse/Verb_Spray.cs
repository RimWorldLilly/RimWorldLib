using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000741 RID: 1857
	public abstract class Verb_Spray : Verb
	{
		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06003AE6 RID: 15078 RVA: 0x00177EA8 File Offset: 0x001760A8
		protected override int ShotsPerBurst
		{
			get
			{
				return base.BurstShotCount;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x00177EB0 File Offset: 0x001760B0
		public override float? AimAngleOverride
		{
			get
			{
				if (this.state == VerbState.Bursting && this.Available())
				{
					return new float?((this.path[this.ShotsPerBurst - this.burstShotsLeft].ToVector3Shifted() - this.caster.DrawPos).AngleFlat());
				}
				return null;
			}
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x00177F14 File Offset: 0x00176114
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			ShootLine shootLine;
			bool flag = base.TryFindShootLineFromTo(this.caster.Position, this.currentTarget, out shootLine, false);
			if (this.verbProps.stopBurstWithoutLos && !flag)
			{
				return false;
			}
			if (base.EquipmentSource != null && this.burstShotsLeft <= 1)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompApparelReloadable comp2 = base.EquipmentSource.GetComp<CompApparelReloadable>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			this.HitCell(this.path[this.ShotsPerBurst - this.burstShotsLeft]);
			this.lastShotTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x00177FE6 File Offset: 0x001761E6
		public override bool Available()
		{
			return this.ShotsPerBurst - this.burstShotsLeft >= 0;
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x00177FFB File Offset: 0x001761FB
		public override void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.initialTargetPosition = this.currentTarget.CenterVector3;
			this.PreparePath();
			base.TryCastNextBurstShot();
		}

		// Token: 0x06003AEB RID: 15083
		protected abstract void PreparePath();

		// Token: 0x06003AEC RID: 15084 RVA: 0x0017802D File Offset: 0x0017622D
		protected virtual void HitCell(IntVec3 cell)
		{
			EffecterDef sprayEffecterDef = this.verbProps.sprayEffecterDef;
			if (sprayEffecterDef == null)
			{
				return;
			}
			sprayEffecterDef.Spawn(this.caster.Position, cell, this.caster.Map, 1f);
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x00178064 File Offset: 0x00176264
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.path, "path", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<Vector3>(ref this.initialTargetPosition, "initialTargetPosition", default(Vector3), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.path == null)
			{
				this.path = new List<IntVec3>();
			}
		}

		// Token: 0x040026B0 RID: 9904
		protected List<IntVec3> path = new List<IntVec3>();

		// Token: 0x040026B1 RID: 9905
		protected Vector3 initialTargetPosition;
	}
}
