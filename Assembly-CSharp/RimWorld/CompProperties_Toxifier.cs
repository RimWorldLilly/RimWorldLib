using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C3 RID: 6595
	public class CompProperties_Toxifier : CompProperties
	{
		// Token: 0x0600A589 RID: 42377 RVA: 0x003B0593 File Offset: 0x003AE793
		public CompProperties_Toxifier()
		{
			this.compClass = typeof(CompToxifier);
		}

		// Token: 0x0600A58A RID: 42378 RVA: 0x003B05C8 File Offset: 0x003AE7C8
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Toxifier.<ConfigErrors>d__4 <ConfigErrors>d__ = new CompProperties_Toxifier.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040061B7 RID: 25015
		public float radius = 5.9f;

		// Token: 0x040061B8 RID: 25016
		public int pollutionIntervalTicks = 60000;

		// Token: 0x040061B9 RID: 25017
		public int cellsToPollute = 6;
	}
}
