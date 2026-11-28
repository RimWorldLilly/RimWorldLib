using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A31 RID: 6705
	public class CompProperties_UseEffectArtifact : CompProperties_UseEffect
	{
		// Token: 0x0600A847 RID: 43079 RVA: 0x003BCCF5 File Offset: 0x003BAEF5
		public CompProperties_UseEffectArtifact()
		{
			this.compClass = typeof(CompUseEffect_Artifact);
		}

		// Token: 0x040062DE RID: 25310
		public SoundDef sound;
	}
}
