using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200069A RID: 1690
	public class CreditRecord_RoleTwoCols : CreditsEntry
	{
		// Token: 0x06003537 RID: 13623 RVA: 0x0015A66F File Offset: 0x0015886F
		public CreditRecord_RoleTwoCols()
		{
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x0015A677 File Offset: 0x00158877
		public CreditRecord_RoleTwoCols(string creditee1, string creditee2, string extra = null)
		{
			this.creditee1 = creditee1;
			this.creditee2 = creditee2;
			this.extra = extra;
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x0015A694 File Offset: 0x00158894
		public override float DrawHeight(float width)
		{
			float a = Text.CalcHeight(this.creditee1, width * 0.5f);
			float b = Text.CalcHeight(this.creditee2, width * 0.5f);
			if (!this.compressed)
			{
				return 50f;
			}
			return Mathf.Max(a, b);
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x0015A6DC File Offset: 0x001588DC
		public override void Draw(Rect rect)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			rect2.width = rect.width / 2f;
			Widgets.Label(rect2, this.creditee1);
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			Widgets.Label(rect3, this.creditee2);
			if (!this.extra.NullOrEmpty())
			{
				Rect rect4 = rect3;
				rect4.yMin += 28f;
				Text.Font = GameFont.Tiny;
				GUI.color = new Color(0.7f, 0.7f, 0.7f);
				Widgets.Label(rect4, this.extra);
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x0015A796 File Offset: 0x00158996
		public CreditRecord_RoleTwoCols Compress()
		{
			this.compressed = true;
			return this;
		}

		// Token: 0x040022DF RID: 8927
		public string creditee1;

		// Token: 0x040022E0 RID: 8928
		public string creditee2;

		// Token: 0x040022E1 RID: 8929
		public string extra;

		// Token: 0x040022E2 RID: 8930
		public bool compressed;
	}
}
