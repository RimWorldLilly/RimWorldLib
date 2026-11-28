using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002364 RID: 9060
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x0600E099 RID: 57497 RVA: 0x00537DD2 File Offset: 0x00535FD2
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600E09A RID: 57498 RVA: 0x00537DE6 File Offset: 0x00535FE6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x0600E09B RID: 57499 RVA: 0x00537E08 File Offset: 0x00536008
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600E09C RID: 57500 RVA: 0x00537E16 File Offset: 0x00536016
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		// Token: 0x0600E09D RID: 57501 RVA: 0x00537E1E File Offset: 0x0053601E
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0400900C RID: 36876
		public ThingOwner contents;
	}
}
