using System;

namespace RimWorld
{
	// Token: 0x02001EF0 RID: 7920
	[DefOf]
	public static class XenotypeIconDefOf
	{
		// Token: 0x0600CAA8 RID: 51880 RVA: 0x004A63EA File Offset: 0x004A45EA
		static XenotypeIconDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(XenotypeIconDefOf));
		}

		// Token: 0x040080D5 RID: 32981
		[MayRequireBiotech]
		public static XenotypeIconDef Basic;
	}
}
