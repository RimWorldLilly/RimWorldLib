using System;

namespace Verse
{
	// Token: 0x0200071C RID: 1820
	public static class AnimalNameDisplayModeExtension
	{
		// Token: 0x060039B7 RID: 14775 RVA: 0x0016FAE4 File Offset: 0x0016DCE4
		public static string ToStringHuman(this AnimalNameDisplayMode mode)
		{
			switch (mode)
			{
			case AnimalNameDisplayMode.None:
				return "None".Translate();
			case AnimalNameDisplayMode.TameNamed:
				return "AnimalNameDisplayMode_TameNamed".Translate();
			case AnimalNameDisplayMode.TameAll:
				return "AnimalNameDisplayMode_TameAll".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x0016FB3C File Offset: 0x0016DD3C
		public static bool ShouldDisplayAnimalName(this AnimalNameDisplayMode mode, Pawn animal)
		{
			switch (mode)
			{
			case AnimalNameDisplayMode.None:
				return false;
			case AnimalNameDisplayMode.TameNamed:
				return animal.Name != null && !animal.Name.Numerical;
			case AnimalNameDisplayMode.TameAll:
				return animal.Name != null;
			default:
				throw new NotImplementedException(Prefs.AnimalNameMode.ToStringSafe<AnimalNameDisplayMode>());
			}
		}
	}
}
