using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015E5 RID: 5605
	public class Trait : IExposable
	{
		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x06008B49 RID: 35657 RVA: 0x003274B0 File Offset: 0x003256B0
		public int Degree
		{
			get
			{
				return this.degree;
			}
		}

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x06008B4A RID: 35658 RVA: 0x003274B8 File Offset: 0x003256B8
		public TraitDegreeData CurrentData
		{
			get
			{
				return this.def.DataAtDegree(this.degree);
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x06008B4B RID: 35659 RVA: 0x003274CB File Offset: 0x003256CB
		public string Label
		{
			get
			{
				return this.CurrentData.GetLabelFor(this.pawn);
			}
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x06008B4C RID: 35660 RVA: 0x003274DE File Offset: 0x003256DE
		public string LabelCap
		{
			get
			{
				return this.CurrentData.GetLabelCapFor(this.pawn);
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x06008B4D RID: 35661 RVA: 0x003274F1 File Offset: 0x003256F1
		public bool ScenForced
		{
			get
			{
				return this.scenForced;
			}
		}

		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x06008B4E RID: 35662 RVA: 0x003274FC File Offset: 0x003256FC
		public bool Suppressed
		{
			get
			{
				if (ModsConfig.BiotechActive)
				{
					if (this.suppressedByTrait || this.suppressedByGene != null)
					{
						return true;
					}
					if (this.sourceGene != null && this.sourceGene.Overridden && this.sourceGene.overriddenByGene.def != this.sourceGene.def)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06008B4F RID: 35663 RVA: 0x00327557 File Offset: 0x00325757
		public Trait()
		{
		}

		// Token: 0x06008B50 RID: 35664 RVA: 0x0032756A File Offset: 0x0032576A
		public Trait(TraitDef def, int degree = 0, bool forced = false)
		{
			this.def = def;
			this.degree = degree;
			this.scenForced = forced;
		}

		// Token: 0x06008B51 RID: 35665 RVA: 0x00327594 File Offset: 0x00325794
		public void ExposeData()
		{
			Scribe_Defs.Look<TraitDef>(ref this.def, "def");
			Scribe_References.Look<Gene>(ref this.sourceGene, "sourceGene", false);
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
			Scribe_Values.Look<bool>(ref this.scenForced, "scenForced", false, false);
			Scribe_Values.Look<bool>(ref this.suppressedByTrait, "suppressedByTrait", false, false);
			Scribe_References.Look<Gene>(ref this.suppressedByGene, "suppressedBy", false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == null)
			{
				this.def = DefDatabase<TraitDef>.GetRandom();
				this.degree = PawnGenerator.RandomTraitDegree(this.def);
			}
		}

		// Token: 0x06008B52 RID: 35666 RVA: 0x00327638 File Offset: 0x00325838
		public float OffsetOfStat(StatDef stat)
		{
			if (this.Suppressed)
			{
				return 0f;
			}
			float num = 0f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statOffsets != null)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					if (currentData.statOffsets[i].stat == stat)
					{
						num += currentData.statOffsets[i].value;
					}
				}
			}
			return num;
		}

		// Token: 0x06008B53 RID: 35667 RVA: 0x003276A8 File Offset: 0x003258A8
		public float MultiplierOfStat(StatDef stat)
		{
			if (this.Suppressed)
			{
				return 1f;
			}
			float num = 1f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statFactors != null)
			{
				for (int i = 0; i < currentData.statFactors.Count; i++)
				{
					if (currentData.statFactors[i].stat == stat)
					{
						num *= currentData.statFactors[i].value;
					}
				}
			}
			return num;
		}

		// Token: 0x06008B54 RID: 35668 RVA: 0x00327718 File Offset: 0x00325918
		public string TipString(Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.suppressedReasons.Clear();
			TraitDegreeData currentData = this.CurrentData;
			stringBuilder.AppendLine(currentData.description.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true).Resolve());
			bool flag = this.CurrentData.skillGains.Count > 0;
			bool flag2 = this.GetPermaThoughts().Any<ThoughtDef>();
			bool flag3 = currentData.statOffsets != null;
			bool flag4 = currentData.statFactors != null;
			if (flag || flag2 || flag3 || flag4 || !Mathf.Approximately(currentData.painFactor, 1f) || currentData.painOffset != 0f || !currentData.aptitudes.NullOrEmpty<Aptitude>())
			{
				stringBuilder.AppendLine();
			}
			if (flag)
			{
				foreach (SkillGain skillGain in this.CurrentData.skillGains)
				{
					if (skillGain.amount != 0)
					{
						stringBuilder.AppendLine("    " + skillGain.skill.skillLabel.CapitalizeFirst() + ":   " + skillGain.amount.ToString("+##;-##"));
					}
				}
			}
			if (flag2)
			{
				foreach (ThoughtDef thoughtDef in this.GetPermaThoughts())
				{
					stringBuilder.AppendLine("    " + "PermanentMoodEffect".Translate() + " " + thoughtDef.stages[0].baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset));
				}
			}
			if (flag3)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					StatModifier statModifier = currentData.statOffsets[i];
					string valueToStringAsOffset = statModifier.ValueToStringAsOffset;
					string value = "    " + statModifier.stat.LabelCap + " " + valueToStringAsOffset;
					stringBuilder.AppendLine(value);
				}
			}
			if (currentData.painOffset != 0f)
			{
				stringBuilder.AppendLine("    " + "Pain".Translate() + " " + currentData.painOffset.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Offset));
			}
			if (flag4)
			{
				for (int j = 0; j < currentData.statFactors.Count; j++)
				{
					StatModifier statModifier2 = currentData.statFactors[j];
					string toStringAsFactor = statModifier2.ToStringAsFactor;
					string value2 = "    " + statModifier2.stat.LabelCap + " " + toStringAsFactor;
					stringBuilder.AppendLine(value2);
				}
			}
			if (!Mathf.Approximately(currentData.hungerRateFactor, 1f))
			{
				string t = currentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor);
				string value3 = "    " + "HungerRate".Translate() + " " + t;
				stringBuilder.AppendLine(value3);
			}
			if (!Mathf.Approximately(currentData.painFactor, 1f))
			{
				stringBuilder.AppendLine("    " + "Pain".Translate() + " " + currentData.painFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
			}
			if (!currentData.aptitudes.NullOrEmpty<Aptitude>())
			{
				stringBuilder.AppendLine().AppendLineTagged(("Aptitudes".Translate().CapitalizeFirst() + ":").AsTipTitle());
				stringBuilder.AppendLine((from x in currentData.aptitudes
				select x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", true));
			}
			if (!currentData.enablesNeeds.NullOrEmpty<NeedDef>())
			{
				stringBuilder.AppendLine().AppendLineTagged(("AddsNeeds".Translate().CapitalizeFirst() + ":").AsTipTitle());
				stringBuilder.AppendLine((from x in currentData.enablesNeeds
				select x.LabelCap.ToString()).ToLineList("  - ", true));
			}
			if (!currentData.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				stringBuilder.AppendLine().AppendLineTagged(("DisablesNeeds".Translate().CapitalizeFirst() + ":").AsTipTitle());
				stringBuilder.AppendLine((from x in currentData.disablesNeeds
				select x.LabelCap.ToString()).ToLineList("  - ", true));
			}
			if (ModsConfig.RoyaltyActive)
			{
				List<MeditationFocusDef> allowedMeditationFocusTypes = this.CurrentData.allowedMeditationFocusTypes;
				if (!allowedMeditationFocusTypes.NullOrEmpty<MeditationFocusDef>())
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("EnablesMeditationFocusType".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + (from f in allowedMeditationFocusTypes
					select f.LabelCap.Resolve()).ToLineList("  - ", false));
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				List<IssueDef> affectedIssues = this.CurrentData.GetAffectedIssues(this.def);
				if (affectedIssues.Count != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("OverridesSomePrecepts".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + (from x in affectedIssues
					select x.LabelCap.Resolve()).ToLineList("  - ", false));
				}
				List<MemeDef> affectedMemes = this.CurrentData.GetAffectedMemes(this.def, true);
				if (affectedMemes.Count > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("AgreeableMemes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + (from x in affectedMemes
					select x.LabelCap.Resolve()).ToLineList("  - ", false));
				}
				List<MemeDef> affectedMemes2 = this.CurrentData.GetAffectedMemes(this.def, false);
				if (affectedMemes2.Count > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("DisagreeableMemes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + (from x in affectedMemes2
					select x.LabelCap.Resolve()).ToLineList("  - ", false));
				}
			}
			if (ModsConfig.BiotechActive)
			{
				if (this.sourceGene != null)
				{
					stringBuilder.AppendLine().AppendLine(("AddedByGene".Translate() + ": " + this.sourceGene.LabelCap).Colorize(ColoredText.GeneColor));
				}
				if (this.Suppressed)
				{
					foreach (Trait trait in pawn.story.traits.allTraits)
					{
						if (trait != this && trait.def.CanSuppress(this))
						{
							List<string> list = this.suppressedReasons;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 2);
							defaultInterpolatedStringHandler.AppendFormatted(trait.Label);
							defaultInterpolatedStringHandler.AppendLiteral(" (");
							defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Trait".Translate());
							defaultInterpolatedStringHandler.AppendLiteral(")");
							list.Add(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					if (this.sourceGene != null && this.sourceGene.Overridden && this.sourceGene.overriddenByGene.def != this.sourceGene.def)
					{
						List<string> list2 = this.suppressedReasons;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 2);
						defaultInterpolatedStringHandler2.AppendFormatted(this.sourceGene.overriddenByGene.Label);
						defaultInterpolatedStringHandler2.AppendLiteral(" (");
						defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("Gene".Translate());
						defaultInterpolatedStringHandler2.AppendLiteral(")");
						list2.Add(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
					if (this.suppressedByGene != null)
					{
						List<string> list3 = this.suppressedReasons;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(3, 2);
						defaultInterpolatedStringHandler3.AppendFormatted(this.suppressedByGene.Label);
						defaultInterpolatedStringHandler3.AppendLiteral(" (");
						defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("Gene".Translate());
						defaultInterpolatedStringHandler3.AppendLiteral(")");
						list3.Add(defaultInterpolatedStringHandler3.ToStringAndClear());
					}
					if (this.suppressedReasons.Any<string>())
					{
						stringBuilder.AppendLine().AppendLine(("Suppressed".Translate().CapitalizeFirst() + ": " + "SuppressedDesc".Translate() + ":\n" + this.suppressedReasons.ToLineList("  - ", true)).Colorize(ColoredText.SubtleGrayColor));
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				if (stringBuilder2[stringBuilder2.Length - 1] == '\n')
				{
					if (stringBuilder.Length > 1)
					{
						StringBuilder stringBuilder3 = stringBuilder;
						if (stringBuilder3[stringBuilder3.Length - 2] == '\r')
						{
							stringBuilder.Remove(stringBuilder.Length - 2, 2);
							goto IL_991;
						}
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
			}
			IL_991:
			return stringBuilder.ToString();
		}

		// Token: 0x06008B55 RID: 35669 RVA: 0x003280E4 File Offset: 0x003262E4
		public override string ToString()
		{
			if (this.def == null)
			{
				return "Trait(null)";
			}
			return string.Concat(new string[]
			{
				"Trait(",
				this.def.ToString(),
				"-",
				this.degree.ToString(),
				")"
			});
		}

		// Token: 0x06008B56 RID: 35670 RVA: 0x0032813E File Offset: 0x0032633E
		private IEnumerable<ThoughtDef> GetPermaThoughts()
		{
			Trait.<GetPermaThoughts>d__27 <GetPermaThoughts>d__ = new Trait.<GetPermaThoughts>d__27(-2);
			<GetPermaThoughts>d__.<>4__this = this;
			return <GetPermaThoughts>d__;
		}

		// Token: 0x06008B57 RID: 35671 RVA: 0x0032814E File Offset: 0x0032634E
		private bool AllowsWorkType(WorkTypeDef workDef)
		{
			return (this.def.disabledWorkTags & workDef.workTags) == WorkTags.None;
		}

		// Token: 0x06008B58 RID: 35672 RVA: 0x00328165 File Offset: 0x00326365
		public void Notify_MentalStateEndedOn(Pawn pawn, bool causedByMood)
		{
			if (causedByMood)
			{
				this.Notify_MentalStateEndedOn(pawn);
			}
		}

		// Token: 0x06008B59 RID: 35673 RVA: 0x00328174 File Offset: 0x00326374
		public void Notify_MentalStateEndedOn(Pawn pawn)
		{
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.mentalBreakInspirationGainSet.NullOrEmpty<InspirationDef>() || Rand.Value > currentData.mentalBreakInspirationGainChance)
			{
				return;
			}
			pawn.mindState.inspirationHandler.TryStartInspiration(currentData.mentalBreakInspirationGainSet.RandomElement<InspirationDef>(), currentData.mentalBreakInspirationGainReasonText, true);
		}

		// Token: 0x06008B5A RID: 35674 RVA: 0x003281C8 File Offset: 0x003263C8
		public List<WorkTypeDef> GetDisabledWorkTypes()
		{
			Trait.tmpDisabledWorktypes.Clear();
			for (int i = 0; i < this.def.disabledWorkTypes.Count; i++)
			{
				Trait.tmpDisabledWorktypes.Add(this.def.disabledWorkTypes[i]);
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[j];
				if (!this.AllowsWorkType(workTypeDef))
				{
					Trait.tmpDisabledWorktypes.Add(workTypeDef);
				}
			}
			return Trait.tmpDisabledWorktypes;
		}

		// Token: 0x04005595 RID: 21909
		public Pawn pawn;

		// Token: 0x04005596 RID: 21910
		public Gene sourceGene;

		// Token: 0x04005597 RID: 21911
		public Gene suppressedByGene;

		// Token: 0x04005598 RID: 21912
		public TraitDef def;

		// Token: 0x04005599 RID: 21913
		private int degree;

		// Token: 0x0400559A RID: 21914
		private bool scenForced;

		// Token: 0x0400559B RID: 21915
		[LoadAlias("suppressed")]
		public bool suppressedByTrait;

		// Token: 0x0400559C RID: 21916
		private List<string> suppressedReasons = new List<string>();

		// Token: 0x0400559D RID: 21917
		private static List<WorkTypeDef> tmpDisabledWorktypes = new List<WorkTypeDef>();
	}
}
