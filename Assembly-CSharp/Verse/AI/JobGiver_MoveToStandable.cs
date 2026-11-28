using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008ED RID: 2285
	public class JobGiver_MoveToStandable : ThinkNode_JobGiver
	{
		// Token: 0x0600460D RID: 17933 RVA: 0x001B832C File Offset: 0x001B652C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.Drafted)
			{
				return null;
			}
			if (pawn.pather.Moving)
			{
				return null;
			}
			if (!pawn.Position.Standable(pawn.Map))
			{
				return JobGiver_MoveToStandable.FindBetterPositionJob(pawn);
			}
			List<Thing> thingList = pawn.Position.GetThingList(pawn.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Pawn pawn2 = thingList[i] as Pawn;
				if (pawn2 != null && pawn2 != pawn && pawn2.Faction == pawn.Faction && pawn2.Drafted && !pawn2.pather.MovingNow)
				{
					return JobGiver_MoveToStandable.FindBetterPositionJob(pawn);
				}
			}
			return null;
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x001B83D0 File Offset: 0x001B65D0
		private static Job FindBetterPositionJob(Pawn pawn)
		{
			IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(pawn.Position, pawn, null, true);
			if (intVec.IsValid && intVec != pawn.Position)
			{
				return JobMaker.MakeJob(JobDefOf.Goto, intVec);
			}
			return null;
		}
	}
}
