using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E82 RID: 7810
	[DefOf]
	public static class PawnCapacityDefOf
	{
		// Token: 0x0600CA3D RID: 51773 RVA: 0x004A5CCF File Offset: 0x004A3ECF
		static PawnCapacityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnCapacityDefOf));
		}

		// Token: 0x04007A1E RID: 31262
		public static PawnCapacityDef Consciousness;

		// Token: 0x04007A1F RID: 31263
		public static PawnCapacityDef Sight;

		// Token: 0x04007A20 RID: 31264
		public static PawnCapacityDef Hearing;

		// Token: 0x04007A21 RID: 31265
		public static PawnCapacityDef Moving;

		// Token: 0x04007A22 RID: 31266
		public static PawnCapacityDef Manipulation;

		// Token: 0x04007A23 RID: 31267
		public static PawnCapacityDef Talking;

		// Token: 0x04007A24 RID: 31268
		public static PawnCapacityDef Breathing;

		// Token: 0x04007A25 RID: 31269
		public static PawnCapacityDef BloodFiltration;

		// Token: 0x04007A26 RID: 31270
		public static PawnCapacityDef BloodPumping;
	}
}
