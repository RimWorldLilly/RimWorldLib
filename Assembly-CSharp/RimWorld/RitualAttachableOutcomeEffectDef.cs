using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001656 RID: 5718
	public class RitualAttachableOutcomeEffectDef : Def
	{
		// Token: 0x170018ED RID: 6381
		// (get) Token: 0x06008FD8 RID: 36824 RVA: 0x0034DDD5 File Offset: 0x0034BFD5
		public RitualAttachableOutcomeEffectWorker Worker
		{
			get
			{
				if (this.workerInstance == null)
				{
					this.workerInstance = (RitualAttachableOutcomeEffectWorker)Activator.CreateInstance(this.workerClass);
					this.workerInstance.def = this;
				}
				return this.workerInstance;
			}
		}

		// Token: 0x170018EE RID: 6382
		// (get) Token: 0x06008FD9 RID: 36825 RVA: 0x0034DE07 File Offset: 0x0034C007
		public bool AppliesToAllOutcomes
		{
			get
			{
				return !this.onlyBestOutcome && !this.onlyPositiveOutcomes && !this.onlyNegativeOutcomes && !this.onlyWorstOutcome;
			}
		}

		// Token: 0x06008FDA RID: 36826 RVA: 0x0034DE2C File Offset: 0x0034C02C
		public bool AppliesToSeveralOutcomes(Precept_Ritual ritual)
		{
			bool flag = false;
			foreach (RitualOutcomePossibility outcomeChance in ritual.outcomeEffect.def.outcomeChances)
			{
				if (this.AppliesToOutcome(ritual.outcomeEffect.def, outcomeChance))
				{
					if (flag)
					{
						return true;
					}
					flag = true;
				}
			}
			return false;
		}

		// Token: 0x170018EF RID: 6383
		// (get) Token: 0x06008FDB RID: 36827 RVA: 0x0034DEA4 File Offset: 0x0034C0A4
		private string RequiredMemeList
		{
			get
			{
				if (this.requiredMemeList == null && !this.requiredMemeAny.NullOrEmpty<MemeDef>())
				{
					this.requiredMemeList = (from m in this.requiredMemeAny
					select m.label.ResolveTags()).ToCommaList(false, false);
				}
				return this.requiredMemeList;
			}
		}

		// Token: 0x06008FDC RID: 36828 RVA: 0x0034DF04 File Offset: 0x0034C104
		public AcceptanceReport CanAttachToRitual(Precept_Ritual ritual)
		{
			if (!this.requiredMemeAny.NullOrEmpty<MemeDef>() && !ritual.ideo.memes.SharesElementWith(this.requiredMemeAny))
			{
				return "RitualAttachedRewardRequiredMemeAny".Translate() + ": " + this.RequiredMemeList.CapitalizeFirst();
			}
			if ((!this.allowedRituals.NullOrEmpty<RitualPatternDef>() && !this.allowedRituals.Contains(ritual.sourcePattern)) || (!this.disallowedRituals.NullOrEmpty<RitualPatternDef>() && this.disallowedRituals.Contains(ritual.sourcePattern)))
			{
				return "RitualAttachedRewardCantUseWithRitual".Translate(this.LabelCap, ritual.LabelCap);
			}
			if (this.requiredFaction != null && Find.FactionManager.FirstFactionOfDef(this.requiredFaction) == null)
			{
				return "RitualAttachedRewardRequiredFaction".Translate(this.requiredFaction.LabelCap);
			}
			return true;
		}

		// Token: 0x06008FDD RID: 36829 RVA: 0x0034E008 File Offset: 0x0034C208
		public bool AppliesToOutcome(RitualOutcomeEffectDef effectDef, RitualOutcomePossibility outcomeChance)
		{
			return ModLister.CheckIdeology("Attachable ritual reward") && (!this.onlyPositiveOutcomes || outcomeChance.Positive) && (!this.onlyNegativeOutcomes || !outcomeChance.Positive) && (!this.onlyBestOutcome || outcomeChance == effectDef.BestOutcome) && (!this.onlyWorstOutcome || outcomeChance == effectDef.WorstOutcome);
		}

		// Token: 0x06008FDE RID: 36830 RVA: 0x0034E070 File Offset: 0x0034C270
		public string DescriptionForRitualValidated(Precept_Ritual ritual)
		{
			if (!this.AppliesToAllOutcomes)
			{
				return "RitualAttachedOutcomesInfo".Translate(ritual.shortDescOverride ?? ritual.def.label, this.AppliesToOutcomesListString(ritual.outcomeEffect.def)) + ", " + this.effectDesc;
			}
			return this.description.CapitalizeFirst();
		}

		// Token: 0x06008FDF RID: 36831 RVA: 0x0034E0E8 File Offset: 0x0034C2E8
		public string DescriptionForRitualValidated(Precept_Ritual ritual, Map map)
		{
			string text = this.DescriptionForRitualValidated(ritual);
			AcceptanceReport report = this.Worker.CanApplyNow(ritual, map);
			if (!report)
			{
				text += " (" + "RitualAttachedOutcomeCantApply".Translate() + ": " + report.Reason + ")";
				text = text.Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}

		// Token: 0x06008FE0 RID: 36832 RVA: 0x0034E160 File Offset: 0x0034C360
		public string TooltipForRitual(Precept_Ritual ritual)
		{
			string text = this.DescriptionForRitualValidated(ritual).ResolveTags();
			if (!this.requiredMemeAny.NullOrEmpty<MemeDef>())
			{
				text = text + "\n\n" + ("UnlockedByMeme".Translate().Resolve() + ":\n").Colorize(ColoredText.TipSectionTitleColor);
				foreach (MemeDef memeDef in this.requiredMemeAny)
				{
					text = text + "  - " + memeDef.LabelCap.Resolve() + "\n";
				}
			}
			return text;
		}

		// Token: 0x06008FE1 RID: 36833 RVA: 0x0034E21C File Offset: 0x0034C41C
		public string AppliesToOutcomesListString(RitualOutcomeEffectDef effectDef)
		{
			this.appliesToOutcomeListTmp.Clear();
			foreach (RitualOutcomePossibility ritualOutcomePossibility in effectDef.outcomeChances)
			{
				if (this.AppliesToOutcome(effectDef, ritualOutcomePossibility))
				{
					this.appliesToOutcomeListTmp.Add(ritualOutcomePossibility.label.ToLower());
				}
			}
			return this.appliesToOutcomeListTmp.ToCommaListOr(false);
		}

		// Token: 0x06008FE2 RID: 36834 RVA: 0x0034E2A0 File Offset: 0x0034C4A0
		public string AppliesToOutcomesString(RitualOutcomeEffectDef effectDef)
		{
			if (this.onlyPositiveOutcomes)
			{
				return "RitualAttachedOutcomes_Positive".Translate();
			}
			if (this.onlyNegativeOutcomes)
			{
				return "RitualAttachedOutcomes_Negative".Translate();
			}
			if (this.onlyBestOutcome)
			{
				return effectDef.outcomeChances.MaxBy((RitualOutcomePossibility o) => o.positivityIndex).label;
			}
			if (this.onlyWorstOutcome)
			{
				return effectDef.outcomeChances.MinBy((RitualOutcomePossibility o) => o.positivityIndex).label;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06008FE3 RID: 36835 RVA: 0x0034E352 File Offset: 0x0034C552
		public override IEnumerable<string> ConfigErrors()
		{
			RitualAttachableOutcomeEffectDef.<ConfigErrors>d__28 <ConfigErrors>d__ = new RitualAttachableOutcomeEffectDef.<ConfigErrors>d__28(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040057D7 RID: 22487
		public Type workerClass = typeof(RitualAttachableOutcomeEffectWorker);

		// Token: 0x040057D8 RID: 22488
		public List<MemeDef> requiredMemeAny;

		// Token: 0x040057D9 RID: 22489
		public List<RitualPatternDef> allowedRituals;

		// Token: 0x040057DA RID: 22490
		public List<RitualPatternDef> disallowedRituals;

		// Token: 0x040057DB RID: 22491
		public FactionDef requiredFaction;

		// Token: 0x040057DC RID: 22492
		public bool onlyPositiveOutcomes = true;

		// Token: 0x040057DD RID: 22493
		public bool onlyBestOutcome;

		// Token: 0x040057DE RID: 22494
		public bool onlyNegativeOutcomes;

		// Token: 0x040057DF RID: 22495
		public bool onlyWorstOutcome;

		// Token: 0x040057E0 RID: 22496
		[MustTranslate]
		public string letterInfoText;

		// Token: 0x040057E1 RID: 22497
		[MustTranslate]
		public string effectDesc;

		// Token: 0x040057E2 RID: 22498
		private RitualAttachableOutcomeEffectWorker workerInstance;

		// Token: 0x040057E3 RID: 22499
		private string requiredMemeList;

		// Token: 0x040057E4 RID: 22500
		private List<string> appliesToOutcomeListTmp = new List<string>();
	}
}
