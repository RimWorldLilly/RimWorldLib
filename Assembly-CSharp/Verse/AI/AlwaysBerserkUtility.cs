using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000851 RID: 2129
	public static class AlwaysBerserkUtility
	{
		// Token: 0x060042F2 RID: 17138 RVA: 0x001A6CE4 File Offset: 0x001A4EE4
		public static bool TryTriggerBerserkBloodRage(Pawn pawn)
		{
			return ModsConfig.AnomalyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.BloodRage, false) && pawn.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_BloodRage".Translate(), MentalBreakDefOf.Berserk);
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x001A6D38 File Offset: 0x001A4F38
		public static bool TryTriggerBerserkFrenzyInducer(Pawn pawn)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.FrenzyField, false);
			return firstHediffOfDef != null && firstHediffOfDef.CurStageIndex > 0 && pawn.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_FrenzyInducer".Translate(), MentalBreakDefOf.Berserk);
		}
	}
}
