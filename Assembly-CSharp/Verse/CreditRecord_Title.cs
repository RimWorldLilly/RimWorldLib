using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200069B RID: 1691
	public class CreditRecord_Title : CreditsEntry
	{
		// Token: 0x0600353C RID: 13628 RVA: 0x0015A7A0 File Offset: 0x001589A0
		public CreditRecord_Title()
		{
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x0015A7A8 File Offset: 0x001589A8
		public CreditRecord_Title(string title)
		{
			this.title = title;
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x0015A7B7 File Offset: 0x001589B7
		public override float DrawHeight(float width)
		{
			return 100f;
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x0015A7C0 File Offset: 0x001589C0
		public override void Draw(Rect rect)
		{
			rect.yMin += 31f;
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(rect.x + 10f, Mathf.Round(rect.yMax) - 14f, rect.width - 20f);
			GUI.color = Color.white;
		}

		// Token: 0x040022E3 RID: 8931
		public string title;
	}
}
