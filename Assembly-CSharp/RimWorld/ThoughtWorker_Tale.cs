using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E15 RID: 3605
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		// Token: 0x06006115 RID: 24853 RVA: 0x00231AB0 File Offset: 0x0022FCB0
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			if (Find.TaleManager.GetLatestTale(this.def.taleDef, other) == null)
			{
				return false;
			}
			return true;
		}
	}
}
