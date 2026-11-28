using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ED0 RID: 7888
	[DefOf]
	public static class GenStepDefOf
	{
		// Token: 0x0600CA8B RID: 51851 RVA: 0x004A61FD File Offset: 0x004A43FD
		static GenStepDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GenStepDefOf));
		}

		// Token: 0x04007F72 RID: 32626
		public static GenStepDef PreciousLump;

		// Token: 0x04007F73 RID: 32627
		[MayRequireAnomaly]
		public static GenStepDef HarbingerTrees;

		// Token: 0x04007F74 RID: 32628
		[MayRequireOdyssey]
		public static GenStepDef GravshipMarker;

		// Token: 0x04007F75 RID: 32629
		[MayRequireOdyssey]
		public static GenStepDef ReserveGravshipArea;

		// Token: 0x04007F76 RID: 32630
		[MayRequireOdyssey]
		public static GenStepDef AncientRuins_Special;

		// Token: 0x04007F77 RID: 32631
		[MayRequireOdyssey]
		public static GenStepDef AncientStockpile;
	}
}
