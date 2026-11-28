using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000092 RID: 146
	public class GameplayTipWindow
	{
		// Token: 0x06000678 RID: 1656 RVA: 0x00025B14 File Offset: 0x00023D14
		public static void DrawWindow(Vector2 offset, bool useWindowStack)
		{
			if (GameplayTipWindow.allTipsCached == null)
			{
				GameplayTipWindow.allTipsCached = DefDatabase<TipSetDef>.AllDefsListForReading.SelectMany(delegate(TipSetDef set)
				{
					if (SteamDeck.IsSteamDeck && set == TipSetDefOf.GameplayTips)
					{
						return set.tips.Skip(11);
					}
					return set.tips;
				}).InRandomOrder(null).ToList<string>();
			}
			Rect rect = new Rect(offset.x, offset.y, GameplayTipWindow.WindowSize.x, GameplayTipWindow.WindowSize.y);
			if (useWindowStack)
			{
				Find.WindowStack.ImmediateWindow(62893997, rect, WindowLayer.Super, delegate
				{
					GameplayTipWindow.DrawContents(rect.AtZero());
				}, true, false, 1f, null, false);
				return;
			}
			Widgets.DrawShadowAround(rect);
			Widgets.DrawWindowBackground(rect);
			GameplayTipWindow.DrawContents(rect);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00025BE4 File Offset: 0x00023DE4
		private static void DrawContents(Rect rect)
		{
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			if (Time.realtimeSinceStartup - GameplayTipWindow.lastTimeUpdatedTooltip > 17.5f || GameplayTipWindow.lastTimeUpdatedTooltip < 0f)
			{
				GameplayTipWindow.currentTipIndex = (GameplayTipWindow.currentTipIndex + 1) % GameplayTipWindow.allTipsCached.Count;
				GameplayTipWindow.lastTimeUpdatedTooltip = Time.realtimeSinceStartup;
			}
			Rect rect2 = rect;
			rect2.x += GameplayTipWindow.TextMargin.x;
			rect2.width -= GameplayTipWindow.TextMargin.x * 2f;
			rect2.y += GameplayTipWindow.TextMargin.y;
			rect2.height -= GameplayTipWindow.TextMargin.y * 2f;
			Widgets.Label(rect2, GameplayTipWindow.allTipsCached[GameplayTipWindow.currentTipIndex]);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00025CC5 File Offset: 0x00023EC5
		public static void ResetTipTimer()
		{
			GameplayTipWindow.lastTimeUpdatedTooltip = -1f;
		}

		// Token: 0x0400022D RID: 557
		private static List<string> allTipsCached;

		// Token: 0x0400022E RID: 558
		private static float lastTimeUpdatedTooltip = -1f;

		// Token: 0x0400022F RID: 559
		private static int currentTipIndex = 0;

		// Token: 0x04000230 RID: 560
		public const float tipUpdateInterval = 17.5f;

		// Token: 0x04000231 RID: 561
		public static readonly Vector2 WindowSize = new Vector2(776f, 60f);

		// Token: 0x04000232 RID: 562
		private static readonly Vector2 TextMargin = new Vector2(15f, 8f);

		// Token: 0x04000233 RID: 563
		private const int InterfaceTipsCount = 11;
	}
}
