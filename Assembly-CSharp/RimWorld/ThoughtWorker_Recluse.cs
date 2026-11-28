using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E75 RID: 3701
	public class ThoughtWorker_Recluse : ThoughtWorker
	{
		// Token: 0x0600620E RID: 25102 RVA: 0x00234CD0 File Offset: 0x00232ED0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.Spawned)
			{
				return ThoughtState.Inactive;
			}
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return ThoughtState.Inactive;
			}
			int freeColonistsAndPrisonersSpawnedCount = p.Map.mapPawns.FreeColonistsAndPrisonersSpawnedCount;
			if (freeColonistsAndPrisonersSpawnedCount <= 1)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (freeColonistsAndPrisonersSpawnedCount <= 4)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (freeColonistsAndPrisonersSpawnedCount <= 10)
			{
				return ThoughtState.Inactive;
			}
			if (freeColonistsAndPrisonersSpawnedCount <= 15)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
