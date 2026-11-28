using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B4B RID: 2891
	public class JobGiver_GetRest : ThinkNode_JobGiver
	{
		// Token: 0x060054A4 RID: 21668 RVA: 0x001F72C2 File Offset: 0x001F54C2
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_GetRest jobGiver_GetRest = (JobGiver_GetRest)base.DeepCopy(resolve);
			jobGiver_GetRest.minCategory = this.minCategory;
			jobGiver_GetRest.maxLevelPercentage = this.maxLevelPercentage;
			return jobGiver_GetRest;
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x001F72E8 File Offset: 0x001F54E8
		public override float GetPriority(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			if (rest == null)
			{
				return 0f;
			}
			if (rest.CurCategory < this.minCategory)
			{
				return 0f;
			}
			if (rest.CurLevelPercentage > this.maxLevelPercentage)
			{
				return 0f;
			}
			if (Find.TickManager.TicksGame < pawn.mindState.canSleepTick)
			{
				return 0f;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds)
			{
				return 0f;
			}
			if (!RestUtility.CanFallAsleep(pawn))
			{
				return 0f;
			}
			TimeAssignmentDef timeAssignmentDef;
			if (pawn.RaceProps.Humanlike)
			{
				timeAssignmentDef = ((pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment);
			}
			else
			{
				int num = GenLocalDate.HourOfDay(pawn);
				if (num < 7 || num > 21)
				{
					timeAssignmentDef = TimeAssignmentDefOf.Sleep;
				}
				else
				{
					timeAssignmentDef = TimeAssignmentDefOf.Anything;
				}
			}
			float curLevel = rest.CurLevel;
			if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
			{
				if (curLevel < 0.3f)
				{
					return 8f;
				}
				return 0f;
			}
			else
			{
				if (timeAssignmentDef == TimeAssignmentDefOf.Work)
				{
					return 0f;
				}
				if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
				{
					if (curLevel < 0.16f)
					{
						return 8f;
					}
					return 0f;
				}
				else if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
				{
					if (curLevel < 0.3f)
					{
						return 8f;
					}
					return 0f;
				}
				else
				{
					if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
					{
						return 8f;
					}
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x001F7444 File Offset: 0x001F5644
		protected override Job TryGiveJob(Pawn pawn)
		{
			Need_Rest rest = pawn.needs.rest;
			if (rest == null || rest.CurCategory < this.minCategory || rest.CurLevelPercentage > this.maxLevelPercentage)
			{
				return null;
			}
			if (RestUtility.DisturbancePreventsLyingDown(pawn))
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
					goto IL_92;
				}
			}
			building_Bed = null;
			IL_92:
			if (building_Bed != null)
			{
				return JobMaker.MakeJob(JobDefOf.LayDown, building_Bed);
			}
			IntVec3 c;
			if (this.TryFindGroundSleepSpotFor(pawn, out c))
			{
				return JobMaker.MakeJob(JobDefOf.LayDown, c);
			}
			return null;
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x001F7514 File Offset: 0x001F5714
		private bool TryFindGroundSleepSpotFor(Pawn pawn, out IntVec3 cell)
		{
			Map map = pawn.Map;
			IntVec3 position = pawn.Position;
			if (pawn.RaceProps.Dryad && pawn.connections != null)
			{
				using (List<Thing>.Enumerator enumerator = pawn.connections.ConnectedThings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing thing = enumerator.Current;
						if (pawn.CanReach(thing, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
						{
							position = thing.Position;
							break;
						}
					}
					goto IL_B8;
				}
			}
			if (JobGiver_GetRest.IsValidCell(pawn, position))
			{
				cell = position;
				return true;
			}
			IL_B8:
			Predicate<IntVec3> <>9__1;
			for (int i = 0; i < 2; i++)
			{
				int num = (i == 0) ? 4 : 12;
				IntVec3 root = position;
				Map map2 = map;
				int radius = num;
				Predicate<IntVec3> extraValidator;
				if ((extraValidator = <>9__1) == null)
				{
					extraValidator = (<>9__1 = ((IntVec3 c) => JobGiver_GetRest.IsValidCell(pawn, c)));
				}
				IntVec3 intVec;
				if (CellFinder.TryRandomClosewalkCellNear(root, map2, radius, out intVec, extraValidator))
				{
					cell = intVec;
					return true;
				}
			}
			cell = CellFinder.RandomClosewalkCellNearNotForbidden(pawn, 4, (IntVec3 c) => JobGiver_GetRest.IsValidCell(pawn, c));
			return JobGiver_GetRest.IsValidCell(pawn, cell);
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x001F766C File Offset: 0x001F586C
		private static bool IsValidCell(Pawn pawn, IntVec3 cell)
		{
			return !cell.IsForbidden(pawn) && !cell.GetTerrain(pawn.Map).avoidWander && pawn.CanReserve(cell, 1, -1, null, false);
		}

		// Token: 0x04003290 RID: 12944
		private RestCategory minCategory;

		// Token: 0x04003291 RID: 12945
		private float maxLevelPercentage = 1f;
	}
}
