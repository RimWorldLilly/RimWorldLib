using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200237C RID: 9084
	public static class BestCaravanPawnUtility
	{
		// Token: 0x0600E193 RID: 57747 RVA: 0x0053B3EB File Offset: 0x005395EB
		public static Pawn FindBestDiplomat(Caravan caravan)
		{
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.NegotiationAbility, null);
		}

		// Token: 0x0600E194 RID: 57748 RVA: 0x0053B3FC File Offset: 0x005395FC
		public static Pawn FindBestNegotiator(Caravan caravan, Faction negotiatingWith = null, TraderKindDef trader = null)
		{
			Predicate<Pawn> pawnValidator = null;
			if (negotiatingWith != null)
			{
				pawnValidator = ((Pawn p) => p.CanTradeWith(negotiatingWith, trader).Accepted);
			}
			return BestCaravanPawnUtility.FindPawnWithBestStat(caravan, StatDefOf.TradePriceImprovement, pawnValidator);
		}

		// Token: 0x0600E195 RID: 57749 RVA: 0x0053B440 File Offset: 0x00539640
		public static Pawn FindBestEntertainingPawnFor(Caravan caravan, Pawn forPawn)
		{
			Pawn pawn = null;
			float num = -1f;
			for (int i = 0; i < caravan.pawns.Count; i++)
			{
				Pawn pawn2 = caravan.pawns[i];
				if (pawn2 != forPawn && pawn2.RaceProps.Humanlike && !pawn2.Dead && !pawn2.Downed && !pawn2.InMentalState && pawn2.IsPrisoner == forPawn.IsPrisoner && !StatDefOf.SocialImpact.Worker.IsDisabledFor(pawn2))
				{
					float statValue = pawn2.GetStatValue(StatDefOf.SocialImpact, true, -1);
					if (pawn == null || statValue > num)
					{
						pawn = pawn2;
						num = statValue;
					}
				}
			}
			return pawn;
		}

		// Token: 0x0600E196 RID: 57750 RVA: 0x0053B4E4 File Offset: 0x005396E4
		public static Pawn FindPawnWithBestStat(Caravan caravan, StatDef stat, Predicate<Pawn> pawnValidator = null)
		{
			Pawn pawn = null;
			float num = -1f;
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn2 = pawnsListForReading[i];
				if (BestCaravanPawnUtility.IsConsciousOwner(pawn2, caravan) && !stat.Worker.IsDisabledFor(pawn2) && (pawnValidator == null || pawnValidator(pawn2)))
				{
					float statValue = pawn2.GetStatValue(stat, true, -1);
					if (pawn == null || statValue > num)
					{
						pawn = pawn2;
						num = statValue;
					}
				}
			}
			return pawn;
		}

		// Token: 0x0600E197 RID: 57751 RVA: 0x0053B55B File Offset: 0x0053975B
		private static bool IsConsciousOwner(Pawn pawn, Caravan caravan)
		{
			return !pawn.Dead && !pawn.Downed && !pawn.InMentalState && caravan.IsOwner(pawn);
		}
	}
}
