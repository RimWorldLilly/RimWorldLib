using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0C RID: 3596
	public class ThoughtWorker_Drunk : ThoughtWorker
	{
		// Token: 0x06006103 RID: 24835 RVA: 0x002315D8 File Offset: 0x0022F7D8
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.IsTeetotaler())
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
			Hediff firstHediffOfDef = other.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh, false);
			if (firstHediffOfDef == null || !firstHediffOfDef.Visible)
			{
				return false;
			}
			return true;
		}
	}
}
