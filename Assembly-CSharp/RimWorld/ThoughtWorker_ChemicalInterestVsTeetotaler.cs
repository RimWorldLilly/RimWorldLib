using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0A RID: 3594
	public class ThoughtWorker_ChemicalInterestVsTeetotaler : ThoughtWorker
	{
		// Token: 0x060060FF RID: 24831 RVA: 0x002314C8 File Offset: 0x0022F6C8
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) <= 0)
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
			if (other.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) >= 0)
			{
				return false;
			}
			return true;
		}
	}
}
