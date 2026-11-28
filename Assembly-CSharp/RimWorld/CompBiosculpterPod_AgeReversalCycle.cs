using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001838 RID: 6200
	public class CompBiosculpterPod_AgeReversalCycle : CompBiosculpterPod_Cycle
	{
		// Token: 0x06009B40 RID: 39744 RVA: 0x0037FB54 File Offset: 0x0037DD54
		public override string Description(Pawn tunedFor)
		{
			int num = 3600000;
			if (tunedFor != null)
			{
				num *= (int)tunedFor.ageTracker.AdultAgingMultiplier;
			}
			return base.Description(tunedFor).Formatted(num.ToStringTicksToPeriod(false, false, false, true, false).Named("DURATION"));
		}

		// Token: 0x06009B41 RID: 39745 RVA: 0x0037FBA0 File Offset: 0x0037DDA0
		public override void CycleCompleted(Pawn pawn)
		{
			int num = (int)(3600000f * pawn.ageTracker.AdultAgingMultiplier);
			long val = (long)(3600000f * pawn.ageTracker.AdultMinAge);
			pawn.ageTracker.AgeBiologicalTicks = Math.Max(val, pawn.ageTracker.AgeBiologicalTicks - (long)num);
			pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.ViaTreatment, false);
			int num2 = (int)(pawn.ageTracker.AgeReversalDemandedDeadlineTicks / 60000L);
			string text = "BiosculpterAgeReversalCompletedMessage".Translate(pawn.Named("PAWN"));
			Ideo ideo = pawn.Ideo;
			if (ideo != null && ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded))
			{
				text += " " + "AgeReversalExpectationDeadline".Translate(pawn.Named("PAWN"), num2.Named("DEADLINE")).CapitalizeFirst();
			}
			Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
		}
	}
}
