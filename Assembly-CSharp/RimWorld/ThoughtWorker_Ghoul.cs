using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1B RID: 3611
	public class ThoughtWorker_Ghoul : ThoughtWorker
	{
		// Token: 0x06006122 RID: 24866 RVA: 0x00231D89 File Offset: 0x0022FF89
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
			return other.IsGhoul;
		}
	}
}
