using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x0200133C RID: 4924
	public class TileMutatorWorker_MixedBiome : TileMutatorWorker
	{
		// Token: 0x060079F5 RID: 31221 RVA: 0x002B6767 File Offset: 0x002B4967
		public TileMutatorWorker_MixedBiome(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079F6 RID: 31222 RVA: 0x002B6770 File Offset: 0x002B4970
		public override bool IsValidTile(PlanetTile tile, PlanetLayer layer)
		{
			return this.def.biomeWhitelist.Contains(layer[tile].PrimaryBiome) && this.GetNeighbourTile(tile, layer) != null;
		}

		// Token: 0x060079F7 RID: 31223 RVA: 0x002B67B0 File Offset: 0x002B49B0
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			PlanetTile? neighbourTile = this.GetNeighbourTile(map.Tile, map.Tile.Layer);
			if (neighbourTile == null)
			{
				Log.Error("No neighbour tile found for mixed biome tile");
				return;
			}
			ModuleBase moduleBase = new DistFromAxis_Directional((float)map.Size.x / 2f);
			float angle = this.GetAngle(map.Tile, neighbourTile.Value.Tile.PrimaryBiome);
			moduleBase = new Rotate(0.0, (double)angle, 0.0, moduleBase);
			moduleBase = new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), moduleBase);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.006f, 40f, 2, -1);
			moduleBase = MapNoiseUtility.AddDisplacementNoise(moduleBase, 0.015f, 20f, 6, -1);
			NoiseDebugUI.StoreNoiseRender(moduleBase, "biomeVal");
			bool[] array = new bool[map.Size.x * map.Size.z];
			for (int i = 0; i < array.Length; i++)
			{
				IntVec3 coordinate = map.cellIndices.IndexToCell(i);
				array[i] = (moduleBase.GetValue(coordinate) < 0f);
			}
			MixedBiomeMapComponent mixedBiomeMapComponent = map.components.First((MapComponent c) => c is MixedBiomeMapComponent) as MixedBiomeMapComponent;
			mixedBiomeMapComponent.biomeGrid = array;
			mixedBiomeMapComponent.secondaryBiome = neighbourTile.Value.Tile.PrimaryBiome;
		}

		// Token: 0x060079F8 RID: 31224 RVA: 0x002B695C File Offset: 0x002B4B5C
		private float GetAngle(PlanetTile tile, BiomeDef otherBiome)
		{
			TileMutatorWorker_MixedBiome.tmpNeighbors.Clear();
			Find.World.grid.GetTileNeighbors(tile, TileMutatorWorker_MixedBiome.tmpNeighbors);
			float num = GenMath.MeanAngle((from t in TileMutatorWorker_MixedBiome.tmpNeighbors
			where t.Tile.PrimaryBiome == otherBiome
			select Find.WorldGrid.GetHeadingFromTo(t, tile)).ToList<float>());
			return (450f - num) % 360f;
		}

		// Token: 0x060079F9 RID: 31225 RVA: 0x002B69E0 File Offset: 0x002B4BE0
		private PlanetTile? GetNeighbourTile(PlanetTile tile, PlanetLayer layer)
		{
			BiomeDef primaryBiome = layer[tile].PrimaryBiome;
			TileMutatorWorker_MixedBiome.tmpNeighbors.Clear();
			Rand.PushState(tile.tileId);
			layer.GetTileNeighbors(tile, TileMutatorWorker_MixedBiome.tmpNeighbors);
			TileMutatorWorker_MixedBiome.tmpNeighbors.Shuffle<PlanetTile>();
			Rand.PopState();
			foreach (PlanetTile planetTile in TileMutatorWorker_MixedBiome.tmpNeighbors)
			{
				BiomeDef primaryBiome2 = layer[planetTile].PrimaryBiome;
				if (this.def.biomeWhitelist.Contains(primaryBiome2) && primaryBiome2 != primaryBiome)
				{
					return new PlanetTile?(planetTile);
				}
			}
			return null;
		}

		// Token: 0x060079FA RID: 31226 RVA: 0x002B6AA4 File Offset: 0x002B4CA4
		public BiomeDef SecondaryBiome(PlanetTile tile, PlanetLayer layer)
		{
			PlanetTile? neighbourTile = this.GetNeighbourTile(tile, layer);
			if (neighbourTile == null)
			{
				return null;
			}
			return layer[neighbourTile.Value].PrimaryBiome;
		}

		// Token: 0x04004CF1 RID: 19697
		private const float DisplaceMacroFrequency = 0.006f;

		// Token: 0x04004CF2 RID: 19698
		private const float DisplaceMacroStrength = 40f;

		// Token: 0x04004CF3 RID: 19699
		private const float DisplaceFrequency = 0.015f;

		// Token: 0x04004CF4 RID: 19700
		private const float DisplaceStrength = 20f;

		// Token: 0x04004CF5 RID: 19701
		private static readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();
	}
}
