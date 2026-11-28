using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D62 RID: 7522
	public class ITab_Book : ITab
	{
		// Token: 0x1700240E RID: 9230
		// (get) Token: 0x0600C2DF RID: 49887 RVA: 0x00470A30 File Offset: 0x0046EC30
		public override bool IsVisible
		{
			get
			{
				return base.SelThing is Book;
			}
		}

		// Token: 0x0600C2E0 RID: 49888 RVA: 0x00470A40 File Offset: 0x0046EC40
		public ITab_Book()
		{
			this.size = new Vector2(Mathf.Min(610f, (float)UI.screenWidth), 350f);
			this.labelKey = "TabBookContents";
		}

		// Token: 0x0600C2E1 RID: 49889 RVA: 0x00470A74 File Offset: 0x0046EC74
		protected override void FillTab()
		{
			Book book = base.SelThing as Book;
			if (book == null)
			{
				return;
			}
			Rect rect = new Rect(0f, 20f, this.size.x, this.size.y - 20f).ContractedBy(10f);
			Rect rect2 = rect;
			rect2.y = 10f;
			rect2.height = 30f;
			Rect rect3 = rect;
			rect3.xMax = rect.center.x - 17f;
			rect3.y = rect2.yMax + 17f;
			rect3.yMax = rect.yMax;
			Rect rect4 = rect;
			rect4.x = rect3.xMax + 20f;
			rect4.xMax = rect.xMax + 10f;
			rect4.y = rect2.yMax + 26f;
			rect4.yMax = rect.yMax;
			BookUIUtility.DrawTitle(rect2, book);
			BookUIUtility.DrawBookInfoPanel(rect3, book);
			BookUIUtility.DrawBookDescPanel(rect4, book, ref this.infoScroll);
		}

		// Token: 0x04006F67 RID: 28519
		private Vector2 infoScroll;

		// Token: 0x04006F68 RID: 28520
		protected const float TopPadding = 20f;

		// Token: 0x04006F69 RID: 28521
		protected const float InitialHeight = 350f;

		// Token: 0x04006F6A RID: 28522
		public const float TitleHeight = 30f;

		// Token: 0x04006F6B RID: 28523
		protected const float InitialWidth = 610f;
	}
}
