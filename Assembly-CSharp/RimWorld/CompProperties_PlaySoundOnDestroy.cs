using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001936 RID: 6454
	public class CompProperties_PlaySoundOnDestroy : CompProperties
	{
		// Token: 0x0600A1EA RID: 41450 RVA: 0x0039DE9F File Offset: 0x0039C09F
		public CompProperties_PlaySoundOnDestroy()
		{
			this.compClass = typeof(CompPlaySoundOnDestroy);
		}

		// Token: 0x04005FA4 RID: 24484
		public SoundDef sound;

		// Token: 0x04005FA5 RID: 24485
		public bool onlyWhenKilled;

		// Token: 0x04005FA6 RID: 24486
		public bool ignoreOnVanish = true;
	}
}
