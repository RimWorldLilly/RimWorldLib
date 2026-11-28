using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F0E RID: 7950
	[DefOf]
	public static class PathGridDefOf
	{
		// Token: 0x0600CAC6 RID: 51910 RVA: 0x004A65E8 File Offset: 0x004A47E8
		static PathGridDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PathGridDefOf));
		}

		// Token: 0x0400815F RID: 33119
		public static PathGridDef Normal;

		// Token: 0x04008160 RID: 33120
		public static PathGridDef FenceBlocked;

		// Token: 0x04008161 RID: 33121
		public static PathGridDef Flying;
	}
}
