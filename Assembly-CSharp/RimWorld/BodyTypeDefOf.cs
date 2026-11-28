using System;

namespace RimWorld
{
	// Token: 0x02001ECC RID: 7884
	[DefOf]
	public static class BodyTypeDefOf
	{
		// Token: 0x0600CA87 RID: 51847 RVA: 0x004A61B9 File Offset: 0x004A43B9
		static BodyTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyTypeDefOf));
		}

		// Token: 0x04007F5B RID: 32603
		public static BodyTypeDef Male;

		// Token: 0x04007F5C RID: 32604
		public static BodyTypeDef Female;

		// Token: 0x04007F5D RID: 32605
		public static BodyTypeDef Thin;

		// Token: 0x04007F5E RID: 32606
		public static BodyTypeDef Hulk;

		// Token: 0x04007F5F RID: 32607
		public static BodyTypeDef Fat;

		// Token: 0x04007F60 RID: 32608
		[MayRequireBiotech]
		public static BodyTypeDef Baby;

		// Token: 0x04007F61 RID: 32609
		[MayRequireBiotech]
		public static BodyTypeDef Child;
	}
}
