using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0E RID: 3598
	public class ThoughtWorker_Ugly : ThoughtWorker
	{
		// Token: 0x06006107 RID: 24839 RVA: 0x002316E8 File Offset: 0x0022F8E8
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn))
			{
				return false;
			}
			if (!pawn.story.CaresAboutOthersAppearance)
			{
				return false;
			}
			float statValue = other.GetStatValue(StatDefOf.PawnBeauty, true, -1);
			if (statValue <= -2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (statValue <= -1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}
