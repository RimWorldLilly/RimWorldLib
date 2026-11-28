using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BA3 RID: 2979
	public class JobGiver_ReachOutside : ThinkNode_JobGiver
	{
		// Token: 0x06005617 RID: 22039 RVA: 0x00200490 File Offset: 0x001FE690
		protected override Job TryGiveJob(Pawn pawn)
		{
			District district = pawn.GetDistrict(RegionType.Set_Passable);
			if (district.Room.PsychologicallyOutdoors && district.TouchesMapEdge)
			{
				return null;
			}
			if (!pawn.CanReachMapEdge())
			{
				return null;
			}
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out intVec))
			{
				return null;
			}
			if (intVec == pawn.Position)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Goto, intVec);
		}
	}
}
