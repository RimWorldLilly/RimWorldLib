using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F04 RID: 7940
	[DefOf]
	public static class MechWeightClassDefOf
	{
		// Token: 0x0600CABC RID: 51900 RVA: 0x004A653E File Offset: 0x004A473E
		static MechWeightClassDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MechWeightClassDefOf));
		}

		// Token: 0x04008129 RID: 33065
		public static MechWeightClassDef Light;

		// Token: 0x0400812A RID: 33066
		public static MechWeightClassDef Medium;

		// Token: 0x0400812B RID: 33067
		public static MechWeightClassDef Heavy;

		// Token: 0x0400812C RID: 33068
		public static MechWeightClassDef UltraHeavy;
	}
}
