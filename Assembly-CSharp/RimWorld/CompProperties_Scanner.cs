using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F6 RID: 6646
	public class CompProperties_Scanner : CompProperties
	{
		// Token: 0x0600A762 RID: 42850 RVA: 0x003BA06C File Offset: 0x003B826C
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Scanner.<ConfigErrors>d__4 <ConfigErrors>d__ = new CompProperties_Scanner.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400628A RID: 25226
		public float scanFindMtbDays;

		// Token: 0x0400628B RID: 25227
		public float scanFindGuaranteedDays = -1f;

		// Token: 0x0400628C RID: 25228
		public StatDef scanSpeedStat;

		// Token: 0x0400628D RID: 25229
		public SoundDef soundWorking;
	}
}
