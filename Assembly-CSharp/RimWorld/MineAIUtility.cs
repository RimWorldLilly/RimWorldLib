using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D2A RID: 3370
	public static class MineAIUtility
	{
		// Token: 0x06005E6A RID: 24170 RVA: 0x0022807C File Offset: 0x0022627C
		public static void ResetStaticData()
		{
			MineAIUtility.NoPathTrans = "NoPath".Translate();
		}

		// Token: 0x06005E6B RID: 24171 RVA: 0x00228092 File Offset: 0x00226292
		public static IEnumerable<Thing> PotentialMineables(Pawn pawn)
		{
			MineAIUtility.<PotentialMineables>d__4 <PotentialMineables>d__ = new MineAIUtility.<PotentialMineables>d__4(-2);
			<PotentialMineables>d__.<>3__pawn = pawn;
			return <PotentialMineables>d__;
		}

		// Token: 0x06005E6C RID: 24172 RVA: 0x002280A4 File Offset: 0x002262A4
		public static Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!t.def.mineable)
			{
				return null;
			}
			if (pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) == null && pawn.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.MineVein) == null)
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			if (!new HistoryEvent(HistoryEventDefOf.Mined, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
			{
				return null;
			}
			bool flag = false;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 intVec = t.Position + GenAdj.AdjacentCells[i];
				if (intVec.InBounds(pawn.Map) && intVec.Standable(pawn.Map) && ReachabilityImmediate.CanReachImmediate(intVec, t, pawn.Map, PathEndMode.Touch, pawn))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < 8; j++)
				{
					IntVec3 intVec2 = t.Position + GenAdj.AdjacentCells[j];
					if (intVec2.InBounds(t.Map) && ReachabilityImmediate.CanReachImmediate(intVec2, t, pawn.Map, PathEndMode.Touch, pawn) && intVec2.WalkableBy(t.Map, pawn) && !intVec2.Standable(t.Map))
					{
						List<Thing> thingList = intVec2.GetThingList(t.Map);
						for (int k = 0; k < thingList.Count; k++)
						{
							if (thingList[k].def.designateHaulable && thingList[k].def.passability == Traversability.PassThroughOnly)
							{
								Job job = HaulAIUtility.HaulAsideJobFor(pawn, thingList[k]);
								if (job != null)
								{
									return job;
								}
							}
						}
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					JobFailReason.Is(MineAIUtility.NoPathTrans, null);
					return null;
				}
			}
			return JobMaker.MakeJob(JobDefOf.Mine, t, 20000, true);
		}

		// Token: 0x04003582 RID: 13698
		private static string NoPathTrans;

		// Token: 0x04003583 RID: 13699
		private const int MiningJobTicks = 20000;

		// Token: 0x04003584 RID: 13700
		private static List<Designation> tmpDesignations = new List<Designation>();
	}
}
