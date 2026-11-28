using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE8 RID: 3560
	public class ThoughtWorker_Precept_NoFish : ThoughtWorker_Precept
	{
		// Token: 0x060060B6 RID: 24758 RVA: 0x0023032F File Offset: 0x0022E52F
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.OdysseyActive || !p.SpawnedOrAnyParentSpawned)
			{
				return ThoughtState.Inactive;
			}
			return !p.MapHeld.waterBodyTracker.AnyBodyContainsFish;
		}
	}
}
