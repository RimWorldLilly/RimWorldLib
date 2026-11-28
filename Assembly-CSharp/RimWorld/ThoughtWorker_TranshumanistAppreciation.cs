using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E13 RID: 3603
	public class ThoughtWorker_TranshumanistAppreciation : ThoughtWorker
	{
		// Token: 0x06006111 RID: 24849 RVA: 0x002319A0 File Offset: 0x0022FBA0
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.Transhumanist))
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (other.def != p.def)
			{
				return false;
			}
			int num = GeneUtility.AddedAndImplantedPartsWithXenogenesCount(other);
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}
	}
}
