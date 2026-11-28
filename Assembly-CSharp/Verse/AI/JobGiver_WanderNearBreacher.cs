using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000903 RID: 2307
	public class JobGiver_WanderNearBreacher : JobGiver_Wander
	{
		// Token: 0x0600465F RID: 18015 RVA: 0x001B9420 File Offset: 0x001B7620
		public JobGiver_WanderNearBreacher()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x001B9448 File Offset: 0x001B7648
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Pawn pawn2 = BreachingUtility.FindPawnToEscort(pawn);
			if (pawn2 == null)
			{
				return IntVec3.Invalid;
			}
			return pawn2.Position;
		}
	}
}
