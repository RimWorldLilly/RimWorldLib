using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B4D RID: 2893
	public class JobGiver_JumpInWater : ThinkNode_JobGiver
	{
		// Token: 0x060054AC RID: 21676 RVA: 0x001F773C File Offset: 0x001F593C
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (Rand.Value < 1f && RCellFinder.TryFindRandomCellNearWith(pawn.Position, (IntVec3 pos) => pos.GetTerrain(pawn.Map).extinguishesFire, pawn.Map, out c, 5, this.MaxDistance.RandomInRange))
			{
				return JobMaker.MakeJob(JobDefOf.Goto, c);
			}
			return null;
		}

		// Token: 0x04003292 RID: 12946
		private const float ActivateChance = 1f;

		// Token: 0x04003293 RID: 12947
		private readonly IntRange MaxDistance = new IntRange(10, 16);
	}
}
