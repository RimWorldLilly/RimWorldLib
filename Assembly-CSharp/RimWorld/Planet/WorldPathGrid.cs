using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022D8 RID: 8920
	public class WorldPathGrid
	{
		// Token: 0x17002647 RID: 9799
		// (get) Token: 0x0600DBFB RID: 56315 RVA: 0x0051EC14 File Offset: 0x0051CE14
		private static int DayOfYearAt0Long
		{
			get
			{
				return GenDate.DayOfYear((long)GenTicks.TicksAbs, 0f);
			}
		}

		// Token: 0x0600DBFC RID: 56316 RVA: 0x0051EC28 File Offset: 0x0051CE28
		public WorldPathGrid()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer layer = planetLayer;
				this.OnPlanetLayerAdded(layer);
			}
			Find.WorldGrid.OnPlanetLayerAdded += this.OnPlanetLayerAdded;
			Find.WorldGrid.OnPlanetLayerRemoved += this.OnPlanetLayerRemoved;
		}

		// Token: 0x0600DBFD RID: 56317 RVA: 0x0051ECD0 File Offset: 0x0051CED0
		public void WorldPathGridTick()
		{
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				if (this.layerAllPathCostsRecalculatedDayOfYear[planetLayer2] != WorldPathGrid.DayOfYearAt0Long)
				{
					this.RecalculateLayerPerceivedPathCosts(planetLayer2);
				}
			}
		}

		// Token: 0x0600DBFE RID: 56318 RVA: 0x0051ED44 File Offset: 0x0051CF44
		private void OnPlanetLayerAdded(PlanetLayer layer)
		{
			this.layerMovementDifficulty[layer] = new float[layer.TilesCount];
			this.layerAllPathCostsRecalculatedDayOfYear[layer] = -1;
		}

		// Token: 0x0600DBFF RID: 56319 RVA: 0x0051ED6A File Offset: 0x0051CF6A
		private void OnPlanetLayerRemoved(PlanetLayer layer)
		{
			this.layerMovementDifficulty.Remove(layer);
			this.layerAllPathCostsRecalculatedDayOfYear.Remove(layer);
		}

		// Token: 0x0600DC00 RID: 56320 RVA: 0x0051ED86 File Offset: 0x0051CF86
		public bool Passable(PlanetTile tile)
		{
			return Find.WorldGrid.InBounds(tile) && this.layerMovementDifficulty[tile.Layer][tile.tileId] < 1000f;
		}

		// Token: 0x0600DC01 RID: 56321 RVA: 0x0051EDB7 File Offset: 0x0051CFB7
		public bool PassableFast(PlanetTile tile)
		{
			return this.layerMovementDifficulty[tile.Layer][tile.tileId] < 1000f;
		}

		// Token: 0x0600DC02 RID: 56322 RVA: 0x0051EDD9 File Offset: 0x0051CFD9
		public float PerceivedMovementDifficultyAt(PlanetTile tile)
		{
			return this.layerMovementDifficulty[tile.Layer][tile.tileId];
		}

		// Token: 0x0600DC03 RID: 56323 RVA: 0x0051EDF4 File Offset: 0x0051CFF4
		public void RecalculatePerceivedMovementDifficultyAt(PlanetTile tile, out bool needsRecache, int? ticksAbs = null)
		{
			needsRecache = false;
			if (!Find.WorldGrid.InBounds(tile))
			{
				return;
			}
			bool flag = this.PassableFast(tile);
			this.layerMovementDifficulty[tile.Layer][tile.tileId] = WorldPathGrid.CalculatedMovementDifficultyAt(tile, true, ticksAbs, null);
			if (flag != this.PassableFast(tile))
			{
				needsRecache = true;
			}
		}

		// Token: 0x0600DC04 RID: 56324 RVA: 0x0051EE48 File Offset: 0x0051D048
		public void RecalculateAllLayersPathCosts()
		{
			foreach (KeyValuePair<PlanetLayer, float[]> keyValuePair in this.layerMovementDifficulty)
			{
				PlanetLayer planetLayer;
				float[] array;
				keyValuePair.Deconstruct(out planetLayer, out array);
				PlanetLayer layer = planetLayer;
				this.RecalculateLayerPerceivedPathCosts(layer);
			}
		}

		// Token: 0x0600DC05 RID: 56325 RVA: 0x0051EEA8 File Offset: 0x0051D0A8
		public void RecalculateLayerPerceivedPathCosts(PlanetLayer layer)
		{
			this.RecalculateLayerPerceivedPathCosts(layer, null);
			this.layerAllPathCostsRecalculatedDayOfYear[layer] = WorldPathGrid.DayOfYearAt0Long;
		}

		// Token: 0x0600DC06 RID: 56326 RVA: 0x0051EED8 File Offset: 0x0051D0D8
		public void RecalculateLayerPerceivedPathCosts(PlanetLayer layer, int? ticksAbs)
		{
			bool flag = false;
			this.layerAllPathCostsRecalculatedDayOfYear[layer] = -1;
			float[] array = this.layerMovementDifficulty[layer];
			for (int i = 0; i < array.Length; i++)
			{
				bool flag2;
				this.RecalculatePerceivedMovementDifficultyAt(new PlanetTile(i, layer), out flag2, ticksAbs);
				flag = (flag || flag2);
			}
			if (flag)
			{
				Find.WorldReachability.ClearCache();
			}
		}

		// Token: 0x0600DC07 RID: 56327 RVA: 0x0051EF30 File Offset: 0x0051D130
		public static float CalculatedMovementDifficultyAt(PlanetTile tile, bool perceivedStatic, int? ticksAbs = null, StringBuilder explanation = null)
		{
			Tile tile2 = Find.WorldGrid[tile];
			if (explanation != null && explanation.Length > 0)
			{
				explanation.AppendLine();
			}
			if (tile2.PrimaryBiome.impassable || tile2.hilliness == Hilliness.Impassable)
			{
				if (explanation != null)
				{
					explanation.Append("Impassable".Translate());
				}
				return 1000f;
			}
			float num = 0f + tile2.PrimaryBiome.movementDifficulty;
			if (explanation != null)
			{
				explanation.Append(tile2.PrimaryBiome.LabelCap + ": " + tile2.PrimaryBiome.movementDifficulty.ToStringWithSign("0.#"));
			}
			float num2 = WorldPathGrid.HillinessMovementDifficultyOffset(tile2.hilliness);
			float num3 = num + num2;
			if (explanation != null && num2 != 0f)
			{
				explanation.AppendLine();
				explanation.Append(tile2.hilliness.GetLabelCap() + ": " + num2.ToStringWithSign("0.#"));
			}
			return num3 + WorldPathGrid.GetCurrentWinterMovementDifficultyOffset(tile, new int?(ticksAbs ?? GenTicks.TicksAbs), explanation);
		}

		// Token: 0x0600DC08 RID: 56328 RVA: 0x0051F050 File Offset: 0x0051D250
		public static float GetCurrentWinterMovementDifficultyOffset(PlanetTile tile, int? ticksAbs = null, StringBuilder explanation = null)
		{
			if (ticksAbs == null)
			{
				ticksAbs = new int?(GenTicks.TicksAbs);
			}
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			SeasonUtility.GetSeason(GenDate.YearPercent((long)ticksAbs.Value, vector.x), vector.y, out num, out num2, out num3, out num4, out num5, out num6);
			float num7 = num4 + num6;
			num7 *= Mathf.InverseLerp(5f, 0f, GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs.Value, tile));
			if (num7 > 0.01f)
			{
				float num8 = 2f * num7;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("Winter".Translate());
					if (num7 < 0.999f)
					{
						explanation.Append(" (" + num7.ToStringPercent("F0") + ")");
					}
					explanation.Append(": ");
					explanation.Append(num8.ToStringWithSign("0.#"));
				}
				return num8;
			}
			return 0f;
		}

		// Token: 0x0600DC09 RID: 56329 RVA: 0x0051F158 File Offset: 0x0051D358
		public static bool WillWinterEverAffectMovementDifficulty(PlanetTile tile)
		{
			int ticksAbs = GenTicks.TicksAbs;
			for (int i = 0; i < 3600000; i += 60000)
			{
				if (GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs + i, tile) < 5f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600DC0A RID: 56330 RVA: 0x0051F194 File Offset: 0x0051D394
		private static float HillinessMovementDifficultyOffset(Hilliness hilliness)
		{
			switch (hilliness)
			{
			case Hilliness.Flat:
				return 0f;
			case Hilliness.SmallHills:
				return 0.5f;
			case Hilliness.LargeHills:
				return 1.5f;
			case Hilliness.Mountainous:
				return 3f;
			case Hilliness.Impassable:
				return 1000f;
			default:
				return 0f;
			}
		}

		// Token: 0x04008D24 RID: 36132
		public readonly Dictionary<PlanetLayer, float[]> layerMovementDifficulty = new Dictionary<PlanetLayer, float[]>();

		// Token: 0x04008D25 RID: 36133
		private readonly Dictionary<PlanetLayer, int> layerAllPathCostsRecalculatedDayOfYear = new Dictionary<PlanetLayer, int>();

		// Token: 0x04008D26 RID: 36134
		private const float ImpassableMovementDifficulty = 1000f;

		// Token: 0x04008D27 RID: 36135
		public const float WinterMovementDifficultyOffset = 2f;

		// Token: 0x04008D28 RID: 36136
		public const float MaxTempForWinterOffset = 5f;
	}
}
