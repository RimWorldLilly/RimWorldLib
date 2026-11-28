using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1A RID: 3610
	public class ThoughtWorker_Joyous : ThoughtWorker
	{
		// Token: 0x06006120 RID: 24864 RVA: 0x00231D28 File Offset: 0x0022FF28
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			return other.story.traits.HasTrait(TraitDefOf.Joyous);
		}
	}
}
