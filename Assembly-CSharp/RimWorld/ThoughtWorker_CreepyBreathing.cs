using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E10 RID: 3600
	public class ThoughtWorker_CreepyBreathing : ThoughtWorker
	{
		// Token: 0x0600610B RID: 24843 RVA: 0x00231804 File Offset: 0x0022FA04
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (!other.story.traits.HasTrait(TraitDefOf.CreepyBreathing))
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
