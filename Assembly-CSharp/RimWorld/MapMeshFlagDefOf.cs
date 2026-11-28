using System;

namespace RimWorld
{
	// Token: 0x02001EFF RID: 7935
	[DefOf]
	public static class MapMeshFlagDefOf
	{
		// Token: 0x0600CAB7 RID: 51895 RVA: 0x004A64E9 File Offset: 0x004A46E9
		static MapMeshFlagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MapMeshFlagDefOf));
		}

		// Token: 0x04008104 RID: 33028
		public static MapMeshFlagDef None;

		// Token: 0x04008105 RID: 33029
		public static MapMeshFlagDef Things;

		// Token: 0x04008106 RID: 33030
		public static MapMeshFlagDef FogOfWar;

		// Token: 0x04008107 RID: 33031
		public static MapMeshFlagDef Buildings;

		// Token: 0x04008108 RID: 33032
		public static MapMeshFlagDef GroundGlow;

		// Token: 0x04008109 RID: 33033
		public static MapMeshFlagDef Terrain;

		// Token: 0x0400810A RID: 33034
		public static MapMeshFlagDef Roofs;

		// Token: 0x0400810B RID: 33035
		public static MapMeshFlagDef Snow;

		// Token: 0x0400810C RID: 33036
		public static MapMeshFlagDef Zone;

		// Token: 0x0400810D RID: 33037
		public static MapMeshFlagDef Plan;

		// Token: 0x0400810E RID: 33038
		public static MapMeshFlagDef PowerGrid;

		// Token: 0x0400810F RID: 33039
		public static MapMeshFlagDef BuildingsDamage;

		// Token: 0x04008110 RID: 33040
		public static MapMeshFlagDef Gas;

		// Token: 0x04008111 RID: 33041
		[MayRequireBiotech]
		public static MapMeshFlagDef Pollution;

		// Token: 0x04008112 RID: 33042
		[MayRequireOdyssey]
		public static MapMeshFlagDef Sand;
	}
}
