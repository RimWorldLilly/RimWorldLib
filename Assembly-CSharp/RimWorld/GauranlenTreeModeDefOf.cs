using System;

namespace RimWorld
{
	// Token: 0x02001EE8 RID: 7912
	[DefOf]
	public static class GauranlenTreeModeDefOf
	{
		// Token: 0x0600CAA2 RID: 51874 RVA: 0x004A6384 File Offset: 0x004A4584
		static GauranlenTreeModeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GauranlenTreeModeDefOf));
		}

		// Token: 0x040080BC RID: 32956
		[MayRequireIdeology]
		public static GauranlenTreeModeDef Gaumaker;
	}
}
