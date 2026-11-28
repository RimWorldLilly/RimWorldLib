using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D65 RID: 7525
	public class ITab_ContentsCasket : ITab_ContentsBase
	{
		// Token: 0x17002417 RID: 9239
		// (get) Token: 0x0600C2F7 RID: 49911 RVA: 0x0047151C File Offset: 0x0046F71C
		public override IList<Thing> container
		{
			get
			{
				Building_Casket building_Casket = base.SelThing as Building_Casket;
				this.listInt.Clear();
				if (building_Casket != null && building_Casket.ContainedThing != null)
				{
					this.listInt.Add(building_Casket.ContainedThing);
				}
				return this.listInt;
			}
		}

		// Token: 0x0600C2F8 RID: 49912 RVA: 0x00471562 File Offset: 0x0046F762
		public ITab_ContentsCasket()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "ContainedItems";
			this.canRemoveThings = false;
		}

		// Token: 0x04006F7A RID: 28538
		private List<Thing> listInt = new List<Thing>();
	}
}
