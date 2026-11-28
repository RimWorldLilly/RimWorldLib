using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A25 RID: 6693
	public class CompProperties_Loudspeaker : CompProperties
	{
		// Token: 0x0600A817 RID: 43031 RVA: 0x003BC592 File Offset: 0x003BA792
		public CompProperties_Loudspeaker()
		{
			this.compClass = typeof(CompLoudspeaker);
		}

		// Token: 0x0600A818 RID: 43032 RVA: 0x003BC5AA File Offset: 0x003BA7AA
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Loudspeaker.<ConfigErrors>d__1 <ConfigErrors>d__ = new CompProperties_Loudspeaker.<ConfigErrors>d__1(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}
	}
}
