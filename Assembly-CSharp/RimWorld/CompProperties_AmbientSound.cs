using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001801 RID: 6145
	public class CompProperties_AmbientSound : CompProperties
	{
		// Token: 0x06009981 RID: 39297 RVA: 0x003787C2 File Offset: 0x003769C2
		public CompProperties_AmbientSound()
		{
			this.compClass = typeof(CompAmbientSound);
		}

		// Token: 0x04005B8C RID: 23436
		public SoundDef sound;

		// Token: 0x04005B8D RID: 23437
		public bool disabledOnUnpowered;

		// Token: 0x04005B8E RID: 23438
		public bool disableOnHacked;

		// Token: 0x04005B8F RID: 23439
		public bool disableOnInteracted;
	}
}
