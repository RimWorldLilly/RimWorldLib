using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005FC RID: 1532
	public static class EnvironmentStatsDrawer
	{
		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x001349F8 File Offset: 0x00132BF8
		private static int DisplayedRoomStatsCount
		{
			get
			{
				int num = 0;
				List<RoomStatDef> allDefsListForReading = DefDatabase<RoomStatDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (!allDefsListForReading[i].isHidden || DebugViewSettings.showAllRoomStats)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x00134A38 File Offset: 0x00132C38
		private static bool ShouldShowWindowNow()
		{
			return (EnvironmentStatsDrawer.ShouldShowRoomStats() || EnvironmentStatsDrawer.ShouldShowBeauty()) && !Mouse.IsInputBlockedNow;
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x00134A54 File Offset: 0x00132C54
		private static bool ShouldShowRoomStats()
		{
			if (!Find.PlaySettings.showRoomStats)
			{
				return false;
			}
			if (Find.CurrentMap == null)
			{
				return false;
			}
			if (!UI.MouseCell().InBounds(Find.CurrentMap) || UI.MouseCell().Fogged(Find.CurrentMap))
			{
				return false;
			}
			Room room = UI.MouseCell().GetRoom(Find.CurrentMap);
			return room != null && room.Role != RoomRoleDefOf.None;
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x00134AC4 File Offset: 0x00132CC4
		private static bool ShouldShowBeauty()
		{
			return Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && UI.MouseCell().GetRoom(Find.CurrentMap) != null;
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x00134B15 File Offset: 0x00132D15
		public static void EnvironmentStatsOnGUI()
		{
			if (Event.current.type != EventType.repaint || !EnvironmentStatsDrawer.ShouldShowWindowNow())
			{
				return;
			}
			EnvironmentStatsDrawer.DrawInfoWindow();
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x00134B34 File Offset: 0x00132D34
		private static void DrawInfoWindow()
		{
			Text.Font = GameFont.Small;
			Rect windowRect = EnvironmentStatsDrawer.GetWindowRect(EnvironmentStatsDrawer.ShouldShowBeauty(), EnvironmentStatsDrawer.ShouldShowRoomStats());
			Find.WindowStack.ImmediateWindow(74975, windowRect, WindowLayer.GameUI, delegate
			{
				EnvironmentStatsDrawer.FillWindow(windowRect);
			}, true, false, 1f, null, false);
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x00134B90 File Offset: 0x00132D90
		public static Rect GetWindowRect(bool shouldShowBeauty, bool shouldShowRoomStats)
		{
			Rect result = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 424f, 24f);
			int num = 0;
			if (shouldShowBeauty)
			{
				num++;
				result.height += 25f;
			}
			if (shouldShowRoomStats)
			{
				num++;
				result.height += 23f;
				result.height += (float)EnvironmentStatsDrawer.DisplayedRoomStatsCount * 25f + 23f;
			}
			result.height += 13f * (float)(num - 1);
			result.x += 26f;
			result.y += 26f;
			if (result.xMax > (float)UI.screenWidth)
			{
				result.x -= result.width + 52f;
			}
			if (result.yMax > (float)UI.screenHeight)
			{
				result.y -= result.height + 52f;
			}
			return result;
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x00134CB8 File Offset: 0x00132EB8
		private static void FillWindow(Rect windowRect)
		{
			EnvironmentStatsDrawer.<>c__DisplayClass21_0 CS$<>8__locals1;
			CS$<>8__locals1.windowRect = windowRect;
			Text.Font = GameFont.Small;
			CS$<>8__locals1.curY = 12f;
			CS$<>8__locals1.dividingLinesSeen = 0;
			if (EnvironmentStatsDrawer.ShouldShowBeauty())
			{
				EnvironmentStatsDrawer.<FillWindow>g__DrawDividingLineIfNecessary|21_0(ref CS$<>8__locals1);
				float beauty = BeautyUtility.AverageBeautyPerceptible(UI.MouseCell(), Find.CurrentMap);
				Rect rect = new Rect(22f, CS$<>8__locals1.curY, CS$<>8__locals1.windowRect.width - 24f - 10f, 100f);
				GUI.color = BeautyDrawer.BeautyColor(beauty, 40f);
				Widgets.Label(rect, "BeautyHere".Translate() + ": " + beauty.ToString("F1"));
				CS$<>8__locals1.curY += 25f;
			}
			if (EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				EnvironmentStatsDrawer.<FillWindow>g__DrawDividingLineIfNecessary|21_0(ref CS$<>8__locals1);
				EnvironmentStatsDrawer.DoRoomInfo(UI.MouseCell().GetRoom(Find.CurrentMap), ref CS$<>8__locals1.curY, CS$<>8__locals1.windowRect);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x00134DBC File Offset: 0x00132FBC
		public static void DrawRoomOverlays()
		{
			if (Find.PlaySettings.showBeauty && UI.MouseCell().InBounds(Find.CurrentMap))
			{
				GenUI.RenderMouseoverBracket();
			}
			if (EnvironmentStatsDrawer.ShouldShowWindowNow() && EnvironmentStatsDrawer.ShouldShowRoomStats())
			{
				Room room = UI.MouseCell().GetRoom(Find.CurrentMap);
				if (room != null && room.Role != RoomRoleDefOf.None)
				{
					room.DrawFieldEdges();
				}
			}
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x00134E20 File Offset: 0x00133020
		public static void DoRoomInfo(Room room, ref float curY, Rect windowRect)
		{
			Rect rect = new Rect(12f, curY, windowRect.width - 24f, 100f);
			GUI.color = Color.white;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(rect.x + 10f, curY, rect.width - 10f, rect.height), room.GetRoomRoleLabel().CapitalizeFirst());
			curY += 30f;
			Text.Font = GameFont.Small;
			Text.WordWrap = false;
			int num = 0;
			bool flag = false;
			for (int i = 0; i < DefDatabase<RoomStatDef>.AllDefsListForReading.Count; i++)
			{
				RoomStatDef roomStatDef = DefDatabase<RoomStatDef>.AllDefsListForReading[i];
				if (!roomStatDef.isHidden || DebugViewSettings.showAllRoomStats)
				{
					float stat = room.GetStat(roomStatDef);
					RoomStatScoreStage scoreStage = roomStatDef.GetScoreStage(stat);
					GUI.color = Color.white;
					Rect rect2 = new Rect(rect.x, curY, rect.width, 23f);
					if (num % 2 == 1)
					{
						Widgets.DrawLightHighlight(rect2);
					}
					Rect rect3 = new Rect(rect.x, curY, 10f, 23f);
					if (room.Role.IsStatRelated(roomStatDef))
					{
						flag = true;
						Widgets.Label(rect3, "*");
						GUI.color = EnvironmentStatsDrawer.RelatedStatColor;
					}
					else
					{
						GUI.color = EnvironmentStatsDrawer.UnrelatedStatColor;
					}
					Rect rect4 = new Rect(rect3.xMax, curY, 110f, 23f);
					Widgets.Label(rect4, roomStatDef.LabelCap);
					Rect rect5 = new Rect(rect4.xMax + 35f, curY, 50f, 23f);
					string label = roomStatDef.ScoreToString(stat);
					Widgets.Label(rect5, label);
					Widgets.Label(new Rect(rect5.xMax + 35f, curY, 160f, 23f), (scoreStage == null) ? "" : scoreStage.label.CapitalizeFirst());
					curY += 25f;
					num++;
				}
			}
			if (flag)
			{
				GUI.color = Color.grey;
				Text.Font = GameFont.Tiny;
				Widgets.Label(new Rect(rect.x, curY, rect.width, 23f), "* " + "StatRelatesToCurrentRoom".Translate());
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
			}
			Text.WordWrap = true;
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x001350A4 File Offset: 0x001332A4
		[CompilerGenerated]
		internal static void <FillWindow>g__DrawDividingLineIfNecessary|21_0(ref EnvironmentStatsDrawer.<>c__DisplayClass21_0 A_0)
		{
			int dividingLinesSeen = A_0.dividingLinesSeen;
			A_0.dividingLinesSeen = dividingLinesSeen + 1;
			if (A_0.dividingLinesSeen <= 1)
			{
				return;
			}
			A_0.curY += 5f;
			GUI.color = new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(12f, A_0.curY, A_0.windowRect.width - 24f);
			GUI.color = Color.white;
			A_0.curY += 8f;
		}

		// Token: 0x04001EC3 RID: 7875
		private const float StatLabelColumnWidth = 110f;

		// Token: 0x04001EC4 RID: 7876
		private const float StatGutterColumnWidth = 10f;

		// Token: 0x04001EC5 RID: 7877
		private const float ScoreColumnWidth = 50f;

		// Token: 0x04001EC6 RID: 7878
		private const float ScoreStageLabelColumnWidth = 160f;

		// Token: 0x04001EC7 RID: 7879
		private static readonly Color RelatedStatColor = new Color(0.85f, 0.85f, 0.85f);

		// Token: 0x04001EC8 RID: 7880
		private static readonly Color UnrelatedStatColor = Color.gray;

		// Token: 0x04001EC9 RID: 7881
		private const float DistFromMouse = 26f;

		// Token: 0x04001ECA RID: 7882
		public const float WindowPadding = 12f;

		// Token: 0x04001ECB RID: 7883
		private const float LineHeight = 23f;

		// Token: 0x04001ECC RID: 7884
		private const float FootnoteHeight = 23f;

		// Token: 0x04001ECD RID: 7885
		private const float TitleHeight = 30f;

		// Token: 0x04001ECE RID: 7886
		private const float SpaceBetweenLines = 2f;

		// Token: 0x04001ECF RID: 7887
		private const float SpaceBetweenColumns = 35f;
	}
}
