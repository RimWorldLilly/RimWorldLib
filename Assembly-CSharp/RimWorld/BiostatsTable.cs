using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C26 RID: 7206
	public static class BiostatsTable
	{
		// Token: 0x0600B7C0 RID: 47040 RVA: 0x0040C6D4 File Offset: 0x0040A8D4
		private static float MaxLabelWidth(int arc)
		{
			float num = 0f;
			int num2 = (arc > 0) ? BiostatsTable.Biostats.Length : (BiostatsTable.Biostats.Length - 1);
			for (int i = 0; i < num2; i++)
			{
				num = Mathf.Max(num, Text.CalcSize(BiostatsTable.Biostats[i].labelKey.Translate().CapitalizeFirst()).x);
			}
			return num;
		}

		// Token: 0x0600B7C1 RID: 47041 RVA: 0x0040C740 File Offset: 0x0040A940
		public static float HeightForBiostats(int arc)
		{
			float num = Text.LineHeight * 3f;
			if (arc > 0)
			{
				num += Text.LineHeight * 1.5f;
			}
			return num;
		}

		// Token: 0x0600B7C2 RID: 47042 RVA: 0x0040C76C File Offset: 0x0040A96C
		public static void Draw(Rect rect, int gcx, int met, int arc, bool drawMax, bool ignoreLimits, int maxGCX = -1)
		{
			int num = (arc > 0) ? BiostatsTable.Biostats.Length : (BiostatsTable.Biostats.Length - 1);
			float num2 = BiostatsTable.MaxLabelWidth(arc);
			float num3 = rect.height / (float)num;
			GUI.BeginGroup(rect);
			for (int i = 0; i < num; i++)
			{
				Rect position = new Rect(0f, (float)i * num3 + (num3 - 22f) / 2f, 22f, 22f);
				Rect rect2 = new Rect(position.xMax + 4f, (float)i * num3, num2, num3);
				Rect rect3 = new Rect(0f, rect2.y, rect.width, rect2.height);
				if (i % 2 == 1)
				{
					Widgets.DrawLightHighlight(rect3);
				}
				Widgets.DrawHighlightIfMouseover(rect3);
				rect3.xMax = rect2.xMax + 4f + 90f;
				TaggedString taggedString = BiostatsTable.Biostats[i].descKey.Translate();
				if (maxGCX >= 0 && BiostatsTable.Biostats[i].displayMaxGCXInfo)
				{
					taggedString += "\n\n" + "MaxComplexityDesc".Translate();
				}
				TooltipHandler.TipRegion(rect3, taggedString);
				GUI.DrawTexture(position, BiostatsTable.Biostats[i].icon);
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect2, BiostatsTable.Biostats[i].labelKey.Translate().CapitalizeFirst());
				Text.Anchor = TextAnchor.UpperLeft;
			}
			float num4 = num2 + 4f + 22f + 4f;
			string text = gcx.ToString();
			string text2 = met.ToStringWithSign();
			if (drawMax && !ignoreLimits)
			{
				if (maxGCX >= 0)
				{
					if (gcx > maxGCX)
					{
						text = text.Colorize(ColorLibrary.RedReadable);
					}
					text = text + " / " + maxGCX.ToString();
				}
				if (met < GeneTuning.BiostatRange.TrueMin)
				{
					text2 = text2 + (" (" + "min".Translate() + " ") + GeneTuning.BiostatRange.TrueMin.ToString() + ")";
					text2 = text2.Colorize(ColorLibrary.RedReadable);
				}
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(new Rect(num4, 0f, 90f, num3), text);
			Widgets.Label(new Rect(num4, num3, 90f, num3), text2);
			if (arc > 0)
			{
				Widgets.Label(new Rect(num4, num3 * 2f, 90f, num3), arc.ToString());
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			float width = rect.width - num2 - 90f - 22f - 4f;
			Rect rect4 = new Rect(num4 + 90f + 4f, num3, width, num3);
			if (rect4.width != BiostatsTable.cachedWidth)
			{
				BiostatsTable.cachedWidth = rect4.width;
				BiostatsTable.truncateCache.Clear();
			}
			string str = BiostatsTable.MetabolismDescAt(met);
			Widgets.Label(rect4, str.Truncate(rect4.width, BiostatsTable.truncateCache));
			if (Mouse.IsOver(rect4) && !str.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect4, str);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.EndGroup();
		}

		// Token: 0x0600B7C3 RID: 47043 RVA: 0x0040CABF File Offset: 0x0040ACBF
		private static string MetabolismDescAt(int met)
		{
			if (met == 0)
			{
				return string.Empty;
			}
			return "HungerRate".Translate() + " x" + GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate((float)met).ToStringPercent();
		}

		// Token: 0x0400694F RID: 26959
		private static float cachedWidth;

		// Token: 0x04006950 RID: 26960
		private const float NumberWidth = 90f;

		// Token: 0x04006951 RID: 26961
		private const float IconSize = 22f;

		// Token: 0x04006952 RID: 26962
		private static readonly BiostatsTable.BiostatData[] Biostats = new BiostatsTable.BiostatData[]
		{
			new BiostatsTable.BiostatData("Complexity", "ComplexityDesc", GeneUtility.GCXTex.Texture, true),
			new BiostatsTable.BiostatData("Metabolism", "MetabolismDesc", GeneUtility.METTex.Texture, false),
			new BiostatsTable.BiostatData("ArchitesRequired", "ArchitesRequiredDesc", GeneUtility.ARCTex.Texture, false)
		};

		// Token: 0x04006953 RID: 26963
		private static Dictionary<string, string> truncateCache = new Dictionary<string, string>();

		// Token: 0x020038CB RID: 14539
		private struct BiostatData
		{
			// Token: 0x06014B4D RID: 84813 RVA: 0x006A0D3B File Offset: 0x0069EF3B
			public BiostatData(string labelKey, string descKey, Texture2D icon, bool displayMaxGCXInfo)
			{
				this.labelKey = labelKey;
				this.descKey = descKey;
				this.icon = icon;
				this.displayMaxGCXInfo = displayMaxGCXInfo;
			}

			// Token: 0x0400E2C7 RID: 58055
			public string labelKey;

			// Token: 0x0400E2C8 RID: 58056
			public string descKey;

			// Token: 0x0400E2C9 RID: 58057
			public Texture2D icon;

			// Token: 0x0400E2CA RID: 58058
			public bool displayMaxGCXInfo;
		}
	}
}
