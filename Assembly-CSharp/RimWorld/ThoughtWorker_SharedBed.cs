using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E30 RID: 3632
	public class ThoughtWorker_SharedBed : ThoughtWorker
	{
		// Token: 0x06006154 RID: 24916 RVA: 0x00232850 File Offset: 0x00230A50
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			return LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(p) != null;
		}
	}
}
