using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EFA RID: 7930
	[DefOf]
	public static class PawnRenderNodeTagDefOf
	{
		// Token: 0x0600CAB2 RID: 51890 RVA: 0x004A6494 File Offset: 0x004A4694
		static PawnRenderNodeTagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnRenderNodeTagDefOf));
		}

		// Token: 0x040080F4 RID: 33012
		public static PawnRenderNodeTagDef Head;

		// Token: 0x040080F5 RID: 33013
		public static PawnRenderNodeTagDef Body;

		// Token: 0x040080F6 RID: 33014
		public static PawnRenderNodeTagDef ApparelHead;

		// Token: 0x040080F7 RID: 33015
		public static PawnRenderNodeTagDef ApparelBody;
	}
}
