using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099A RID: 2458
	public class RecipeTooltipLayout
	{
		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06004A43 RID: 19011 RVA: 0x001CDFAF File Offset: 0x001CC1AF
		public Vector2 Size
		{
			get
			{
				return this.maxSize;
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06004A44 RID: 19012 RVA: 0x001CDFB7 File Offset: 0x001CC1B7
		public bool Empty
		{
			get
			{
				return this.Size.x <= 0f || this.Size.y <= 0f;
			}
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x001CDFE4 File Offset: 0x001CC1E4
		public void Label(string text, bool draw, GameFont font = GameFont.Small)
		{
			GameFont font2 = Text.Font;
			Text.Font = font;
			Vector2 vector = Text.CalcSize(text);
			Rect rect = new Rect(this.curPos.x, this.curPos.y, vector.x, vector.y);
			if (draw)
			{
				Widgets.Label(rect, text);
			}
			this.curPos.x = rect.xMax;
			this.ExpandToFit(rect.xMax, rect.yMax);
			Text.Font = font2;
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x001CE064 File Offset: 0x001CC264
		public void Icon(Texture2D icon, Color color, float iconSize, bool draw)
		{
			Rect position = new Rect(this.curPos.x, this.curPos.y, iconSize, iconSize);
			if (draw)
			{
				Color color2 = GUI.color;
				GUI.color = color;
				GUI.DrawTexture(position, icon);
				GUI.color = color2;
			}
			this.curPos.x = position.xMax;
			this.ExpandToFit(position.xMax, position.yMax);
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x001CE0D1 File Offset: 0x001CC2D1
		public void Gap(float x, float y)
		{
			this.curPos.x = this.curPos.x + x;
			this.curPos.y = this.curPos.y + y;
			this.ExpandToFit(this.curPos.x, this.curPos.y);
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x001CE110 File Offset: 0x001CC310
		public void Reset(float newIndent = 0f)
		{
			this.curPos = Vector2.zero;
			this.maxSize = Vector2.zero;
			this.indent = newIndent;
			this.curPos.x = Mathf.Max(this.curPos.x, this.indent);
			this.curPos.y = Mathf.Max(this.curPos.y, this.indent);
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x001CE17C File Offset: 0x001CC37C
		public void Expand(float width, float height)
		{
			this.maxSize.x = this.maxSize.x + width;
			this.maxSize.y = this.maxSize.y + height;
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x001CE1A0 File Offset: 0x001CC3A0
		public void Newline(GameFont font = GameFont.Small)
		{
			this.curPos.x = this.indent;
			this.curPos.y = this.curPos.y + Text.LineHeightOf(font);
			this.ExpandToFit(this.curPos.x, this.curPos.y);
		}

		// Token: 0x06004A4B RID: 19019 RVA: 0x001CE1EF File Offset: 0x001CC3EF
		private void ExpandToFit(float x, float y)
		{
			this.maxSize.x = Mathf.Max(this.maxSize.x, x);
			this.maxSize.y = Mathf.Max(this.maxSize.y, y);
		}

		// Token: 0x04002F83 RID: 12163
		private Vector2 curPos;

		// Token: 0x04002F84 RID: 12164
		private Vector2 maxSize;

		// Token: 0x04002F85 RID: 12165
		private float indent;
	}
}
