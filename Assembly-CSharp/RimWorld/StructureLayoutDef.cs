using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9A RID: 3994
	public class StructureLayoutDef : LayoutDef
	{
		// Token: 0x04003CFC RID: 15612
		public ThingDef wallDef;

		// Token: 0x04003CFD RID: 15613
		public ThingDef wallStuffDef;

		// Token: 0x04003CFE RID: 15614
		public ThingDef doorDef;

		// Token: 0x04003CFF RID: 15615
		public ThingDef doorStuffDef;

		// Token: 0x04003D00 RID: 15616
		public ThingDef wallLampDef;

		// Token: 0x04003D01 RID: 15617
		public float wallLampChancePerPosition = 0.6f;

		// Token: 0x04003D02 RID: 15618
		public ThingDef exteriorDoorDef;

		// Token: 0x04003D03 RID: 15619
		public ThingDef exteriorDoorStuffDef;

		// Token: 0x04003D04 RID: 15620
		public TerrainDef terrainDef;

		// Token: 0x04003D05 RID: 15621
		public TerrainDef defaultAffordanceTerrainDef;

		// Token: 0x04003D06 RID: 15622
		public LayoutRoomDef importantRoomDef;

		// Token: 0x04003D07 RID: 15623
		public float roomToExteriorDoorRatio = 0.33f;

		// Token: 0x04003D08 RID: 15624
		public bool spawnDoors = true;

		// Token: 0x04003D09 RID: 15625
		public bool clearDoorFaction = true;

		// Token: 0x04003D0A RID: 15626
		public bool canConnectRoomsExternally = true;

		// Token: 0x04003D0B RID: 15627
		public float areaPrunePercent = 0.3f;

		// Token: 0x04003D0C RID: 15628
		public bool canDisconnectRooms = true;

		// Token: 0x04003D0D RID: 15629
		public bool ensureOneDoorUnlocked;

		// Token: 0x04003D0E RID: 15630
		public LayoutRoomDef corridorDef;

		// Token: 0x04003D0F RID: 15631
		public IntRange wallDamageRange = IntRange.Invalid;

		// Token: 0x04003D10 RID: 15632
		public CorridorShape corridorShapes = CorridorShape.All;

		// Token: 0x04003D11 RID: 15633
		public TerrainDef surroundingTerrainDef;

		// Token: 0x04003D12 RID: 15634
		public TerrainDef surroundingScatterTerrainDef;

		// Token: 0x04003D13 RID: 15635
		public IntRange surroundingScatterRange = new IntRange(2, 6);

		// Token: 0x04003D14 RID: 15636
		public List<ScatteredPrefabs> scatteredPrefabs = new List<ScatteredPrefabs>();
	}
}
