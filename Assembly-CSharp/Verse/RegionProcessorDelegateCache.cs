using System;

namespace Verse
{
	// Token: 0x02000302 RID: 770
	public abstract class RegionProcessorDelegateCache
	{
		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001778 RID: 6008 RVA: 0x0008D1BC File Offset: 0x0008B3BC
		public RegionProcessor RegionProcessorDelegate
		{
			get
			{
				return this.regionProcessor;
			}
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x0008D1C4 File Offset: 0x0008B3C4
		public RegionEntryPredicate RegionEntryPredicateDelegate
		{
			get
			{
				return this.regionEntryPredicate;
			}
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x0008D1CC File Offset: 0x0008B3CC
		public RegionProcessorDelegateCache()
		{
			this.regionProcessor = new RegionProcessor(this.RegionProcessor);
			this.regionEntryPredicate = new RegionEntryPredicate(this.RegionEntryPredicate);
		}

		// Token: 0x0600177B RID: 6011
		protected abstract bool RegionEntryPredicate(Region from, Region to);

		// Token: 0x0600177C RID: 6012
		protected abstract bool RegionProcessor(Region reg);

		// Token: 0x040013E6 RID: 5094
		private RegionProcessor regionProcessor;

		// Token: 0x040013E7 RID: 5095
		private RegionEntryPredicate regionEntryPredicate;
	}
}
