using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200234D RID: 9037
	public class WorldGenStep_Lakes : WorldGenStep
	{
		// Token: 0x1700274C RID: 10060
		// (get) Token: 0x0600DFBB RID: 57275 RVA: 0x005332AC File Offset: 0x005314AC
		public override int SeedPart
		{
			get
			{
				return 401463656;
			}
		}

		// Token: 0x0600DFBC RID: 57276 RVA: 0x005332B3 File Offset: 0x005314B3
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			this.GenerateLakes(layer);
		}

		// Token: 0x0600DFBD RID: 57277 RVA: 0x005332BC File Offset: 0x005314BC
		private void GenerateLakes(PlanetLayer layer)
		{
			bool[] touched = new bool[layer.TilesCount];
			List<int> oceanChunk = new List<int>();
			Predicate<PlanetTile> <>9__0;
			Action<PlanetTile> <>9__1;
			foreach (Tile tile3 in layer.Tiles)
			{
				PlanetTile tile2 = tile3.tile;
				if (!touched[tile2.tileId] && layer[tile2.tileId].PrimaryBiome == BiomeDefOf.Ocean)
				{
					WorldFloodFiller filler = layer.Filler;
					PlanetTile rootTile = tile2;
					Predicate<PlanetTile> passCheck;
					if ((passCheck = <>9__0) == null)
					{
						passCheck = (<>9__0 = ((PlanetTile tid) => layer[tid].PrimaryBiome == BiomeDefOf.Ocean));
					}
					Action<PlanetTile> processor;
					if ((processor = <>9__1) == null)
					{
						processor = (<>9__1 = delegate(PlanetTile tile)
						{
							oceanChunk.Add(tile.tileId);
							touched[tile.tileId] = true;
						});
					}
					filler.FloodFill(rootTile, passCheck, processor, int.MaxValue, null);
					if (oceanChunk.Count <= 15)
					{
						for (int i = 0; i < oceanChunk.Count; i++)
						{
							layer[oceanChunk[i]].PrimaryBiome = BiomeDefOf.Lake;
						}
					}
					oceanChunk.Clear();
				}
			}
		}

		// Token: 0x04008F91 RID: 36753
		private const int LakeMaxSize = 15;
	}
}
