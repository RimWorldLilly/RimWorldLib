using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200069C RID: 1692
	public class CreditRecord_TitleLocalization : CreditsEntry
	{
		// Token: 0x06003540 RID: 13632 RVA: 0x0015A857 File Offset: 0x00158A57
		public CreditRecord_TitleLocalization()
		{
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x0015A85F File Offset: 0x00158A5F
		public CreditRecord_TitleLocalization(string title)
		{
			this.title = title;
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x0015A86E File Offset: 0x00158A6E
		public override float DrawHeight(float width)
		{
			return 40f;
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x0015A878 File Offset: 0x00158A78
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, this.title);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Widgets.DrawLineHorizontal(rect.x + 10f, Mathf.Round(rect.yMax) - 14f, rect.width - 20f);
			GUI.color = Color.white;
		}

		// Token: 0x040022E4 RID: 8932
		public string title;
	}
}
