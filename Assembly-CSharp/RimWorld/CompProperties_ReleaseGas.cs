using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002097 RID: 8343
	public class CompProperties_ReleaseGas : CompProperties
	{
		// Token: 0x0600D24D RID: 53837 RVA: 0x004D3F9F File Offset: 0x004D219F
		public CompProperties_ReleaseGas()
		{
			this.compClass = typeof(CompReleaseGas);
		}

		// Token: 0x04008538 RID: 34104
		public GasType gasType;

		// Token: 0x04008539 RID: 34105
		public float cellsToFill;

		// Token: 0x0400853A RID: 34106
		public float durationSeconds;

		// Token: 0x0400853B RID: 34107
		public EffecterDef effecterReleasing;
	}
}
