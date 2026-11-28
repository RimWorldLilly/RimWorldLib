using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022E3 RID: 8931
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		// Token: 0x0600DC5E RID: 56414 RVA: 0x005218CC File Offset: 0x0051FACC
		public override void GenerateWhereAppropriate(PlanetLayer layer)
		{
			this.edgeTiles.Clear();
			for (int i = 0; i < layer.TilesCount; i++)
			{
				PlanetTile planetTile = new PlanetTile(i, layer);
				if (this.IsRoot(planetTile))
				{
					this.edgeTiles.Add(planetTile);
				}
			}
			if (!this.edgeTiles.Any<PlanetTile>())
			{
				return;
			}
			this.group.Clear();
			layer.Filler.FloodFill(PlanetTile.Invalid, new Predicate<PlanetTile>(this.CanTraverse), delegate(PlanetTile tile, int traversalDist)
			{
				this.group.Add(tile);
				return false;
			}, int.MaxValue, this.edgeTiles);
			this.group.RemoveAll((PlanetTile x) => layer[x].feature != null);
			if (this.group.Count < this.def.minSize || this.group.Count > this.def.maxSize)
			{
				return;
			}
			base.AddFeature(layer, this.group, this.group);
		}

		// Token: 0x0600DC5F RID: 56415 RVA: 0x005219E3 File Offset: 0x0051FBE3
		private bool IsRoot(PlanetTile tile)
		{
			return Find.WorldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && Find.WorldGrid[tile].feature == null;
		}

		// Token: 0x0600DC60 RID: 56416 RVA: 0x00521A10 File Offset: 0x0051FC10
		private bool CanTraverse(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			return primaryBiome == BiomeDefOf.Ocean || primaryBiome == BiomeDefOf.Lake;
		}

		// Token: 0x04008D4C RID: 36172
		private readonly List<PlanetTile> group = new List<PlanetTile>();

		// Token: 0x04008D4D RID: 36173
		private readonly List<PlanetTile> edgeTiles = new List<PlanetTile>();
	}
}
