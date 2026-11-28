using System;

namespace Verse
{
	// Token: 0x0200011D RID: 285
	public class BodyPartGroupDef : Def
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x000361F8 File Offset: 0x000343F8
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x00036214 File Offset: 0x00034414
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x040004BA RID: 1210
		[MustTranslate]
		public string labelShort;

		// Token: 0x040004BB RID: 1211
		public int listOrder;

		// Token: 0x040004BC RID: 1212
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
