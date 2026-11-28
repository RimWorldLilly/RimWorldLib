using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196B RID: 6507
	public class CompProperties_Serum : CompProperties
	{
		// Token: 0x0600A356 RID: 41814 RVA: 0x003A5298 File Offset: 0x003A3498
		public CompProperties_Serum()
		{
			this.compClass = typeof(CompSerum);
		}

		// Token: 0x0600A357 RID: 41815 RVA: 0x003A52B0 File Offset: 0x003A34B0
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			CompProperties_Serum.<SpecialDisplayStats>d__1 <SpecialDisplayStats>d__ = new CompProperties_Serum.<SpecialDisplayStats>d__1(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}
	}
}
