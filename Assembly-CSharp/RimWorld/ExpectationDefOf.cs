using System;

namespace RimWorld
{
	// Token: 0x02001ECF RID: 7887
	[DefOf]
	public static class ExpectationDefOf
	{
		// Token: 0x0600CA8A RID: 51850 RVA: 0x004A61EC File Offset: 0x004A43EC
		static ExpectationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ExpectationDefOf));
		}

		// Token: 0x04007F6D RID: 32621
		public static ExpectationDef ExtremelyLow;

		// Token: 0x04007F6E RID: 32622
		public static ExpectationDef VeryLow;

		// Token: 0x04007F6F RID: 32623
		public static ExpectationDef Low;

		// Token: 0x04007F70 RID: 32624
		public static ExpectationDef Moderate;

		// Token: 0x04007F71 RID: 32625
		public static ExpectationDef High;
	}
}
