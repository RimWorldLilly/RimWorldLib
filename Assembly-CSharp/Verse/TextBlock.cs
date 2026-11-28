using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200063B RID: 1595
	public readonly struct TextBlock : IDisposable
	{
		// Token: 0x06003208 RID: 12808 RVA: 0x0013F33C File Offset: 0x0013D53C
		public TextBlock(GameFont newFont)
		{
			this = new TextBlock(new GameFont?(newFont), null, null, null);
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x0013F370 File Offset: 0x0013D570
		public TextBlock(TextAnchor newAnchor)
		{
			this = new TextBlock(null, new TextAnchor?(newAnchor), null, null);
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x0013F3A4 File Offset: 0x0013D5A4
		public TextBlock(bool newWordWrap)
		{
			this = new TextBlock(null, null, new bool?(newWordWrap), null);
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x0013F3D8 File Offset: 0x0013D5D8
		public TextBlock(Color newColor, TextAnchor newAnchor, bool newWordWrap)
		{
			this = new TextBlock(null, new TextAnchor?(newAnchor), new bool?(newWordWrap), new Color?(newColor));
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x0013F408 File Offset: 0x0013D608
		public TextBlock(Color newColor)
		{
			this = new TextBlock(null, null, null, new Color?(newColor));
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x0013F43C File Offset: 0x0013D63C
		public TextBlock(TextAnchor newAnchor, Color newColor)
		{
			this = new TextBlock(null, new TextAnchor?(newAnchor), null, new Color?(newColor));
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x0013F470 File Offset: 0x0013D670
		public TextBlock(GameFont newFont, TextAnchor newAnchor)
		{
			this = new TextBlock(new GameFont?(newFont), new TextAnchor?(newAnchor), null, null);
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x0013F4A4 File Offset: 0x0013D6A4
		public TextBlock(GameFont newFont, Color newColor)
		{
			this = new TextBlock(new GameFont?(newFont), null, null, new Color?(newColor));
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x0013F4D8 File Offset: 0x0013D6D8
		public TextBlock(GameFont? newFont, TextAnchor? newAnchor, Color? newColor)
		{
			this = new TextBlock(newFont, newAnchor, null, newColor);
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x0013F4F8 File Offset: 0x0013D6F8
		public TextBlock(GameFont? newFont, TextAnchor? newAnchor, bool? newWordWrap)
		{
			this = new TextBlock(newFont, newAnchor, newWordWrap, null);
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x0013F518 File Offset: 0x0013D718
		public TextBlock(GameFont? newFont, TextAnchor? newAnchor, bool? newWordWrap, Color? newColor)
		{
			this.oldFont = Text.Font;
			this.oldAnchor = Text.Anchor;
			this.oldWordWrap = Text.WordWrap;
			this.oldColor = GUI.color;
			if (newFont != null)
			{
				Text.Font = newFont.Value;
			}
			if (newAnchor != null)
			{
				Text.Anchor = newAnchor.Value;
			}
			if (newWordWrap != null)
			{
				Text.WordWrap = newWordWrap.Value;
			}
			if (newColor != null)
			{
				GUI.color = newColor.Value;
			}
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x0013F5A5 File Offset: 0x0013D7A5
		public static TextBlock Default()
		{
			return new TextBlock(new GameFont?(GameFont.Small), new TextAnchor?(TextAnchor.UpperLeft), new bool?(true), new Color?(Color.white));
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x0013F5C8 File Offset: 0x0013D7C8
		public void Dispose()
		{
			Text.Font = this.oldFont;
			Text.Anchor = this.oldAnchor;
			Text.WordWrap = this.oldWordWrap;
			GUI.color = this.oldColor;
		}

		// Token: 0x04002002 RID: 8194
		private readonly GameFont oldFont;

		// Token: 0x04002003 RID: 8195
		private readonly TextAnchor oldAnchor;

		// Token: 0x04002004 RID: 8196
		private readonly bool oldWordWrap;

		// Token: 0x04002005 RID: 8197
		private readonly Color oldColor;
	}
}
