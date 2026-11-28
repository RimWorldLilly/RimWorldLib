using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020008FA RID: 2298
	public class JobGiver_WanderHerd : JobGiver_Wander
	{
		// Token: 0x06004646 RID: 17990 RVA: 0x001B8EE8 File Offset: 0x001B70E8
		public JobGiver_WanderHerd()
		{
			this.wanderRadius = 5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x001B8F10 File Offset: 0x001B7110
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Predicate<Thing> isHerdValidator = delegate(Thing t)
			{
				if (((Pawn)t).RaceProps != pawn.RaceProps || t == pawn)
				{
					return false;
				}
				if (t.Faction != pawn.Faction)
				{
					return false;
				}
				IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn2 = allPawnsSpawned[i];
					if (pawn2.RaceProps.Humanlike && (pawn2.Position - t.Position).LengthHorizontalSquared < 225)
					{
						return false;
					}
				}
				return true;
			};
			return WanderUtility.GetHerdWanderRoot(pawn, isHerdValidator);
		}

		// Token: 0x04002D9C RID: 11676
		private const int MinDistToHumanlike = 15;
	}
}
