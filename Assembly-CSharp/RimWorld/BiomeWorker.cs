using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F40 RID: 3904
	public abstract class BiomeWorker
	{
		// Token: 0x060064BA RID: 25786
		public abstract float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile);

		// Token: 0x060064BB RID: 25787 RVA: 0x0023F900 File Offset: 0x0023DB00
		public virtual bool CanPlaceOnLayer(BiomeDef biome, PlanetLayer layer)
		{
			return (biome.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || biome.layerWhitelist.Contains(layer.Def)) && (biome.layerBlacklist.NullOrEmpty<PlanetLayerDef>() || !biome.layerBlacklist.Contains(layer.Def)) && (!layer.Def.onlyAllowWhitelistedBiomes || (!biome.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && biome.layerWhitelist.Contains(layer.Def)));
		}
	}
}
