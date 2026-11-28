using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E4 RID: 5860
	public class RitualOutcomeEffectWorker_Execution : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x1700192F RID: 6447
		// (get) Token: 0x060092A2 RID: 37538 RVA: 0x00359520 File Offset: 0x00357720
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060092A3 RID: 37539 RVA: 0x00359523 File Offset: 0x00357723
		public RitualOutcomeEffectWorker_Execution()
		{
		}

		// Token: 0x060092A4 RID: 37540 RVA: 0x0035952B File Offset: 0x0035772B
		public RitualOutcomeEffectWorker_Execution(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092A5 RID: 37541 RVA: 0x00359534 File Offset: 0x00357734
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
			bool flag = false;
			foreach (Pawn pawn in totalPresence.Keys)
			{
				if (pawn.IsSlave)
				{
					Need_Suppression need_Suppression;
					if (pawn.needs.TryGetNeed<Need_Suppression>(out need_Suppression))
					{
						need_Suppression.CurLevel = 1f;
					}
					flag = true;
				}
				else
				{
					base.GiveMemoryToPawn(pawn, outcome.memory, jobRitual);
				}
			}
			string text2 = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst() + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			string text3 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text3.NullOrEmpty())
			{
				text2 = text2 + "\n\n" + text3;
			}
			if (flag)
			{
				text2 += "\n\n" + "RitualOutcomeExtraDesc_Execution".Translate();
			}
			if (text != null)
			{
				text2 = text2 + "\n\n" + text;
			}
			string text4;
			this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text4);
			if (text4 != null)
			{
				text2 = text2 + "\n\n" + text4;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text2, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
		}
	}
}
