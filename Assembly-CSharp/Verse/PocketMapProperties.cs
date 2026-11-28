using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000186 RID: 390
	public class PocketMapProperties
	{
		// Token: 0x04000981 RID: 2433
		public BiomeDef biome;

		// Token: 0x04000982 RID: 2434
		public List<TileMutatorDef> tileMutators = new List<TileMutatorDef>();

		// Token: 0x04000983 RID: 2435
		public float temperature;

		// Token: 0x04000984 RID: 2436
		public bool destroyOnParentMapAbandoned = true;

		// Token: 0x04000985 RID: 2437
		public bool preventPrisonerEscape = true;

		// Token: 0x04000986 RID: 2438
		public bool canLaunchGravship;

		// Token: 0x04000987 RID: 2439
		public bool canBeCleaned;
	}
}
