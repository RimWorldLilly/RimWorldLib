using System;

namespace RimWorld
{
	// Token: 0x02001EDD RID: 7901
	[DefOf]
	public static class MemeDefOf
	{
		// Token: 0x0600CA97 RID: 51863 RVA: 0x004A62C9 File Offset: 0x004A44C9
		static MemeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MemeDefOf));
		}

		// Token: 0x04008071 RID: 32881
		[MayRequireIdeology]
		public static MemeDef MaleSupremacy;

		// Token: 0x04008072 RID: 32882
		[MayRequireIdeology]
		public static MemeDef FemaleSupremacy;

		// Token: 0x04008073 RID: 32883
		[MayRequireIdeology]
		public static MemeDef TreeConnection;

		// Token: 0x04008074 RID: 32884
		[MayRequireIdeology]
		public static MemeDef Tunneler;

		// Token: 0x04008075 RID: 32885
		[MayRequireIdeology]
		public static MemeDef Darkness;
	}
}
