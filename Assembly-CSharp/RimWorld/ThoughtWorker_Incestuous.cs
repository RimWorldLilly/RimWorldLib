using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E01 RID: 3585
	public class ThoughtWorker_Incestuous : ThoughtWorker
	{
		// Token: 0x060060ED RID: 24813 RVA: 0x002310A8 File Offset: 0x0022F2A8
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			if (LovePartnerRelationUtility.IncestOpinionOffsetFor(other, pawn) == 0f)
			{
				return false;
			}
			return true;
		}
	}
}
