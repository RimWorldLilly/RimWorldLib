using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EF3 RID: 7923
	[DefOf]
	public static class HeadTypeDefOf
	{
		// Token: 0x0600CAAB RID: 51883 RVA: 0x004A641D File Offset: 0x004A461D
		static HeadTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HeadTypeDefOf));
		}

		// Token: 0x040080DB RID: 32987
		public static HeadTypeDef Skull;

		// Token: 0x040080DC RID: 32988
		public static HeadTypeDef Stump;
	}
}
