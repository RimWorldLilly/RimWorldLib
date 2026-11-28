using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200205C RID: 8284
	public class PlaceWorker_RequireNaturePsycaster : PlaceWorker
	{
		// Token: 0x0600D125 RID: 53541 RVA: 0x004CE8DC File Offset: 0x004CCADC
		public override bool IsBuildDesignatorVisible(BuildableDef def)
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				if (MeditationFocusDefOf.Natural.CanPawnUse(p))
				{
					return true;
				}
			}
			return false;
		}
	}
}
