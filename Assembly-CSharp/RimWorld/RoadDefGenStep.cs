using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FDA RID: 4058
	public abstract class RoadDefGenStep
	{
		// Token: 0x060066CC RID: 26316
		public abstract void Place(Map map, IntVec3 position, TerrainDef rockDef, IntVec3 origin, GenStep_Roads.DistanceElement[,] distance);

		// Token: 0x04003FFE RID: 16382
		public SimpleCurve chancePerPositionCurve;

		// Token: 0x04003FFF RID: 16383
		public float antialiasingMultiplier = 1f;

		// Token: 0x04004000 RID: 16384
		public int periodicSpacing;
	}
}
