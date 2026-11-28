using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022DB RID: 8923
	public class WorldReachability
	{
		// Token: 0x0600DC19 RID: 56345 RVA: 0x0051FD8C File Offset: 0x0051DF8C
		public WorldReachability()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer layer = planetLayer;
				this.OnPlanetLayerAdded(layer);
			}
			this.nextFieldID = 1;
			this.InvalidateAllFields();
			Find.WorldGrid.OnPlanetLayerAdded += this.OnPlanetLayerAdded;
			Find.WorldGrid.OnPlanetLayerRemoved += this.OnPlanetLayerRemoved;
		}

		// Token: 0x0600DC1A RID: 56346 RVA: 0x0051FE38 File Offset: 0x0051E038
		private void OnPlanetLayerAdded(PlanetLayer layer)
		{
			this.layerFields[layer] = new int[layer.TilesCount];
		}

		// Token: 0x0600DC1B RID: 56347 RVA: 0x0051FE51 File Offset: 0x0051E051
		private void OnPlanetLayerRemoved(PlanetLayer layer)
		{
			if (this.layerFields.ContainsKey(layer))
			{
				this.layerFields.Remove(layer);
			}
		}

		// Token: 0x0600DC1C RID: 56348 RVA: 0x0051FE6E File Offset: 0x0051E06E
		public void ClearCache()
		{
			this.InvalidateAllFields();
		}

		// Token: 0x0600DC1D RID: 56349 RVA: 0x0051FE76 File Offset: 0x0051E076
		public bool CanReach(Caravan c, PlanetTile tile)
		{
			return this.CanReach(c.Tile, tile);
		}

		// Token: 0x0600DC1E RID: 56350 RVA: 0x0051FE88 File Offset: 0x0051E088
		public bool CanReach(PlanetTile startTile, PlanetTile destTile)
		{
			int[] array = this.layerFields[startTile.Layer];
			if (startTile.Layer != destTile.Layer)
			{
				return false;
			}
			if (!startTile.Valid || startTile.tileId >= array.Length || !destTile.Valid || destTile.tileId >= array.Length)
			{
				return false;
			}
			if (array[startTile.tileId] == this.impassableFieldID || array[destTile.tileId] == this.impassableFieldID)
			{
				return false;
			}
			if (this.IsValidField(array[startTile.tileId]) || this.IsValidField(array[destTile.tileId]))
			{
				return array[startTile.tileId] == array[destTile.tileId];
			}
			this.FloodFillAt(startTile);
			return array[startTile.tileId] != this.impassableFieldID && array[startTile.tileId] == array[destTile.tileId];
		}

		// Token: 0x0600DC1F RID: 56351 RVA: 0x0051FF64 File Offset: 0x0051E164
		public int GetLocalFieldId(PlanetTile tile)
		{
			return this.layerFields[tile.Layer][tile.tileId] - this.minValidFieldID;
		}

		// Token: 0x0600DC20 RID: 56352 RVA: 0x0051FF88 File Offset: 0x0051E188
		private void InvalidateAllFields()
		{
			if (this.nextFieldID == 2147483646)
			{
				this.nextFieldID = 1;
			}
			this.minValidFieldID = this.nextFieldID;
			this.impassableFieldID = this.nextFieldID;
			this.nextFieldID++;
			foreach (KeyValuePair<PlanetLayer, int[]> keyValuePair in this.layerFields)
			{
				PlanetLayer planetLayer;
				int[] array;
				keyValuePair.Deconstruct(out planetLayer, out array);
				PlanetLayer layer = planetLayer;
				int[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i] < this.minValidFieldID)
					{
						this.FloodFillAt(new PlanetTile(i, layer));
					}
				}
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair2 in Find.WorldGrid.PlanetLayers)
			{
				PlanetLayer planetLayer;
				int num;
				keyValuePair2.Deconstruct(out num, out planetLayer);
				planetLayer.FastTileFinder.DirtyCache();
			}
		}

		// Token: 0x0600DC21 RID: 56353 RVA: 0x005200A4 File Offset: 0x0051E2A4
		private bool IsValidField(int fieldID)
		{
			return fieldID >= this.minValidFieldID;
		}

		// Token: 0x0600DC22 RID: 56354 RVA: 0x005200B4 File Offset: 0x0051E2B4
		private void FloodFillAt(PlanetTile tile)
		{
			World world = Find.World;
			int[] fields = this.layerFields[tile.Layer];
			if (world.Impassable(tile))
			{
				fields[tile.tileId] = this.impassableFieldID;
				return;
			}
			tile.Layer.Filler.FloodFill(tile, (PlanetTile x) => !world.Impassable(x), delegate(PlanetTile x)
			{
				fields[x.tileId] = this.nextFieldID;
			}, int.MaxValue, null);
			this.nextFieldID++;
		}

		// Token: 0x04008D31 RID: 36145
		private readonly Dictionary<PlanetLayer, int[]> layerFields = new Dictionary<PlanetLayer, int[]>();

		// Token: 0x04008D32 RID: 36146
		private int nextFieldID;

		// Token: 0x04008D33 RID: 36147
		private int impassableFieldID;

		// Token: 0x04008D34 RID: 36148
		private int minValidFieldID;
	}
}
