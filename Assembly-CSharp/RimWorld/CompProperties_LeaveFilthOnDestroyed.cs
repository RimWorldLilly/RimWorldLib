using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D9 RID: 6361
	public class CompProperties_LeaveFilthOnDestroyed : CompProperties
	{
		// Token: 0x06009F94 RID: 40852 RVA: 0x003938E4 File Offset: 0x00391AE4
		public CompProperties_LeaveFilthOnDestroyed()
		{
			this.compClass = typeof(CompLeaveFilthOnDestroyed);
		}

		// Token: 0x04005E57 RID: 24151
		public int thickness;

		// Token: 0x04005E58 RID: 24152
		public ThingDef filthDef;
	}
}
