using System;

namespace RimWorld
{
	// Token: 0x02001EDB RID: 7899
	[DefOf]
	public static class ShipJobDefOf
	{
		// Token: 0x0600CA95 RID: 51861 RVA: 0x004A62A7 File Offset: 0x004A44A7
		static ShipJobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShipJobDefOf));
		}

		// Token: 0x04008066 RID: 32870
		[MayRequireAnyOf("Ludeon.RimWorld.Royalty,Ludeon.RimWorld.Biotech")]
		public static ShipJobDef Arrive;

		// Token: 0x04008067 RID: 32871
		[MayRequireRoyalty]
		public static ShipJobDef FlyAway;

		// Token: 0x04008068 RID: 32872
		[MayRequireAnyOf("Ludeon.RimWorld.Royalty,Ludeon.RimWorld.Biotech")]
		public static ShipJobDef WaitTime;

		// Token: 0x04008069 RID: 32873
		[MayRequireRoyalty]
		public static ShipJobDef WaitForever;

		// Token: 0x0400806A RID: 32874
		[MayRequireAnyOf("Ludeon.RimWorld.Royalty,Ludeon.RimWorld.Biotech")]
		public static ShipJobDef Unload;

		// Token: 0x0400806B RID: 32875
		[MayRequireRoyalty]
		public static ShipJobDef Unload_Destination;

		// Token: 0x0400806C RID: 32876
		[MayRequireRoyalty]
		public static ShipJobDef WaitSendable;
	}
}
