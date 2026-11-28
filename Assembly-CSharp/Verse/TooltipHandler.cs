using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000640 RID: 1600
	public static class TooltipHandler
	{
		// Token: 0x06003229 RID: 12841 RVA: 0x0013FAC0 File Offset: 0x0013DCC0
		public static void ClearTooltipsFrom(Rect rect)
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.dyingTips.Clear();
				foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
				{
					if (keyValuePair.Value.lastTriggerFrame == TooltipHandler.frame)
					{
						TooltipHandler.dyingTips.Add(keyValuePair.Key);
					}
				}
				for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
				{
					TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
				}
			}
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x0013FB80 File Offset: 0x0013DD80
		public static void TipRegion(Rect rect, Func<string> textGetter, int uniqueId)
		{
			TooltipHandler.TipRegion(rect, new TipSignal(textGetter, uniqueId));
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x0013FB8F File Offset: 0x0013DD8F
		public static void TipRegionByKey(Rect rect, string key)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate());
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x0013FBB2 File Offset: 0x0013DDB2
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1));
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x0013FBD6 File Offset: 0x0013DDD6
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2));
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x0013FBFB File Offset: 0x0013DDFB
		public static void TipRegionByKey(Rect rect, string key, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			TooltipHandler.TipRegion(rect, key.Translate(arg1, arg2, arg3));
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x0013FC24 File Offset: 0x0013DE24
		public static void TipRegion(Rect rect, TipSignal tip)
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			if (!Mouse.IsOver(rect) && !DebugViewSettings.drawTooltipEdges)
			{
				return;
			}
			if (tip.textGetter == null && tip.text.NullOrEmpty())
			{
				return;
			}
			if (SteamDeck.KeyboardShowing)
			{
				return;
			}
			if (DebugViewSettings.drawTooltipEdges)
			{
				Widgets.DrawBox(rect, 1, null);
			}
			if (!TooltipHandler.activeTips.ContainsKey(tip.uniqueId))
			{
				ActiveTip value = new ActiveTip(tip);
				TooltipHandler.activeTips.Add(tip.uniqueId, value);
				TooltipHandler.activeTips[tip.uniqueId].firstTriggerTime = (double)Time.realtimeSinceStartup;
			}
			TooltipHandler.activeTips[tip.uniqueId].lastTriggerFrame = TooltipHandler.frame;
			TooltipHandler.activeTips[tip.uniqueId].signal.text = tip.text;
			TooltipHandler.activeTips[tip.uniqueId].signal.textGetter = tip.textGetter;
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x0013FD1B File Offset: 0x0013DF1B
		public static void DoTooltipGUI()
		{
			if (CellInspectorDrawer.active)
			{
				return;
			}
			TooltipHandler.DrawActiveTips();
			if (Event.current.type == EventType.repaint)
			{
				TooltipHandler.CleanActiveTooltips();
				TooltipHandler.frame++;
			}
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x0013FD48 File Offset: 0x0013DF48
		private static void DrawActiveTips()
		{
			if (TooltipHandler.activeTips.Count == 0)
			{
				return;
			}
			TooltipHandler.drawingTips.Clear();
			foreach (ActiveTip activeTip in TooltipHandler.activeTips.Values)
			{
				if ((double)Time.realtimeSinceStartup > activeTip.firstTriggerTime + (double)activeTip.signal.delay)
				{
					TooltipHandler.drawingTips.Add(activeTip);
				}
			}
			if (TooltipHandler.drawingTips.Any<ActiveTip>())
			{
				TooltipHandler.drawingTips.SortStable(TooltipHandler.compareTooltipsByPriorityCached);
				Vector2 pos = TooltipHandler.CalculateInitialTipPosition(TooltipHandler.drawingTips);
				for (int i = 0; i < TooltipHandler.drawingTips.Count; i++)
				{
					pos.y += TooltipHandler.drawingTips[i].DrawTooltip(pos);
					pos.y += 2f;
				}
				TooltipHandler.drawingTips.Clear();
			}
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x0013FE48 File Offset: 0x0013E048
		private static void CleanActiveTooltips()
		{
			TooltipHandler.dyingTips.Clear();
			foreach (KeyValuePair<int, ActiveTip> keyValuePair in TooltipHandler.activeTips)
			{
				if (keyValuePair.Value.lastTriggerFrame != TooltipHandler.frame)
				{
					TooltipHandler.dyingTips.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < TooltipHandler.dyingTips.Count; i++)
			{
				TooltipHandler.activeTips.Remove(TooltipHandler.dyingTips[i]);
			}
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x0013FEEC File Offset: 0x0013E0EC
		private static Vector2 CalculateInitialTipPosition(List<ActiveTip> drawingTips)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < drawingTips.Count; i++)
			{
				Rect tipRect = drawingTips[i].TipRect;
				num += tipRect.height;
				num2 = Mathf.Max(num2, tipRect.width);
				if (i != drawingTips.Count - 1)
				{
					num += 2f;
				}
			}
			return GenUI.GetMouseAttachedWindowPos(num2, num);
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x0013FF58 File Offset: 0x0013E158
		private static int CompareTooltipsByPriority(ActiveTip A, ActiveTip B)
		{
			int num = (int)(-(int)A.signal.priority);
			int value = (int)(-(int)B.signal.priority);
			return num.CompareTo(value);
		}

		// Token: 0x04002016 RID: 8214
		private static Dictionary<int, ActiveTip> activeTips = new Dictionary<int, ActiveTip>();

		// Token: 0x04002017 RID: 8215
		private static int frame = 0;

		// Token: 0x04002018 RID: 8216
		private static List<int> dyingTips = new List<int>(32);

		// Token: 0x04002019 RID: 8217
		private const float SpaceBetweenTooltips = 2f;

		// Token: 0x0400201A RID: 8218
		private static List<ActiveTip> drawingTips = new List<ActiveTip>();

		// Token: 0x0400201B RID: 8219
		private static Func<ActiveTip, ActiveTip, int> compareTooltipsByPriorityCached = new Func<ActiveTip, ActiveTip, int>(TooltipHandler.CompareTooltipsByPriority);
	}
}
