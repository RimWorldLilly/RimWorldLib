using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF2 RID: 4082
	public class StyleItemCategoryDef : Def
	{
		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x0600671F RID: 26399 RVA: 0x00248ABF File Offset: 0x00246CBF
		public List<StyleItemDef> ItemsInCategory
		{
			get
			{
				if (this.cachedStyleItems == null)
				{
					this.cachedStyleItems = new List<StyleItemDef>();
					this.cachedStyleItems.AddRange(from x in StyleItemDef.AllStyleItemDefs
					where x.StyleItemCategory == this
					select x);
				}
				return this.cachedStyleItems;
			}
		}

		// Token: 0x040040D6 RID: 16598
		private List<StyleItemDef> cachedStyleItems;
	}
}
