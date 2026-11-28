using System;

namespace RimWorld
{
	// Token: 0x02001EF1 RID: 7921
	[DefOf]
	public static class DebugTabMenuDefOf
	{
		// Token: 0x0600CAA9 RID: 51881 RVA: 0x004A63FB File Offset: 0x004A45FB
		static DebugTabMenuDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DebugTabMenuDefOf));
		}

		// Token: 0x040080D6 RID: 32982
		public static DebugTabMenuDef Actions;

		// Token: 0x040080D7 RID: 32983
		public static DebugTabMenuDef Settings;

		// Token: 0x040080D8 RID: 32984
		public static DebugTabMenuDef Output;
	}
}
