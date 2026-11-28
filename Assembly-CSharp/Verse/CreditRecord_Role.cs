using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000699 RID: 1689
	public class CreditRecord_Role : CreditsEntry
	{
		// Token: 0x06003531 RID: 13617 RVA: 0x0015A4DA File Offset: 0x001586DA
		public CreditRecord_Role()
		{
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x0015A4E2 File Offset: 0x001586E2
		public CreditRecord_Role(string roleKey, string creditee, string extra = null)
		{
			this.roleKey = roleKey;
			this.creditee = creditee;
			this.extra = extra;
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x0015A500 File Offset: 0x00158700
		public override float DrawHeight(float width)
		{
			if (this.roleKey.NullOrEmpty())
			{
				width *= 0.5f;
			}
			Text.Font = (this.smallFont ? GameFont.Small : GameFont.Medium);
			float result = this.compressed ? Text.CalcHeight(this.creditee, width * 0.5f) : 50f;
			Text.Font = GameFont.Medium;
			return result;
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x0015A55C File Offset: 0x0015875C
		public override void Draw(Rect rect)
		{
			Text.Font = (this.smallFont ? GameFont.Small : GameFont.Medium);
			Text.Anchor = TextAnchor.MiddleLeft;
			Rect rect2 = rect;
			rect2.width = 0f;
			if (!this.roleKey.NullOrEmpty())
			{
				rect2.width = rect.width / 2f;
				if (this.displayKey)
				{
					Widgets.Label(rect2, this.roleKey);
				}
			}
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			if (this.roleKey.NullOrEmpty())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			Widgets.Label(rect3, this.creditee);
			Text.Anchor = TextAnchor.MiddleLeft;
			if (!this.extra.NullOrEmpty())
			{
				Rect rect4 = rect3;
				rect4.yMin += 28f;
				Text.Font = GameFont.Tiny;
				GUI.color = new Color(0.7f, 0.7f, 0.7f);
				Widgets.Label(rect4, this.extra);
				GUI.color = Color.white;
			}
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x0015A65B File Offset: 0x0015885B
		public CreditRecord_Role Compress()
		{
			this.compressed = true;
			return this;
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x0015A665 File Offset: 0x00158865
		public CreditRecord_Role WithSmallFont()
		{
			this.smallFont = true;
			return this;
		}

		// Token: 0x040022D9 RID: 8921
		public string roleKey;

		// Token: 0x040022DA RID: 8922
		public string creditee;

		// Token: 0x040022DB RID: 8923
		public string extra;

		// Token: 0x040022DC RID: 8924
		public bool displayKey;

		// Token: 0x040022DD RID: 8925
		public bool compressed;

		// Token: 0x040022DE RID: 8926
		public bool smallFont;
	}
}
