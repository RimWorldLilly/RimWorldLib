using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E71 RID: 7793
	[DefOf]
	public static class TerrainDefOf
	{
		// Token: 0x0600CA2C RID: 51756 RVA: 0x004A5BAE File Offset: 0x004A3DAE
		static TerrainDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TerrainDefOf));
		}

		// Token: 0x0400771C RID: 30492
		public static TerrainDef Sand;

		// Token: 0x0400771D RID: 30493
		public static TerrainDef SoftSand;

		// Token: 0x0400771E RID: 30494
		public static TerrainDef Soil;

		// Token: 0x0400771F RID: 30495
		public static TerrainDef SoilRich;

		// Token: 0x04007720 RID: 30496
		public static TerrainDef Underwall;

		// Token: 0x04007721 RID: 30497
		public static TerrainDef Concrete;

		// Token: 0x04007722 RID: 30498
		public static TerrainDef AncientConcrete;

		// Token: 0x04007723 RID: 30499
		public static TerrainDef MetalTile;

		// Token: 0x04007724 RID: 30500
		public static TerrainDef AncientTile;

		// Token: 0x04007725 RID: 30501
		public static TerrainDef Gravel;

		// Token: 0x04007726 RID: 30502
		public static TerrainDef WaterDeep;

		// Token: 0x04007727 RID: 30503
		public static TerrainDef WaterShallow;

		// Token: 0x04007728 RID: 30504
		public static TerrainDef WaterMovingChestDeep;

		// Token: 0x04007729 RID: 30505
		public static TerrainDef WaterMovingShallow;

		// Token: 0x0400772A RID: 30506
		public static TerrainDef WaterOceanDeep;

		// Token: 0x0400772B RID: 30507
		public static TerrainDef WaterOceanShallow;

		// Token: 0x0400772C RID: 30508
		public static TerrainDef PavedTile;

		// Token: 0x0400772D RID: 30509
		public static TerrainDef WoodPlankFloor;

		// Token: 0x0400772E RID: 30510
		public static TerrainDef TileSandstone;

		// Token: 0x0400772F RID: 30511
		public static TerrainDef Ice;

		// Token: 0x04007730 RID: 30512
		public static TerrainDef Marsh;

		// Token: 0x04007731 RID: 30513
		public static TerrainDef Mud;

		// Token: 0x04007732 RID: 30514
		public static TerrainDef FlagstoneSandstone;

		// Token: 0x04007733 RID: 30515
		public static TerrainDef Bridge;

		// Token: 0x04007734 RID: 30516
		public static TerrainDef AncientBridge;

		// Token: 0x04007735 RID: 30517
		public static TerrainDef Sandstone_Smooth;

		// Token: 0x04007736 RID: 30518
		public static TerrainDef PackedDirt;

		// Token: 0x04007737 RID: 30519
		public static TerrainDef BrokenAsphalt;

		// Token: 0x04007738 RID: 30520
		public static TerrainDef Riverbank;

		// Token: 0x04007739 RID: 30521
		[MayRequireIdeology]
		public static TerrainDef FungalGravel;

		// Token: 0x0400773A RID: 30522
		[MayRequireAnomaly]
		public static TerrainDef Flesh;

		// Token: 0x0400773B RID: 30523
		[MayRequireAnomaly]
		public static TerrainDef Voidmetal;

		// Token: 0x0400773C RID: 30524
		[MayRequireAnomaly]
		public static TerrainDef GraySurface;

		// Token: 0x0400773D RID: 30525
		[MayRequireOdyssey]
		public static TerrainDef Substructure;

		// Token: 0x0400773E RID: 30526
		[MayRequireOdyssey]
		public static TerrainDef AncientMegastructure;

		// Token: 0x0400773F RID: 30527
		[MayRequireOdyssey]
		public static TerrainDef LavaDeep;

		// Token: 0x04007740 RID: 30528
		[MayRequireOdyssey]
		public static TerrainDef LavaShallow;

		// Token: 0x04007741 RID: 30529
		[MayRequireOdyssey]
		public static TerrainDef CooledLava;

		// Token: 0x04007742 RID: 30530
		[MayRequireOdyssey]
		public static TerrainDef VolcanicRock;

		// Token: 0x04007743 RID: 30531
		[MayRequireOdyssey]
		public static TerrainDef ThinIce;

		// Token: 0x04007744 RID: 30532
		[MayRequireOdyssey]
		public static TerrainDef ShallowFloodwater;

		// Token: 0x04007745 RID: 30533
		[MayRequireOdyssey]
		public static TerrainDef MarshFlood;

		// Token: 0x04007746 RID: 30534
		[MayRequireOdyssey]
		public static TerrainDef Space;

		// Token: 0x04007747 RID: 30535
		[MayRequireOdyssey]
		public static TerrainDef ToxicWaterShallow;

		// Token: 0x04007748 RID: 30536
		[MayRequireOdyssey]
		public static TerrainDef ToxicWaterDeep;

		// Token: 0x04007749 RID: 30537
		[MayRequireOdyssey]
		public static TerrainDef InsectSludge;

		// Token: 0x0400774A RID: 30538
		[MayRequireOdyssey]
		public static TerrainDef OrbitalPlatform;

		// Token: 0x0400774B RID: 30539
		[MayRequireOdyssey]
		public static TerrainDef MechanoidPlatform;

		// Token: 0x0400774C RID: 30540
		[MayRequireOdyssey]
		public static TerrainDef DryLakeBed;

		// Token: 0x0400774D RID: 30541
		[MayRequireOdyssey]
		public static TerrainDef HotSpring;

		// Token: 0x0400774E RID: 30542
		[MayRequireOdyssey]
		public static TerrainDef HeavyBridge;

		// Token: 0x0400774F RID: 30543
		[MayRequireOdyssey]
		public static TerrainDef AncientHeavyBridge;

		// Token: 0x04007750 RID: 30544
		[MayRequireOdyssey]
		public static TerrainDef Vacstone_Rough;

		// Token: 0x04007751 RID: 30545
		[MayRequireOdyssey]
		public static TerrainDef Vacstone_RoughHewn;
	}
}
