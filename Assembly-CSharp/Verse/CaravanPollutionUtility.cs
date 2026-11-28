using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020002E8 RID: 744
	public static class CaravanPollutionUtility
	{
		// Token: 0x060016A6 RID: 5798 RVA: 0x00087BF4 File Offset: 0x00085DF4
		public static void CheckDamageFromPollution(Caravan caravan, int delta)
		{
			if (!caravan.IsHashIntervalTick(3451, delta))
			{
				return;
			}
			if (Find.WorldGrid[caravan.Tile].PollutionLevel() < PollutionLevel.Moderate)
			{
				return;
			}
			float extraFactor = CaravanPollutionUtility.ToxicDamagePollutionFactor(caravan.Tile);
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				ToxicUtility.DoPawnToxicDamage(pawnsListForReading[i], extraFactor);
			}
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00087C5A File Offset: 0x00085E5A
		public static float ToxicDamagePollutionFactor(PlanetTile tile)
		{
			if (Find.WorldGrid[tile].PollutionLevel() == PollutionLevel.Moderate)
			{
				return 0.5f;
			}
			return 1f;
		}

		// Token: 0x0400136D RID: 4973
		private const float ModeratePollutionToxicDamageFactor = 0.5f;
	}
}
