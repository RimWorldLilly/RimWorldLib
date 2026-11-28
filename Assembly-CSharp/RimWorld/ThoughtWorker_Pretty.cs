using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0D RID: 3597
	public class ThoughtWorker_Pretty : ThoughtWorker
	{
		// Token: 0x06006105 RID: 24837 RVA: 0x00231664 File Offset: 0x0022F864
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (RelationsUtility.IsDisfigured(other, pawn, false))
			{
				return false;
			}
			if (PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn))
			{
				return false;
			}
			float statValue = other.GetStatValue(StatDefOf.PawnBeauty, true, -1);
			if (statValue >= 2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (statValue >= 1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}
