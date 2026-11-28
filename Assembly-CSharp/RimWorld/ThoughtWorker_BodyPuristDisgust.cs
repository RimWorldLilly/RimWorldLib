using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E14 RID: 3604
	public class ThoughtWorker_BodyPuristDisgust : ThoughtWorker
	{
		// Token: 0x06006113 RID: 24851 RVA: 0x00231A28 File Offset: 0x0022FC28
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.BodyPurist))
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
