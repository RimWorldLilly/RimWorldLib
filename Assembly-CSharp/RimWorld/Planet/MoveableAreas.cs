using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F3 RID: 8947
	public class MoveableAreas : IExposable
	{
		// Token: 0x0600DD32 RID: 56626 RVA: 0x00525C90 File Offset: 0x00523E90
		public void ExposeData()
		{
			Scribe_Collections.Look<MoveableStockpile>(ref this.stockpiles, "stockpiles", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<MoveableGrowZone>(ref this.growZones, "growZones", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<MoveableArea_Allowed>(ref this.allowedAreas, "allowedAreas", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<MoveableStorageGroup>(ref this.storageGroups, "storageGroups", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea_Allowed>(ref this.homeArea, "homeArea", Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea>(ref this.buildRoofArea, "buildRoofArea", Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea>(ref this.noRoofArea, "noRoofArea", Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea>(ref this.snowClearArea, "snowClearArea", Array.Empty<object>());
			Scribe_Deep.Look<MoveableArea>(ref this.pollutionClearArea, "pollutionClearArea", Array.Empty<object>());
		}

		// Token: 0x04008DBE RID: 36286
		public List<MoveableStockpile> stockpiles = new List<MoveableStockpile>();

		// Token: 0x04008DBF RID: 36287
		public List<MoveableGrowZone> growZones = new List<MoveableGrowZone>();

		// Token: 0x04008DC0 RID: 36288
		public List<MoveableArea_Allowed> allowedAreas = new List<MoveableArea_Allowed>();

		// Token: 0x04008DC1 RID: 36289
		public List<MoveableStorageGroup> storageGroups = new List<MoveableStorageGroup>();

		// Token: 0x04008DC2 RID: 36290
		public MoveableArea_Allowed homeArea;

		// Token: 0x04008DC3 RID: 36291
		public MoveableArea buildRoofArea;

		// Token: 0x04008DC4 RID: 36292
		public MoveableArea noRoofArea;

		// Token: 0x04008DC5 RID: 36293
		public MoveableArea snowClearArea;

		// Token: 0x04008DC6 RID: 36294
		public MoveableArea pollutionClearArea;
	}
}
