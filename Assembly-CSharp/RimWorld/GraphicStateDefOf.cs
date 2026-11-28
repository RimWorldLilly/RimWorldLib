using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F0B RID: 7947
	[DefOf]
	public static class GraphicStateDefOf
	{
		// Token: 0x0600CAC3 RID: 51907 RVA: 0x004A65B5 File Offset: 0x004A47B5
		static GraphicStateDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GraphicStateDefOf));
		}

		// Token: 0x04008150 RID: 33104
		public static GraphicStateDef Swimming;

		// Token: 0x04008151 RID: 33105
		public static GraphicStateDef Stationary;
	}
}
