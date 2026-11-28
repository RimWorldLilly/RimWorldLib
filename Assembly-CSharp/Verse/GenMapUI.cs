using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000378 RID: 888
	[StaticConstructorOnStartup]
	public static class GenMapUI
	{
		// Token: 0x06001B1D RID: 6941 RVA: 0x000A6E90 File Offset: 0x000A5090
		public static Vector2 LabelDrawPosFor(Thing thing, float worldOffsetZ)
		{
			Vector3 drawPos = thing.DrawPos;
			drawPos.z += worldOffsetZ;
			Vector2 vector = Find.Camera.WorldToScreenPoint(drawPos) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			if (thing is Pawn)
			{
				Pawn pawn = (Pawn)thing;
				if (!pawn.RaceProps.Humanlike)
				{
					vector.y -= 4f;
				}
				else if (pawn.DevelopmentalStage.Baby())
				{
					vector.y -= 8f;
				}
			}
			return vector;
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x000A6F2C File Offset: 0x000A512C
		public static Vector2 LabelDrawPosFor(IntVec3 center)
		{
			Vector3 position = center.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Vector2 vector = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			vector.y -= 1f;
			return vector;
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x000A6F83 File Offset: 0x000A5183
		public static void DrawThingLabel(Thing thing, string text)
		{
			GenMapUI.DrawThingLabel(thing, text, GenMapUI.DefaultThingLabelColor);
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x000A6F91 File Offset: 0x000A5191
		public static void DrawThingLabel(Thing thing, string text, Color textColor)
		{
			GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(thing, -0.4f), text, textColor);
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x000A6FA8 File Offset: 0x000A51A8
		public static void DrawThingLabel(Vector2 screenPos, string text, Color textColor)
		{
			Text.Font = GameFont.Tiny;
			float x = Text.CalcSize(text).x;
			float num = Text.TinyFontSupported ? 4f : 6f;
			float height = Text.TinyFontSupported ? 12f : 16f;
			GUI.DrawTexture(new Rect(screenPos.x - x / 2f - num, screenPos.y, x + num * 2f, height), TexUI.GrayTextBG);
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(screenPos.x - x / 2f, screenPos.y - 3f, x, 999f), text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x000A706C File Offset: 0x000A526C
		public static void DrawPawnLabel(Pawn pawn, Vector2 pos, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			float num = Prefs.DisableTinyText ? 6f : 4f;
			float height = Prefs.DisableTinyText ? 16f : 12f;
			Rect bgRect = new Rect(pos.x - pawnLabelNameWidth / 2f - num, pos.y, pawnLabelNameWidth + num * 2f, height);
			GenMapUI.DrawPawnLabel(pawn, bgRect, alpha, truncateToWidth, truncatedLabelsCache, font, alwaysDrawBg, alignCenter);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x000A70E8 File Offset: 0x000A52E8
		public static void DrawPawnLabel(Pawn pawn, Rect bgRect, float alpha = 1f, float truncateToWidth = 9999f, Dictionary<string, string> truncatedLabelsCache = null, GameFont font = GameFont.Tiny, bool alwaysDrawBg = true, bool alignCenter = true)
		{
			GUI.color = new Color(1f, 1f, 1f, alpha);
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float pawnLabelNameWidth = GenMapUI.GetPawnLabelNameWidth(pawn, truncateToWidth, truncatedLabelsCache, font);
			float summaryHealthPercent = pawn.health.summaryHealth.SummaryHealthPercent;
			if (alwaysDrawBg || summaryHealthPercent < 0.999f)
			{
				GUI.DrawTexture(bgRect, TexUI.GrayTextBG);
			}
			if (summaryHealthPercent < 0.999f)
			{
				Widgets.FillableBar(bgRect.ContractedBy(1f), summaryHealthPercent, GenMapUI.OverlayHealthTex, BaseContent.ClearTex, false);
			}
			Color color = PawnNameColorUtility.PawnNameColorOf(pawn);
			color.a = alpha;
			GUI.color = color;
			Rect rect;
			if (alignCenter)
			{
				Text.Anchor = TextAnchor.UpperCenter;
				rect = new Rect(bgRect.center.x - pawnLabelNameWidth / 2f, bgRect.y - 2f, pawnLabelNameWidth, 100f);
			}
			else
			{
				Text.Anchor = TextAnchor.UpperLeft;
				rect = new Rect(bgRect.x + 2f, bgRect.center.y - Text.CalcSize(pawnLabel).y / 2f, pawnLabelNameWidth, 100f);
			}
			Widgets.Label(rect, pawnLabel);
			if (pawn.Drafted)
			{
				Widgets.DrawLineHorizontal(bgRect.center.x - pawnLabelNameWidth / 2f, bgRect.y + 11f + (float)(Text.TinyFontSupported ? 0 : 3), pawnLabelNameWidth);
			}
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x000A725C File Offset: 0x000A545C
		public static void DrawText(Vector2 worldPos, string text, Color textColor)
		{
			Vector3 position = new Vector3(worldPos.x, 0f, worldPos.y);
			Vector2 vector = Find.Camera.WorldToScreenPoint(position) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			Text.Font = GameFont.Tiny;
			GUI.color = textColor;
			Text.Anchor = TextAnchor.UpperCenter;
			float x = Text.CalcSize(text).x;
			Widgets.Label(new Rect(vector.x - x / 2f, vector.y - 2f, x, 999f), text);
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x000A730C File Offset: 0x000A550C
		private static float GetPawnLabelNameWidth(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string pawnLabel = GenMapUI.GetPawnLabel(pawn, truncateToWidth, truncatedLabelsCache, font);
			float num;
			if (font == GameFont.Tiny)
			{
				num = pawnLabel.GetWidthCached();
			}
			else
			{
				num = Text.CalcSize(pawnLabel).x;
			}
			if (Math.Abs(Math.Round((double)Prefs.UIScale) - (double)Prefs.UIScale) > 1.401298464324817E-45)
			{
				num += 0.5f;
			}
			if (num < 20f)
			{
				num = 20f;
			}
			Text.Font = font2;
			return num;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x000A7384 File Offset: 0x000A5584
		private static string GetPawnLabel(Pawn pawn, float truncateToWidth, Dictionary<string, string> truncatedLabelsCache, GameFont font)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			string result = pawn.LabelShortCap.Truncate(truncateToWidth, truncatedLabelsCache);
			Text.Font = font2;
			return result;
		}

		// Token: 0x04001639 RID: 5689
		public static readonly Texture2D OverlayHealthTex = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.25f));

		// Token: 0x0400163A RID: 5690
		public static readonly Texture2D OverlayEntropyTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.55f, 0.84f, 0.5f));

		// Token: 0x0400163B RID: 5691
		public const float NameBGHeight_Tiny = 12f;

		// Token: 0x0400163C RID: 5692
		public const float NameBGExtraWidth_Tiny = 4f;

		// Token: 0x0400163D RID: 5693
		public const float NameBGHeight_Small = 16f;

		// Token: 0x0400163E RID: 5694
		public const float NameBGExtraWidth_Small = 6f;

		// Token: 0x0400163F RID: 5695
		public const float LabelOffsetYStandard = -0.4f;

		// Token: 0x04001640 RID: 5696
		public const float PsychicEntropyBarHeight = 4f;

		// Token: 0x04001641 RID: 5697
		private const float AnimalLabelNudgeUpPixels = 4f;

		// Token: 0x04001642 RID: 5698
		private const float BabyLabelNudgeUpPixels = 8f;

		// Token: 0x04001643 RID: 5699
		public static readonly Color DefaultThingLabelColor = new Color(1f, 1f, 1f, 0.75f);
	}
}
