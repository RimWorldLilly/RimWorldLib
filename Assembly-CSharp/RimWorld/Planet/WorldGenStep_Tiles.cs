using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002354 RID: 9044
	public class WorldGenStep_Tiles : WorldGenStep
	{
		// Token: 0x17002754 RID: 10068
		// (get) Token: 0x0600DFF2 RID: 57330 RVA: 0x00535327 File Offset: 0x00533527
		public override int SeedPart
		{
			get
			{
				return 16315992;
			}
		}

		// Token: 0x0600DFF3 RID: 57331 RVA: 0x00535330 File Offset: 0x00533530
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			layer.Tiles.Clear();
			for (int i = 0; i < layer.TilesCount; i++)
			{
				Tile item = this.GenerateTileFor(new PlanetTile(i, layer), layer);
				layer.Tiles.Add(item);
			}
		}

		// Token: 0x0600DFF4 RID: 57332 RVA: 0x00535374 File Offset: 0x00533574
		protected virtual Tile GenerateTileFor(PlanetTile tile, PlanetLayer layer)
		{
			return new Tile(tile)
			{
				PrimaryBiome = tile.LayerDef.DefaultBiome,
				elevation = tile.Layer.Radius
			};
		}
	}
}
