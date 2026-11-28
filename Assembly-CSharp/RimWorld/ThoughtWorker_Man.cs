using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E11 RID: 3601
	public class ThoughtWorker_Man : ThoughtWorker
	{
		// Token: 0x0600610D RID: 24845 RVA: 0x00231898 File Offset: 0x0022FA98
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.DislikesMen))
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
			if (other.gender != Gender.Male)
			{
				return false;
			}
			return true;
		}
	}
}
