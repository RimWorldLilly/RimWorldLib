using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EC8 RID: 7880
	[DefOf]
	public static class ApparelLayerDefOf
	{
		// Token: 0x0600CA83 RID: 51843 RVA: 0x004A6175 File Offset: 0x004A4375
		static ApparelLayerDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ApparelLayerDefOf));
		}

		// Token: 0x04007F47 RID: 32583
		public static ApparelLayerDef OnSkin;

		// Token: 0x04007F48 RID: 32584
		public static ApparelLayerDef Shell;

		// Token: 0x04007F49 RID: 32585
		public static ApparelLayerDef Middle;

		// Token: 0x04007F4A RID: 32586
		public static ApparelLayerDef Belt;

		// Token: 0x04007F4B RID: 32587
		public static ApparelLayerDef Overhead;

		// Token: 0x04007F4C RID: 32588
		public static ApparelLayerDef EyeCover;
	}
}
