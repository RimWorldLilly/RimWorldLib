using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EEE RID: 7918
	[DefOf]
	public static class GeneDefOf
	{
		// Token: 0x0600CAA6 RID: 51878 RVA: 0x004A63C8 File Offset: 0x004A45C8
		static GeneDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(GeneDefOf));
		}

		// Token: 0x040080CC RID: 32972
		[MayRequireBiotech]
		public static GeneDef Bloodfeeder;

		// Token: 0x040080CD RID: 32973
		[MayRequireBiotech]
		public static GeneDef Deathless;

		// Token: 0x040080CE RID: 32974
		[MayRequireBiotech]
		public static GeneDef XenogermReimplanter;

		// Token: 0x040080CF RID: 32975
		[MayRequireBiotech]
		public static GeneDef Hemogenic;

		// Token: 0x040080D0 RID: 32976
		[MayRequireBiotech]
		public static GeneDef FireTerror;

		// Token: 0x040080D1 RID: 32977
		[MayRequireBiotech]
		public static GeneDef Inbred;

		// Token: 0x040080D2 RID: 32978
		[MayRequireBiotech]
		public static GeneDef WebbedPhalanges;
	}
}
