using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E72 RID: 3698
	public class ThoughtWorker_ToxicFallout : ThoughtWorker
	{
		// Token: 0x06006207 RID: 25095 RVA: 0x00234AC9 File Offset: 0x00232CC9
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && p.MapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}
	}
}
