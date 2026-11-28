using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200067D RID: 1661
	public class DefaultWindowDrawing : IWindowDrawing
	{
		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06003446 RID: 13382 RVA: 0x00150C30 File Offset: 0x0014EE30
		public GUIStyle EmptyStyle
		{
			get
			{
				return Widgets.EmptyStyle;
			}
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x00150C38 File Offset: 0x0014EE38
		public bool DoCloseButton(Rect rect, string text)
		{
			return Widgets.ButtonText(rect, text, true, true, true, null);
		}

		// Token: 0x06003448 RID: 13384 RVA: 0x00150C58 File Offset: 0x0014EE58
		public bool DoClostButtonSmall(Rect rect)
		{
			return Widgets.CloseButtonFor(rect);
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x00150C60 File Offset: 0x0014EE60
		public void DoGrayOut(Rect rect)
		{
			Widgets.DrawRectFast(rect, new Color(0f, 0f, 0f, 0.5f), null);
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x00150C82 File Offset: 0x0014EE82
		public void DoWindowBackground(Rect rect)
		{
			Widgets.DrawWindowBackground(rect);
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x00150C8A File Offset: 0x0014EE8A
		public void BeginGroup(Rect rect)
		{
			Widgets.BeginGroup(rect);
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x00150C92 File Offset: 0x0014EE92
		public void EndGroup()
		{
			Widgets.EndGroup();
		}
	}
}
