using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E07 RID: 3591
	public class ThoughtWorker_TeetotalerVsChemicalInterest : ThoughtWorker
	{
		// Token: 0x060060F9 RID: 24825 RVA: 0x00231310 File Offset: 0x0022F510
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p.story == null || p.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) >= 0)
			{
				return false;
			}
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (other.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) <= 0)
			{
				return false;
			}
			return true;
		}
	}
}
