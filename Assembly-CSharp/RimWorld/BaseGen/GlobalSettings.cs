using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020020DA RID: 8410
	public class GlobalSettings
	{
		// Token: 0x0600D3AF RID: 54191 RVA: 0x004E1C18 File Offset: 0x004DFE18
		public void Clear()
		{
			this.map = null;
			this.minBuildings = 0;
			this.minBarracks = 0;
			this.requiredWorshippedTerminalRooms = 0;
			this.requiredGravcoreRooms = 0;
			this.minEmptyNodes = 0;
			this.minLandingPads = 0;
			this.minThroneRooms = 0;
			this.maxFarms = -1;
			this.mainRect = CellRect.Empty;
			this.basePart_buildingsResolved = 0;
			this.basePart_emptyNodesResolved = 0;
			this.basePart_landingPadsResolved = 0;
			this.basePart_barracksResolved = 0;
			this.basePart_throneRoomsResolved = 0;
			this.basePart_batteriesCoverage = 0f;
			this.basePart_farmsCoverage = 0f;
			this.basePart_farmsCount = 0;
			this.basePart_powerPlantsCoverage = 0f;
			this.basePart_breweriesCoverage = 0f;
			this.basePart_worshippedTerminalsResolved = 0;
			this.basePart_gravcoresResolved = 0;
		}

		// Token: 0x0600D3B0 RID: 54192 RVA: 0x004E1CD3 File Offset: 0x004DFED3
		public void ClearResult()
		{
			this.landingPadsGenerated = 0;
		}

		// Token: 0x04008607 RID: 34311
		public Map map;

		// Token: 0x04008608 RID: 34312
		public int minBuildings;

		// Token: 0x04008609 RID: 34313
		public int minEmptyNodes;

		// Token: 0x0400860A RID: 34314
		public int minLandingPads;

		// Token: 0x0400860B RID: 34315
		public int minBarracks;

		// Token: 0x0400860C RID: 34316
		public int requiredWorshippedTerminalRooms;

		// Token: 0x0400860D RID: 34317
		public int requiredGravcoreRooms;

		// Token: 0x0400860E RID: 34318
		public int minThroneRooms;

		// Token: 0x0400860F RID: 34319
		public int maxFarms = -1;

		// Token: 0x04008610 RID: 34320
		public CellRect mainRect;

		// Token: 0x04008611 RID: 34321
		public int basePart_buildingsResolved;

		// Token: 0x04008612 RID: 34322
		public int basePart_emptyNodesResolved;

		// Token: 0x04008613 RID: 34323
		public int basePart_landingPadsResolved;

		// Token: 0x04008614 RID: 34324
		public int basePart_barracksResolved;

		// Token: 0x04008615 RID: 34325
		public int basePart_throneRoomsResolved;

		// Token: 0x04008616 RID: 34326
		public int basePart_worshippedTerminalsResolved;

		// Token: 0x04008617 RID: 34327
		public int basePart_gravcoresResolved;

		// Token: 0x04008618 RID: 34328
		public float basePart_batteriesCoverage;

		// Token: 0x04008619 RID: 34329
		public float basePart_farmsCoverage;

		// Token: 0x0400861A RID: 34330
		public int basePart_farmsCount;

		// Token: 0x0400861B RID: 34331
		public float basePart_powerPlantsCoverage;

		// Token: 0x0400861C RID: 34332
		public float basePart_breweriesCoverage;

		// Token: 0x0400861D RID: 34333
		public int landingPadsGenerated;
	}
}
