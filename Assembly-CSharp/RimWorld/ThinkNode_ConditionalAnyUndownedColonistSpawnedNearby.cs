using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D6D RID: 3437
	public class ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby : ThinkNode_Conditional
	{
		// Token: 0x06005F5D RID: 24413 RVA: 0x0022C148 File Offset: 0x0022A348
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.Spawned)
			{
				return pawn.Map.mapPawns.FreeColonistsSpawned.Any((Pawn x) => !x.Downed);
			}
			return false;
		}
	}
}
