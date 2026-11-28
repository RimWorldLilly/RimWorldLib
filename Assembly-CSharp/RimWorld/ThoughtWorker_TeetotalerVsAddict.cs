using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E08 RID: 3592
	public class ThoughtWorker_TeetotalerVsAddict : ThoughtWorker
	{
		// Token: 0x060060FB RID: 24827 RVA: 0x002313AC File Offset: 0x0022F5AC
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
			List<Hediff> hediffs = other.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.IsAddiction)
				{
					return true;
				}
			}
			return false;
		}
	}
}
