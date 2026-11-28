using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A56 RID: 2646
	public class JobDriver_Mine : JobDriver
	{
		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x06004FD4 RID: 20436 RVA: 0x001E2424 File Offset: 0x001E0624
		private Thing MineTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x001E2445 File Offset: 0x001E0645
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.MineTarget, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x001E2468 File Offset: 0x001E0668
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Mine.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_Mine.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x001E2478 File Offset: 0x001E0678
		private void DoDamage(Thing target, Toil mine, Pawn actor, IntVec3 mineablePos)
		{
			int num = target.def.building.isNaturalRock ? 80 : 40;
			Mineable mineable = target as Mineable;
			if (mineable == null || target.HitPoints > num)
			{
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, (float)num, 0f, -1f, mine.actor, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				target.TakeDamage(dinfo);
				return;
			}
			bool flag = base.Map.designationManager.DesignationAt(mineable.Position, DesignationDefOf.MineVein) != null;
			mineable.Notify_TookMiningDamage(target.HitPoints, mine.actor);
			mineable.HitPoints = 0;
			mineable.DestroyMined(actor);
			if (flag)
			{
				foreach (IntVec3 b in GenAdj.AdjacentCells)
				{
					Designator_MineVein.FloodFillDesignations(mineablePos + b, base.Map, mineable.def);
				}
			}
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x001E2560 File Offset: 0x001E0760
		private void ResetTicksToPickHit()
		{
			float num = this.pawn.GetStatValue(StatDefOf.MiningSpeed, true, -1);
			if (num < 0.6f && this.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.6f;
			}
			this.ticksToPickHit = (int)Math.Round((double)(100f / num));
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x001E25B4 File Offset: 0x001E07B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}

		// Token: 0x04003110 RID: 12560
		private int ticksToPickHit = -1000;

		// Token: 0x04003111 RID: 12561
		private Effecter effecter;

		// Token: 0x04003112 RID: 12562
		public const int BaseTicksBetweenPickHits = 100;

		// Token: 0x04003113 RID: 12563
		private const int BaseDamagePerPickHit_NaturalRock = 80;

		// Token: 0x04003114 RID: 12564
		private const int BaseDamagePerPickHit_NotNaturalRock = 40;

		// Token: 0x04003115 RID: 12565
		private const float MinMiningSpeedFactorForNPCs = 0.6f;
	}
}
