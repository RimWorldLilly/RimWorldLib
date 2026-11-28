using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001349 RID: 4937
	public class TileMutatorWorker_RiverIsland : TileMutatorWorker_River
	{
		// Token: 0x170014D9 RID: 5337
		// (get) Token: 0x06007A43 RID: 31299 RVA: 0x002B90C8 File Offset: 0x002B72C8
		protected override float GetCurveAmplitude
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x06007A44 RID: 31300 RVA: 0x002B90CF File Offset: 0x002B72CF
		public override string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x06007A45 RID: 31301 RVA: 0x002B90DC File Offset: 0x002B72DC
		public TileMutatorWorker_RiverIsland(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A46 RID: 31302 RVA: 0x002B90E8 File Offset: 0x002B72E8
		public override void Init(Map map)
		{
			base.Init(map);
			this.riverIslandAmplitude = TileMutatorWorker_RiverIsland.RiverIslandAmplitudeRange.RandomInRange;
			this.riverIslandCentrality = TileMutatorWorker_RiverIsland.RiverIslandCentralityRange.RandomInRange;
			this.widthFrac = TileMutatorWorker_RiverIsland.RiverWidthFracRange.RandomInRange;
		}

		// Token: 0x06007A47 RID: 31303 RVA: 0x002B9138 File Offset: 0x002B7338
		protected override void GenerateRiverGraph(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
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
			ValueTuple<Vector3, Vector3> mapEdgeNodes = base.GetMapEdgeNodes(map, headingFromTo);
			Vector3 item = mapEdgeNodes.Item1;
			Vector3 item2 = mapEdgeNodes.Item2;
			if (base.IsFlowingAToB(item, item2, headingFromTo))
			{
				this.nodeA = new RiverNode
				{
					start = item,
					end = item2,
					width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
				};
				this.nodeB = new RiverNode
				{
					start = item,
					end = item2,
					width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
				};
			}
			else
			{
				this.nodeA = new RiverNode
				{
					start = item2,
					end = item,
					width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
				};
				this.nodeB = new RiverNode
				{
					start = item2,
					end = item,
					width = source.First<SurfaceTile.RiverLink>().river.widthOnMap
				};
			}
			map.waterInfo.riverGraph.Add(this.nodeA);
			map.waterInfo.riverGraph.Add(this.nodeB);
		}

		// Token: 0x06007A48 RID: 31304 RVA: 0x002B92DC File Offset: 0x002B74DC
		protected override Vector2 GetDisplacedPoint(RiverNode riverNode, float t)
		{
			Vector3 vector = riverNode.end - riverNode.start;
			Vector3 a = riverNode.start + t * vector;
			float d = -4f * Mathf.Pow(t, 2f) + 4f * t;
			float num = t * Vector3.Distance(riverNode.start, riverNode.end);
			Vector3 normalized = new Vector3(-vector.z, 0f, vector.x).normalized;
			float num2 = Mathf.InverseLerp(this.riverIslandCentrality, 1f - this.riverIslandCentrality, t);
			float num3 = (float)this.riverBendNoise.GetValue((double)(num * this.GetWidthNoiseFactor(riverNode)), 0.0, (double)riverNode.seed);
			float num4 = -4f * Mathf.Pow(num2, 2f) + 4f * num2;
			if (riverNode == this.nodeA)
			{
				num3 += this.riverIslandAmplitude * num4;
			}
			else
			{
				num3 -= this.riverIslandAmplitude * num4;
			}
			Vector3 vector2 = a + num3 * this.GetCurveAmplitude * normalized * d;
			return new Vector2(vector2.x, vector2.z);
		}

		// Token: 0x06007A49 RID: 31305 RVA: 0x002B9414 File Offset: 0x002B7614
		protected override float GetRiverWidthAt(RiverNode riverNode, Vector2 cell)
		{
			float tvalue = base.GetTValue(riverNode, cell);
			float num = (riverNode == this.nodeA) ? this.widthFrac : (1f - this.widthFrac);
			float num2 = Mathf.InverseLerp(this.riverIslandCentrality, 1f - this.riverIslandCentrality, tvalue);
			float num3 = -4f * Mathf.Pow(num2, 2f) + 4f * num2;
			float num4 = 1f - num * num3;
			return Mathf.Max(riverNode.width / 2f * num4 * (1f + this.riverWidthNoise.GetValue(cell) * 0.15f), 1.5f);
		}

		// Token: 0x04004D34 RID: 19764
		private const float MinWidth = 1.5f;

		// Token: 0x04004D35 RID: 19765
		private const float RiverCurveAmplitude = 20f;

		// Token: 0x04004D36 RID: 19766
		private static readonly FloatRange RiverIslandAmplitudeRange = new FloatRange(1f, 2f);

		// Token: 0x04004D37 RID: 19767
		private static readonly FloatRange RiverIslandCentralityRange = new FloatRange(0f, 0.3f);

		// Token: 0x04004D38 RID: 19768
		private static readonly FloatRange RiverWidthFracRange = new FloatRange(0.2f, 0.8f);

		// Token: 0x04004D39 RID: 19769
		private RiverNode nodeA;

		// Token: 0x04004D3A RID: 19770
		private RiverNode nodeB;

		// Token: 0x04004D3B RID: 19771
		private float widthFrac;

		// Token: 0x04004D3C RID: 19772
		private float riverIslandAmplitude;

		// Token: 0x04004D3D RID: 19773
		private float riverIslandCentrality;
	}
}
