using System;

namespace RimWorld
{
	// Token: 0x02001F00 RID: 7936
	[DefOf]
	public static class RenderSkipFlagDefOf
	{
		// Token: 0x0600CAB8 RID: 51896 RVA: 0x004A64FA File Offset: 0x004A46FA
		static RenderSkipFlagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(RenderSkipFlagDefOf));
		}

		// Token: 0x04008113 RID: 33043
		public static RenderSkipFlagDef None;

		// Token: 0x04008114 RID: 33044
		public static RenderSkipFlagDef Hair;

		// Token: 0x04008115 RID: 33045
		public static RenderSkipFlagDef Head;

		// Token: 0x04008116 RID: 33046
		public static RenderSkipFlagDef Beard;

		// Token: 0x04008117 RID: 33047
		public static RenderSkipFlagDef Eyes;

		// Token: 0x04008118 RID: 33048
		[MayRequireIdeology]
		public static RenderSkipFlagDef Tattoos;
	}
}
