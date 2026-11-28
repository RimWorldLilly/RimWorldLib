using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E1 RID: 6369
	public class CompProperties_LootSpawn : CompProperties
	{
		// Token: 0x06009FAE RID: 40878 RVA: 0x00393DAE File Offset: 0x00391FAE
		public CompProperties_LootSpawn()
		{
			this.compClass = typeof(CompLootSpawn);
		}

		// Token: 0x04005E6F RID: 24175
		public ThingSetMakerDef contents;
	}
}
