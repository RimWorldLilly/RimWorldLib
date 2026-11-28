using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2C RID: 7212
	public static class PlayerPawnsDisplayOrderUtility
	{
		// Token: 0x0600B7FD RID: 47101 RVA: 0x0040F15C File Offset: 0x0040D35C
		public static void Sort(List<Pawn> pawns)
		{
			pawns.SortBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter);
		}

		// Token: 0x0600B7FE RID: 47102 RVA: 0x0040F169 File Offset: 0x0040D369
		public static IEnumerable<Pawn> InOrder(IEnumerable<Pawn> pawns)
		{
			return pawns.OrderBy(PlayerPawnsDisplayOrderUtility.displayOrderGetter);
		}

		// Token: 0x04006995 RID: 27029
		private static Func<Pawn, int> displayOrderGetter = delegate(Pawn x)
		{
			if (x.playerSettings == null)
			{
				return 999999;
			}
			return x.playerSettings.displayOrder;
		};
	}
}
