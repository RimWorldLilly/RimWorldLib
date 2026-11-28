using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005FA RID: 1530
	public static class BeautyDrawer
	{
		// Token: 0x06003009 RID: 12297 RVA: 0x001338F0 File Offset: 0x00131AF0
		public static void BeautyDrawerOnGUI()
		{
			if (Event.current.type != EventType.repaint || !BeautyDrawer.ShouldShow())
			{
				return;
			}
			BeautyDrawer.DrawBeautyAroundMouse();
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x0013390C File Offset: 0x00131B0C
		private static bool ShouldShow()
		{
			return !Mouse.IsInputBlockedNow && UI.MouseCell().InBounds(Find.CurrentMap) && !UI.MouseCell().Fogged(Find.CurrentMap) && (CellInspectorDrawer.active || Find.PlaySettings.showBeauty);
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x00133960 File Offset: 0x00131B60
		private static void DrawBeautyAroundMouse()
		{
			BeautyUtility.FillBeautyRelevantCells(UI.MouseCell(), Find.CurrentMap);
			for (int i = 0; i < BeautyUtility.beautyRelevantCells.Count; i++)
			{
				IntVec3 intVec = BeautyUtility.beautyRelevantCells[i];
				float num = BeautyUtility.CellBeauty(intVec, Find.CurrentMap, BeautyDrawer.beautyCountedThings);
				if (num != 0f)
				{
					GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(intVec), Mathf.RoundToInt(num).ToStringCached(), BeautyDrawer.BeautyColor(num, 8f));
				}
			}
			BeautyDrawer.beautyCountedThings.Clear();
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x001339EC File Offset: 0x00131BEC
		public static Color BeautyColor(float beauty, float scale)
		{
			float num = Mathf.InverseLerp(-scale, scale, beauty);
			num = Mathf.Clamp01(num);
			return Color.Lerp(Color.Lerp(BeautyDrawer.ColorUgly, BeautyDrawer.ColorBeautiful, num), Color.white, 0.5f);
		}

		// Token: 0x04001EB6 RID: 7862
		private static HashSet<Thing> beautyCountedThings = new HashSet<Thing>();

		// Token: 0x04001EB7 RID: 7863
		private static Color ColorUgly = Color.red;

		// Token: 0x04001EB8 RID: 7864
		private static Color ColorBeautiful = Color.green;
	}
}
