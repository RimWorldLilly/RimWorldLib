using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B8 RID: 184
	public class GeneDef : Def, IRenderNodePropertiesParent
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0002A1FC File Offset: 0x000283FC
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					if (this.iconPath.NullOrEmpty())
					{
						this.cachedIcon = BaseContent.BadTex;
					}
					else
					{
						this.cachedIcon = (ContentFinder<Texture2D>.Get(this.iconPath, true) ?? BaseContent.BadTex);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x0002A254 File Offset: 0x00028454
		public Color IconColor
		{
			get
			{
				if (this.iconColor != null)
				{
					return this.iconColor.Value;
				}
				if (this.skinColorBase != null)
				{
					return this.skinColorBase.Value;
				}
				if (this.skinColorOverride != null)
				{
					return this.skinColorOverride.Value;
				}
				if (this.hairColorOverride != null)
				{
					return this.hairColorOverride.Value;
				}
				return Color.white;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0002A2CA File Offset: 0x000284CA
		public string LabelShortAdj
		{
			get
			{
				if (!this.labelShortAdj.NullOrEmpty())
				{
					return this.labelShortAdj;
				}
				return this.label;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x0002A2E8 File Offset: 0x000284E8
		public string DescriptionFull
		{
			get
			{
				string result;
				if ((result = this.cachedDescription) == null)
				{
					result = (this.cachedDescription = this.GetDescriptionFull());
				}
				return result;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0002A30E File Offset: 0x0002850E
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>();
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0002A31E File Offset: 0x0002851E
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0002A328 File Offset: 0x00028528
		public bool RandomChosen
		{
			get
			{
				return this.randomChosen || (this.biostatArc <= 0 && this.biostatCpx == 0 && this.biostatMet == 0 && (this.hairColorOverride != null || this.skinColorBase != null || this.skinColorOverride != null || this.bodyType != null || !this.forcedHeadTypes.NullOrEmpty<HeadTypeDef>() || this.hairTagFilter != null || this.beardTagFilter != null || this.HasDefinedGraphicProperties));
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0002A3B4 File Offset: 0x000285B4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.displayCategory == null)
			{
				this.displayCategory = GeneCategoryDefOf.Miscellaneous;
			}
			if (this.renderNodeProperties != null)
			{
				for (int i = 0; i < this.renderNodeProperties.Count; i++)
				{
					this.renderNodeProperties[i].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0002A40C File Offset: 0x0002860C
		public int AptitudeFor(SkillDef skill)
		{
			int num = 0;
			if (this.aptitudes.NullOrEmpty<Aptitude>())
			{
				return num;
			}
			for (int i = 0; i < this.aptitudes.Count; i++)
			{
				if (this.aptitudes[i].skill == skill)
				{
					num += this.aptitudes[i].level;
				}
			}
			return num;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0002A46C File Offset: 0x0002866C
		private string GetDescriptionFull()
		{
			GeneDef.<>c__DisplayClass107_0 CS$<>8__locals1 = new GeneDef.<>c__DisplayClass107_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.sb = new StringBuilder();
			if (!this.description.NullOrEmpty())
			{
				CS$<>8__locals1.sb.Append(this.description).AppendLine().AppendLine();
			}
			bool flag = false;
			if (this.prerequisite != null)
			{
				CS$<>8__locals1.sb.AppendLine("Requires".Translate() + ": " + this.prerequisite.LabelCap);
				flag = true;
			}
			if (this.minAgeActive > 0f)
			{
				CS$<>8__locals1.sb.AppendLine("TakesEffectAfterAge".Translate() + ": " + this.minAgeActive.ToString());
				flag = true;
			}
			if (flag)
			{
				CS$<>8__locals1.sb.AppendLine();
			}
			bool flag2 = false;
			if (this.biostatCpx != 0)
			{
				CS$<>8__locals1.sb.AppendLineTagged("Complexity".Translate().Colorize(GeneUtility.GCXColor) + ": " + this.biostatCpx.ToStringWithSign());
				flag2 = true;
			}
			if (this.biostatMet != 0)
			{
				CS$<>8__locals1.sb.AppendLineTagged("Metabolism".Translate().CapitalizeFirst().Colorize(GeneUtility.METColor) + ": " + this.biostatMet.ToStringWithSign());
				flag2 = true;
			}
			if (this.biostatArc != 0)
			{
				CS$<>8__locals1.sb.AppendLineTagged("ArchitesRequired".Translate().Colorize(GeneUtility.ARCColor) + ": " + this.biostatArc.ToStringWithSign());
				flag2 = true;
			}
			if (flag2)
			{
				CS$<>8__locals1.sb.AppendLine();
			}
			if (this.forcedTraits != null)
			{
				CS$<>8__locals1.sb.AppendLineTagged(("ForcedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.Append((from x in this.forcedTraits
				select x.def.DataAtDegree(x.degree).label).ToLineList("  - ", true)).AppendLine().AppendLine();
			}
			if (this.suppressedTraits != null)
			{
				CS$<>8__locals1.sb.AppendLineTagged(("SuppressedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.Append((from x in this.suppressedTraits
				select x.def.DataAtDegree(x.degree).label).ToLineList("  - ", true)).AppendLine().AppendLine();
			}
			if (this.aptitudes != null)
			{
				CS$<>8__locals1.sb.AppendLineTagged(("Aptitudes".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.Append((from x in this.aptitudes
				select x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", true)).AppendLine().AppendLine();
			}
			CS$<>8__locals1.effectsTitleWritten = false;
			if (this.passionMod != null)
			{
				PassionMod.PassionModType modType = this.passionMod.modType;
				if (modType != PassionMod.PassionModType.AddOneLevel)
				{
					if (modType == PassionMod.PassionModType.DropAll)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("PassionModDrop".Translate(this.passionMod.skill));
					}
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("PassionModAdd".Translate(this.passionMod.skill));
				}
			}
			if (!this.statFactors.NullOrEmpty<StatModifier>())
			{
				for (int i = 0; i < this.statFactors.Count; i++)
				{
					StatModifier statModifier = this.statFactors[i];
					if (statModifier.stat.CanShowWithLoadedMods())
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier.stat.LabelCap + " " + statModifier.ToStringAsFactor);
					}
				}
			}
			if (!this.conditionalStatAffecters.NullOrEmpty<ConditionalStatAffecter>())
			{
				for (int j = 0; j < this.conditionalStatAffecters.Count; j++)
				{
					if (!this.conditionalStatAffecters[j].statFactors.NullOrEmpty<StatModifier>())
					{
						for (int k = 0; k < this.conditionalStatAffecters[j].statFactors.Count; k++)
						{
							StatModifier statModifier2 = this.conditionalStatAffecters[j].statFactors[k];
							if (statModifier2.stat.CanShowWithLoadedMods())
							{
								CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier2.stat.LabelCap + " " + statModifier2.ToStringAsFactor + " (" + this.conditionalStatAffecters[j].Label + ")");
							}
						}
					}
				}
			}
			if (!this.statOffsets.NullOrEmpty<StatModifier>())
			{
				for (int l = 0; l < this.statOffsets.Count; l++)
				{
					StatModifier statModifier3 = this.statOffsets[l];
					if (statModifier3.stat.CanShowWithLoadedMods())
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier3.stat.LabelCap + " " + statModifier3.ValueToStringAsOffset);
					}
				}
			}
			if (!this.conditionalStatAffecters.NullOrEmpty<ConditionalStatAffecter>())
			{
				for (int m = 0; m < this.conditionalStatAffecters.Count; m++)
				{
					if (!this.conditionalStatAffecters[m].statOffsets.NullOrEmpty<StatModifier>())
					{
						for (int n = 0; n < this.conditionalStatAffecters[m].statOffsets.Count; n++)
						{
							StatModifier statModifier4 = this.conditionalStatAffecters[m].statOffsets[n];
							if (statModifier4.stat.CanShowWithLoadedMods())
							{
								CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(statModifier4.stat.LabelCap + " " + statModifier4.ValueToStringAsOffset + " (" + this.conditionalStatAffecters[m].Label.UncapitalizeFirst() + ")");
							}
						}
					}
				}
			}
			if (!this.capMods.NullOrEmpty<PawnCapacityModifier>())
			{
				for (int num = 0; num < this.capMods.Count; num++)
				{
					PawnCapacityModifier pawnCapacityModifier = this.capMods[num];
					if (pawnCapacityModifier.offset != 0f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(pawnCapacityModifier.capacity.GetLabelFor().CapitalizeFirst() + " " + (pawnCapacityModifier.offset * 100f).ToString("+#;-#") + "%");
					}
					if (pawnCapacityModifier.postFactor != 1f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(pawnCapacityModifier.capacity.GetLabelFor().CapitalizeFirst() + " x" + pawnCapacityModifier.postFactor.ToStringPercent());
					}
					if (pawnCapacityModifier.setMax != 999f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(pawnCapacityModifier.capacity.GetLabelFor().CapitalizeFirst() + " " + "max".Translate().CapitalizeFirst() + ": " + pawnCapacityModifier.setMax.ToStringPercent());
					}
				}
			}
			if (!this.customEffectDescriptions.NullOrEmpty<string>())
			{
				foreach (string str in this.customEffectDescriptions)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3(str.ResolveTags());
				}
			}
			if (!this.damageFactors.NullOrEmpty<DamageFactor>())
			{
				for (int num2 = 0; num2 < this.damageFactors.Count; num2++)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("DamageType".Translate(this.damageFactors[num2].damageDef.label).CapitalizeFirst() + " x" + this.damageFactors[num2].factor.ToStringPercent());
				}
			}
			if (this.resourceLossPerDay != 0f && !this.resourceLabel.NullOrEmpty())
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("ResourceLossPerDay".Translate(this.resourceLabel.Named("RESOURCE"), (-Mathf.RoundToInt(this.resourceLossPerDay * 100f)).ToStringWithSign().Named("OFFSET")).CapitalizeFirst());
			}
			if (!Mathf.Approximately(this.painFactor, 1f))
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("Pain".Translate() + " x" + this.painFactor.ToStringPercent());
			}
			if (this.painOffset != 0f)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("Pain".Translate() + " " + (this.painOffset * 100f).ToString("+###0;-###0") + "%");
			}
			if (this.chemical != null)
			{
				if (!Mathf.Approximately(this.addictionChanceFactor, 1f))
				{
					if (this.addictionChanceFactor <= 0f)
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AddictionImmune".Translate(this.chemical).CapitalizeFirst());
					}
					else
					{
						CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AddictionChanceFactor".Translate(this.chemical).CapitalizeFirst() + " x" + this.addictionChanceFactor.ToStringPercent());
					}
				}
				if (this.overdoseChanceFactor != 1f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("OverdoseChanceFactor".Translate(this.chemical).CapitalizeFirst() + " x" + this.overdoseChanceFactor.ToStringPercent());
				}
				if (this.toleranceBuildupFactor != 1f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("ToleranceBuildupFactor".Translate(this.chemical).CapitalizeFirst() + " x" + this.toleranceBuildupFactor.ToStringPercent());
				}
			}
			if (!this.enablesNeeds.NullOrEmpty<NeedDef>())
			{
				if (this.enablesNeeds.Count == 1)
				{
					GeneDef.<>c__DisplayClass107_0 CS$<>8__locals2 = CS$<>8__locals1;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("AddsNeed".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(this.enablesNeeds[0].LabelCap);
					CS$<>8__locals2.<GetDescriptionFull>g__AppendEffectLine|3(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					GeneDef.<>c__DisplayClass107_0 CS$<>8__locals3 = CS$<>8__locals1;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("AddsNeeds".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted((from x in this.enablesNeeds
					select x.label).ToCommaList(false, false).CapitalizeFirst());
					CS$<>8__locals3.<GetDescriptionFull>g__AppendEffectLine|3(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			if (!this.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				if (this.disablesNeeds.Count == 1)
				{
					GeneDef.<>c__DisplayClass107_0 CS$<>8__locals4 = CS$<>8__locals1;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("DisablesNeed".Translate());
					defaultInterpolatedStringHandler3.AppendLiteral(": ");
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>(this.disablesNeeds[0].LabelCap);
					CS$<>8__locals4.<GetDescriptionFull>g__AppendEffectLine|3(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				else
				{
					GeneDef.<>c__DisplayClass107_0 CS$<>8__locals5 = CS$<>8__locals1;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler4.AppendFormatted<TaggedString>("DisablesNeeds".Translate());
					defaultInterpolatedStringHandler4.AppendLiteral(": ");
					defaultInterpolatedStringHandler4.AppendFormatted((from x in this.disablesNeeds
					select x.label).ToCommaList(false, false).CapitalizeFirst());
					CS$<>8__locals5.<GetDescriptionFull>g__AppendEffectLine|3(defaultInterpolatedStringHandler4.ToStringAndClear());
				}
			}
			if (this.missingGeneRomanceChanceFactor != 1f)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("MissingGeneRomanceChance".Translate(this.label.Named("GENE")) + " x" + this.missingGeneRomanceChanceFactor.ToStringPercent());
			}
			if (this.ignoreDarkness)
			{
				CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("UnaffectedByDarkness".Translate());
			}
			if (this.foodPoisoningChanceFactor != 1f)
			{
				if (this.foodPoisoningChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("FoodPoisoningImmune".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("Stat_Hediff_FoodPoisoningChanceFactor_Name".Translate() + " x" + this.foodPoisoningChanceFactor.ToStringPercent());
				}
			}
			if (this.socialFightChanceFactor != 1f)
			{
				if (this.socialFightChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("WillNeverSocialFight".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("SocialFightChanceFactor".Translate() + " x" + this.socialFightChanceFactor.ToStringPercent());
				}
			}
			if (this.aggroMentalBreakSelectionChanceFactor != 1f)
			{
				if (this.aggroMentalBreakSelectionChanceFactor >= 999f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AlwaysAggroMentalBreak".Translate());
				}
				else if (this.aggroMentalBreakSelectionChanceFactor <= 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("NeverAggroMentalBreak".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("AggroMentalBreakSelectionChanceFactor".Translate() + " x" + this.aggroMentalBreakSelectionChanceFactor.ToStringPercent());
				}
			}
			if (this.prisonBreakMTBFactor != 1f)
			{
				if (this.prisonBreakMTBFactor < 0f)
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("WillNeverPrisonBreak".Translate());
				}
				else
				{
					CS$<>8__locals1.<GetDescriptionFull>g__AppendEffectLine|3("PrisonBreakIntervalFactor".Translate() + " x" + this.prisonBreakMTBFactor.ToStringPercent());
				}
			}
			bool flag3 = CS$<>8__locals1.effectsTitleWritten;
			if (!this.makeImmuneTo.NullOrEmpty<HediffDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine((from x in this.makeImmuneTo
				select x.label).ToLineList("  - ", true));
				flag3 = true;
			}
			if (!this.hediffGiversCannotGive.NullOrEmpty<HediffDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine((from x in this.hediffGiversCannotGive
				select x.label).ToLineList("  - ", true));
				flag3 = true;
			}
			if (this.biologicalAgeTickFactorFromAgeCurve != null)
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("AgeFactors".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine((from p in this.biologicalAgeTickFactorFromAgeCurve
				select "PeriodYears".Translate(p.x).ToString() + ": x" + p.y.ToStringPercent()).ToLineList("  - ", true));
				flag3 = true;
			}
			if (this.disabledWorkTags != WorkTags.None)
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				IEnumerable<WorkTypeDef> source = from x in DefDatabase<WorkTypeDef>.AllDefsListForReading
				where (CS$<>8__locals1.<>4__this.disabledWorkTags & x.workTags) > WorkTags.None
				select x;
				CS$<>8__locals1.sb.AppendLineTagged(("DisabledWorkLabel".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				CS$<>8__locals1.sb.AppendLine("  - " + (from x in source
				select x.labelShort).ToCommaList(false, false).CapitalizeFirst());
				if (this.disabledWorkTags.ExactlyOneWorkTagSet())
				{
					CS$<>8__locals1.sb.AppendLine("  - " + this.disabledWorkTags.LabelTranslated().CapitalizeFirst());
				}
				flag3 = true;
			}
			if (!this.abilities.NullOrEmpty<AbilityDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				if (this.abilities.Count == 1)
				{
					CS$<>8__locals1.sb.AppendLineTagged(("GivesAbility".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				}
				else
				{
					CS$<>8__locals1.sb.AppendLineTagged(("GivesAbilities".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				}
				CS$<>8__locals1.sb.AppendLine((from x in this.abilities
				select x.label).ToLineList("  - ", true));
				flag3 = true;
			}
			IEnumerable<ThoughtDef> enumerable = DefDatabase<ThoughtDef>.AllDefs.Where(delegate(ThoughtDef x)
			{
				if ((x.requiredGenes.NotNullAndContains(CS$<>8__locals1.<>4__this) || x.nullifyingGenes.NotNullAndContains(CS$<>8__locals1.<>4__this)) && x.stages != null)
				{
					return x.stages.Any((ThoughtStage y) => y.baseMoodEffect != 0f);
				}
				return false;
			});
			if (enumerable.Any<ThoughtDef>())
			{
				if (flag3)
				{
					CS$<>8__locals1.sb.AppendLine();
				}
				CS$<>8__locals1.sb.AppendLineTagged(("Mood".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				foreach (ThoughtDef thoughtDef in enumerable)
				{
					ThoughtStage thoughtStage = thoughtDef.stages.FirstOrDefault((ThoughtStage x) => x.baseMoodEffect != 0f);
					if (thoughtStage != null)
					{
						string text = thoughtStage.LabelCap + ": " + thoughtStage.baseMoodEffect.ToStringWithSign("0.##");
						if (thoughtDef.requiredGenes.NotNullAndContains(this))
						{
							CS$<>8__locals1.sb.AppendLine("  - " + text);
						}
						else if (thoughtDef.nullifyingGenes.NotNullAndContains(this))
						{
							CS$<>8__locals1.sb.AppendLine("  - " + "Removes".Translate() + ": " + text);
						}
					}
				}
			}
			return CS$<>8__locals1.sb.ToString().TrimEndNewlines();
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002B824 File Offset: 0x00029A24
		public bool ConflictsWith(GeneDef other)
		{
			if (this == other)
			{
				return true;
			}
			if (this.exclusionTags != null && other.exclusionTags != null)
			{
				for (int i = 0; i < this.exclusionTags.Count; i++)
				{
					if (other.exclusionTags.Contains(this.exclusionTags[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0002B879 File Offset: 0x00029A79
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			GeneDef.<SpecialDisplayStats>d__109 <SpecialDisplayStats>d__ = new GeneDef.<SpecialDisplayStats>d__109(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0002B890 File Offset: 0x00029A90
		public override IEnumerable<string> ConfigErrors()
		{
			GeneDef.<ConfigErrors>d__110 <ConfigErrors>d__ = new GeneDef.<ConfigErrors>d__110(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040002CD RID: 717
		public Type geneClass = typeof(Gene);

		// Token: 0x040002CE RID: 718
		[MustTranslate]
		public string labelShortAdj;

		// Token: 0x040002CF RID: 719
		[MustTranslate]
		public List<string> customEffectDescriptions;

		// Token: 0x040002D0 RID: 720
		[NoTranslate]
		public string iconPath;

		// Token: 0x040002D1 RID: 721
		private Color? iconColor;

		// Token: 0x040002D2 RID: 722
		public GeneCategoryDef displayCategory;

		// Token: 0x040002D3 RID: 723
		public float displayOrderInCategory;

		// Token: 0x040002D4 RID: 724
		public List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x040002D5 RID: 725
		public bool neverGrayHair;

		// Token: 0x040002D6 RID: 726
		public bool skinIsHairColor;

		// Token: 0x040002D7 RID: 727
		public bool tattoosVisible = true;

		// Token: 0x040002D8 RID: 728
		public FurDef fur;

		// Token: 0x040002D9 RID: 729
		public SoundDef soundCall;

		// Token: 0x040002DA RID: 730
		public SoundDef soundDeath;

		// Token: 0x040002DB RID: 731
		public SoundDef soundWounded;

		// Token: 0x040002DC RID: 732
		public Type resourceGizmoType = typeof(GeneGizmo_Resource);

		// Token: 0x040002DD RID: 733
		public float resourceLossPerDay;

		// Token: 0x040002DE RID: 734
		[MustTranslate]
		public string resourceLabel;

		// Token: 0x040002DF RID: 735
		[MustTranslate]
		public string resourceDescription;

		// Token: 0x040002E0 RID: 736
		public List<float> resourceGizmoThresholds;

		// Token: 0x040002E1 RID: 737
		public bool showGizmoOnWorldView;

		// Token: 0x040002E2 RID: 738
		public bool showGizmoWhenDrafted;

		// Token: 0x040002E3 RID: 739
		public bool showGizmoOnMultiSelect;

		// Token: 0x040002E4 RID: 740
		public List<AbilityDef> abilities;

		// Token: 0x040002E5 RID: 741
		public List<GeneticTraitData> forcedTraits;

		// Token: 0x040002E6 RID: 742
		public List<GeneticTraitData> suppressedTraits;

		// Token: 0x040002E7 RID: 743
		public List<NeedDef> enablesNeeds;

		// Token: 0x040002E8 RID: 744
		public List<NeedDef> disablesNeeds;

		// Token: 0x040002E9 RID: 745
		public WorkTags disabledWorkTags;

		// Token: 0x040002EA RID: 746
		public bool ignoreDarkness;

		// Token: 0x040002EB RID: 747
		public EndogeneCategory endogeneCategory;

		// Token: 0x040002EC RID: 748
		public bool dislikesSunlight;

		// Token: 0x040002ED RID: 749
		public float minAgeActive;

		// Token: 0x040002EE RID: 750
		public float lovinMTBFactor = 1f;

		// Token: 0x040002EF RID: 751
		public bool immuneToToxGasExposure;

		// Token: 0x040002F0 RID: 752
		public bool immuneToVacuumBurns;

		// Token: 0x040002F1 RID: 753
		public bool randomChosen;

		// Token: 0x040002F2 RID: 754
		public int? waterCellCost;

		// Token: 0x040002F3 RID: 755
		public HistoryEventDef deathHistoryEvent;

		// Token: 0x040002F4 RID: 756
		public List<Aptitude> aptitudes;

		// Token: 0x040002F5 RID: 757
		public PassionMod passionMod;

		// Token: 0x040002F6 RID: 758
		public List<StatModifier> statOffsets;

		// Token: 0x040002F7 RID: 759
		public List<StatModifier> statFactors;

		// Token: 0x040002F8 RID: 760
		public List<ConditionalStatAffecter> conditionalStatAffecters;

		// Token: 0x040002F9 RID: 761
		public float painOffset;

		// Token: 0x040002FA RID: 762
		public float painFactor = 1f;

		// Token: 0x040002FB RID: 763
		public float foodPoisoningChanceFactor = 1f;

		// Token: 0x040002FC RID: 764
		public List<DamageFactor> damageFactors;

		// Token: 0x040002FD RID: 765
		public SimpleCurve biologicalAgeTickFactorFromAgeCurve;

		// Token: 0x040002FE RID: 766
		public List<HediffDef> makeImmuneTo;

		// Token: 0x040002FF RID: 767
		public List<HediffDef> hediffGiversCannotGive;

		// Token: 0x04000300 RID: 768
		public ChemicalDef chemical;

		// Token: 0x04000301 RID: 769
		public float addictionChanceFactor = 1f;

		// Token: 0x04000302 RID: 770
		public float overdoseChanceFactor = 1f;

		// Token: 0x04000303 RID: 771
		public float toleranceBuildupFactor = 1f;

		// Token: 0x04000304 RID: 772
		public bool sterilize;

		// Token: 0x04000305 RID: 773
		public List<PawnCapacityModifier> capMods;

		// Token: 0x04000306 RID: 774
		public bool preventPermanentWounds;

		// Token: 0x04000307 RID: 775
		public bool dontMindRawFood;

		// Token: 0x04000308 RID: 776
		public Color? hairColorOverride;

		// Token: 0x04000309 RID: 777
		public Color? skinColorBase;

		// Token: 0x0400030A RID: 778
		public Color? skinColorOverride;

		// Token: 0x0400030B RID: 779
		public float randomBrightnessFactor;

		// Token: 0x0400030C RID: 780
		public TagFilter hairTagFilter;

		// Token: 0x0400030D RID: 781
		public TagFilter beardTagFilter;

		// Token: 0x0400030E RID: 782
		public GeneticBodyType? bodyType;

		// Token: 0x0400030F RID: 783
		public List<HeadTypeDef> forcedHeadTypes;

		// Token: 0x04000310 RID: 784
		public float minMelanin = -1f;

		// Token: 0x04000311 RID: 785
		public HairDef forcedHair;

		// Token: 0x04000312 RID: 786
		public bool womenCanHaveBeards;

		// Token: 0x04000313 RID: 787
		public float socialFightChanceFactor = 1f;

		// Token: 0x04000314 RID: 788
		public float aggroMentalBreakSelectionChanceFactor = 1f;

		// Token: 0x04000315 RID: 789
		public float mentalBreakMtbDays;

		// Token: 0x04000316 RID: 790
		public MentalBreakDef mentalBreakDef;

		// Token: 0x04000317 RID: 791
		public float missingGeneRomanceChanceFactor = 1f;

		// Token: 0x04000318 RID: 792
		public float prisonBreakMTBFactor = 1f;

		// Token: 0x04000319 RID: 793
		public int biostatCpx = 1;

		// Token: 0x0400031A RID: 794
		public int biostatMet;

		// Token: 0x0400031B RID: 795
		public int biostatArc;

		// Token: 0x0400031C RID: 796
		public List<string> exclusionTags;

		// Token: 0x0400031D RID: 797
		public GeneDef prerequisite;

		// Token: 0x0400031E RID: 798
		public float selectionWeight = 1f;

		// Token: 0x0400031F RID: 799
		public bool canGenerateInGeneSet = true;

		// Token: 0x04000320 RID: 800
		public GeneSymbolPack symbolPack;

		// Token: 0x04000321 RID: 801
		public float marketValueFactor = 1f;

		// Token: 0x04000322 RID: 802
		public bool removeOnRedress;

		// Token: 0x04000323 RID: 803
		public bool passOnDirectly = true;

		// Token: 0x04000324 RID: 804
		public float selectionWeightFactorDarkSkin = 1f;

		// Token: 0x04000325 RID: 805
		public float selectionWeightCultist = 1f;

		// Token: 0x04000326 RID: 806
		[Unsaved(false)]
		private string cachedDescription;

		// Token: 0x04000327 RID: 807
		[Unsaved(false)]
		private Texture2D cachedIcon;
	}
}
