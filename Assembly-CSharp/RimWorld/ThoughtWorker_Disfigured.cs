using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E03 RID: 3587
	public class ThoughtWorker_Disfigured : ThoughtWorker
	{
		// Token: 0x060060F1 RID: 24817 RVA: 0x00231130 File Offset: 0x0022F330
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || other.Dead)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!RelationsUtility.IsDisfigured(other, pawn, false))
			{
				return false;
			}
			if (PawnUtility.IsBiologicallyBlind(pawn))
			{
				return false;
			}
			if (!pawn.story.CaresAboutOthersAppearance)
			{
				return false;
			}
			if (pawn.Ideo != null && pawn.Ideo.IdeoApprovesOfBlindness() && !RelationsUtility.IsDisfigured(other, pawn, true) && (PawnUtility.IsBiologicallyBlind(other) || ThoughtWorker_Precept_HalfBlind.IsHalfBlind(other)))
			{
				return false;
			}
			return true;
		}
	}
}
