using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001369 RID: 4969
	public class CompStatEntrySchoolDesk : CompStatEntry
	{
		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x06007B24 RID: 31524 RVA: 0x002BD28C File Offset: 0x002BB48C
		private CompProperties_StatEntry Props
		{
			get
			{
				return (CompProperties_StatEntry)this.props;
			}
		}

		// Token: 0x06007B25 RID: 31525 RVA: 0x002BD299 File Offset: 0x002BB499
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompStatEntrySchoolDesk.<SpecialDisplayStats>d__2 <SpecialDisplayStats>d__ = new CompStatEntrySchoolDesk.<SpecialDisplayStats>d__2(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}
	}
}
