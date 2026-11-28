using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A23 RID: 6691
	public class CompProperties_Lightball : CompProperties
	{
		// Token: 0x0600A80B RID: 43019 RVA: 0x003BC207 File Offset: 0x003BA407
		public CompProperties_Lightball()
		{
			this.compClass = typeof(CompLightball);
		}

		// Token: 0x0600A80C RID: 43020 RVA: 0x003BC21F File Offset: 0x003BA41F
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Lightball.<ConfigErrors>d__3 <ConfigErrors>d__ = new CompProperties_Lightball.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x040062C8 RID: 25288
		public List<SoundDef> soundDefsPerSpeakerCount;

		// Token: 0x040062C9 RID: 25289
		public int maxSpeakerDistance;
	}
}
