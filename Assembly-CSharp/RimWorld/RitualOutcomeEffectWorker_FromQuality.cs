using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016DF RID: 5855
	public class RitualOutcomeEffectWorker_FromQuality : RitualOutcomeEffectWorker
	{
		// Token: 0x1700192A RID: 6442
		// (get) Token: 0x0600927C RID: 37500 RVA: 0x00357B78 File Offset: 0x00355D78
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return this.def.allowAttachableOutcome;
			}
		}

		// Token: 0x1700192B RID: 6443
		// (get) Token: 0x0600927D RID: 37501 RVA: 0x00357B85 File Offset: 0x00355D85
		public virtual bool GivesDevelopmentPoints
		{
			get
			{
				return this.def.givesDevelopmentPoints;
			}
		}

		// Token: 0x0600927E RID: 37502 RVA: 0x00357B92 File Offset: 0x00355D92
		public RitualOutcomeEffectWorker_FromQuality()
		{
		}

		// Token: 0x0600927F RID: 37503 RVA: 0x00357B9A File Offset: 0x00355D9A
		public RitualOutcomeEffectWorker_FromQuality(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06009280 RID: 37504 RVA: 0x00357BA4 File Offset: 0x00355DA4
		public virtual RitualOutcomePossibility GetOutcome(float quality, LordJob_Ritual ritual)
		{
			return (from o in this.def.outcomeChances
			where this.OutcomePossible(o, ritual)
			select o).RandomElementByWeight((RitualOutcomePossibility c) => RitualOutcomeEffectWorker_FromQuality.ChanceWithQuality(c, quality));
		}

		// Token: 0x06009281 RID: 37505 RVA: 0x00357BFC File Offset: 0x00355DFC
		public virtual float GetOutcomeChanceAtQuality(LordJob_Ritual ritual, RitualOutcomePossibility outcome, float quality)
		{
			float num = (from o in this.def.outcomeChances
			where this.OutcomePossible(o, ritual)
			select o).Sum((RitualOutcomePossibility c) => RitualOutcomeEffectWorker_FromQuality.ChanceWithQuality(c, quality));
			return RitualOutcomeEffectWorker_FromQuality.ChanceWithQuality(outcome, quality) / num;
		}

		// Token: 0x06009282 RID: 37506 RVA: 0x00357C60 File Offset: 0x00355E60
		protected static float ChanceWithQuality(RitualOutcomePossibility outcome, float quality)
		{
			if (!outcome.Positive)
			{
				return outcome.chance;
			}
			return Mathf.Max(outcome.chance * quality, 0f);
		}

		// Token: 0x06009283 RID: 37507 RVA: 0x00357C84 File Offset: 0x00355E84
		protected virtual void ApplyAttachableOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcomeChance, out string extraLetterText, ref LookTargets letterLookTargets)
		{
			extraLetterText = null;
			if (jobRitual.Ritual.attachableOutcomeEffect != null && jobRitual.Ritual.attachableOutcomeEffect.AppliesToOutcome(jobRitual.Ritual.outcomeEffect.def, outcomeChance))
			{
				jobRitual.Ritual.attachableOutcomeEffect.Worker.Apply(totalPresence, jobRitual, outcomeChance, out extraLetterText, ref letterLookTargets);
			}
		}

		// Token: 0x06009284 RID: 37508 RVA: 0x00357CE4 File Offset: 0x00355EE4
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = this.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			LookTargets lookTargets = jobRitual.selectedTarget;
			string text;
			this.ApplyExtraOutcome(totalPresence, jobRitual, outcome, out text, ref lookTargets);
			string text2 = null;
			if (jobRitual.Ritual != null)
			{
				this.ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out text2, ref lookTargets);
			}
			string text3 = outcome.description.Formatted(jobRitual.Ritual.Label).CapitalizeFirst();
			string text4 = this.def.OutcomeMoodBreakdown(outcome);
			if (!text4.NullOrEmpty())
			{
				text3 = text3 + "\n\n" + text4;
			}
			if (text != null)
			{
				text3 = text3 + "\n\n" + text;
			}
			if (text2 != null)
			{
				text3 = text3 + "\n\n" + text2;
			}
			text3 = text3 + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			string text5;
			this.ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out text5);
			if (text5 != null)
			{
				text3 = text3 + "\n\n" + text5;
			}
			Find.LetterStack.ReceiveLetter("OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL")), text3, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, lookTargets, null, null, null, null, 0, true);
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				if (!outcome.roleIdsNotGainingMemory.NullOrEmpty<string>())
				{
					RitualRole ritualRole = jobRitual.assignments.RoleForPawn(keyValuePair.Key, true);
					if (ritualRole != null && outcome.roleIdsNotGainingMemory.Contains(ritualRole.id))
					{
						continue;
					}
				}
				if (outcome.memory != null)
				{
					this.GiveMemoryToPawn(keyValuePair.Key, outcome.memory, jobRitual);
				}
			}
		}

		// Token: 0x06009285 RID: 37509 RVA: 0x00357ED4 File Offset: 0x003560D4
		protected void GiveMemoryToPawn(Pawn pawn, ThoughtDef memory, LordJob_Ritual jobRitual)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			if (((needs != null) ? needs.mood : null) != null)
			{
				Thought_AttendedRitual newThought = (Thought_AttendedRitual)base.MakeMemory(pawn, jobRitual, memory);
				pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
			}
		}

		// Token: 0x06009286 RID: 37510 RVA: 0x00357F20 File Offset: 0x00356120
		protected virtual void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
		}

		// Token: 0x06009287 RID: 37511 RVA: 0x00357F28 File Offset: 0x00356128
		protected virtual void ApplyDevelopmentPoints(Precept_Ritual ritual, RitualOutcomePossibility outcome, out string extraOutcomeDesc)
		{
			if (((ritual != null) ? ritual.ideo : null) != null && ritual.ideo.Fluid)
			{
				if (ritual.ideo.development.Points == ritual.ideo.development.NextReformationDevelopmentPoints)
				{
					extraOutcomeDesc = "RitualOutcomeExtraDesc_DevelopmentPointsAwardedCapped".Translate(ritual.ideo.development.NextReformationDevelopmentPoints);
					return;
				}
				int num = this.def.outcomeChances.IndexOf(outcome);
				int num2;
				if (num >= 0 && ritual.ideo.development.TryGainDevelopmentPointsForRitualOutcome(ritual, num, out num2))
				{
					if (num2 > 0)
					{
						extraOutcomeDesc = "RitualOutcomeExtraDesc_DevelopmentPointsAwarded".Translate(ritual.ideo.development.Points - num2, ritual.ideo.development.Points, num2.ToStringWithSign());
						return;
					}
					extraOutcomeDesc = "RitualOutcomeExtraDesc_NoDevelopmentPointsAwarded".Translate();
					return;
				}
			}
			extraOutcomeDesc = null;
		}

		// Token: 0x06009288 RID: 37512 RVA: 0x00358030 File Offset: 0x00356230
		protected float GetQuality(LordJob_Ritual jobRitual, float progress)
		{
			float num = this.def.startingQuality;
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality && ritualOutcomeComp.Applies(jobRitual))
				{
					num += ritualOutcomeComp.QualityOffset(jobRitual, base.DataForComp(ritualOutcomeComp));
				}
			}
			if (jobRitual.repeatPenalty && jobRitual.Ritual != null)
			{
				num += jobRitual.Ritual.RepeatQualityPenalty;
			}
			Map map = jobRitual.Map;
			Precept_Ritual ritual = jobRitual.Ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (ritual != null) ? ritual.def : null);
			if (expectationsOffset != null)
			{
				num += expectationsOffset.Item2;
			}
			return Mathf.Clamp(num * Mathf.Lerp(RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min, RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max, progress), this.def.minQuality, this.def.maxQuality);
		}

		// Token: 0x06009289 RID: 37513 RVA: 0x00358130 File Offset: 0x00356330
		public static Tuple<ExpectationDef, float> GetExpectationsOffset(Map map, PreceptDef ritual)
		{
			if (ritual == null || !ritual.receivesExpectationsQualityOffset)
			{
				return null;
			}
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(map);
			if (Math.Abs(expectationDef.ritualQualityOffset) > 1E-45f)
			{
				return new Tuple<ExpectationDef, float>(expectationDef, expectationDef.ritualQualityOffset);
			}
			return null;
		}

		// Token: 0x0600928A RID: 37514 RVA: 0x00358174 File Offset: 0x00356374
		public virtual string OutcomeQualityBreakdownDesc(float quality, float progress, LordJob_Ritual jobRitual)
		{
			TaggedString taggedString = "RitualOutcomeQualitySpecific".Translate(jobRitual.Ritual.Label, quality.ToStringPercent()).CapitalizeFirst() + ":\n";
			if (this.def.startingQuality > 0f)
			{
				taggedString += "\n  - " + "StartingRitualQuality".Translate(this.def.startingQuality.ToStringPercent()) + ".";
			}
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality && ritualOutcomeComp.Applies(jobRitual) && Mathf.Abs(ritualOutcomeComp.QualityOffset(jobRitual, base.DataForComp(ritualOutcomeComp))) >= 1E-45f)
				{
					taggedString += "\n  - " + ritualOutcomeComp.GetDesc(jobRitual, base.DataForComp(ritualOutcomeComp)).CapitalizeFirst();
				}
			}
			if (jobRitual.repeatPenalty && jobRitual.Ritual != null)
			{
				taggedString += "\n  - " + "RitualOutcomePerformedRecently".Translate() + ": " + jobRitual.Ritual.RepeatQualityPenalty.ToStringPercent();
			}
			Map map = jobRitual.Map;
			Precept_Ritual ritual = jobRitual.Ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (ritual != null) ? ritual.def : null);
			if (expectationsOffset != null)
			{
				taggedString += "\n  - " + "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap) + ": " + expectationsOffset.Item2.ToStringPercent();
			}
			if (progress < 1f)
			{
				taggedString += "\n  - " + "RitualOutcomeProgress".Translate(jobRitual.Ritual.Label).CapitalizeFirst() + ": x" + Mathf.Lerp(RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min, RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max, progress).ToStringPercent();
			}
			return taggedString;
		}

		// Token: 0x0600928B RID: 37515 RVA: 0x003583C0 File Offset: 0x003565C0
		protected virtual bool OutcomePossible(RitualOutcomePossibility chance, LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x0600928C RID: 37516 RVA: 0x003583C4 File Offset: 0x003565C4
		public override string ExtraAlertParagraph(Precept_Ritual ritual)
		{
			string text = "";
			foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
			{
				if (ritualOutcomeComp is RitualOutcomeComp_Quality)
				{
					string desc = ritualOutcomeComp.GetDesc(null, null);
					if (!desc.NullOrEmpty())
					{
						text = text + "\n  - " + desc.CapitalizeFirst();
					}
				}
			}
			string text2 = ("RitualOutcomeQualityAbstract".Translate(ritual.Label).Resolve().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor) + text;
			return string.Concat(new string[]
			{
				text2,
				"\n  - ",
				"RitualOutcomeProgress".Translate(ritual.Label).Resolve().CapitalizeFirst(),
				": ",
				"OutcomeBonusDesc_QualitySingleOffset".Translate("x" + (RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.min * 100f).ToString() + "-" + RitualOutcomeEffectWorker_FromQuality.ProgressToQualityMapping.max.ToStringPercent()).Resolve(),
				"."
			});
		}

		// Token: 0x040058C3 RID: 22723
		public static FloatRange ProgressToQualityMapping = new FloatRange(0.25f, 1f);
	}
}
