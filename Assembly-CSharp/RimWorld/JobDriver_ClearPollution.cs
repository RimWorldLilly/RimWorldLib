using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A20 RID: 2592
	public class JobDriver_ClearPollution : JobDriver
	{
		// Token: 0x06004E4E RID: 20046 RVA: 0x001DD349 File Offset: 0x001DB549
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x001DD367 File Offset: 0x001DB567
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x001DD38A File Offset: 0x001DB58A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ClearPollution.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_ClearPollution.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x001DD39C File Offset: 0x001DB59C
		private void ClearPollutionAt(IntVec3 root, Map map)
		{
			JobDriver_ClearPollution.tmpCellsToClear.Clear();
			JobDriver_ClearPollution.tmpCellsToClear.AddRange(from c in map.areaManager.PollutionClear.ActiveCells
			where c.DistanceToSquared(root) < 100
			select c);
			JobDriver_ClearPollution.tmpCellsToClear.SortBy((IntVec3 c) => c.DistanceToSquared(root));
			int num = 6;
			for (int i = 0; i < JobDriver_ClearPollution.tmpCellsToClear.Count; i++)
			{
				if (this.CanUnpollute(root, map, JobDriver_ClearPollution.tmpCellsToClear[i], false))
				{
					JobDriver_ClearPollution.tmpCellsToClear[i].Unpollute(map);
					num--;
					if (num <= 0)
					{
						break;
					}
				}
			}
			JobDriver_ClearPollution.tmpCellsToClear.Clear();
			if (num > 0)
			{
				num -= this.UnpolluteRadially(root, map, num, false);
				if (num > 0)
				{
					this.UnpolluteRadially(root, map, num, true);
				}
			}
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x001DD480 File Offset: 0x001DB680
		private int UnpolluteRadially(IntVec3 root, Map map, int maxToUnpollute = 5, bool ignoreOtherPawnsCleaningCell = false)
		{
			int num = 0;
			foreach (IntVec3 c in GenRadial.RadialCellsAround(root, 10f, true))
			{
				if (this.CanUnpollute(root, map, c, ignoreOtherPawnsCleaningCell))
				{
					c.Unpollute(map);
					num++;
					if (num >= maxToUnpollute)
					{
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x001DD4F0 File Offset: 0x001DB6F0
		private bool CanUnpollute(IntVec3 root, Map map, IntVec3 c, bool ignoreOtherPawnsCleaningCell = false)
		{
			return c.IsPolluted(map) && (ignoreOtherPawnsCleaningCell || !this.AnyOtherPawnCleaning(this.pawn, c)) && (c.GetRoom(map) == null || root.GetRoom(map) == c.GetRoom(map)) && c.DistanceToSquared(root) <= 100;
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x001DD548 File Offset: 0x001DB748
		private bool AnyOtherPawnCleaning(Pawn pawn, IntVec3 cell)
		{
			List<Pawn> freeColonistsSpawned = pawn.Map.mapPawns.FreeColonistsSpawned;
			for (int i = 0; i < freeColonistsSpawned.Count; i++)
			{
				if (freeColonistsSpawned[i] != pawn && freeColonistsSpawned[i].CurJobDef == JobDefOf.ClearPollution)
				{
					LocalTargetInfo target = freeColonistsSpawned[i].CurJob.GetTarget(TargetIndex.A);
					if (target.IsValid && target.Cell == cell)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040030AA RID: 12458
		private const float WorkAmount = 5600f;

		// Token: 0x040030AB RID: 12459
		private const int MaxDistanceToClear = 10;

		// Token: 0x040030AC RID: 12460
		private const int MaxDistanceToClearSquared = 100;

		// Token: 0x040030AD RID: 12461
		private const int PollutionCellsToClearPerJob = 6;

		// Token: 0x040030AE RID: 12462
		private const TargetIndex CleanCellIndex = TargetIndex.A;

		// Token: 0x040030AF RID: 12463
		private float workDone;

		// Token: 0x040030B0 RID: 12464
		private static List<IntVec3> tmpCellsToClear = new List<IntVec3>();
	}
}
