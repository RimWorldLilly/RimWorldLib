using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D82 RID: 3458
	public class ThinkNode_ConditionalSpawnedOrAnyParentSpawned : ThinkNode_Conditional
	{
		// Token: 0x06005F8B RID: 24459 RVA: 0x0022C841 File Offset: 0x0022AA41
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.SpawnedOrAnyParentSpawned;
		}
	}
}
