using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CE6 RID: 7398
	public static class FactionUIUtility
	{
		// Token: 0x0600BE58 RID: 48728 RVA: 0x00448CB8 File Offset: 0x00446EB8
		public static void DoWindowContents(Rect fillRect, ref Vector2 scrollPosition, ref float scrollViewHeight, Faction scrollToFaction = null)
		{
			Rect rect = new Rect(0f, 0f, fillRect.width, fillRect.height);
			Widgets.BeginGroup(rect);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			if (Prefs.DevMode)
			{
				Widgets.CheckboxLabeled(new Rect(rect.width - 120f, 0f, 120f, 24f), "DEV: Show all", ref FactionUIUtility.showAll, false, null, null, false, false);
			}
			else
			{
				FactionUIUtility.showAll = false;
			}
			Rect outRect = new Rect(0f, 50f, rect.width, rect.height - 50f);
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, scrollViewHeight);
			FactionUIUtility.visibleFactions.Clear();
			foreach (Faction faction in Find.FactionManager.AllFactionsInViewOrder)
			{
				if ((!faction.IsPlayer && !faction.Hidden) || FactionUIUtility.showAll)
				{
					FactionUIUtility.visibleFactions.Add(faction);
				}
			}
			if (FactionUIUtility.visibleFactions.Count > 0)
			{
				Widgets.Label(new Rect(614f, 50f, 200f, 100f), "EnemyOf".Translate());
				outRect.yMin += Text.LineHeight;
				Widgets.BeginScrollView(outRect, ref scrollPosition, rect2, true);
				float num = 0f;
				int num2 = 0;
				foreach (Faction faction2 in FactionUIUtility.visibleFactions)
				{
					if ((!faction2.IsPlayer && !faction2.Hidden) || FactionUIUtility.showAll)
					{
						if (faction2 == scrollToFaction)
						{
							scrollPosition.y = num;
						}
						if (num2 % 2 == 1)
						{
							Widgets.DrawLightHighlight(new Rect(rect2.x, num, rect2.width, 80f));
						}
						num += FactionUIUtility.DrawFactionRow(faction2, num, rect2);
						num2++;
					}
				}
				if (Event.current.type == EventType.layout)
				{
					scrollViewHeight = num;
				}
				Widgets.EndScrollView();
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect, "NoFactions".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			Widgets.EndGroup();
		}

		// Token: 0x0600BE59 RID: 48729 RVA: 0x00448F24 File Offset: 0x00447124
		private static float DrawFactionRow(Faction faction, float rowY, Rect fillRect)
		{
			float num = fillRect.width - 300f - 40f - 70f - 54f - 16f - 120f;
			Faction[] array = (from f in Find.FactionManager.AllFactionsInViewOrder
			where f != faction && f.HostileTo(faction) && ((!f.IsPlayer && !f.Hidden) || FactionUIUtility.showAll)
			select f).ToArray<Faction>();
			Rect rect = new Rect(90f, rowY, 300f, 80f);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect position = new Rect(24f, rowY + (rect.height - 42f) / 2f, 42f, 42f);
			GUI.color = faction.Color;
			GUI.DrawTexture(position, faction.def.FactionIcon);
			GUI.color = Color.white;
			string label = faction.Name.CapitalizeFirst() + "\n" + faction.def.LabelCap + "\n" + ((faction.leader != null) ? (faction.LeaderTitle.CapitalizeFirst() + ": " + faction.leader.Name.ToStringFull) : "");
			Widgets.Label(rect, label);
			Rect rect2 = new Rect(0f, rowY, rect.xMax, 80f);
			if (Mouse.IsOver(rect2))
			{
				TipSignal tip = new TipSignal(() => string.Concat(new string[]
				{
					faction.Name.Colorize(ColoredText.TipSectionTitleColor),
					"\n",
					faction.def.LabelCap.Resolve(),
					"\n\n",
					faction.def.Description
				}), faction.loadID ^ 1938473043);
				TooltipHandler.TipRegion(rect2, tip);
				Widgets.DrawHighlight(rect2);
			}
			if (Widgets.ButtonInvisible(rect2, false))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(faction));
			}
			Rect rect3 = new Rect(rect.xMax, rowY, 40f, 80f);
			Widgets.InfoCardButtonCentered(rect3, faction);
			Rect rect4 = new Rect(rect3.xMax, rowY, 60f, 80f);
			if (ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode)
			{
				if (faction.ideos != null)
				{
					float num2 = rect4.x;
					float num3 = rect4.y;
					if (faction.ideos.PrimaryIdeo != null)
					{
						if (num2 + 40f > rect4.xMax)
						{
							num2 = rect4.x;
							num3 += 45f;
						}
						Rect rect5 = new Rect(num2, num3 + (rect4.height - 40f) / 2f, 40f, 40f);
						IdeoUIUtility.DoIdeoIcon(rect5, faction.ideos.PrimaryIdeo, true, delegate
						{
							IdeoUIUtility.OpenIdeoInfo(faction.ideos.PrimaryIdeo);
						});
						num2 += rect5.width + 5f;
						num2 = rect4.x;
						num3 += 45f;
					}
					List<Ideo> minor = faction.ideos.IdeosMinorListForReading;
					int k;
					int i;
					Action <>9__3;
					for (i = 0; i < minor.Count; i = k + 1)
					{
						if (num2 + 22f > rect4.xMax)
						{
							num2 = rect4.x;
							num3 += 27f;
						}
						if (num3 + 22f > rect4.yMax)
						{
							break;
						}
						Rect rect6 = new Rect(num2, num3 + (rect4.height - 22f) / 2f, 22f, 22f);
						Rect rect7 = rect6;
						Ideo ideo = minor[i];
						bool doTooltip = true;
						Action extraAction;
						if ((extraAction = <>9__3) == null)
						{
							extraAction = (<>9__3 = delegate()
							{
								IdeoUIUtility.OpenIdeoInfo(minor[i]);
							});
						}
						IdeoUIUtility.DoIdeoIcon(rect7, ideo, doTooltip, extraAction);
						num2 += rect6.width + 5f;
						k = i;
					}
				}
			}
			else
			{
				rect4.width = 0f;
			}
			Rect rect8 = new Rect(rect4.xMax, rowY, 70f, 80f);
			if (!faction.IsPlayer)
			{
				string text = faction.PlayerRelationKind.GetLabelCap();
				if (faction.defeated)
				{
					text = text.Colorize(ColorLibrary.Grey);
				}
				GUI.color = faction.PlayerRelationKind.GetColor();
				Text.Anchor = TextAnchor.MiddleCenter;
				if (faction.HasGoodwill && !faction.def.permanentEnemy)
				{
					Widgets.Label(new Rect(rect8.x, rect8.y - 10f, rect8.width, rect8.height), text);
					Text.Font = GameFont.Medium;
					Widgets.Label(new Rect(rect8.x, rect8.y + 10f, rect8.width, rect8.height), faction.PlayerGoodwill.ToStringWithSign());
					Text.Font = GameFont.Small;
				}
				else
				{
					Widgets.Label(rect8, text);
				}
				GenUI.ResetLabelAlign();
				GUI.color = Color.white;
				if (Mouse.IsOver(rect8))
				{
					TaggedString taggedString = "";
					if (faction.def.permanentEnemy)
					{
						taggedString = "CurrentGoodwillTip_PermanentEnemy".Translate();
					}
					else if (faction.HasGoodwill)
					{
						taggedString = "Goodwill".Translate().Colorize(ColoredText.TipSectionTitleColor) + ": " + (faction.PlayerGoodwill.ToStringWithSign() + ", " + faction.PlayerRelationKind.GetLabel()).Colorize(faction.PlayerRelationKind.GetColor());
						TaggedString ongoingEvents = FactionUIUtility.GetOngoingEvents(faction);
						if (!ongoingEvents.NullOrEmpty())
						{
							taggedString += "\n\n" + "OngoingEvents".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + ongoingEvents;
						}
						TaggedString recentEvents = FactionUIUtility.GetRecentEvents(faction);
						if (!recentEvents.NullOrEmpty())
						{
							taggedString += "\n\n" + "RecentEvents".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + recentEvents;
						}
						string s = "";
						switch (faction.PlayerRelationKind)
						{
						case FactionRelationKind.Hostile:
							s = "CurrentGoodwillTip_Hostile".Translate(0.ToString("F0"));
							break;
						case FactionRelationKind.Neutral:
							s = "CurrentGoodwillTip_Neutral".Translate(-75.ToString("F0"), 75.ToString("F0"));
							break;
						case FactionRelationKind.Ally:
							s = "CurrentGoodwillTip_Ally".Translate(0.ToString("F0"));
							break;
						}
						taggedString += "\n\n" + s.Colorize(ColoredText.SubtleGrayColor);
					}
					if (taggedString != "")
					{
						TooltipHandler.TipRegion(rect8, taggedString);
					}
					Widgets.DrawHighlight(rect8);
				}
			}
			Rect rect9 = new Rect(rect8.xMax, rowY, 54f, 80f);
			if (!faction.IsPlayer && faction.HasGoodwill && !faction.def.permanentEnemy)
			{
				FactionRelationKind relationKindForGoodwill = FactionUIUtility.GetRelationKindForGoodwill(faction.NaturalGoodwill);
				GUI.color = relationKindForGoodwill.GetColor();
				Rect rect10 = rect9.ContractedBy(7f);
				rect10.y = rowY + 30f;
				rect10.height = 20f;
				Text.Anchor = TextAnchor.UpperCenter;
				Widgets.DrawRectFast(rect10, Color.black, null);
				Widgets.Label(rect10, faction.NaturalGoodwill.ToStringWithSign());
				GenUI.ResetLabelAlign();
				GUI.color = Color.white;
				if (Mouse.IsOver(rect9))
				{
					TaggedString taggedString2 = "NaturalGoodwill".Translate().Colorize(ColoredText.TipSectionTitleColor) + ": " + faction.NaturalGoodwill.ToStringWithSign().Colorize(relationKindForGoodwill.GetColor());
					int goodwill = Mathf.Clamp(faction.NaturalGoodwill - 50, -100, 100);
					int goodwill2 = Mathf.Clamp(faction.NaturalGoodwill + 50, -100, 100);
					taggedString2 += string.Concat(new string[]
					{
						"\n",
						"NaturalGoodwillRange".Translate().Colorize(ColoredText.TipSectionTitleColor),
						": ",
						goodwill.ToString().Colorize(FactionUIUtility.GetRelationKindForGoodwill(goodwill).GetColor()),
						" "
					}) + "RangeTo".Translate() + " " + goodwill2.ToString().Colorize(FactionUIUtility.GetRelationKindForGoodwill(goodwill2).GetColor());
					TaggedString naturalGoodwillExplanation = FactionUIUtility.GetNaturalGoodwillExplanation(faction);
					if (!naturalGoodwillExplanation.NullOrEmpty())
					{
						taggedString2 += "\n\n" + "AffectedBy".Translate().Colorize(ColoredText.TipSectionTitleColor) + "\n" + naturalGoodwillExplanation;
					}
					taggedString2 += "\n\n" + "NaturalGoodwillDescription".Translate(1.25f.ToStringPercent()).Colorize(ColoredText.SubtleGrayColor);
					TooltipHandler.TipRegion(rect9, taggedString2);
					Widgets.DrawHighlight(rect9);
				}
			}
			float num4 = rect9.xMax + 17f;
			for (int j = 0; j < array.Length; j++)
			{
				if (num4 >= rect9.xMax + num)
				{
					num4 = rect9.xMax;
					rowY += 27f;
				}
				FactionUIUtility.DrawFactionIconWithTooltip(new Rect(num4, rowY + 29f, 22f, 22f), array[j]);
				num4 += 27f;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return 80f;
		}

		// Token: 0x0600BE5A RID: 48730 RVA: 0x00449994 File Offset: 0x00447B94
		public static void DrawFactionIconWithTooltip(Rect r, Faction faction)
		{
			GUI.color = faction.Color;
			GUI.DrawTexture(r, faction.def.FactionIcon);
			GUI.color = Color.white;
			if (Mouse.IsOver(r))
			{
				TipSignal tip = new TipSignal(() => string.Concat(new string[]
				{
					faction.Name.Colorize(ColoredText.TipSectionTitleColor),
					"\n",
					faction.def.LabelCap.Resolve(),
					"\n\n",
					faction.def.Description
				}), faction.loadID ^ 1938473043);
				TooltipHandler.TipRegion(r, tip);
				Widgets.DrawHighlight(r);
			}
			if (Widgets.ButtonInvisible(r, false))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(faction));
			}
		}

		// Token: 0x0600BE5B RID: 48731 RVA: 0x00449A38 File Offset: 0x00447C38
		public static void DrawRelatedFactionInfo(Rect rect, Faction faction, ref float curY)
		{
			Text.Anchor = TextAnchor.LowerRight;
			curY += 10f;
			FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
			string text = faction.Name.CapitalizeFirst() + "\n" + "goodwill".Translate().CapitalizeFirst() + ": " + faction.PlayerGoodwill.ToStringWithSign();
			GUI.color = Color.gray;
			Rect rect2 = new Rect(rect.x, curY, rect.width, Text.CalcHeight(text, rect.width));
			Widgets.Label(rect2, text);
			curY += rect2.height;
			GUI.color = playerRelationKind.GetColor();
			Rect rect3 = new Rect(rect2.x, curY - 7f, rect2.width, 25f);
			Widgets.Label(rect3, playerRelationKind.GetLabelCap());
			curY += rect3.height;
			GUI.color = Color.white;
			GenUI.ResetLabelAlign();
		}

		// Token: 0x0600BE5C RID: 48732 RVA: 0x00449B40 File Offset: 0x00447D40
		private static TaggedString GetRecentEvents(Faction other)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<HistoryEventDef> allDefsListForReading = DefDatabase<HistoryEventDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				int recentCountWithinTicks = Find.HistoryEventsManager.GetRecentCountWithinTicks(allDefsListForReading[i], 3600000, other);
				if (recentCountWithinTicks > 0)
				{
					Find.HistoryEventsManager.GetRecent(allDefsListForReading[i], 3600000, FactionUIUtility.tmpTicks, FactionUIUtility.tmpCustomGoodwill, other);
					int num = 0;
					for (int j = 0; j < FactionUIUtility.tmpTicks.Count; j++)
					{
						num += FactionUIUtility.tmpCustomGoodwill[j];
					}
					if (num != 0)
					{
						string text = "- " + allDefsListForReading[i].LabelCap;
						if (recentCountWithinTicks != 1)
						{
							text = text + " x" + recentCountWithinTicks.ToString();
						}
						text = text + ": " + num.ToStringWithSign().Colorize((num >= 0) ? FactionRelationKind.Ally.GetColor() : FactionRelationKind.Hostile.GetColor());
						stringBuilder.AppendInNewLine(text);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600BE5D RID: 48733 RVA: 0x00449C58 File Offset: 0x00447E58
		private static FactionRelationKind GetRelationKindForGoodwill(int goodwill)
		{
			if (goodwill <= -75)
			{
				return FactionRelationKind.Hostile;
			}
			if (goodwill >= 75)
			{
				return FactionRelationKind.Ally;
			}
			return FactionRelationKind.Neutral;
		}

		// Token: 0x0600BE5E RID: 48734 RVA: 0x00449C6C File Offset: 0x00447E6C
		private static TaggedString GetOngoingEvents(Faction other)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<GoodwillSituationManager.CachedSituation> situations = Find.GoodwillSituationManager.GetSituations(other);
			for (int i = 0; i < situations.Count; i++)
			{
				if (situations[i].maxGoodwill < 100)
				{
					string text = "- " + situations[i].def.Worker.GetPostProcessedLabelCap(other);
					text = text + ": " + (situations[i].maxGoodwill.ToStringWithSign() + " " + "max".Translate()).Colorize(FactionRelationKind.Hostile.GetColor());
					stringBuilder.AppendInNewLine(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600BE5F RID: 48735 RVA: 0x00449D28 File Offset: 0x00447F28
		private static TaggedString GetNaturalGoodwillExplanation(Faction other)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<GoodwillSituationManager.CachedSituation> situations = Find.GoodwillSituationManager.GetSituations(other);
			for (int i = 0; i < situations.Count; i++)
			{
				if (situations[i].naturalGoodwillOffset != 0)
				{
					string text = "- " + situations[i].def.Worker.GetPostProcessedLabelCap(other);
					text = text + ": " + situations[i].naturalGoodwillOffset.ToStringWithSign().Colorize((situations[i].naturalGoodwillOffset >= 0) ? FactionRelationKind.Ally.GetColor() : FactionRelationKind.Hostile.GetColor());
					stringBuilder.AppendInNewLine(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04006D19 RID: 27929
		private static bool showAll;

		// Token: 0x04006D1A RID: 27930
		private static List<Faction> visibleFactions = new List<Faction>();

		// Token: 0x04006D1B RID: 27931
		private const float FactionIconRectSize = 42f;

		// Token: 0x04006D1C RID: 27932
		private const float FactionIconRectGapX = 24f;

		// Token: 0x04006D1D RID: 27933
		private const float RowHeight = 80f;

		// Token: 0x04006D1E RID: 27934
		private const float LabelRowHeight = 50f;

		// Token: 0x04006D1F RID: 27935
		private const float FactionIconSpacing = 5f;

		// Token: 0x04006D20 RID: 27936
		private const float IdeoIconSpacing = 5f;

		// Token: 0x04006D21 RID: 27937
		private const float BasicsColumnWidth = 300f;

		// Token: 0x04006D22 RID: 27938
		private const float InfoColumnWidth = 40f;

		// Token: 0x04006D23 RID: 27939
		private const float IdeosColumnWidth = 60f;

		// Token: 0x04006D24 RID: 27940
		private const float RelationsColumnWidth = 70f;

		// Token: 0x04006D25 RID: 27941
		private const float NaturalGoodwillColumnWidth = 54f;

		// Token: 0x04006D26 RID: 27942
		private static List<int> tmpTicks = new List<int>();

		// Token: 0x04006D27 RID: 27943
		private static List<int> tmpCustomGoodwill = new List<int>();
	}
}
