using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BAD RID: 2989
	public class JobGiver_WanderColony : JobGiver_Wander
	{
		// Token: 0x06005631 RID: 22065 RVA: 0x00200B1C File Offset: 0x001FED1C
		public JobGiver_WanderColony()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => true);
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x00200B71 File Offset: 0x001FED71
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.GetColonyWanderRoot(pawn);
		}
	}
}
