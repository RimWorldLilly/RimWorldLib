using System;

namespace Verse
{
	// Token: 0x02000720 RID: 1824
	public static class ShowWeaponsUnderPortraitModeExtension
	{
		// Token: 0x060039BB RID: 14779 RVA: 0x0016FC54 File Offset: 0x0016DE54
		public static string ToStringHuman(this ShowWeaponsUnderPortraitMode mode)
		{
			switch (mode)
			{
			case ShowWeaponsUnderPortraitMode.Never:
				return "Never".Translate().CapitalizeFirst();
			case ShowWeaponsUnderPortraitMode.WhileDrafted:
				return "ShowWeapons_WhileDrafted".Translate().CapitalizeFirst();
			case ShowWeaponsUnderPortraitMode.Always:
				return "ShowWeapons_Always".Translate().CapitalizeFirst();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
