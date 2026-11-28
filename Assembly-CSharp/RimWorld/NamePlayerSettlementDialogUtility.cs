using System;
using RimWorld.Planet;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001CC0 RID: 7360
	public static class NamePlayerSettlementDialogUtility
	{
		// Token: 0x0600BD02 RID: 48386 RVA: 0x0043A352 File Offset: 0x00438552
		public static bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x0600BD03 RID: 48387 RVA: 0x0043A375 File Offset: 0x00438575
		public static void Named(Settlement factionBase, string s)
		{
			factionBase.Name = s;
			factionBase.namedByPlayer = true;
		}

		// Token: 0x04006C04 RID: 27652
		public const int CharacterLimit = 64;
	}
}
