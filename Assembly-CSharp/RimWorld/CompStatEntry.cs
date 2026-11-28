using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001368 RID: 4968
	public class CompStatEntry : ThingComp
	{
		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x06007B21 RID: 31521 RVA: 0x002BD267 File Offset: 0x002BB467
		private CompProperties_StatEntry Props
		{
			get
			{
				return (CompProperties_StatEntry)this.props;
			}
		}

		// Token: 0x06007B22 RID: 31522 RVA: 0x002BD274 File Offset: 0x002BB474
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompStatEntry.<SpecialDisplayStats>d__2 <SpecialDisplayStats>d__ = new CompStatEntry.<SpecialDisplayStats>d__2(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}
	}
}
