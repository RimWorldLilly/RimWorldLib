using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200064D RID: 1613
	public class ListableOption
	{
		// Token: 0x060032A8 RID: 12968 RVA: 0x001431CF File Offset: 0x001413CF
		public ListableOption(string label, Action action, string uiHighlightTag = null)
		{
			this.label = label;
			this.action = action;
			this.uiHighlightTag = uiHighlightTag;
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x001431F8 File Offset: 0x001413F8
		public virtual float DrawOption(Vector2 pos, float width)
		{
			float b = Text.CalcHeight(this.label, width);
			float num = Mathf.Max(this.minHeight, b);
			Rect rect = new Rect(pos.x, pos.y, width, num);
			if (Widgets.ButtonText(rect, this.label, true, true, true, null))
			{
				this.action();
			}
			if (this.uiHighlightTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.uiHighlightTag);
			}
			return num;
		}

		// Token: 0x0400205D RID: 8285
		public string label;

		// Token: 0x0400205E RID: 8286
		public Action action;

		// Token: 0x0400205F RID: 8287
		private string uiHighlightTag;

		// Token: 0x04002060 RID: 8288
		public float minHeight = 45f;
	}
}
