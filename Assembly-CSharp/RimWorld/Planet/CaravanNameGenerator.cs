using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023A0 RID: 9120
	public static class CaravanNameGenerator
	{
		// Token: 0x0600E2F4 RID: 58100 RVA: 0x00546714 File Offset: 0x00544914
		public static string GenerateCaravanName(Caravan caravan)
		{
			Pawn pawn;
			if ((pawn = IdeoUtility.FindFirstPawnWithLeaderRole(caravan)) == null && (pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null)) == null)
			{
				pawn = (BestCaravanPawnUtility.FindBestDiplomat(caravan) ?? caravan.PawnsListForReading.Find((Pawn x) => caravan.IsOwner(x)));
			}
			Pawn pawn2 = pawn;
			TaggedString taggedString = (pawn2 != null) ? "CaravanLeaderCaravanName".Translate(pawn2.LabelShort, pawn2).CapitalizeFirst() : caravan.def.label;
			for (int i = 1; i <= 1000; i++)
			{
				TaggedString taggedString2 = taggedString;
				if (i != 1)
				{
					taggedString2 += " " + i.ToString();
				}
				if (!CaravanNameGenerator.CaravanNameInUse(taggedString2))
				{
					return taggedString2;
				}
			}
			Log.Error("Ran out of caravan names.");
			return caravan.def.label;
		}

		// Token: 0x0600E2F5 RID: 58101 RVA: 0x00546820 File Offset: 0x00544A20
		private static bool CaravanNameInUse(string name)
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].Name == name)
				{
					return true;
				}
			}
			return false;
		}
	}
}
