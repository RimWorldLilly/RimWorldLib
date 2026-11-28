using System;

namespace RimWorld
{
	// Token: 0x02001EE7 RID: 7911
	[DefOf]
	public static class LayoutRoomDefOf
	{
		// Token: 0x0600CAA1 RID: 51873 RVA: 0x004A6373 File Offset: 0x004A4573
		static LayoutRoomDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(LayoutRoomDefOf));
		}

		// Token: 0x040080B3 RID: 32947
		[MayRequireAnomaly]
		public static LayoutRoomDef LabyrinthObelisk;

		// Token: 0x040080B4 RID: 32948
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientRuinsCorridor;

		// Token: 0x040080B5 RID: 32949
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientRuinsReactorCorridor;

		// Token: 0x040080B6 RID: 32950
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientOrbitalCorridor;

		// Token: 0x040080B7 RID: 32951
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientRuinsReactor;

		// Token: 0x040080B8 RID: 32952
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientRuinsTerraformer;

		// Token: 0x040080B9 RID: 32953
		[MayRequireOdyssey]
		public static LayoutRoomDef MechanoidEngineRoom;

		// Token: 0x040080BA RID: 32954
		[MayRequireOdyssey]
		public static LayoutRoomDef AncientEngineRoom;

		// Token: 0x040080BB RID: 32955
		[MayRequireOdyssey]
		public static LayoutRoomDef Mechhive_Stabilizer;
	}
}
