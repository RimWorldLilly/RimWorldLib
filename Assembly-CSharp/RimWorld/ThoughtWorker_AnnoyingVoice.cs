using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0F RID: 3599
	public class ThoughtWorker_AnnoyingVoice : ThoughtWorker
	{
		// Token: 0x06006109 RID: 24841 RVA: 0x00231770 File Offset: 0x0022F970
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!other.story.traits.HasTrait(TraitDefOf.AnnoyingVoice))
			{
				return false;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				return false;
			}
			if (pawn.story.traits.HasTrait(TraitDefOf.Kind))
			{
				return false;
			}
			return true;
		}
	}
}
