using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B69 RID: 2921
	public class JobGiver_ShamblerWander : JobGiver_Wander
	{
		// Token: 0x06005514 RID: 21780 RVA: 0x001FA5DD File Offset: 0x001F87DD
		public JobGiver_ShamblerWander()
		{
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.ticksBetweenWandersRange = new IntRange(480, 900);
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x001FA601 File Offset: 0x001F8801
		protected override Job TryGiveJob(Pawn pawn)
		{
			pawn.mindState.nextMoveOrderIsWait = !Rand.Chance(JobGiver_ShamblerWander.ChanceToWander);
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x001FA622 File Offset: 0x001F8822
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			if (pawn.IsGhoul)
			{
				return pawn.Position;
			}
			return this.GetWanderRootHerd(pawn);
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x001FA63C File Offset: 0x001F883C
		private IntVec3 GetWanderRootHerd(Pawn pawn)
		{
			Predicate<Thing> isHerdValidator = (Thing t) => ((Pawn)t).IsShambler && t != pawn && t.Faction == pawn.Faction;
			return WanderUtility.GetHerdWanderRoot(pawn, isHerdValidator);
		}

		// Token: 0x040032BD RID: 12989
		private static float ChanceToWander = 0.2f;
	}
}
