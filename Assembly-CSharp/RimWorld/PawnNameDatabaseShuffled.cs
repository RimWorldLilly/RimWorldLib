using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CF RID: 5327
	[StaticConstructorOnStartup]
	public static class PawnNameDatabaseShuffled
	{
		// Token: 0x0600823C RID: 33340 RVA: 0x002E9EFC File Offset: 0x002E80FC
		static PawnNameDatabaseShuffled()
		{
			foreach (object obj in Enum.GetValues(typeof(PawnNameCategory)))
			{
				PawnNameCategory pawnNameCategory = (PawnNameCategory)obj;
				if (pawnNameCategory != PawnNameCategory.NoName)
				{
					PawnNameDatabaseShuffled.banks.Add(pawnNameCategory, new NameBank(pawnNameCategory));
				}
			}
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			nameBank.AddNamesFromFile(PawnNameSlot.First, Gender.Male, "First_Male");
			nameBank.AddNamesFromFile(PawnNameSlot.First, Gender.Female, "First_Female");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.Male, "Nick_Male");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.Female, "Nick_Female");
			nameBank.AddNamesFromFile(PawnNameSlot.Nick, Gender.None, "Nick_Unisex");
			nameBank.AddNamesFromFile(PawnNameSlot.Last, Gender.None, "Last");
			foreach (NameBank nameBank2 in PawnNameDatabaseShuffled.banks.Values)
			{
				nameBank2.ErrorCheck();
			}
		}

		// Token: 0x0600823D RID: 33341 RVA: 0x002EA00C File Offset: 0x002E820C
		public static NameBank BankOf(PawnNameCategory category)
		{
			return PawnNameDatabaseShuffled.banks[category];
		}

		// Token: 0x040050AD RID: 20653
		private static Dictionary<PawnNameCategory, NameBank> banks = new Dictionary<PawnNameCategory, NameBank>();
	}
}
