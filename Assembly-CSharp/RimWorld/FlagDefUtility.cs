using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5E RID: 3934
	public static class FlagDefUtility
	{
		// Token: 0x06006531 RID: 25905 RVA: 0x002413C5 File Offset: 0x0023F5C5
		public static bool HasFlag(this ulong flags, RenderSkipFlagDef flagDef)
		{
			return (flags & flagDef) > 0UL;
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x002413D3 File Offset: 0x0023F5D3
		public static void SetMaskFromIndex(Def def, ref ulong mask)
		{
			if (def.defName == "None")
			{
				mask = 0UL;
				return;
			}
			mask = 1UL << (int)def.index;
		}
	}
}
