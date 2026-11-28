using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181F RID: 6175
	public class CompProperties_Atomizer : CompProperties_ThingContainer
	{
		// Token: 0x06009A7D RID: 39549 RVA: 0x0037BC84 File Offset: 0x00379E84
		public CompProperties_Atomizer()
		{
			this.compClass = typeof(CompAtomizer);
		}

		// Token: 0x04005BCD RID: 23501
		public ThingDef thingDef;

		// Token: 0x04005BCE RID: 23502
		public EffecterDef resolveEffecter;

		// Token: 0x04005BCF RID: 23503
		public EffecterDef workingEffecter;

		// Token: 0x04005BD0 RID: 23504
		public SoundDef materialsAddedSound;

		// Token: 0x04005BD1 RID: 23505
		public int resolveEffecterTicks = 40;

		// Token: 0x04005BD2 RID: 23506
		public int ticksPerAtomize = 2500;

		// Token: 0x04005BD3 RID: 23507
		public Vector3 contentsDrawOffset = Vector3.zero;
	}
}
