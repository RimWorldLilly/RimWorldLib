using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DED RID: 7661
	[StaticConstructorOnStartup]
	public static class SkillUI
	{
		// Token: 0x0600C741 RID: 51009 RVA: 0x00490D60 File Offset: 0x0048EF60
		public static void Reset()
		{
			SkillUI.skillDefsInListOrderCached = (from sd in DefDatabase<SkillDef>.AllDefs
			orderby sd.listOrder descending
			select sd).ToList<SkillDef>();
		}

		// Token: 0x0600C742 RID: 51010 RVA: 0x00490D98 File Offset: 0x0048EF98
		public static void DrawSkillsOf(Pawn p, Vector2 offset, SkillUI.SkillDrawMode mode, Rect container)
		{
			Text.Font = GameFont.Small;
			if (p.DevelopmentalStage.Baby())
			{
				Color color = GUI.color;
				GUI.color = Color.gray;
				TextAnchor anchor = Text.Anchor;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(new Rect(offset.x, offset.y, 230f, container.height), "SkillsDevelopLaterBaby".Translate());
				GUI.color = color;
				Text.Anchor = anchor;
				return;
			}
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				float x = Text.CalcSize(allDefsListForReading[i].skillLabel.CapitalizeFirst()).x;
				if (x > SkillUI.levelLabelWidth)
				{
					SkillUI.levelLabelWidth = x;
				}
			}
			for (int j = 0; j < SkillUI.skillDefsInListOrderCached.Count; j++)
			{
				SkillDef skillDef = SkillUI.skillDefsInListOrderCached[j];
				float y = (float)j * 27f + offset.y;
				SkillUI.DrawSkill(p.skills.GetSkill(skillDef), new Vector2(offset.x, y), mode, "");
			}
		}

		// Token: 0x0600C743 RID: 51011 RVA: 0x00490EAA File Offset: 0x0048F0AA
		public static void DrawSkill(SkillRecord skill, Vector2 topLeft, SkillUI.SkillDrawMode mode, string tooltipPrefix = "")
		{
			SkillUI.DrawSkill(skill, new Rect(topLeft.x, topLeft.y, 230f, 24f), mode, "");
		}

		// Token: 0x0600C744 RID: 51012 RVA: 0x00490ED4 File Offset: 0x0048F0D4
		public static void DrawSkill(SkillRecord skill, Rect holdingRect, SkillUI.SkillDrawMode mode, string tooltipPrefix = "")
		{
			if (Mouse.IsOver(holdingRect))
			{
				GUI.DrawTexture(holdingRect, TexUI.HighlightTex);
			}
			Widgets.BeginGroup(holdingRect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect = new Rect(6f, 0f, SkillUI.levelLabelWidth + 6f, holdingRect.height);
			Widgets.Label(rect, skill.def.skillLabel.CapitalizeFirst());
			int level = skill.GetLevel(true);
			Rect position = new Rect(rect.xMax, 0f, 24f, 24f);
			if (!skill.TotallyDisabled)
			{
				if (skill.passion > Passion.None)
				{
					Texture2D image = (skill.passion == Passion.Major) ? SkillUI.PassionMajorIcon : SkillUI.PassionMinorIcon;
					GUI.DrawTexture(position, image);
				}
				Rect rect2 = new Rect(position.xMax, 0f, holdingRect.width - position.xMax, holdingRect.height);
				float fillPercent = Mathf.Max(0.01f, (float)level / 20f);
				Texture2D fillTex = SkillUI.SkillBarFillTex;
				if ((ModsConfig.BiotechActive || ModsConfig.AnomalyActive) && skill.Aptitude != 0)
				{
					fillTex = ((skill.Aptitude > 0) ? SkillUI.SkillBarAptitudePositiveTex : SkillUI.SkillBarAptitudeNegativeTex);
				}
				Widgets.FillableBar(rect2, fillPercent, fillTex, null, false);
			}
			Rect rect3 = new Rect(position.xMax + 4f, 0f, 999f, holdingRect.height);
			rect3.yMin += 3f;
			string label;
			if (skill.TotallyDisabled)
			{
				GUI.color = SkillUI.DisabledSkillColor;
				label = "-";
			}
			else
			{
				if ((ModsConfig.BiotechActive || ModsConfig.AnomalyActive) && level == 0 && skill.Aptitude != 0)
				{
					GUI.color = ((skill.Aptitude > 0) ? ColorLibrary.BrightGreen : ColorLibrary.RedReadable);
				}
				label = level.ToStringCached();
			}
			GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
			Widgets.Label(rect3, label);
			GenUI.ResetLabelAlign();
			GUI.color = Color.white;
			Widgets.EndGroup();
			if (Mouse.IsOver(holdingRect))
			{
				string text = SkillUI.GetSkillDescription(skill);
				if (tooltipPrefix != "")
				{
					text = tooltipPrefix + "\n\n" + text;
				}
				TooltipHandler.TipRegion(holdingRect, new TipSignal(text, skill.def.GetHashCode() * 397945));
			}
		}

		// Token: 0x0600C745 RID: 51013 RVA: 0x00491108 File Offset: 0x0048F308
		private static string GetSkillDescription(SkillRecord sk)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TaggedString taggedString = sk.def.LabelCap.AsTipTitle();
			if (sk.TotallyDisabled)
			{
				taggedString += " (" + "DisabledLower".Translate() + ")";
			}
			stringBuilder.AppendLineTagged(taggedString);
			stringBuilder.AppendLineTagged(sk.def.description.Colorize(ColoredText.SubtleGrayColor)).AppendLine();
			stringBuilder.AppendLineTagged(("SkillLevel".Translate().CapitalizeFirst() + ": ").AsTipTitle() + sk.GetLevelForUI(true).ToString() + " - " + sk.LevelDescriptor);
			if (!sk.PermanentlyDisabled && sk.Aptitude != 0)
			{
				stringBuilder.AppendLine("  - " + "LearnedLevel".Translate() + ": " + sk.GetLevel(false).ToString());
				if (ModsConfig.BiotechActive && sk.Pawn.genes != null)
				{
					foreach (Gene gene in sk.Pawn.genes.GenesListForReading)
					{
						if (gene.Active)
						{
							int num = gene.def.AptitudeFor(sk.def);
							if (num != 0)
							{
								StringBuilder stringBuilder2 = stringBuilder;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 2);
								defaultInterpolatedStringHandler.AppendLiteral("  - ");
								defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("GeneLabelWithDesc".Translate(gene.def.Named("GENE")).CapitalizeFirst());
								defaultInterpolatedStringHandler.AppendLiteral(": ");
								defaultInterpolatedStringHandler.AppendFormatted(num.ToStringWithSign());
								stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
					}
				}
				if (ModsConfig.AnomalyActive)
				{
					Pawn_StoryTracker story = sk.Pawn.story;
					if (((story != null) ? story.traits : null) != null)
					{
						foreach (Trait trait in sk.Pawn.story.traits.allTraits)
						{
							if (!trait.Suppressed)
							{
								int num2 = trait.CurrentData.AptitudeFor(sk.def);
								if (num2 != 0)
								{
									StringBuilder stringBuilder3 = stringBuilder;
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(6, 2);
									defaultInterpolatedStringHandler2.AppendLiteral("  - ");
									defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("TraitLabelWithDesc".Translate(trait.CurrentData.GetLabelFor(sk.Pawn).Named("TRAITLABEL")).CapitalizeFirst());
									defaultInterpolatedStringHandler2.AppendLiteral(": ");
									defaultInterpolatedStringHandler2.AppendFormatted(num2.ToStringWithSign());
									stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
								}
							}
						}
					}
					if (sk.Pawn.health.hediffSet != null)
					{
						foreach (Hediff hediff in sk.Pawn.health.hediffSet.hediffs)
						{
							int num3 = hediff.def.AptitudeFor(sk.def);
							if (num3 != 0)
							{
								stringBuilder.AppendLine("  - " + hediff.LabelCap + ": " + num3.ToStringWithSign());
							}
						}
					}
				}
				int unclampedLevel = sk.GetUnclampedLevel();
				if (unclampedLevel < 0)
				{
					stringBuilder.AppendLine("  - " + "NegativeLevelAdjustedToZero".Translate());
				}
				else if (unclampedLevel > 20)
				{
					stringBuilder.AppendLine("  - " + "LevelCappedAtMax".Translate(20));
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				stringBuilder.AppendLine();
				string str = (sk.GetLevel(false) == 20) ? "Experience".Translate() : "ProgressToNextLevel".Translate();
				if (sk.PermanentlyDisabled)
				{
					stringBuilder.AppendLineTagged((str + ": ").AsTipTitle() + "0 / " + SkillRecord.XpRequiredToLevelUpFrom(0).ToString());
				}
				else
				{
					stringBuilder.AppendLineTagged((str + ": ").AsTipTitle() + sk.xpSinceLastLevel.ToString("F0") + " / " + sk.XpRequiredForLevelUp.ToString());
				}
			}
			if (!sk.TotallyDisabled)
			{
				if (sk.LearningSaturatedToday)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("LearnedMaxToday".Translate(sk.xpSinceMidnight.ToString("F0"), 4000, 0.2f.ToStringPercent("F0")));
				}
				float statValue = sk.Pawn.GetStatValue(StatDefOf.GlobalLearningFactor, true, -1);
				float num4 = 1f;
				float num5 = statValue * sk.passion.GetLearningFactor();
				if (sk.def == SkillDefOf.Animals)
				{
					num4 = sk.Pawn.GetStatValue(StatDefOf.AnimalsLearningFactor, true, -1);
					num5 *= num4;
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLineTagged(("LearningSpeed".Translate() + ": ").AsTipTitle() + num5.ToStringPercent());
				stringBuilder.AppendLine("  - " + "StatsReport_BaseValue".Translate() + ": " + 1f.ToStringPercent());
				stringBuilder.AppendLine("  - " + sk.passion.GetLabel() + ": x" + sk.passion.GetLearningFactor().ToStringPercent("F0"));
				if (statValue != 1f)
				{
					stringBuilder.AppendLine("  - " + StatDefOf.GlobalLearningFactor.LabelCap + ": x" + statValue.ToStringPercent());
				}
				if (Math.Abs(num4 - 1f) > 1E-45f)
				{
					stringBuilder.AppendLine("  - " + StatDefOf.AnimalsLearningFactor.LabelCap + ": x" + num4.ToStringPercent());
				}
				if (statValue != 1f)
				{
					float baseValueFor = StatDefOf.GlobalLearningFactor.Worker.GetBaseValueFor(StatRequest.For(sk.Pawn));
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged((StatDefOf.GlobalLearningFactor.LabelCap + ": ").AsTipTitle() + statValue.ToStringPercent());
					stringBuilder.AppendLine("  - " + "StatsReport_BaseValue".Translate() + ": " + baseValueFor.ToStringPercent());
					SkillUI.ListGlobalLearningSpeedOffsets(sk, stringBuilder);
					SkillUI.ListGlobalLearningSpeedFactors(sk, stringBuilder);
				}
				if (ModsConfig.BiotechActive && sk.Pawn.DevelopmentalStage.Child())
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLineTagged("ChildrenLearn".Translate().Colorize(ColoredText.SubtleGrayColor));
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600C746 RID: 51014 RVA: 0x004918B8 File Offset: 0x0048FAB8
		private static void ListGlobalLearningSpeedOffsets(SkillRecord sk, StringBuilder sb)
		{
			foreach (Hediff hediff in sk.Pawn.health.hediffSet.hediffs)
			{
				if (hediff.Visible)
				{
					HediffStage curStage = hediff.CurStage;
					if (curStage != null)
					{
						float statOffsetFromList = curStage.statOffsets.GetStatOffsetFromList(StatDefOf.GlobalLearningFactor);
						if (statOffsetFromList != 0f)
						{
							sb.AppendLine(string.Concat(new string[]
							{
								"  - ",
								hediff.LabelCap,
								": ",
								(statOffsetFromList >= 0f) ? "+" : "",
								statOffsetFromList.ToStringPercent()
							}));
						}
					}
				}
			}
			Pawn_StoryTracker story = sk.Pawn.story;
			if (((story != null) ? story.traits : null) != null)
			{
				foreach (Trait trait in sk.Pawn.story.traits.allTraits)
				{
					if (!trait.Suppressed)
					{
						float statOffsetFromList2 = trait.CurrentData.statOffsets.GetStatOffsetFromList(StatDefOf.GlobalLearningFactor);
						if (statOffsetFromList2 != 0f)
						{
							sb.AppendLine(string.Concat(new string[]
							{
								"  - ",
								trait.CurrentData.LabelCap,
								": ",
								(statOffsetFromList2 >= 0f) ? "+" : "",
								statOffsetFromList2.ToStringPercent()
							}));
						}
					}
				}
			}
			if (ModsConfig.BiotechActive && sk.Pawn.genes != null)
			{
				foreach (Gene gene in sk.Pawn.genes.GenesListForReading)
				{
					if (gene.Active)
					{
						float statOffsetFromList3 = gene.def.statOffsets.GetStatOffsetFromList(StatDefOf.GlobalLearningFactor);
						if (statOffsetFromList3 != 0f)
						{
							sb.AppendLine("  - " + gene.def.LabelCap + ": " + ((statOffsetFromList3 >= 0f) ? "+" : "") + statOffsetFromList3.ToStringPercent());
						}
					}
				}
			}
		}

		// Token: 0x0600C747 RID: 51015 RVA: 0x00491B5C File Offset: 0x0048FD5C
		private static void ListGlobalLearningSpeedFactors(SkillRecord sk, StringBuilder sb)
		{
			foreach (Hediff hediff in sk.Pawn.health.hediffSet.hediffs)
			{
				if (hediff.Visible)
				{
					HediffStage curStage = hediff.CurStage;
					if (curStage != null)
					{
						float statFactorFromList = curStage.statFactors.GetStatFactorFromList(StatDefOf.GlobalLearningFactor);
						if (!Mathf.Approximately(statFactorFromList, 1f))
						{
							sb.AppendLine("  - " + hediff.LabelCap + ": x" + statFactorFromList.ToStringPercent());
						}
					}
				}
			}
			Pawn_StoryTracker story = sk.Pawn.story;
			if (((story != null) ? story.traits : null) != null)
			{
				foreach (Trait trait in sk.Pawn.story.traits.allTraits)
				{
					if (!trait.Suppressed)
					{
						float statFactorFromList2 = trait.CurrentData.statFactors.GetStatFactorFromList(StatDefOf.GlobalLearningFactor);
						if (!Mathf.Approximately(statFactorFromList2, 1f))
						{
							sb.AppendLine("  - " + trait.CurrentData.LabelCap + ": x" + statFactorFromList2.ToStringPercent());
						}
					}
				}
			}
			if (ModsConfig.BiotechActive && sk.Pawn.genes != null)
			{
				foreach (Gene gene in sk.Pawn.genes.GenesListForReading)
				{
					if (gene.Active)
					{
						float statFactorFromList3 = gene.def.statFactors.GetStatFactorFromList(StatDefOf.GlobalLearningFactor);
						if (!Mathf.Approximately(statFactorFromList3, 1f))
						{
							sb.AppendLine("  - " + gene.def.LabelCap + ": x" + statFactorFromList3.ToStringPercent());
						}
					}
				}
			}
		}

		// Token: 0x0600C748 RID: 51016 RVA: 0x00491D94 File Offset: 0x0048FF94
		public static string GetLabel(this Passion passion)
		{
			switch (passion)
			{
			case Passion.None:
				return "PassionNone".Translate();
			case Passion.Minor:
				return "PassionMinor".Translate();
			case Passion.Major:
				return "PassionMajor".Translate();
			default:
				Log.Error("Unknown passion type.");
				return string.Empty;
			}
		}

		// Token: 0x0600C749 RID: 51017 RVA: 0x00491DF4 File Offset: 0x0048FFF4
		public static float GetLearningFactor(this Passion passion)
		{
			switch (passion)
			{
			case Passion.None:
				return 0.35f;
			case Passion.Minor:
				return 1f;
			case Passion.Major:
				return 1.5f;
			default:
				Log.Error("Unknown passion type.");
				return 1f;
			}
		}

		// Token: 0x0400715D RID: 29021
		private static float levelLabelWidth = -1f;

		// Token: 0x0400715E RID: 29022
		private static List<SkillDef> skillDefsInListOrderCached;

		// Token: 0x0400715F RID: 29023
		private const float SkillWidth = 230f;

		// Token: 0x04007160 RID: 29024
		public const float SkillHeight = 24f;

		// Token: 0x04007161 RID: 29025
		public const float SkillYSpacing = 3f;

		// Token: 0x04007162 RID: 29026
		private const float LeftEdgeMargin = 6f;

		// Token: 0x04007163 RID: 29027
		private const float IncButX = 205f;

		// Token: 0x04007164 RID: 29028
		private const float IncButSpacing = 10f;

		// Token: 0x04007165 RID: 29029
		private static readonly Color DisabledSkillColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04007166 RID: 29030
		public static Texture2D PassionMinorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinor", true);

		// Token: 0x04007167 RID: 29031
		public static Texture2D PassionMajorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMajor", true);

		// Token: 0x04007168 RID: 29032
		private static Texture2D SkillBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

		// Token: 0x04007169 RID: 29033
		private static Texture2D SkillBarAptitudePositiveTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.8f, 1f, 0.6f, 0.25f));

		// Token: 0x0400716A RID: 29034
		private static Texture2D SkillBarAptitudeNegativeTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0.5f, 0.6f, 0.25f));

		// Token: 0x02003B4C RID: 15180
		public enum SkillDrawMode : byte
		{
			// Token: 0x0400EB27 RID: 60199
			Gameplay,
			// Token: 0x0400EB28 RID: 60200
			Menu
		}
	}
}
