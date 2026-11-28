using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B94 RID: 2964
	public class LearningGiver_Skydreaming : LearningGiver
	{
		// Token: 0x060055E8 RID: 21992 RVA: 0x001FF6C4 File Offset: 0x001FD8C4
		public override bool CanDo(Pawn pawn)
		{
			IntVec3 intVec;
			return base.CanDo(pawn) && RCellFinder.TryFindAllowedUnroofedSpotOutsideColony(pawn.Position, pawn, out intVec, 35, int.MaxValue, 45, null);
		}

		// Token: 0x060055E9 RID: 21993 RVA: 0x001FF6F4 File Offset: 0x001FD8F4
		public override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindAllowedUnroofedSpotOutsideColony(pawn.Position, pawn, out c, 35, 2147483647, 45, null))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}
	}
}
