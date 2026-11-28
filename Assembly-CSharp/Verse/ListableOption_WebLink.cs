using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064E RID: 1614
	public class ListableOption_WebLink : ListableOption
	{
		// Token: 0x060032AA RID: 12970 RVA: 0x0014326E File Offset: 0x0014146E
		public ListableOption_WebLink(string label, Texture2D image) : base(label, null, null)
		{
			this.minHeight = 24f;
			this.image = image;
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x0014328B File Offset: 0x0014148B
		public ListableOption_WebLink(string label, string url, Texture2D image) : this(label, image)
		{
			this.url = url;
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x0014329C File Offset: 0x0014149C
		public ListableOption_WebLink(string label, Action action, Texture2D image) : this(label, image)
		{
			this.action = action;
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x001432B0 File Offset: 0x001414B0
		public override float DrawOption(Vector2 pos, float width)
		{
			float num = width - ListableOption_WebLink.Imagesize.x - 3f;
			float num2 = Text.CalcHeight(this.label, num);
			float num3 = Mathf.Max(this.minHeight, num2);
			Rect rect = new Rect(pos.x, pos.y, width, num3);
			GUI.color = Color.white;
			if (this.image != null)
			{
				Rect position = new Rect(pos.x, pos.y + num3 / 2f - ListableOption_WebLink.Imagesize.y / 2f, ListableOption_WebLink.Imagesize.x, ListableOption_WebLink.Imagesize.y);
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
				GUI.DrawTexture(position, this.image);
			}
			Widgets.Label(new Rect(rect.xMax - num, pos.y, num, num2), this.label);
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.action != null)
				{
					this.action();
				}
				else
				{
					Application.OpenURL(this.url);
				}
			}
			return num3;
		}

		// Token: 0x04002061 RID: 8289
		public Texture2D image;

		// Token: 0x04002062 RID: 8290
		public string url;

		// Token: 0x04002063 RID: 8291
		private static readonly Vector2 Imagesize = new Vector2(24f, 18f);
	}
}
