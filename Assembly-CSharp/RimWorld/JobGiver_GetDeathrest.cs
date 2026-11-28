using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B43 RID: 2883
	public class JobGiver_GetDeathrest : ThinkNode_JobGiver
	{
		// Token: 0x0600547D RID: 21629 RVA: 0x001F6490 File Offset: 0x001F4690
		public override float GetPriority(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds)
			{
				return 0f;
			}
			Need_Deathrest need_Deathrest;
			if (pawn.needs == null || !pawn.needs.TryGetNeed<Need_Deathrest>(out need_Deathrest) || need_Deathrest.CurLevelPercentage > this.maxNeedPercent)
			{
				return 0f;
			}
			return 7.75f;
		}

		// Token: 0x0600547E RID: 21630 RVA: 0x001F64F8 File Offset: 0x001F46F8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!ModsConfig.BiotechActive)
			{
				return null;
			}
			Need_Deathrest need_Deathrest;
			if (pawn.needs == null || !pawn.needs.TryGetNeed<Need_Deathrest>(out need_Deathrest))
			{
				return null;
			}
			if (need_Deathrest.CurLevelPercentage > this.maxNeedPercent)
			{
				return null;
			}
			Lord lord = pawn.GetLord();
			Building_Bed building_Bed;
			if ((lord == null || lord.CurLordToil == null || lord.CurLordToil.AllowRestingInBed) && !pawn.IsWildMan() && (!pawn.InMentalState || pawn.MentalState.AllowRestingInBed))
			{
				Pawn_RopeTracker roping = pawn.roping;
				if (roping == null || !roping.IsRoped)
				{
					building_Bed = RestUtility.FindBedFor(pawn);
					goto IL_8D;
				}
			}
			building_Bed = null;
			IL_8D:
			if (building_Bed != null)
			{
				return JobMaker.MakeJob(JobDefOf.Deathrest, building_Bed);
			}
			return JobMaker.MakeJob(JobDefOf.Deathrest, this.FindGroundSleepSpotFor(pawn));
		}

		// Token: 0x0600547F RID: 21631 RVA: 0x001F65BC File Offset: 0x001F47BC
		private IntVec3 FindGroundSleepSpotFor(Pawn pawn)
		{
			Map map = pawn.Map;
			IntVec3 position = pawn.Position;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < 2; i++)
			{
				int num = (i == 0) ? 4 : 12;
				IntVec3 root = position;
				Map map2 = map;
				int radius = num;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__0) == null)
				{
					extraValidator = (<>9__0 = ((IntVec3 x) => !x.IsForbidden(pawn) && !x.GetTerrain(map).avoidWander));
				}
				IntVec3 result;
				if (CellFinder.TryRandomClosewalkCellNear(root, map2, radius, out result, extraValidator))
				{
					return result;
				}
			}
			return CellFinder.RandomClosewalkCellNearNotForbidden(pawn, 4, null);
		}

		// Token: 0x06005480 RID: 21632 RVA: 0x001F664A File Offset: 0x001F484A
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetDeathrest jobGiver_GetDeathrest = (JobGiver_GetDeathrest)base.DeepCopy(resolve);
			jobGiver_GetDeathrest.maxNeedPercent = this.maxNeedPercent;
			return jobGiver_GetDeathrest;
		}

		// Token: 0x04003284 RID: 12932
		public float maxNeedPercent = 0.05f;
	}
}
