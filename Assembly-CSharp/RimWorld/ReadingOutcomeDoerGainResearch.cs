using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000EAE RID: 3758
	public class ReadingOutcomeDoerGainResearch : BookOutcomeDoer
	{
		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060062E8 RID: 25320 RVA: 0x00239582 File Offset: 0x00237782
		public new BookOutcomeProperties_GainResearch Props
		{
			get
			{
				return (BookOutcomeProperties_GainResearch)this.props;
			}
		}

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x060062E9 RID: 25321 RVA: 0x0023958F File Offset: 0x0023778F
		public virtual int RoundTo { get; } = 10;

		// Token: 0x060062EA RID: 25322 RVA: 0x00239598 File Offset: 0x00237798
		private bool IsValid(ResearchProjectDef project)
		{
			return project.generalRules != null && (this.Props.exclude.Count <= 0 || !this.Props.exclude.ContainsAny((BookOutcomeProperties_GainResearch.BookResearchItem i) => i.project == project)) && ((!this.Props.ignoreZeroBaseCost || project.baseCost != 0f) && (this.Props.tabs.NullOrEmpty<BookOutcomeProperties_GainResearch.BookTabItem>() || this.Props.tabs.Exists((BookOutcomeProperties_GainResearch.BookTabItem x) => x.tab == project.tab))) && project.TechprintCount == 0;
		}

		// Token: 0x060062EB RID: 25323 RVA: 0x00239654 File Offset: 0x00237854
		public override bool BenefitDetailsCanChange(Pawn reader = null)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				ResearchProjectDef researchProjectDef2 = researchProjectDef;
				if (researchProjectDef2.IsHidden)
				{
					return true;
				}
				if (!researchProjectDef2.IsFinished)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060062EC RID: 25324 RVA: 0x002396CC File Offset: 0x002378CC
		public override void OnBookGenerated(Pawn author = null)
		{
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			List<ResearchProjectDef> list = new List<ResearchProjectDef>(allDefsListForReading.Count);
			if (this.Props.include.Count > 0)
			{
				for (int i = 0; i < this.Props.include.Count; i++)
				{
					ResearchProjectDef project = this.Props.include[i].project;
					if (!project.IsFinished && this.IsValid(project))
					{
						list.Add(project);
					}
				}
			}
			else
			{
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					if (allDefsListForReading[j].PrerequisitesCompleted && !allDefsListForReading[j].IsFinished && this.IsValid(allDefsListForReading[j]))
					{
						list.Add(allDefsListForReading[j]);
					}
				}
			}
			if (list.Count == 0)
			{
				for (int k = 0; k < allDefsListForReading.Count; k++)
				{
					if (this.IsValid(allDefsListForReading[k]))
					{
						list.Add(allDefsListForReading[k]);
					}
				}
			}
			if (list.Empty<ResearchProjectDef>())
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(122, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Research book had no valid projects and failed to find any backup projects.\n");
				defaultInterpolatedStringHandler.AppendLiteral("ignoring zero base cost: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.Props.ignoreZeroBaseCost);
				defaultInterpolatedStringHandler.AppendLiteral(").\n");
				defaultInterpolatedStringHandler.AppendLiteral("tabs: ");
				defaultInterpolatedStringHandler.AppendFormatted((from x in this.Props.tabs
				select x.tab.generalTitle).ToCommaList(false, false));
				defaultInterpolatedStringHandler.AppendLiteral(".\n");
				defaultInterpolatedStringHandler.AppendLiteral("excludes: ");
				string value;
				if (!this.Props.exclude.Empty<BookOutcomeProperties_GainResearch.BookResearchItem>())
				{
					value = (from x in this.Props.exclude
					select x.project.label).ToCommaList(false, false);
				}
				else
				{
					value = "(none)";
				}
				defaultInterpolatedStringHandler.AppendFormatted(value);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 15242436);
				return;
			}
			int count = Rand.Chance(0.25f) ? 2 : 1;
			List<ResearchProjectDef> list2 = list.TakeRandomDistinct(count);
			float num = this.GetBaseValue();
			if (list2.Count > 1)
			{
				num *= 1.25f;
			}
			float value2 = num / (float)list2.Count;
			for (int l = 0; l < list2.Count; l++)
			{
				this.values[list2[l]] = value2;
			}
		}

		// Token: 0x060062ED RID: 25325 RVA: 0x0023995D File Offset: 0x00237B5D
		public override void Reset()
		{
			this.values.Clear();
		}

		// Token: 0x060062EE RID: 25326 RVA: 0x0023996C File Offset: 0x00237B6C
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				if (!researchProjectDef.IsFinished)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060062EF RID: 25327 RVA: 0x002399D4 File Offset: 0x00237BD4
		protected virtual float GetBaseValue()
		{
			return BookUtility.GetResearchExpForQuality(base.Quality);
		}

		// Token: 0x060062F0 RID: 25328 RVA: 0x002399E4 File Offset: 0x00237BE4
		public override void OnReadingTick(Pawn reader, float factor)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				ResearchProjectDef researchProjectDef2 = researchProjectDef;
				float num2 = num;
				if (this.IsProjectVisible(researchProjectDef2) && !researchProjectDef2.IsFinished)
				{
					Find.ResearchManager.AddProgress(researchProjectDef2, num2 * factor, null);
				}
			}
		}

		// Token: 0x060062F1 RID: 25329 RVA: 0x00239A64 File Offset: 0x00237C64
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			ReadingOutcomeDoerGainResearch.<GetHyperlinks>d__14 <GetHyperlinks>d__ = new ReadingOutcomeDoerGainResearch.<GetHyperlinks>d__14(-2);
			<GetHyperlinks>d__.<>4__this = this;
			return <GetHyperlinks>d__;
		}

		// Token: 0x060062F2 RID: 25330 RVA: 0x00239A74 File Offset: 0x00237C74
		public override string GetBenefitsString(Pawn reader = null)
		{
			if (this.values.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				ResearchProjectDef researchProjectDef2 = researchProjectDef;
				float num2 = num * 2500f;
				if (this.RoundTo != 0)
				{
					num2 = Mathf.Round(num2 / (float)this.RoundTo) * (float)this.RoundTo;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(researchProjectDef2.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PerHour".Translate(num2.ToStringDecimalIfSmall()));
				string text = defaultInterpolatedStringHandler.ToStringAndClear();
				if (researchProjectDef2.IsFinished)
				{
					string str = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler2.AppendLiteral(" (");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("AlreadyUnlocked".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(")");
					text = str + defaultInterpolatedStringHandler2.ToStringAndClear();
					text = text.Colorize(Color.gray);
				}
				else if (!this.IsProjectVisible(researchProjectDef2))
				{
					string str2 = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(3, 1);
					defaultInterpolatedStringHandler3.AppendLiteral(" (");
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("WhenDiscovered".Translate());
					defaultInterpolatedStringHandler3.AppendLiteral(")");
					text = str2 + defaultInterpolatedStringHandler3.ToStringAndClear();
					text = text.Colorize(Color.gray);
				}
				stringBuilder.AppendLine(" - " + text);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060062F3 RID: 25331 RVA: 0x00239C3C File Offset: 0x00237E3C
		private bool IsProjectVisible(ResearchProjectDef project)
		{
			return !this.Props.usesHiddenProjects || project.CanStartNow;
		}

		// Token: 0x060062F4 RID: 25332 RVA: 0x00239C53 File Offset: 0x00237E53
		public override IEnumerable<RulePack> GetTopicRulePacks()
		{
			return from x in this.values.Keys
			select x.generalRules;
		}

		// Token: 0x060062F5 RID: 25333 RVA: 0x00239C84 File Offset: 0x00237E84
		public override void PostExposeData()
		{
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.values, "values", LookMode.Undefined, LookMode.Undefined);
		}

		// Token: 0x04003692 RID: 13970
		protected Dictionary<ResearchProjectDef, float> values = new Dictionary<ResearchProjectDef, float>();

		// Token: 0x04003693 RID: 13971
		private const float MultipleProjectFactor = 1.25f;
	}
}
