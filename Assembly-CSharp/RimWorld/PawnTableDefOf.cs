using System;

namespace RimWorld
{
	// Token: 0x02001EBB RID: 7867
	[DefOf]
	public static class PawnTableDefOf
	{
		// Token: 0x0600CA76 RID: 51830 RVA: 0x004A6098 File Offset: 0x004A4298
		static PawnTableDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnTableDefOf));
		}

		// Token: 0x04007EDE RID: 32478
		public static PawnTableDef Work;

		// Token: 0x04007EDF RID: 32479
		public static PawnTableDef Assign;

		// Token: 0x04007EE0 RID: 32480
		public static PawnTableDef Restrict;

		// Token: 0x04007EE1 RID: 32481
		public static PawnTableDef Animals;

		// Token: 0x04007EE2 RID: 32482
		public static PawnTableDef Wildlife;

		// Token: 0x04007EE3 RID: 32483
		[MayRequireBiotech]
		public static PawnTableDef Mechs;
	}
}
