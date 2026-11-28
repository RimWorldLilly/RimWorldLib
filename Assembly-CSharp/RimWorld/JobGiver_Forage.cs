using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B41 RID: 2881
	public class JobGiver_Forage : ThinkNode_JobGiver
	{
		// Token: 0x06005477 RID: 21623 RVA: 0x001F630C File Offset: 0x001F450C
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c2;
			if (!CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, 30, (IntVec3 c) => c.GetFertility(pawn.Map) >= 0.7f && c.GetTerrain(pawn.Map).natural && !c.IsForbidden(pawn) && pawn.CanReserveAndReach(c, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false), out c2, -1))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Forage, c2);
		}

		// Token: 0x04003282 RID: 12930
		private const int SearchRadius = 30;

		// Token: 0x04003283 RID: 12931
		private const float MinFertility = 0.7f;
	}
}
