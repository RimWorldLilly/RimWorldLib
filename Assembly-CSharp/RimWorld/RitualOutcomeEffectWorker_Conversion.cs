using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E3 RID: 5859
	public class RitualOutcomeEffectWorker_Conversion : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x1700192E RID: 6446
		// (get) Token: 0x0600929E RID: 37534 RVA: 0x003592EE File Offset: 0x003574EE
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600929F RID: 37535 RVA: 0x003592F1 File Offset: 0x003574F1
		public RitualOutcomeEffectWorker_Conversion()
		{
		}

		// Token: 0x060092A0 RID: 37536 RVA: 0x003592F9 File Offset: 0x003574F9
		public RitualOutcomeEffectWorker_Conversion(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092A1 RID: 37537 RVA: 0x00359304 File Offset: 0x00357504
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = base.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			LookTargets lookTargets = jobRitual.selectedTarget;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			Pawn pawn = jobRitual.PawnWithRole("moralist");
			Pawn pawn2 = jobRitual.PawnWithRole("convertee");
			float ideoCertaintyOffset = outcome.ideoCertaintyOffset;
			if (ideoCertaintyOffset <= -1f)
			{
				pawn2.ideo.SetIdeo(pawn.Ideo);
			}
			else
			{
				pawn2.ideo.OffsetCertainty(ideoCertaintyOffset);
			}
			foreach (Pawn pawn3 in totalPresence.Keys)
			{
				if (pawn3 != pawn && pawn3 != pawn2 && outcome.memory != null)
				{
					Thought_AttendedRitual newThought = (Thought_AttendedRitual)base.MakeMemory(pawn3, jobRitual, outcome.memory);
					pawn3.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
				}
			}
			TaggedString taggedString = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst();
			string text2 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text2.NullOrEmpty())
			{
				taggedString += "\n\n" + text2;
			}
			if (text != null)
			{
				taggedString += "\n\n" + text;
			}
			taggedString += "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			string text3;
			this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text3);
			if (text3 != null)
			{
				taggedString += "\n\n" + text3;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), taggedString, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
		}
	}
}
