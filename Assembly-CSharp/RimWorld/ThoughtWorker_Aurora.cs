using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E2D RID: 3629
	public class ThoughtWorker_Aurora : ThoughtWorker_GameCondition
	{
		// Token: 0x0600614E RID: 24910 RVA: 0x002326D8 File Offset: 0x002308D8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Map mapHeld = p.MapHeld;
			bool flag;
			if (mapHeld == null)
			{
				flag = false;
			}
			else
			{
				GameConditionManager gameConditionManager = mapHeld.GameConditionManager;
				flag = ((gameConditionManager != null) ? new bool?(gameConditionManager.IsAlwaysDarkOutside) : null).GetValueOrDefault();
			}
			if (flag)
			{
				return false;
			}
			return base.CurrentStateInternal(p).Active && p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && !PawnUtility.IsBiologicallyOrArtificiallyBlind(p);
		}
	}
}
