using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001345 RID: 4933
	public class TileMutatorWorker_River : TileMutatorWorker
	{
		// Token: 0x170014D2 RID: 5330
		// (get) Token: 0x06007A16 RID: 31254 RVA: 0x002B74D8 File Offset: 0x002B56D8
		protected virtual float GetCurveFrequency
		{
			get
			{
				return 0.007f;
			}
		}

		// Token: 0x170014D3 RID: 5331
		// (get) Token: 0x06007A17 RID: 31255 RVA: 0x002B74DF File Offset: 0x002B56DF
		protected virtual float GetCurveAmplitude
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x06007A18 RID: 31256 RVA: 0x002B74E6 File Offset: 0x002B56E6
		public TileMutatorWorker_River(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A19 RID: 31257 RVA: 0x002B74FC File Offset: 0x002B56FC
		public override string GetLabel(PlanetTile tile)
		{
			SurfaceTile surfaceTile = tile.Tile as SurfaceTile;
			if (surfaceTile != null)
			{
				return surfaceTile.Rivers[0].river.label;
			}
			throw new Exception("Attempted to get river label on a tile which is not a SurfaceTile");
		}

		// Token: 0x06007A1A RID: 31258 RVA: 0x002B753C File Offset: 0x002B573C
		public override void Init(Map map)
		{
			if (map.waterInfo.lakeCenter.IsValid)
			{
				this.riverCenter = map.waterInfo.lakeCenter;
			}
			else
			{
				this.riverCenter = this.GetRiverCenter(map);
			}
			this.riverBendNoise = new Perlin((double)this.GetCurveFrequency, 2.0, 1.0, 2, Rand.Int, QualityMode.Medium);
			this.shallowizer = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Int, QualityMode.Medium);
			this.shallowizer = new Abs(this.shallowizer);
			this.riverbankNoise = new Perlin(0.029999999329447746, 2.0, 2.0, 2, Rand.Int, QualityMode.Medium);
			this.riverWidthNoise = new Perlin(0.019999999552965164, 2.0, 1.0, 3, Rand.Int, QualityMode.Medium);
		}

		// Token: 0x06007A1B RID: 31259 RVA: 0x002B7640 File Offset: 0x002B5840
		public override void GeneratePostTerrain(Map map)
		{
			this.GenerateRiverGraph(map);
			if (map.waterInfo.riverGraph.NullOrEmpty<RiverNode>())
			{
				return;
			}
			this.GenerateDepthMaps(map);
			List<IntVec3> list = new List<IntVec3>();
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				Building edifice = intVec.GetEdifice(map);
				TerrainDef terrainDef = this.RiverTerrainAt(intVec, map);
				TerrainDef terrainDef2 = this.RiverBankTerrainAt(intVec, map);
				float num;
				float depth = this.GetDepth(intVec, map, out num);
				float value = this.riverbankNoise.GetValue(intVec);
				float num2 = (float)map.Biome.riverbankSizeRange.Lerped(value) * 0.3f;
				bool flag = num > 20f;
				if (depth > -num2)
				{
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
					if (flag)
					{
						map.roofGrid.SetRoof(intVec, null);
					}
					else if (edifice != null)
					{
						list.Add(edifice.Position);
					}
				}
				if (terrainDef != null)
				{
					map.terrainGrid.SetTerrain(intVec, terrainDef);
					elevation[intVec] = -1f;
				}
				else if (terrainDef2 != null && edifice == null)
				{
					map.terrainGrid.SetTerrain(intVec, terrainDef2);
				}
			}
			RoofCollapseCellsFinder.RemoveBulkCollapsingRoofs(list, map);
		}

		// Token: 0x06007A1C RID: 31260 RVA: 0x002B7788 File Offset: 0x002B5988
		public override void GeneratePostFog(Map map)
		{
			if (map.waterInfo.riverGraph.NullOrEmpty<RiverNode>())
			{
				return;
			}
			this.GenerateRiverLookupTexture(map);
		}

		// Token: 0x06007A1D RID: 31261 RVA: 0x002B77A4 File Offset: 0x002B59A4
		protected virtual void GenerateRiverGraph(Map map)
		{
			SurfaceTile surfaceTile;
			if (map.TileInfo.Isnt(out surfaceTile))
			{
				return;
			}
			List<SurfaceTile.RiverLink> source = (from rl in surfaceTile.Rivers
			orderby -((SurfaceTile)rl.neighbor.Tile).riverDist
			select rl).ToList<SurfaceTile.RiverLink>();
			WorldGrid worldGrid = Find.WorldGrid;
			SurfaceTile.RiverLink riverLink = source.First<SurfaceTile.RiverLink>();
			PlanetTile tile = riverLink.neighbor.Tile.tile;
			riverLink = source.Last<SurfaceTile.RiverLink>();
			float headingFromTo = worldGrid.GetHeadingFromTo(tile, riverLink.neighbor.Tile.tile);
			ValueTuple<Vector3, Vector3> mapEdgeNodes = this.GetMapEdgeNodes(map, headingFromTo);
			Vector3 item = mapEdgeNodes.Item1;
			Vector3 item2 = mapEdgeNodes.Item2;
			if (this.IsFlowingAToB(item, item2, headingFromTo))
			{
				RiverNode item3 = new RiverNode
				{
					start = item,
					end = item2,
					width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
				};
				map.waterInfo.riverGraph.Add(item3);
				return;
			}
			RiverNode item4 = new RiverNode
			{
				start = item2,
				end = item,
				width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
			};
			map.waterInfo.riverGraph.Add(item4);
		}

		// Token: 0x06007A1E RID: 31262 RVA: 0x002B78D0 File Offset: 0x002B5AD0
		protected virtual IntVec3 GetRiverCenter(Map map)
		{
			return new IntVec3((int)(Rand.Range(0.3f, 0.7f) * (float)map.Size.x), 0, (int)(Rand.Range(0.3f, 0.7f) * (float)map.Size.z));
		}

		// Token: 0x06007A1F RID: 31263 RVA: 0x002B7920 File Offset: 0x002B5B20
		protected ValueTuple<Vector3, Vector3> GetMapEdgeNodes(Map map, float angle)
		{
			float slope = Mathf.Tan((450f - angle) % 360f * 0.017453292f);
			List<Vector2> list = new List<Vector2>();
			GenGeo.LineRectIntersection(new Vector2((float)this.riverCenter.x, (float)this.riverCenter.z), slope, new Vector2(-25f, -25f), new Vector2((float)(map.Size.x + 25), (float)(map.Size.z + 25)), ref list);
			return new ValueTuple<Vector3, Vector3>(new Vector3(list[0].x, 0f, list[0].y), new Vector3(list[1].x, 0f, list[1].y));
		}

		// Token: 0x06007A20 RID: 31264 RVA: 0x002B79ED File Offset: 0x002B5BED
		protected bool IsFlowingAToB(Vector3 a, Vector3 b, float angle)
		{
			return Mathf.RoundToInt((b - a).AngleFlat() - angle) % 360 == 0;
		}

		// Token: 0x06007A21 RID: 31265 RVA: 0x002B7A0C File Offset: 0x002B5C0C
		private void GenerateDepthMaps(Map map)
		{
			foreach (RiverNode riverNode in map.waterInfo.riverGraph)
			{
				int num = map.Size.x + 50;
				int num2 = map.Size.z + 50;
				float[] array = new float[num * num2];
				this.nodeDepthMaps.Add(riverNode, array);
				for (int i = 0; i < num; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						float num3 = float.MinValue;
						Vector2 vector = new Vector2((float)(i - 25), (float)(j - 25));
						float tvalue = this.GetTValue(riverNode, vector);
						if (tvalue >= 0f && tvalue <= 1f)
						{
							Vector2 displacedPoint = this.GetDisplacedPoint(riverNode, tvalue);
							float riverWidthAt = this.GetRiverWidthAt(riverNode, vector);
							Vector2 normalized = (this.GetDisplacedPoint(riverNode, tvalue - 0.001f) - displacedPoint).normalized;
							Vector2 rhs = new Vector2(-normalized.y, normalized.x);
							float num4 = Mathf.Abs(Vector2.Dot(vector - displacedPoint, rhs));
							num3 = riverWidthAt - num4;
						}
						array[i + j * num] = num3;
					}
				}
			}
		}

		// Token: 0x06007A22 RID: 31266 RVA: 0x002B7B7C File Offset: 0x002B5D7C
		protected virtual float GetRiverWidthAt(RiverNode riverNode, Vector2 cell)
		{
			return riverNode.width / 2f * (1f + this.riverWidthNoise.GetValue(cell) * 0.15f * this.GetWidthNoiseFactor(riverNode));
		}

		// Token: 0x06007A23 RID: 31267 RVA: 0x002B7BB0 File Offset: 0x002B5DB0
		private float GetDepth(IntVec3 cell, Map map, out float riverWidth)
		{
			float num = float.MinValue;
			riverWidth = 0f;
			foreach (RiverNode riverNode in map.waterInfo.riverGraph)
			{
				if (this.GetSegmentDepth(cell, map, riverNode) > num)
				{
					num = Mathf.Max(num, this.GetSegmentDepth(cell, map, riverNode));
					riverWidth = riverNode.width;
				}
			}
			return num;
		}

		// Token: 0x06007A24 RID: 31268 RVA: 0x002B7C34 File Offset: 0x002B5E34
		private float GetSegmentDepth(IntVec3 cell, Map map, RiverNode riverNode)
		{
			int num = cell.x + 25 + (cell.z + 25) * (map.Size.x + 50);
			return this.nodeDepthMaps[riverNode][num];
		}

		// Token: 0x06007A25 RID: 31269 RVA: 0x002B7C74 File Offset: 0x002B5E74
		protected float GetTValue(RiverNode riverNode, Vector2 point)
		{
			Vector2 b = new Vector2(riverNode.start.x, riverNode.start.z);
			Vector2 vector = new Vector2(riverNode.end.x, riverNode.end.z) - b;
			Vector2 lhs = point - b;
			float num = Vector2.Dot(vector, vector);
			return Vector2.Dot(lhs, vector) / num;
		}

		// Token: 0x06007A26 RID: 31270 RVA: 0x002B7CD8 File Offset: 0x002B5ED8
		protected virtual Vector2 GetDisplacedPoint(RiverNode riverNode, float t)
		{
			Vector3 vector = riverNode.end - riverNode.start;
			Vector3 a = riverNode.start + t * vector;
			float d = -4f * Mathf.Pow(t, 2f) + 4f * t;
			float num = t * Vector3.Distance(riverNode.start, riverNode.end);
			Vector3 normalized = new Vector3(-vector.z, 0f, vector.x).normalized;
			float num2 = (float)this.riverBendNoise.GetValue((double)(num * this.GetWidthNoiseFactor(riverNode)), 0.0, (double)riverNode.seed);
			Vector3 vector2 = a + num2 * this.GetCurveAmplitude * normalized * d;
			return new Vector2(vector2.x, vector2.z);
		}

		// Token: 0x06007A27 RID: 31271 RVA: 0x002B7DAF File Offset: 0x002B5FAF
		protected virtual float GetWidthNoiseFactor(RiverNode riverNode)
		{
			return 6f / riverNode.width;
		}

		// Token: 0x06007A28 RID: 31272 RVA: 0x002B7DC0 File Offset: 0x002B5FC0
		private TerrainDef RiverTerrainAt(IntVec3 cell, Map map)
		{
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(cell);
			if (terrainDef.IsWater && !terrainDef.IsRiver && terrainDef != TerrainDefOf.Marsh)
			{
				return null;
			}
			float num;
			float depth = this.GetDepth(cell, map, out num);
			if (depth > 4f && this.shallowizer.GetValue(cell) > 0.2f)
			{
				return MapGenUtility.DeepMovingWaterTerrainAt(cell, map);
			}
			if (depth > 0f)
			{
				return MapGenUtility.ShallowMovingWaterTerrainAt(cell, map);
			}
			return null;
		}

		// Token: 0x06007A29 RID: 31273 RVA: 0x002B7E34 File Offset: 0x002B6034
		private TerrainDef RiverBankTerrainAt(IntVec3 cell, Map map)
		{
			TerrainDef existing = map.terrainGrid.TerrainAt(cell);
			if (existing.IsWater && existing != TerrainDefOf.Marsh)
			{
				return null;
			}
			if (!map.Biome.terrainsByFertility.Any((TerrainThreshold tt) => tt.terrain == existing))
			{
				return null;
			}
			float num;
			float depth = this.GetDepth(cell, map, out num);
			float value = this.riverbankNoise.GetValue(cell);
			int num2 = map.Biome.riverbankSizeRange.Lerped(value);
			if (depth > (float)(-(float)num2))
			{
				return MapGenUtility.RiverbankTerrainAt(cell, map);
			}
			return null;
		}

		// Token: 0x06007A2A RID: 31274 RVA: 0x002B7ECC File Offset: 0x002B60CC
		private void GenerateRiverLookupTexture(Map map)
		{
			List<IntVec3> list = new List<IntVec3>();
			List<float> list2 = new List<float>();
			List<Vector2> list3 = new List<Vector2>();
			int num = 2;
			foreach (RiverNode riverNode in map.waterInfo.riverGraph)
			{
				float magnitude = (riverNode.end - riverNode.start).magnitude;
				Vector3 normalized = (riverNode.end - riverNode.start).normalized;
				Vector2 b = (riverNode.start - normalized).ToVector2();
				int num2 = 0;
				while ((float)num2 < magnitude)
				{
					Vector2 displacedPoint = this.GetDisplacedPoint(riverNode, (float)num2 / magnitude);
					IntVec3 item = displacedPoint.ToVector3().ToIntVec3();
					list.Add(item);
					list2.Add(riverNode.width);
					list3.Add(displacedPoint - b);
					b = displacedPoint;
					num2 += num;
				}
			}
			map.waterInfo.riverFlowMap = new List<float>();
			for (int i = 0; i < map.Size.x * map.Size.z * 2; i++)
			{
				map.waterInfo.riverFlowMap.Add(0f);
			}
			for (int j = 0; j < map.Size.x; j++)
			{
				for (int k = 0; k < map.Size.z; k++)
				{
					IntVec3 intVec = new IntVec3(j, 0, k);
					if (intVec.GetTerrain(map).IsRiver)
					{
						int num3 = intVec.x * map.Size.z + intVec.z;
						int num4 = 0;
						Vector2 vector = Vector2.zero;
						for (int l = 0; l < list.Count; l++)
						{
							if ((list[l] - intVec).Magnitude <= list2[l] * 1.5f / 2f + 1f)
							{
								num4++;
								vector += list3[l];
							}
						}
						vector /= (float)num4;
						map.waterInfo.riverFlowMap[num3 * 2] = vector.x;
						map.waterInfo.riverFlowMap[num3 * 2 + 1] = vector.y;
					}
				}
			}
		}

		// Token: 0x04004D11 RID: 19729
		private const float RiverCurveFrequency = 0.007f;

		// Token: 0x04004D12 RID: 19730
		private const float RiverCurveLacunarity = 2f;

		// Token: 0x04004D13 RID: 19731
		private const float RiverCurvePersistence = 1f;

		// Token: 0x04004D14 RID: 19732
		private const int RiverCurveOctaves = 2;

		// Token: 0x04004D15 RID: 19733
		private const float RiverCurveAmplitude = 40f;

		// Token: 0x04004D16 RID: 19734
		private const float RiverWidthFrequency = 0.02f;

		// Token: 0x04004D17 RID: 19735
		private const float RiverWidthLacunarity = 2f;

		// Token: 0x04004D18 RID: 19736
		private const float RiverWidthPersistence = 1f;

		// Token: 0x04004D19 RID: 19737
		private const int RiverWidthOctaves = 3;

		// Token: 0x04004D1A RID: 19738
		protected const float RiverWidthNoiseAmplitude = 0.15f;

		// Token: 0x04004D1B RID: 19739
		private const float RemoveRoofWidthThreshold = 20f;

		// Token: 0x04004D1C RID: 19740
		private const float RiverCaveBankFactor = 0.3f;

		// Token: 0x04004D1D RID: 19741
		private const float ShallowFactor = 0.2f;

		// Token: 0x04004D1E RID: 19742
		private const int Oversample = 25;

		// Token: 0x04004D1F RID: 19743
		protected ModuleBase riverBendNoise;

		// Token: 0x04004D20 RID: 19744
		protected ModuleBase riverWidthNoise;

		// Token: 0x04004D21 RID: 19745
		protected ModuleBase shallowizer;

		// Token: 0x04004D22 RID: 19746
		private ModuleBase riverbankNoise;

		// Token: 0x04004D23 RID: 19747
		protected IntVec3 riverCenter;

		// Token: 0x04004D24 RID: 19748
		private Dictionary<RiverNode, float[]> nodeDepthMaps = new Dictionary<RiverNode, float[]>();
	}
}
