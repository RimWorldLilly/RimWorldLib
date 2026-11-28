using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB4 RID: 2740
	public class JobGiver_WanderHive : JobGiver_Wander
	{
		// Token: 0x06005286 RID: 21126 RVA: 0x001EBEAB File Offset: 0x001EA0AB
		public JobGiver_WanderHive()
		{
			this.wanderRadius = 7.5f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x001EBED0 File Offset: 0x001EA0D0
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			if (hive == null || !hive.Spawned)
			{
				return pawn.Position;
			}
			return hive.Position;
		}
	}
}
