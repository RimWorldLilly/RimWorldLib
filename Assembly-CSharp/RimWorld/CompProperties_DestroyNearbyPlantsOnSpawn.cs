using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185E RID: 6238
	public class CompProperties_DestroyNearbyPlantsOnSpawn : CompProperties
	{
		// Token: 0x06009C1E RID: 39966 RVA: 0x003837D9 File Offset: 0x003819D9
		public CompProperties_DestroyNearbyPlantsOnSpawn()
		{
			this.compClass = typeof(CompDestroyNearbyPlantsOnSpawn);
		}

		// Token: 0x04005CC7 RID: 23751
		public int radius;
	}
}
