using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E2 RID: 5858
	public class RitualOutcomeEffectWorker_Trial : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x1700192D RID: 6445
		// (get) Token: 0x06009298 RID: 37528 RVA: 0x003590E4 File Offset: 0x003572E4
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06009299 RID: 37529 RVA: 0x003590E7 File Offset: 0x003572E7
		public RitualOutcomeEffectWorker_Trial()
		{
		}

		// Token: 0x0600929A RID: 37530 RVA: 0x003590EF File Offset: 0x003572EF
		public RitualOutcomeEffectWorker_Trial(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600929B RID: 37531 RVA: 0x003590F8 File Offset: 0x003572F8
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = base.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			Pawn pawn = jobRitual.PawnWithRole("leader");
			Pawn pawn2 = jobRitual.PawnWithRole("convict");
			LookTargets lookTargets = pawn2;
			string text = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			}
			string str = pawn2.LabelShort + " " + outcome.label;
			TaggedString taggedString = outcome.description.Formatted(pawn2.Named("PAWN"), pawn.Named("PROSECUTOR"));
			string text2 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text2.NullOrEmpty())
			{
				taggedString += "\n\n" + text2;
			}
			taggedString += "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			if (text != null)
			{
				taggedString += "\n\n" + text;
			}
			if (outcome.Positive)
			{
				ChoiceLetter let = LetterMaker.MakeLetter(str, taggedString, LetterDefOf.RitualOutcomePositive, lookTargets, null, null, null);
				Find.LetterStack.ReceiveLetter(let, null, 0, true);
				pawn2.guilt.Notify_Guilty(900000);
				pawn2.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.TrialConvicted, null, null);
				return;
			}
			Find.LetterStack.ReceiveLetter(str, taggedString, LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.TrialFailed, null, null);
			pawn2.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.TrialExonerated, null, null);
		}

		// Token: 0x0600929C RID: 37532 RVA: 0x003592B0 File Offset: 0x003574B0
		public override RitualOutcomePossibility GetOutcome(float quality, LordJob_Ritual ritual)
		{
			if (!Rand.Chance(quality))
			{
				return this.def.outcomeChances[0];
			}
			return this.def.outcomeChances[1];
		}

		// Token: 0x0600929D RID: 37533 RVA: 0x003592DD File Offset: 0x003574DD
		public override string ExpectedQualityLabel()
		{
			return "ExpectedConvictionChance".Translate();
		}

		// Token: 0x040058C6 RID: 22726
		public const int ConvictGuiltyForDays = 15;
	}
}
