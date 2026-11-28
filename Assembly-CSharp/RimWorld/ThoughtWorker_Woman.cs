using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E12 RID: 3602
	public class ThoughtWorker_Woman : ThoughtWorker
	{
		// Token: 0x0600610F RID: 24847 RVA: 0x0023191C File Offset: 0x0022FB1C
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.story.traits.HasTrait(TraitDefOf.DislikesWomen))
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
			if (other.gender != Gender.Female)
			{
				return false;
			}
			return true;
		}
	}
}
