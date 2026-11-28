using System;

namespace Verse
{
	// Token: 0x02000395 RID: 917
	public static class PawnRenderFlagsExtension
	{
		// Token: 0x06001BEF RID: 7151 RVA: 0x000ACFE9 File Offset: 0x000AB1E9
		public static bool FlagSet(this PawnRenderFlags flags, PawnRenderFlags flag)
		{
			return (flags & flag) > PawnRenderFlags.None;
		}
	}
}
