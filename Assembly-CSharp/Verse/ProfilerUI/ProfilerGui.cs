using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using UnityStubs.Gui;
using Verse.Sound;

namespace Verse.ProfilerUI
{
	// Token: 0x02000790 RID: 1936
	public static class ProfilerGui
	{
		// Token: 0x06003D6D RID: 15725 RVA: 0x0018705B File Offset: 0x0018525B
		public static Color Hex(uint argb)
		{
			return new Color32((byte)(argb >> 16), (byte)(argb >> 8), (byte)argb, (byte)(argb >> 24));
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x06003D6E RID: 15726 RVA: 0x00187077 File Offset: 0x00185277
		public static bool Painting
		{
			get
			{
				return Event.current.type == EventType.repaint;
			}
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x00187086 File Offset: 0x00185286
		public static void Fill(Rect rect, Color color)
		{
			if (ProfilerGui.Painting && rect.width > 0f && rect.height > 0f)
			{
				GuiBackend.Current.FillRect(rect, color);
			}
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x001870B7 File Offset: 0x001852B7
		public static void Fill(float x0, float y0, float x1, float y1, Color color)
		{
			ProfilerGui.Fill(Rect.MinMaxRect(x0, y0, x1, y1), color);
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x001870CC File Offset: 0x001852CC
		public static void Arrow(Rect cell, bool down, Color color)
		{
			if (!ProfilerGui.Painting)
			{
				return;
			}
			Vector2 center = cell.center;
			if (down)
			{
				GuiBackend.Current.DrawTextureQuad(new Vector2(center.x - 3.5f, center.y - 2.5f), new Vector2(center.x + 3.5f, center.y - 2.5f), new Vector2(center.x, center.y + 2.5f), new Vector2(center.x, center.y + 2.5f), null, color);
				return;
			}
			GuiBackend.Current.DrawTextureQuad(new Vector2(center.x - 2.5f, center.y - 3.5f), new Vector2(center.x + 2.5f, center.y), new Vector2(center.x - 2.5f, center.y + 3.5f), new Vector2(center.x - 2.5f, center.y + 3.5f), null, color);
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x001871DC File Offset: 0x001853DC
		public static void Outline(Rect rect, Color color)
		{
			ProfilerGui.Fill(new Rect(rect.x, rect.y, rect.width, 1f), color);
			ProfilerGui.Fill(new Rect(rect.x, rect.yMax - 1f, rect.width, 1f), color);
			ProfilerGui.Fill(new Rect(rect.x, rect.y + 1f, 1f, rect.height - 2f), color);
			ProfilerGui.Fill(new Rect(rect.xMax - 1f, rect.y + 1f, 1f, rect.height - 2f), color);
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x001872A1 File Offset: 0x001854A1
		public static void Bordered(Rect rect, Color fill, Color border)
		{
			ProfilerGui.Fill(rect, border);
			ProfilerGui.Fill(rect.ContractedBy(1f), fill);
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x001872BC File Offset: 0x001854BC
		public static void Label(Rect rect, string text, Color color, TextAnchor anchor = TextAnchor.MiddleLeft, GameFont font = GameFont.Tiny)
		{
			if (!ProfilerGui.Painting || string.IsNullOrEmpty(text) || rect.width <= 0f)
			{
				return;
			}
			GUIStyle guistyle = Text.fontStyles[(int)font];
			RectOffset padding = guistyle.padding;
			Rect rect2 = new Rect(rect.x + (float)padding.left, rect.y + (float)padding.top, Mathf.Max(0f, rect.width - (float)padding.horizontal), Mathf.Max(0f, rect.height - (float)padding.vertical));
			GuiBackend.Current.DrawText(rect2, text, guistyle.FontIndex, (float)guistyle.fontSize, guistyle.fontStyle, anchor, false, color, rect, true);
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x00187374 File Offset: 0x00185574
		public static float Width(string text, GameFont font = GameFont.Tiny)
		{
			GUIStyle guistyle = Text.fontStyles[(int)font];
			return GuiBackend.Current.MeasureText(guistyle.FontIndex, (float)guistyle.fontSize, guistyle.fontStyle, text, -1f, true).x + (float)guistyle.padding.horizontal;
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x001873BF File Offset: 0x001855BF
		public static string Ellipsize(string text, float width)
		{
			Text.Font = GameFont.Tiny;
			string result = (Text.CalcSize(text).x <= width) ? text : text.Truncate(width, null);
			Text.Font = GameFont.Small;
			return result;
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x001873E8 File Offset: 0x001855E8
		private static void MeasureRun(string text, out string trimmed, out float leadingPad, out float width)
		{
			if (ProfilerGui.spaceWidth < 0f)
			{
				ProfilerGui.spaceWidth = Mathf.Max(3f, ProfilerGui.Width("a a", GameFont.Tiny) - ProfilerGui.Width("aa", GameFont.Tiny));
			}
			if (!ProfilerGui.trimmedRuns.TryGetValue(text, out trimmed))
			{
				if (ProfilerGui.trimmedRuns.Count >= 512)
				{
					ProfilerGui.trimmedRuns.Clear();
				}
				Dictionary<string, string> dictionary = ProfilerGui.trimmedRuns;
				string value;
				trimmed = (value = text.Trim(' '));
				dictionary.Add(text, value);
			}
			int num = 0;
			while (num < text.Length && text[num] == ' ')
			{
				num++;
			}
			leadingPad = (float)num * ProfilerGui.spaceWidth;
			width = ProfilerGui.Width(trimmed, GameFont.Tiny) + (float)(text.Length - trimmed.Length) * ProfilerGui.spaceWidth;
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x001874B0 File Offset: 0x001856B0
		public static float Run(float x, float y, float height, string text, Color color)
		{
			string text2;
			float num;
			float num2;
			ProfilerGui.MeasureRun(text, out text2, out num, out num2);
			ProfilerGui.Label(new Rect(x + num, y, num2 + 4f, height), text2, color, TextAnchor.MiddleLeft, GameFont.Tiny);
			return x + num2;
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x001874E8 File Offset: 0x001856E8
		public static float RunRightAligned(float right, float y, float height, string text, Color color)
		{
			string text2;
			float num;
			float num2;
			ProfilerGui.MeasureRun(text, out text2, out num, out num2);
			ProfilerGui.Run(right - num2, y, height, text, color);
			return right - num2;
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x00187512 File Offset: 0x00185712
		public static void ClearOverlays()
		{
			ProfilerGui.overlays.Clear();
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x0018751E File Offset: 0x0018571E
		public static void AddOverlay(Rect rect)
		{
			ProfilerGui.overlays.Add(new Rect(GUIUtility.GUIToScreenPoint(rect.position), rect.size));
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x00187544 File Offset: 0x00185744
		private static bool UnderOverlay()
		{
			Vector2 point = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
			for (int i = 0; i < ProfilerGui.overlays.Count; i++)
			{
				if (ProfilerGui.overlays[i].Contains(point))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x0018758F File Offset: 0x0018578F
		public static bool Hovered(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !ProfilerGui.UnderOverlay();
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x001875AE File Offset: 0x001857AE
		public static void MouseoverSound(Rect rect, SoundDef sound = null)
		{
			if (ProfilerGui.Painting && !ProfilerGui.UnderOverlay())
			{
				MouseoverSounds.DoRegion(rect, sound ?? SoundDefOf.Mouseover_Standard);
			}
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x001875CE File Offset: 0x001857CE
		public static bool Pressed(Rect rect, SoundDef sound = null)
		{
			ProfilerGui.MouseoverSound(rect, null);
			if (!ProfilerGui.Clicked(rect, 0))
			{
				return false;
			}
			(sound ?? SoundDefOf.Click).PlayOneShotOnCamera(null);
			return true;
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x001875F4 File Offset: 0x001857F4
		public static bool Clicked(Rect rect, int button = 0)
		{
			Event current = Event.current;
			if (current.type != EventType.mouseDown || current.button != button || !rect.Contains(current.mousePosition))
			{
				return false;
			}
			current.Use();
			return true;
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x00187630 File Offset: 0x00185830
		public static bool Chip(Rect rect, string label, bool active = false, bool enabled = true, Color? activeFill = null)
		{
			bool flag = enabled && ProfilerGui.Hovered(rect);
			Color color = active ? (activeFill ?? ProfilerGui.ChipHover) : (flag ? ProfilerGui.ChipHover : ProfilerGui.ChipBackground);
			ProfilerGui.Fill(rect, color);
			ProfilerGui.Label(rect, label, (!enabled) ? ProfilerGui.TextFaint : (active ? ProfilerGui.TextMain : (flag ? ProfilerGui.TextBright : ProfilerGui.TextDim)), TextAnchor.MiddleCenter, GameFont.Tiny);
			return enabled && ProfilerGui.Pressed(rect, null);
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x001876B8 File Offset: 0x001858B8
		public static bool Ghost(Rect rect, string label, bool active = false)
		{
			bool flag = ProfilerGui.Hovered(rect);
			if (active || flag)
			{
				ProfilerGui.Fill(rect, active ? ProfilerGui.ChipHover : ProfilerGui.ChipBackground);
			}
			ProfilerGui.Label(rect, label, active ? ProfilerGui.TextMain : (flag ? ProfilerGui.TextBright : ProfilerGui.TextDim), TextAnchor.MiddleCenter, GameFont.Tiny);
			return ProfilerGui.Pressed(rect, null);
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x00187710 File Offset: 0x00185910
		public static int Segments(ref float x, float y, string[] labels, int activeIndex, Color? activeFill = null, int redIndex = -1)
		{
			float num = 4f;
			for (int i = 0; i < labels.Length; i++)
			{
				num += ProfilerGui.Width(labels[i], GameFont.Tiny) + 18f;
			}
			ProfilerGui.Bordered(new Rect(x, y, num, 22f), ProfilerGui.SunkenBackground, ProfilerGui.Divider);
			int result = -1;
			float num2 = x + 2f;
			for (int j = 0; j < labels.Length; j++)
			{
				Rect rect = new Rect(num2, y + 2f, ProfilerGui.Width(labels[j], GameFont.Tiny) + 18f, 18f);
				bool flag = j == activeIndex;
				if (flag)
				{
					ProfilerGui.Fill(rect, (j == redIndex) ? ProfilerGui.SpikeChip : ProfilerGui.ChipHover);
				}
				ProfilerGui.Label(rect, labels[j], flag ? ProfilerGui.TextMain : (ProfilerGui.Hovered(rect) ? ProfilerGui.TextDim : ProfilerGui.TextFaint), TextAnchor.MiddleCenter, GameFont.Tiny);
				ProfilerGui.MouseoverSound(rect, SoundDefOf.Mouseover_Tab);
				if (!flag && ProfilerGui.Clicked(rect, 0))
				{
					SoundDefOf.RowTabSelect.PlayOneShotOnCamera(null);
					result = j;
				}
				num2 += rect.width;
			}
			x += num;
			return result;
		}

		// Token: 0x06003D84 RID: 15748 RVA: 0x00187831 File Offset: 0x00185A31
		public static void SetTooltip(string text)
		{
			ProfilerGui.tooltipText = text;
			ProfilerGui.tooltipAt = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x0018784D File Offset: 0x00185A4D
		public static void ClearTooltip()
		{
			ProfilerGui.tooltipText = null;
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x00187858 File Offset: 0x00185A58
		public static void FlushTooltip(Rect bounds)
		{
			if (!ProfilerGui.Painting)
			{
				return;
			}
			string text = ProfilerGui.tooltipText;
			ProfilerGui.tooltipText = null;
			if (text == null)
			{
				return;
			}
			Text.Font = GameFont.Tiny;
			Text.WordWrap = false;
			Vector2 vector = Text.CalcSize(text);
			Text.WordWrap = true;
			Text.Font = GameFont.Small;
			vector += new Vector2(16f, 10f);
			Vector2 vector2 = GUIUtility.ScreenToGUIPoint(ProfilerGui.tooltipAt);
			float num = vector2.x + 12f;
			if (num + vector.x > bounds.xMax)
			{
				num = vector2.x - 12f - vector.x;
			}
			float b = Mathf.Min(vector2.y + 14f, bounds.yMax - vector.y);
			Rect rect = new Rect(Mathf.Max(bounds.x, num), Mathf.Max(bounds.y, b), vector.x, vector.y);
			ProfilerGui.Bordered(rect, ProfilerGui.TitleBar, ProfilerGui.FocusBorder);
			ProfilerGui.Label(new Rect(rect.x + 8f, rect.y + 5f, vector.x, vector.y), text, ProfilerGui.TextMain, TextAnchor.UpperLeft, GameFont.Tiny);
		}

		// Token: 0x0400282F RID: 10287
		public static readonly Color TextMain = ProfilerGui.Hex(4293126642U);

		// Token: 0x04002830 RID: 10288
		public static readonly Color TextBright = ProfilerGui.Hex(4291416541U);

		// Token: 0x04002831 RID: 10289
		public static readonly Color TextDim = ProfilerGui.Hex(4288324786U);

		// Token: 0x04002832 RID: 10290
		public static readonly Color TextMuted = ProfilerGui.Hex(4290232524U);

		// Token: 0x04002833 RID: 10291
		public static readonly Color TextFaint = ProfilerGui.Hex(4284245104U);

		// Token: 0x04002834 RID: 10292
		public static readonly Color TextDisabled = ProfilerGui.Hex(4286416788U);

		// Token: 0x04002835 RID: 10293
		public static readonly Color Regression = ProfilerGui.Hex(4292898911U);

		// Token: 0x04002836 RID: 10294
		public static readonly Color Improvement = ProfilerGui.Hex(4284465030U);

		// Token: 0x04002837 RID: 10295
		public static readonly Color WindowBackground = ProfilerGui.Hex(4279638046U);

		// Token: 0x04002838 RID: 10296
		public static readonly Color SectionBackground = ProfilerGui.Hex(4279506460U);

		// Token: 0x04002839 RID: 10297
		public static readonly Color SunkenBackground = ProfilerGui.Hex(4279374874U);

		// Token: 0x0400283A RID: 10298
		public static readonly Color TitleBar = ProfilerGui.Hex(4280099114U);

		// Token: 0x0400283B RID: 10299
		public static readonly Color TitleBarUnfocused = ProfilerGui.Hex(4279967270U);

		// Token: 0x0400283C RID: 10300
		public static readonly Color ChipBackground = ProfilerGui.Hex(4280559922U);

		// Token: 0x0400283D RID: 10301
		public static readonly Color ChipHover = ProfilerGui.Hex(4281152578U);

		// Token: 0x0400283E RID: 10302
		public static readonly Color FieldBackground = ProfilerGui.Hex(4280165165U);

		// Token: 0x0400283F RID: 10303
		public static readonly Color RowAlternate = ProfilerGui.Hex(4279835941U);

		// Token: 0x04002840 RID: 10304
		public static readonly Color RowSelected = ProfilerGui.Hex(4280495165U);

		// Token: 0x04002841 RID: 10305
		public static readonly Color BarTrack = ProfilerGui.Hex(4280033321U);

		// Token: 0x04002842 RID: 10306
		public static readonly Color BarFill = ProfilerGui.Hex(4284773519U);

		// Token: 0x04002843 RID: 10307
		public static readonly Color Divider = ProfilerGui.Hex(4281020476U);

		// Token: 0x04002844 RID: 10308
		public static readonly Color SurfaceBorder = ProfilerGui.Hex(4280691509U);

		// Token: 0x04002845 RID: 10309
		public static readonly Color FocusBorder = ProfilerGui.Hex(4282007890U);

		// Token: 0x04002846 RID: 10310
		public static readonly Color ScrollThumb = ProfilerGui.Hex(4282139473U);

		// Token: 0x04002847 RID: 10311
		public static readonly Color ScrollThumbHover = ProfilerGui.Hex(4283061352U);

		// Token: 0x04002848 RID: 10312
		public static readonly Color SpikeChip = ProfilerGui.Hex(4283050533U);

		// Token: 0x04002849 RID: 10313
		public static readonly StringBuilder Scratch = new StringBuilder(256);

		// Token: 0x0400284A RID: 10314
		private static string tooltipText;

		// Token: 0x0400284B RID: 10315
		private static Vector2 tooltipAt;

		// Token: 0x0400284C RID: 10316
		private static readonly List<Rect> overlays = new List<Rect>();

		// Token: 0x0400284D RID: 10317
		private static float spaceWidth = -1f;

		// Token: 0x0400284E RID: 10318
		private static readonly Dictionary<string, string> trimmedRuns = new Dictionary<string, string>();
	}
}
