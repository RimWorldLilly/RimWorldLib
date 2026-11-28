using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200234A RID: 9034
	public class WorldGenStep_AncientSites : WorldGenStep
	{
		// Token: 0x17002749 RID: 10057
		// (get) Token: 0x0600DFB0 RID: 57264 RVA: 0x00533104 File Offset: 0x00531304
		public override int SeedPart
		{
			get
			{
				return 976238715;
			}
		}

		// Token: 0x0600DFB1 RID: 57265 RVA: 0x0053310B File Offset: 0x0053130B
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			this.GenerateAncientSites(layer);
		}

		// Token: 0x0600DFB2 RID: 57266 RVA: 0x00533114 File Offset: 0x00531314
		private void GenerateAncientSites(PlanetLayer layer)
		{
			int num = GenMath.RoundRandom((float)layer.TilesCount / 100000f * this.ancientSitesPer100kTiles.RandomInRange);
			Dictionary<PlanetLayer, List<PlanetTile>> ancientSites = Find.World.genData.ancientSites;
			List<PlanetTile> list;
			if (!ancientSites.TryGetValue(layer, out list))
			{
				list = (ancientSites[layer] = new List<PlanetTile>());
			}
			for (int i = 0; i < num; i++)
			{
				list.Add(TileFinder.RandomSettlementTileFor(layer, null, false, null));
			}
		}

		// Token: 0x04008F90 RID: 36752
		public FloatRange ancientSitesPer100kTiles;
	}
}
