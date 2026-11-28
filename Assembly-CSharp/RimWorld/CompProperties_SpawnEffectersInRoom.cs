using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200209B RID: 8347
	public class CompProperties_SpawnEffectersInRoom : CompProperties
	{
		// Token: 0x0600D25D RID: 53853 RVA: 0x004D4388 File Offset: 0x004D2588
		public CompProperties_SpawnEffectersInRoom()
		{
			this.compClass = typeof(CompSpawnEffectersInRoom);
		}

		// Token: 0x04008545 RID: 34117
		public EffecterDef effecter;

		// Token: 0x04008546 RID: 34118
		public float radius = 10f;
	}
}
