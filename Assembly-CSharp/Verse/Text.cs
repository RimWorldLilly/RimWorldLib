using System;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200063A RID: 1594
	public static class Text
	{
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x060031F5 RID: 12789 RVA: 0x0013ED2E File Offset: 0x0013CF2E
		public static bool TinyFontSupported
		{
			get
			{
				return (LongEventHandler.AnyEventNowOrWaiting || LanguageDatabase.activeLanguage.info.canBeTiny) && !Prefs.DisableTinyText && !SteamDeck.IsSteamDeck;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x060031F6 RID: 12790 RVA: 0x0013ED5D File Offset: 0x0013CF5D
		// (set) Token: 0x060031F7 RID: 12791 RVA: 0x0013ED64 File Offset: 0x0013CF64
		public static GameFont Font
		{
			get
			{
				return Text.fontInt;
			}
			set
			{
				if (value == GameFont.Tiny && !Text.TinyFontSupported)
				{
					Text.fontInt = GameFont.Small;
					return;
				}
				Text.fontInt = value;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x060031F8 RID: 12792 RVA: 0x0013ED7D File Offset: 0x0013CF7D
		// (set) Token: 0x060031F9 RID: 12793 RVA: 0x0013ED84 File Offset: 0x0013CF84
		public static TextAnchor Anchor
		{
			get
			{
				return Text.anchorInt;
			}
			set
			{
				Text.anchorInt = value;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x060031FA RID: 12794 RVA: 0x0013ED8C File Offset: 0x0013CF8C
		// (set) Token: 0x060031FB RID: 12795 RVA: 0x0013ED93 File Offset: 0x0013CF93
		public static bool WordWrap
		{
			get
			{
				return Text.wordWrapInt;
			}
			set
			{
				Text.wordWrapInt = value;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x060031FC RID: 12796 RVA: 0x0013ED9B File Offset: 0x0013CF9B
		public static float LineHeight
		{
			get
			{
				return Text.lineHeights[(int)Text.Font];
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x0013EDA8 File Offset: 0x0013CFA8
		public static float SpaceBetweenLines
		{
			get
			{
				return Text.spaceBetweenLines[(int)Text.Font];
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x060031FE RID: 12798 RVA: 0x0013EDB8 File Offset: 0x0013CFB8
		public static GUIStyle CurFontStyle
		{
			get
			{
				GUIStyle guistyle;
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					guistyle = Text.fontStyles[0];
					break;
				case GameFont.Small:
					guistyle = Text.fontStyles[1];
					break;
				case GameFont.Medium:
					guistyle = Text.fontStyles[2];
					break;
				default:
					throw new NotImplementedException();
				}
				guistyle.alignment = Text.anchorInt;
				guistyle.wordWrap = Text.wordWrapInt;
				return guistyle;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x0013EE1C File Offset: 0x0013D01C
		public static GUIStyle CurTextFieldStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textFieldStyles[0];
				case GameFont.Small:
					return Text.textFieldStyles[1];
				case GameFont.Medium:
					return Text.textFieldStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06003200 RID: 12800 RVA: 0x0013EE60 File Offset: 0x0013D060
		public static GUIStyle CurTextAreaStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textAreaStyles[0];
				case GameFont.Small:
					return Text.textAreaStyles[1];
				case GameFont.Medium:
					return Text.textAreaStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x0013EEA4 File Offset: 0x0013D0A4
		public static GUIStyle CurTextAreaReadOnlyStyle
		{
			get
			{
				switch (Text.fontInt)
				{
				case GameFont.Tiny:
					return Text.textAreaReadOnlyStyles[0];
				case GameFont.Small:
					return Text.textAreaReadOnlyStyles[1];
				case GameFont.Medium:
					return Text.textAreaReadOnlyStyles[2];
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x0013EEE8 File Offset: 0x0013D0E8
		static Text()
		{
			Text.fonts[0] = Resources.Load<Font>("Fonts/Calibri_tiny");
			Text.fonts[1] = Resources.Load<Font>("Fonts/Arial_small");
			Text.fonts[2] = Resources.Load<Font>("Fonts/Arial_medium");
			Text.fontStyles[0] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[0].font = Text.fonts[0];
			Text.fontStyles[1] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[1].font = Text.fonts[1];
			Text.fontStyles[1].contentOffset = new Vector2(0f, -1f);
			Text.fontStyles[2] = new GUIStyle(GUI.skin.label);
			Text.fontStyles[2].font = Text.fonts[2];
			for (int i = 0; i < Text.textFieldStyles.Length; i++)
			{
				Text.textFieldStyles[i] = new GUIStyle(GUI.skin.textField);
				Text.textFieldStyles[i].alignment = TextAnchor.MiddleLeft;
			}
			Text.textFieldStyles[0].font = Text.fonts[0];
			Text.textFieldStyles[1].font = Text.fonts[1];
			Text.textFieldStyles[2].font = Text.fonts[2];
			for (int j = 0; j < Text.textAreaStyles.Length; j++)
			{
				Text.textAreaStyles[j] = new GUIStyle(Text.textFieldStyles[j]);
				Text.textAreaStyles[j].alignment = TextAnchor.UpperLeft;
				Text.textAreaStyles[j].wordWrap = true;
			}
			for (int k = 0; k < Text.textAreaReadOnlyStyles.Length; k++)
			{
				Text.textAreaReadOnlyStyles[k] = new GUIStyle(Text.textAreaStyles[k]);
				GUIStyle guistyle = Text.textAreaReadOnlyStyles[k];
				guistyle.normal.background = null;
				guistyle.active.background = null;
				guistyle.onHover.background = null;
				guistyle.hover.background = null;
				guistyle.onFocused.background = null;
				guistyle.focused.background = null;
			}
			GUI.skin.settings.doubleClickSelectsWord = true;
			int num = 0;
			foreach (object obj in Enum.GetValues(typeof(GameFont)))
			{
				Text.Font = (GameFont)obj;
				Text.lineHeights[num] = Text.CalcHeight("W", 999f);
				Text.spaceBetweenLines[num] = Text.CalcHeight("W\nW", 999f) - Text.CalcHeight("W", 999f) * 2f;
				num++;
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x0013F200 File Offset: 0x0013D400
		public static float LineHeightOf(GameFont font)
		{
			return Text.lineHeights[(int)font];
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x0013F209 File Offset: 0x0013D409
		public static float CalcHeight(string text, float width)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcHeight(Text.tmpTextGUIContent, width);
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x0013F22B File Offset: 0x0013D42B
		public static Vector2 CalcSize(string text)
		{
			Text.tmpTextGUIContent.text = text.StripTags();
			return Text.CurFontStyle.CalcSize(Text.tmpTextGUIContent);
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x0013F24C File Offset: 0x0013D44C
		public static string ClampTextWithEllipsis(Rect rect, string text)
		{
			if (text.Length <= 4)
			{
				return text;
			}
			Vector2 vector = Text.CalcSize(text);
			if (vector.x <= rect.width - 13f)
			{
				return text;
			}
			while (vector.x > rect.width - 13f)
			{
				if (text.Length == 0)
				{
					return "";
				}
				text = text.Substring(0, text.Length - 1);
				vector = Text.CalcSize(text + "...");
			}
			return text + "...";
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x0013F2D4 File Offset: 0x0013D4D4
		public static void StartOfOnGUI()
		{
			if (!Text.WordWrap)
			{
				Log.ErrorOnce("Word wrap was false at end of frame.", 764362);
				Text.WordWrap = true;
			}
			if (Text.Anchor != TextAnchor.UpperLeft)
			{
				Log.ErrorOnce("Alignment was " + Text.Anchor.ToString() + " at end of frame.", 15558);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x04001FF5 RID: 8181
		private static GameFont fontInt = GameFont.Small;

		// Token: 0x04001FF6 RID: 8182
		private static TextAnchor anchorInt = TextAnchor.UpperLeft;

		// Token: 0x04001FF7 RID: 8183
		private static bool wordWrapInt = true;

		// Token: 0x04001FF8 RID: 8184
		private static Font[] fonts = new Font[3];

		// Token: 0x04001FF9 RID: 8185
		public static readonly GUIStyle[] fontStyles = new GUIStyle[3];

		// Token: 0x04001FFA RID: 8186
		public static readonly GUIStyle[] textFieldStyles = new GUIStyle[3];

		// Token: 0x04001FFB RID: 8187
		public static readonly GUIStyle[] textAreaStyles = new GUIStyle[3];

		// Token: 0x04001FFC RID: 8188
		public static readonly GUIStyle[] textAreaReadOnlyStyles = new GUIStyle[3];

		// Token: 0x04001FFD RID: 8189
		private static readonly float[] lineHeights = new float[3];

		// Token: 0x04001FFE RID: 8190
		private static readonly float[] spaceBetweenLines = new float[3];

		// Token: 0x04001FFF RID: 8191
		private static GUIContent tmpTextGUIContent = new GUIContent();

		// Token: 0x04002000 RID: 8192
		private const int NumFonts = 3;

		// Token: 0x04002001 RID: 8193
		public const float SmallFontHeight = 22f;
	}
}
