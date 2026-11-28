using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x02001346 RID: 4934
	public class TileMutatorWorker_Headwater : TileMutatorWorker_River
	{
		// Token: 0x06007A2B RID: 31275 RVA: 0x002B8158 File Offset: 0x002B6358
		public TileMutatorWorker_Headwater(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A2C RID: 31276 RVA: 0x002B8161 File Offset: 0x002B6361
		public override string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x06007A2D RID: 31277 RVA: 0x002B8170 File Offset: 0x002B6370
		public override void Init(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.Init(map);
			this.lakeNoise = new DistFromPoint((float)map.Size.x * 0.35f);
			this.lakeNoise = new ScaleBias(-1.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Scale((double)TileMutatorWorker_Lake.LakeSquashRange.RandomInRange, 1.0, 1.0, this.lakeNoise);
			this.lakeNoise = new Rotate(0.0, (double)Rand.Range(0f, 360f), 0.0, this.lakeNoise);
			this.lakeNoise = new Translate((double)(-(double)map.Center.x), 0.0, (double)(-(double)map.Center.z), this.lakeNoise);
			this.lakeNoise = MapNoiseUtility.AddDisplacementNoise(this.lakeNoise, 0.006f, 15f, 2, -1);
			this.lakeNoise = MapNoiseUtility.AddDisplacementNoise(this.lakeNoise, 0.015f, 15f, 4, -1);
		}

		// Token: 0x06007A2E RID: 31278 RVA: 0x002B829E File Offset: 0x002B649E
		protected override IntVec3 GetRiverCenter(Map map)
		{
			return map.Center;
		}

		// Token: 0x06007A2F RID: 31279 RVA: 0x002B82A8 File Offset: 0x002B64A8
		protected override void GenerateRiverGraph(Map map)
		{
			SurfaceTile surfaceTile;
			if (map.TileInfo.Isnt(out surfaceTile))
			{
				return;
			}
			SurfaceTile.RiverLink riverLink = surfaceTile.Rivers.First<SurfaceTile.RiverLink>();
			float headingFromTo = Find.WorldGrid.GetHeadingFromTo(surfaceTile.tile, riverLink.neighbor.Tile.tile);
			ValueTuple<Vector3, Vector3> mapEdgeNodes = base.GetMapEdgeNodes(map, headingFromTo);
			Vector3 item = mapEdgeNodes.Item1;
			Vector3 item2 = mapEdgeNodes.Item2;
			if (base.IsFlowingAToB(item, item2, headingFromTo))
			{
				RiverNode item3 = new RiverNode
				{
					start = this.riverCenter.ToVector3Shifted(),
					end = item2,
					width = riverLink.river.widthOnMap
				};
				map.waterInfo.riverGraph.Add(item3);
				return;
			}
			RiverNode item4 = new RiverNode
			{
				start = this.riverCenter.ToVector3Shifted(),
				end = item,
				width = riverLink.river.widthOnMap
			};
			map.waterInfo.riverGraph.Add(item4);
		}

		// Token: 0x06007A30 RID: 31280 RVA: 0x002B839C File Offset: 0x002B659C
		public override void GeneratePostElevationFertility(Map map)
		{
			base.GeneratePostElevationFertility(map);
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			foreach (IntVec3 intVec in map.AllCells)
			{
				if (this.GetValAt(intVec) > 0.45f)
				{
					elevation[intVec] = 0f;
				}
			}
		}

		// Token: 0x06007A31 RID: 31281 RVA: 0x002B840C File Offset: 0x002B660C
		public override void GeneratePostTerrain(Map map)
		{
			base.GeneratePostTerrain(map);
			foreach (IntVec3 cell in map.AllCells)
			{
				this.ProcessCell(cell, map);
			}
		}

		// Token: 0x06007A32 RID: 31282 RVA: 0x002B8464 File Offset: 0x002B6664
		private void ProcessCell(IntVec3 cell, Map map)
		{
			float valAt = this.GetValAt(cell);
			if (valAt > 0.75f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.DeepFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.5f)
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.ShallowFreshWaterTerrainAt(cell, map));
				return;
			}
			if (valAt > 0.45f && MapGenUtility.ShouldGenerateBeachSand(cell, map))
			{
				map.terrainGrid.SetTerrain(cell, MapGenUtility.LakeshoreTerrainAt(cell, map));
			}
		}

		// Token: 0x06007A33 RID: 31283 RVA: 0x002B84D5 File Offset: 0x002B66D5
		private float GetValAt(IntVec3 cell)
		{
			return (float)this.lakeNoise.GetValue((double)cell.x, 0.0, (double)cell.z);
		}

		// Token: 0x04004D25 RID: 19749
		private ModuleBase lakeNoise;

		// Token: 0x04004D26 RID: 19750
		private const float LakeRadius = 0.35f;

		// Token: 0x04004D27 RID: 19751
		private const float MacroNoiseStrength = 15f;
	}
}
