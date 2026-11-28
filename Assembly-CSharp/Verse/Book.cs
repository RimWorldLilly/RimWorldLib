using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020000D1 RID: 209
	public class Book : ThingWithComps
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002E398 File Offset: 0x0002C598
		public CompBook BookComp
		{
			get
			{
				CompBook result;
				if ((result = this.cachedComp) == null)
				{
					result = (this.cachedComp = base.GetComp<CompBook>());
				}
				return result;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x0002E3C0 File Offset: 0x0002C5C0
		private Graphic OpenGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.openGraphic) == null)
				{
					result = (this.openGraphic = this.BookComp.Props.openGraphic.Graphic);
				}
				return result;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0002E3F8 File Offset: 0x0002C5F8
		public Graphic VerticalGraphic
		{
			get
			{
				Graphic result;
				if ((result = this.verticalGraphic) == null)
				{
					result = (this.verticalGraphic = this.BookComp.Props.verticalGraphic.Graphic);
				}
				return result;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0002E42D File Offset: 0x0002C62D
		public override string LabelNoCount
		{
			get
			{
				return this.title + GenLabel.LabelExtras(this, true, true);
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0002E442 File Offset: 0x0002C642
		public override string LabelNoParenthesis
		{
			get
			{
				return this.title;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0002E44A File Offset: 0x0002C64A
		public string FlavorUI
		{
			get
			{
				return this.descriptionFlavor;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0002E452 File Offset: 0x0002C652
		public float MentalBreakChancePerHour
		{
			get
			{
				return this.mentalBreakChancePerHour;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0002E45A File Offset: 0x0002C65A
		public float JoyFactor
		{
			get
			{
				return this.joyFactor;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0002E462 File Offset: 0x0002C662
		public string Title
		{
			get
			{
				return this.title;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0002E46A File Offset: 0x0002C66A
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x0002E472 File Offset: 0x0002C672
		public bool IsOpen
		{
			get
			{
				return this.isOpen;
			}
			set
			{
				this.isOpen = value;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0002E47B File Offset: 0x0002C67B
		public virtual bool IsReadable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0002E47E File Offset: 0x0002C67E
		public override string DescriptionFlavor
		{
			get
			{
				return this.DescriptionDetailed;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x0002E486 File Offset: 0x0002C686
		public override string DescriptionDetailed
		{
			get
			{
				this.EnsureDescriptionUpToDate();
				return this.description;
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0002E494 File Offset: 0x0002C694
		public override void PostPostMake()
		{
			base.PostPostMake();
			if (!this.HasComp<CompQuality>())
			{
				this.GenerateBook(null, null);
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002E4BF File Offset: 0x0002C6BF
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Books, OpportunityType.GoodToKnow);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0002E4D4 File Offset: 0x0002C6D4
		public override bool CanStackWith(Thing other)
		{
			return false;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0002E4D8 File Offset: 0x0002C6D8
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.isOpen)
			{
				Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
				Rot4 rotation;
				if (pawn_CarryTracker != null)
				{
					rotation = pawn_CarryTracker.pawn.Rotation;
				}
				else
				{
					rotation = base.Rotation;
				}
				this.OpenGraphic.Draw(drawLoc, flip ? rotation.Opposite : rotation, this, 0f);
				return;
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002E53C File Offset: 0x0002C73C
		public void PawnReadNow(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Reading, this);
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0002E570 File Offset: 0x0002C770
		public override void PostQualitySet()
		{
			base.PostQualitySet();
			this.GenerateBook(null, null);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002E594 File Offset: 0x0002C794
		public void OnBookReadTick(Pawn pawn, int delta, float roomBonusFactor)
		{
			float factor = pawn.GetStatValue(StatDefOf.ReadingSpeed, true, -1) * roomBonusFactor * (float)delta;
			foreach (BookOutcomeDoer bookOutcomeDoer in this.BookComp.Doers)
			{
				bookOutcomeDoer.OnReadingTick(pawn, factor);
			}
			MentalBreakDef breakDef;
			if (ModsConfig.AnomalyActive && this.MentalBreakChancePerHour > 0f && Rand.MTBEventOccurs(1f / this.MentalBreakChancePerHour, 2500f, (float)delta) && pawn.mindState.mentalBreaker.TryGetRandomMentalBreak(this.BookComp.Props.mentalBreakIntensity, out breakDef))
			{
				TaggedString taggedString = "BookMentalBreakMessage".Translate(this.Label);
				pawn.mindState.mentalBreaker.TryDoMentalBreak(taggedString, breakDef);
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002E678 File Offset: 0x0002C878
		public bool ProvidesOutcome(Pawn reader)
		{
			using (IEnumerator<BookOutcomeDoer> enumerator = this.BookComp.Doers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.DoesProvidesOutcome(reader))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002E6D4 File Offset: 0x0002C8D4
		public void SetMentalBreakChance(float chance)
		{
			this.mentalBreakChancePerHour = Mathf.Clamp01(chance);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002E6E2 File Offset: 0x0002C8E2
		public void SetJoyFactor(float factor)
		{
			this.joyFactor = factor;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0002E6EB File Offset: 0x0002C8EB
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			Book.<GetHyperlinks>d__49 <GetHyperlinks>d__ = new Book.<GetHyperlinks>d__49(-2);
			<GetHyperlinks>d__.<>4__this = this;
			return <GetHyperlinks>d__;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002E6FB File Offset: 0x0002C8FB
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Book.<GetFloatMenuOptions>d__50 <GetFloatMenuOptions>d__ = new Book.<GetFloatMenuOptions>d__50(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002E712 File Offset: 0x0002C912
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Book.<SpecialDisplayStats>d__51 <SpecialDisplayStats>d__ = new Book.<SpecialDisplayStats>d__51(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002E724 File Offset: 0x0002C924
		public virtual void GenerateBook(Pawn author = null, long? fixedDate = null)
		{
			int thingIDNumber = this.thingIDNumber;
			int num = this.generations;
			this.generations = num + 1;
			using (Rand.Block(Gen.HashCombineInt(thingIDNumber, num)))
			{
				this.mentalBreakChancePerHour = 0f;
				Book.subjects.Clear();
				GrammarRequest grammarRequest = default(GrammarRequest);
				long absTicks = fixedDate ?? ((long)GenTicks.TicksAbs - (long)(this.BookComp.Props.ageYearsRange.RandomInRange * 3600000f));
				grammarRequest.Rules.Add(new Rule_String("date", GenDate.DateFullStringAt(absTicks, Vector2.zero)));
				grammarRequest.Rules.Add(new Rule_String("date_season", GenDate.DateMonthYearStringAt(absTicks, Vector2.zero)));
				if (this.HasComp<CompQuality>())
				{
					grammarRequest.Constants.Add("quality", ((int)base.GetComp<CompQuality>().Quality).ToString());
				}
				foreach (Rule item in ((author == null) ? TaleData_Pawn.GenerateRandom(true) : TaleData_Pawn.GenerateFrom(author)).GetRules("ANYPAWN", grammarRequest.Constants))
				{
					grammarRequest.Rules.Add(item);
				}
				this.AppendDoerRules(author, ref grammarRequest);
				Book.AppendRulesForSubject(Book.subjects, grammarRequest.Rules, grammarRequest.Constants, "primary", 0);
				Book.AppendRulesForSubject(Book.subjects, grammarRequest.Rules, grammarRequest.Constants, "secondary", 1);
				Book.AppendRulesForSubject(Book.subjects, grammarRequest.Rules, grammarRequest.Constants, "tertiary", 2);
				GrammarRequest request = grammarRequest;
				request.Includes.Add(this.BookComp.Props.nameMaker);
				this.title = GenText.CapitalizeAsTitle(GrammarResolver.Resolve("title", request, null, false, null, null, null, true)).StripTags();
				GrammarRequest request2 = grammarRequest;
				request2.Includes.Add(this.BookComp.Props.descriptionMaker);
				request2.Includes.Add(RulePackDefOf.TalelessImages);
				request2.Includes.Add(RulePackDefOf.ArtDescriptionRoot_Taleless);
				request2.Includes.Add(RulePackDefOf.ArtDescriptionUtility_Global);
				this.descriptionFlavor = GrammarResolver.Resolve("desc", request2, null, false, null, null, null, true).StripTags();
				this.description = this.GenerateFullDescription();
				Book.subjects.Clear();
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0002E9E0 File Offset: 0x0002CBE0
		private void AppendDoerRules(Pawn author, ref GrammarRequest common)
		{
			foreach (BookOutcomeDoer bookOutcomeDoer in this.BookComp.Doers)
			{
				bookOutcomeDoer.Reset();
				bookOutcomeDoer.OnBookGenerated(author);
				IEnumerable<RulePack> topicRulePacks = bookOutcomeDoer.GetTopicRulePacks();
				if (topicRulePacks != null)
				{
					foreach (RulePack rulePack in topicRulePacks)
					{
						common.IncludesBare.Add(rulePack);
						List<ValueTuple<string, string>> list = new List<ValueTuple<string, string>>();
						foreach (Rule rule in rulePack.Rules)
						{
							if (rule.keyword.StartsWith("subject_"))
							{
								list.Add(new ValueTuple<string, string>(rule.keyword.Substring("subject_".Length), GrammarResolver.Resolve(rule.keyword, common, null, false, null, null, null, false)));
							}
						}
						Book.subjects.Add(new Book.BookSubjectSymbol
						{
							keyword = GrammarResolver.Resolve("subject", common, null, false, null, null, null, false),
							subSymbols = list
						});
					}
				}
				IEnumerable<Rule_String> topicRuleStrings = bookOutcomeDoer.GetTopicRuleStrings();
				if (topicRuleStrings != null)
				{
					foreach (Rule_String item in topicRuleStrings)
					{
						common.Rules.Add(item);
					}
				}
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002EBE0 File Offset: 0x0002CDE0
		private void EnsureDescriptionUpToDate()
		{
			if (this.descCanBeInvalidated)
			{
				this.description = this.GenerateFullDescription();
			}
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002EBF8 File Offset: 0x0002CDF8
		private string GenerateFullDescription()
		{
			string result;
			using (Rand.Block(Gen.HashCombineInt(this.thingIDNumber, this.generations)))
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.descCanBeInvalidated = false;
				stringBuilder.AppendLine(this.title.Colorize(ColoredText.TipSectionTitleColor) + GenLabel.LabelExtras(this, false, true) + "\n");
				stringBuilder.AppendLine(this.descriptionFlavor + "\n");
				if (this.MentalBreakChancePerHour > 0f)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 2);
					defaultInterpolatedStringHandler.AppendLiteral(" - ");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("BookMentalBreak".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PerHour".Translate(this.MentalBreakChancePerHour.ToStringPercent("0.0")));
					stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				foreach (BookOutcomeDoer bookOutcomeDoer in this.BookComp.Doers)
				{
					string benefitsString = bookOutcomeDoer.GetBenefitsString(null);
					if (!string.IsNullOrEmpty(benefitsString))
					{
						if (bookOutcomeDoer.BenefitDetailsCanChange(null))
						{
							this.descCanBeInvalidated = true;
						}
						stringBuilder.AppendLine(benefitsString);
					}
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			return result;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0002ED8C File Offset: 0x0002CF8C
		private static void AppendRulesForSubject(List<Book.BookSubjectSymbol> subjects, List<Rule> rules, Dictionary<string, string> constants, string postfix, int i)
		{
			if (i < subjects.Count)
			{
				rules.Add(new Rule_String("subject_" + postfix, subjects[i].keyword));
				constants.Add("length_subject_" + postfix, subjects[i].keyword.Length.ToString());
				constants.Add("has_subject_" + postfix, "true");
				using (List<ValueTuple<string, string>>.Enumerator enumerator = subjects[i].subSymbols.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ValueTuple<string, string> valueTuple = enumerator.Current;
						rules.Add(new Rule_String("subject_" + postfix + "_" + valueTuple.Item1, valueTuple.Item2));
					}
					return;
				}
			}
			constants.Add("has_subject_" + postfix, "false");
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0002EE90 File Offset: 0x0002D090
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<string>(ref this.descriptionFlavor, "descriptionFlavor", null, false);
			Scribe_Values.Look<float>(ref this.mentalBreakChancePerHour, "mentalBreakChancePerHour", 0f, false);
			Scribe_Values.Look<float>(ref this.joyFactor, "joyFactor", 0f, false);
			Scribe_Values.Look<bool>(ref this.isOpen, "isOpen", false, false);
			Scribe_Values.Look<bool>(ref this.descCanBeInvalidated, "descCanBeInvalidated", false, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<int>(ref this.generations, "generations", 0, false);
		}

		// Token: 0x0400039D RID: 925
		private string title;

		// Token: 0x0400039E RID: 926
		private bool isOpen;

		// Token: 0x0400039F RID: 927
		private bool descCanBeInvalidated;

		// Token: 0x040003A0 RID: 928
		private float mentalBreakChancePerHour;

		// Token: 0x040003A1 RID: 929
		private float joyFactor = 1f;

		// Token: 0x040003A2 RID: 930
		private string descriptionFlavor;

		// Token: 0x040003A3 RID: 931
		private string description;

		// Token: 0x040003A4 RID: 932
		private int generations;

		// Token: 0x040003A5 RID: 933
		private Graphic openGraphic;

		// Token: 0x040003A6 RID: 934
		private Graphic verticalGraphic;

		// Token: 0x040003A7 RID: 935
		private CompBook cachedComp;

		// Token: 0x040003A8 RID: 936
		private static List<Book.BookSubjectSymbol> subjects = new List<Book.BookSubjectSymbol>();

		// Token: 0x020024FC RID: 9468
		private struct BookSubjectSymbol
		{
			// Token: 0x04009745 RID: 38725
			public string keyword;

			// Token: 0x04009746 RID: 38726
			public List<ValueTuple<string, string>> subSymbols;
		}
	}
}
