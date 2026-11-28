using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EBF RID: 7871
	[DefOf]
	public static class SubcameraDefOf
	{
		// Token: 0x0600CA7A RID: 51834 RVA: 0x004A60DC File Offset: 0x004A42DC
		static SubcameraDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SubcameraDefOf));
		}

		// Token: 0x04007F08 RID: 32520
		public static SubcameraDef WaterDepth;

		// Token: 0x04007F09 RID: 32521
		[MayRequireOdyssey]
		public static SubcameraDef GravshipMask;
	}
}
