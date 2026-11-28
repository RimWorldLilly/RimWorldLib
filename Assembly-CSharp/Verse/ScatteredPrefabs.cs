using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002D1 RID: 721
	public class ScatteredPrefabs
	{
		// Token: 0x04001310 RID: 4880
		public List<GenStep_ScatterGroupPrefabs.ScatterGroup> prefabs = new List<GenStep_ScatterGroupPrefabs.ScatterGroup>();

		// Token: 0x04001311 RID: 4881
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x04001312 RID: 4882
		public int minSpacing;

		// Token: 0x04001313 RID: 4883
		public float maxDistFromStructure;
	}
}
