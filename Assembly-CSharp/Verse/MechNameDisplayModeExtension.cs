using System;

namespace Verse
{
	// Token: 0x0200071E RID: 1822
	public static class MechNameDisplayModeExtension
	{
		// Token: 0x060039B9 RID: 14777 RVA: 0x0016FB94 File Offset: 0x0016DD94
		public static string ToStringHuman(this MechNameDisplayMode mode)
		{
			switch (mode)
			{
			case MechNameDisplayMode.None:
				return "Never".Translate().CapitalizeFirst();
			case MechNameDisplayMode.WhileDrafted:
				return "MechNameDisplayMode_WhileDrafted".Translate().CapitalizeFirst();
			case MechNameDisplayMode.Always:
				return "MechNameDisplayMode_Always".Translate().CapitalizeFirst();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x0016FC04 File Offset: 0x0016DE04
		public static bool ShouldDisplayMechName(this MechNameDisplayMode mode, Pawn mech)
		{
			switch (mode)
			{
			case MechNameDisplayMode.None:
				return false;
			case MechNameDisplayMode.WhileDrafted:
				return mech.Name != null && mech.Drafted;
			case MechNameDisplayMode.Always:
				return mech.Name != null;
			default:
				throw new NotImplementedException(Prefs.MechNameMode.ToStringSafe<MechNameDisplayMode>());
			}
		}
	}
}
