using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001E23 RID: 7715
	[StaticConstructorOnStartup]
	public static class WidgetsWork
	{
		// Token: 0x17002504 RID: 9476
		// (get) Token: 0x0600C874 RID: 51316 RVA: 0x00497B6B File Offset: 0x00495D6B
		private static Texture2D WorkBoxBGTex_AgeDisabled
		{
			get
			{
				if (WidgetsWork.workBoxBGTex_AgeDisabled == null)
				{
					WidgetsWork.workBoxBGTex_AgeDisabled = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_AgeDisabled", true);
				}
				return WidgetsWork.workBoxBGTex_AgeDisabled;
			}
		}

		// Token: 0x0600C875 RID: 51317 RVA: 0x00497B90 File Offset: 0x00495D90
		public static Color ColorOfPriority(int prio)
		{
			switch (prio)
			{
			case 1:
				return new Color(0f, 1f, 0f);
			case 2:
				return new Color(1f, 0.9f, 0.5f);
			case 3:
				return new Color(0.8f, 0.7f, 0.5f);
			case 4:
				return new Color(0.74f, 0.74f, 0.74f);
			default:
				return Color.grey;
			}
		}

		// Token: 0x0600C876 RID: 51318 RVA: 0x00497C10 File Offset: 0x00495E10
		public static void DrawWorkBoxFor(float x, float y, Pawn p, WorkTypeDef wType, bool incapableBecauseOfCapacities)
		{
			if (p.WorkTypeIsDisabled(wType))
			{
				int value;
				if (p.IsWorkTypeDisabledByAge(wType, out value))
				{
					Rect rect = new Rect(x, y, 25f, 25f);
					if (Event.current.type == EventType.mouseDown && Mouse.IsOver(rect))
					{
						Messages.Message("MessageWorkTypeDisabledAge".Translate(p, p.ageTracker.AgeBiologicalYears, wType.labelShort, value), p, MessageTypeDefOf.RejectInput, false);
						SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
					}
					GUI.DrawTexture(rect, WidgetsWork.WorkBoxBGTex_AgeDisabled);
				}
				return;
			}
			Rect rect2 = new Rect(x, y, 25f, 25f);
			if (incapableBecauseOfCapacities)
			{
				GUI.color = new Color(1f, 0.3f, 0.3f);
			}
			WidgetsWork.DrawWorkBoxBackground(rect2, p, wType);
			GUI.color = Color.white;
			if (Find.PlaySettings.useWorkPriorities)
			{
				int priority = p.workSettings.GetPriority(wType);
				if (priority > 0)
				{
					Text.Anchor = TextAnchor.MiddleCenter;
					GUI.color = WidgetsWork.ColorOfPriority(priority);
					Widgets.Label(rect2.ContractedBy(-3f), priority.ToStringCached());
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
				}
				if (Event.current.type == EventType.mouseDown && Mouse.IsOver(rect2))
				{
					bool flag = p.workSettings.WorkIsActive(wType);
					if (Event.current.button == 0)
					{
						int num = p.workSettings.GetPriority(wType) - 1;
						if (num < 0)
						{
							num = 4;
						}
						p.workSettings.SetPriority(wType, num);
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					}
					if (Event.current.button == 1)
					{
						int num2 = p.workSettings.GetPriority(wType) + 1;
						if (num2 > 4)
						{
							num2 = 0;
						}
						p.workSettings.SetPriority(wType, num2);
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					}
					if (!flag && p.workSettings.WorkIsActive(wType) && wType.relevantSkills.Any<SkillDef>() && p.skills.AverageOfRelevantSkillsFor(wType) <= 2f)
					{
						SoundDefOf.Crunch.PlayOneShotOnCamera(null);
					}
					if (!flag && p.workSettings.WorkIsActive(wType) && p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(wType))
					{
						Messages.Message("MessageIdeoOpposedWorkTypeSelected".Translate(p, wType.gerundLabel), p, MessageTypeDefOf.CautionInput, false);
						SoundDefOf.DislikedWorkTypeActivated.PlayOneShotOnCamera(null);
					}
					Event.current.Use();
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorkTab, KnowledgeAmount.SpecificInteraction);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ManualWorkPriorities, KnowledgeAmount.SmallInteraction);
					return;
				}
			}
			else
			{
				if (p.workSettings.GetPriority(wType) > 0)
				{
					GUI.DrawTexture(rect2, WidgetsWork.WorkBoxCheckTex);
				}
				if (Widgets.ButtonInvisible(rect2, true))
				{
					if (p.workSettings.GetPriority(wType) > 0)
					{
						p.workSettings.SetPriority(wType, 0);
						SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
					}
					else
					{
						p.workSettings.SetPriority(wType, 3);
						SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
						if (wType.relevantSkills.Any<SkillDef>() && p.skills.AverageOfRelevantSkillsFor(wType) <= 2f)
						{
							SoundDefOf.Crunch.PlayOneShotOnCamera(null);
						}
						if (p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(wType))
						{
							Messages.Message("MessageIdeoOpposedWorkTypeSelected".Translate(p, wType.gerundLabel), p, MessageTypeDefOf.CautionInput, false);
							SoundDefOf.DislikedWorkTypeActivated.PlayOneShotOnCamera(null);
						}
					}
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorkTab, KnowledgeAmount.SpecificInteraction);
				}
			}
		}

		// Token: 0x0600C877 RID: 51319 RVA: 0x00497FA8 File Offset: 0x004961A8
		public static string TipForPawnWorker(Pawn p, WorkTypeDef wDef, bool incapableBecauseOfCapacities)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = wDef.gerundLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor);
			int priority = p.workSettings.GetPriority(wDef);
			text = text + ": " + ("Priority" + priority.ToString()).Translate().Colorize(WidgetsWork.ColorOfPriority(priority));
			stringBuilder.AppendLine(text);
			if (p.WorkTypeIsDisabled(wDef))
			{
				string text2 = "CannotDoThisWork".Translate(p.LabelShort, p);
				List<string> reasonsForDisabledWorkType = p.GetReasonsForDisabledWorkType(wDef);
				if (!reasonsForDisabledWorkType.NullOrEmpty<string>())
				{
					string text3 = "\n\n" + string.Join(". ", reasonsForDisabledWorkType);
					if (reasonsForDisabledWorkType.Count == 1)
					{
						text3 += ".";
					}
					text2 += text3.Colorize(ColorLibrary.RedReadable);
				}
				stringBuilder.Append(text2);
			}
			else
			{
				float num = p.skills.AverageOfRelevantSkillsFor(wDef);
				if (wDef.relevantSkills.Any<SkillDef>())
				{
					string text4 = "";
					foreach (SkillDef skillDef in wDef.relevantSkills)
					{
						text4 = text4 + skillDef.skillLabel.CapitalizeFirst() + ", ";
					}
					text4 = text4.Substring(0, text4.Length - 2);
					stringBuilder.AppendLine("RelevantSkills".Translate(text4, num.ToString("0.#"), 20));
				}
				if (p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(wDef))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("SelectedWorkTypeOpposedByIdeo".Translate(p).Colorize(WidgetsWork.ColorOfPriority(2)));
				}
				if (wDef.relevantSkills.Any<SkillDef>() && num <= 2f && p.workSettings.WorkIsActive(wDef))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("SelectedWorkTypeWithVeryBadSkill".Translate().Colorize(WidgetsWork.ColorOfPriority(2)));
				}
				stringBuilder.AppendLine();
				stringBuilder.Append(wDef.description);
				if (incapableBecauseOfCapacities)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("IncapableOfWorkTypeBecauseOfCapacities".Translate());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600C878 RID: 51320 RVA: 0x00498240 File Offset: 0x00496440
		private static void DrawWorkBoxBackground(Rect rect, Pawn p, WorkTypeDef workDef)
		{
			float num = p.skills.AverageOfRelevantSkillsFor(workDef);
			Texture2D image;
			Texture2D image2;
			float a;
			if (num < 4f)
			{
				image = WidgetsWork.WorkBoxBGTex_Awful;
				image2 = WidgetsWork.WorkBoxBGTex_Bad;
				a = num / 4f;
			}
			else if (num <= 14f)
			{
				image = WidgetsWork.WorkBoxBGTex_Bad;
				image2 = WidgetsWork.WorkBoxBGTex_Mid;
				a = (num - 4f) / 10f;
			}
			else
			{
				image = WidgetsWork.WorkBoxBGTex_Mid;
				image2 = WidgetsWork.WorkBoxBGTex_Excellent;
				a = (num - 14f) / 6f;
			}
			GUI.DrawTexture(rect, image);
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, a);
			GUI.DrawTexture(rect, image2);
			if (p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(workDef))
			{
				GUI.color = Color.white;
				GUI.DrawTexture(rect, WidgetsWork.WorkBoxOverlay_PreceptWarning);
			}
			if (workDef.relevantSkills.Any<SkillDef>() && num <= 2f && p.workSettings.WorkIsActive(workDef))
			{
				GUI.color = Color.white;
				GUI.DrawTexture(rect.ContractedBy(-2f), WidgetsWork.WorkBoxOverlay_Warning);
			}
			Passion passion = p.skills.MaxPassionOfRelevantSkillsFor(workDef);
			if (passion > Passion.None)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.4f);
				Rect position = rect;
				position.xMin = rect.center.x;
				position.yMin = rect.center.y;
				if (passion == Passion.Minor)
				{
					GUI.DrawTexture(position, WidgetsWork.PassionWorkboxMinorIcon);
				}
				else if (passion == Passion.Major)
				{
					GUI.DrawTexture(position, WidgetsWork.PassionWorkboxMajorIcon);
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x0400720C RID: 29196
		public const float WorkBoxSize = 25f;

		// Token: 0x0400720D RID: 29197
		public static readonly Texture2D WorkBoxBGTex_Awful = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_Awful", true);

		// Token: 0x0400720E RID: 29198
		public static readonly Texture2D WorkBoxBGTex_Bad = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_Bad", true);

		// Token: 0x0400720F RID: 29199
		private const int AwfulBGMax = 4;

		// Token: 0x04007210 RID: 29200
		public static readonly Texture2D WorkBoxBGTex_Mid = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_Mid", true);

		// Token: 0x04007211 RID: 29201
		private const int BadBGMax = 14;

		// Token: 0x04007212 RID: 29202
		public static readonly Texture2D WorkBoxBGTex_Excellent = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_Excellent", true);

		// Token: 0x04007213 RID: 29203
		public static readonly Texture2D WorkBoxCheckTex = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxCheck", true);

		// Token: 0x04007214 RID: 29204
		public static readonly Texture2D PassionWorkboxMinorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinorGray", true);

		// Token: 0x04007215 RID: 29205
		public static readonly Texture2D PassionWorkboxMajorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMajorGray", true);

		// Token: 0x04007216 RID: 29206
		public static readonly Texture2D WorkBoxOverlay_Warning = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxOverlay_Warning", true);

		// Token: 0x04007217 RID: 29207
		public static readonly Texture2D WorkBoxOverlay_PreceptWarning = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxOverlay_PreceptWarning", true);

		// Token: 0x04007218 RID: 29208
		private const int WarnIfSelectedMax = 2;

		// Token: 0x04007219 RID: 29209
		private static Texture2D workBoxBGTex_AgeDisabled;

		// Token: 0x0400721A RID: 29210
		private const float PassionOpacity = 0.4f;
	}
}
