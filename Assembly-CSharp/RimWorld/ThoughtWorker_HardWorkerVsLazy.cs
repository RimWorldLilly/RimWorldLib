using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E04 RID: 3588
	public class ThoughtWorker_HardWorkerVsLazy : ThoughtWorker
	{
		// Token: 0x060060F3 RID: 24819 RVA: 0x002311E4 File Offset: 0x0022F3E4
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) <= 0)
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
			if (other.story.traits.DegreeOfTrait(TraitDefOf.Industriousness) > 0)
			{
				return false;
			}
			return true;
		}
	}
}
