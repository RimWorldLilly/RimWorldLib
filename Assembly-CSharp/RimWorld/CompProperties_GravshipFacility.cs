using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020019E7 RID: 6631
	public class CompProperties_GravshipFacility : CompProperties_Facility
	{
		// Token: 0x0600A6F5 RID: 42741 RVA: 0x003B7C97 File Offset: 0x003B5E97
		public CompProperties_GravshipFacility()
		{
			this.compClass = typeof(CompGravshipFacility);
		}

		// Token: 0x0600A6F6 RID: 42742 RVA: 0x003B7CAF File Offset: 0x003B5EAF
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			CompProperties_GravshipFacility.<SpecialDisplayStats>d__5 <SpecialDisplayStats>d__ = new CompProperties_GravshipFacility.<SpecialDisplayStats>d__5(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x04006257 RID: 25175
		public bool providesFuel;

		// Token: 0x04006258 RID: 25176
		public float fuelSavingsPercent;

		// Token: 0x04006259 RID: 25177
		public GravshipComponentTypeDef componentTypeDef;

		// Token: 0x0400625A RID: 25178
		public bool onlyRequiresLooseConnection;
	}
}
