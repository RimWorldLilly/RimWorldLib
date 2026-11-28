using System;

namespace Verse
{
	// Token: 0x02000718 RID: 1816
	public static class HighlightDisplayEnumExtensions
	{
		// Token: 0x060039B4 RID: 14772 RVA: 0x0016F9F0 File Offset: 0x0016DBF0
		public static string ToStringHuman(this DotHighlightDisplayMode mode)
		{
			switch (mode)
			{
			case DotHighlightDisplayMode.None:
				return "None".Translate();
			case DotHighlightDisplayMode.HighlightHostiles:
				return "DotHighlightDisplayMode_HighlightHostiles".Translate();
			case DotHighlightDisplayMode.HighlightAll:
				return "DotHighlightDisplayMode_HighlightAll".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x0016FA46 File Offset: 0x0016DC46
		public static string ToStringHuman(this HighlightStyleMode mode)
		{
			if (mode == HighlightStyleMode.Dots)
			{
				return "HighlightStyleMode_Dots".Translate();
			}
			if (mode != HighlightStyleMode.Silhouettes)
			{
				throw new NotImplementedException();
			}
			return "HighlightStyleMode_Silhouettes".Translate();
		}
	}
}
