using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001348 RID: 4936
	public class TileMutatorWorker_RiverDelta : TileMutatorWorker_River
	{
		// Token: 0x170014D4 RID: 5332
		// (get) Token: 0x06007A37 RID: 31287 RVA: 0x002B8938 File Offset: 0x002B6B38
		private static FloatRange SegmentLengthRange
		{
			get
			{
				return new FloatRange(30f, 70f);
			}
		}

		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x06007A38 RID: 31288 RVA: 0x002B8949 File Offset: 0x002B6B49
		private static FloatRange SplitAngleRange
		{
			get
			{
				return new FloatRange(15f, 30f);
			}
		}

		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x06007A39 RID: 31289 RVA: 0x002B895A File Offset: 0x002B6B5A
		private static FloatRange InitialLengthRange
		{
			get
			{
				return new FloatRange(0.2f, 0.5f);
			}
		}

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x06007A3A RID: 31290 RVA: 0x002B896B File Offset: 0x002B6B6B
		protected override float GetCurveFrequency
		{
			get
			{
				return 0.02f;
			}
		}

		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x06007A3B RID: 31291 RVA: 0x002B8972 File Offset: 0x002B6B72
		protected override float GetCurveAmplitude
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06007A3C RID: 31292 RVA: 0x002B8979 File Offset: 0x002B6B79
		protected override float GetWidthNoiseFactor(RiverNode riverNode)
		{
			return 1f;
		}

		// Token: 0x06007A3D RID: 31293 RVA: 0x002B8980 File Offset: 0x002B6B80
		public TileMutatorWorker_RiverDelta(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A3E RID: 31294 RVA: 0x002B8989 File Offset: 0x002B6B89
		public override string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x06007A3F RID: 31295 RVA: 0x002B8998 File Offset: 0x002B6B98
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
			SurfaceTile.RiverLink riverLink = (from rl in surfaceTile.Rivers
			where !rl.neighbor.Tile.WaterCovered
			select rl).MaxBy((SurfaceTile.RiverLink rl) => rl.river.widthOnMap);
			float? num = Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Ocean);
			float num2 = (((num != null) ? num : Find.World.CoastAngleAt(map.Tile, BiomeDefOf.Lake)).Value + 180f) % 360f;
			float angle = (450f - num2) % 360f;
			ValueTuple<Vector3, Vector3> mapEdgeNodes = base.GetMapEdgeNodes(map, angle);
			Vector3 item = mapEdgeNodes.Item1;
			Vector3 item2 = mapEdgeNodes.Item2;
			RiverNode riverNode;
			if (base.IsFlowingAToB(item, item2, angle))
			{
				riverNode = new RiverNode
				{
					start = item,
					end = item + (item2 - item) * TileMutatorWorker_RiverDelta.InitialLengthRange.RandomInRange,
					width = riverLink.river.widthOnMap
				};
			}
			else
			{
				riverNode = new RiverNode
				{
					start = item2,
					end = item2 + (item - item2) * TileMutatorWorker_RiverDelta.InitialLengthRange.RandomInRange,
					width = riverLink.river.widthOnMap
				};
			}
			map.waterInfo.riverGraph.Add(riverNode);
			this.SplitRiver(map, riverNode, num2);
			this.MergeNearbyNodes(map, riverNode);
		}

		// Token: 0x06007A40 RID: 31296 RVA: 0x002B8B48 File Offset: 0x002B6D48
		private void SplitRiver(Map map, RiverNode prevSegment, float baseAngle)
		{
			if (!prevSegment.end.InBounds(map) || map.terrainGrid.TerrainAt(prevSegment.end.ToIntVec3()).IsOcean)
			{
				return;
			}
			Vector3 normalized = (prevSegment.end - prevSegment.start).normalized;
			Vector3 a = new Vector3(-normalized.z, 0f, normalized.x);
			float num = 5f * (prevSegment.width / 10f);
			if (!Rand.Chance(0.9f))
			{
				float f = (baseAngle + TileMutatorWorker_RiverDelta.SplitAngleRange.RandomInRange - TileMutatorWorker_RiverDelta.SplitAngleRange.Average) * 0.017453292f;
				float randomInRange = TileMutatorWorker_RiverDelta.SegmentLengthRange.RandomInRange;
				float width = prevSegment.width;
				Vector3 start = prevSegment.end - normalized * width / 4f;
				Vector3 end = new Vector3(prevSegment.end.x + randomInRange * Mathf.Cos(f), 0f, prevSegment.end.z + randomInRange * Mathf.Sin(f));
				RiverNode riverNode = new RiverNode
				{
					start = start,
					end = end,
					width = width
				};
				map.waterInfo.riverGraph.Add(riverNode);
				prevSegment.childNodes.Add(riverNode);
				this.SplitRiver(map, riverNode, baseAngle);
				return;
			}
			float f2 = (baseAngle - TileMutatorWorker_RiverDelta.SplitAngleRange.RandomInRange - num) * 0.017453292f;
			float randomInRange2 = TileMutatorWorker_RiverDelta.SegmentLengthRange.RandomInRange;
			float num2 = Mathf.Max(prevSegment.width * Rand.Range(0.33f, 0.67f), 1.5f);
			Vector3 start2 = prevSegment.end - normalized * num2 / 4f - a * (prevSegment.width / 2f - num2 / 2f);
			Vector3 end2 = new Vector3(prevSegment.end.x + randomInRange2 * Mathf.Cos(f2), 0f, prevSegment.end.z + randomInRange2 * Mathf.Sin(f2));
			RiverNode riverNode2 = new RiverNode
			{
				start = start2,
				end = end2,
				width = num2 * 1.1f
			};
			map.waterInfo.riverGraph.Add(riverNode2);
			prevSegment.childNodes.Add(riverNode2);
			this.SplitRiver(map, riverNode2, baseAngle);
			f2 = (baseAngle + TileMutatorWorker_RiverDelta.SplitAngleRange.RandomInRange + num) * 0.017453292f;
			randomInRange2 = TileMutatorWorker_RiverDelta.SegmentLengthRange.RandomInRange;
			num2 = Mathf.Max(prevSegment.width - num2, 1.5f) + 1f;
			Vector3 start3 = prevSegment.end - normalized * num2 / 4f + a * (prevSegment.width / 2f - num2 / 2f);
			Vector3 end3 = new Vector3(prevSegment.end.x + randomInRange2 * Mathf.Cos(f2), 0f, prevSegment.end.z + randomInRange2 * Mathf.Sin(f2));
			RiverNode riverNode3 = new RiverNode
			{
				start = start3,
				end = end3,
				width = num2 * 1.1f
			};
			map.waterInfo.riverGraph.Add(riverNode3);
			prevSegment.childNodes.Add(riverNode3);
			this.SplitRiver(map, riverNode3, baseAngle);
		}

		// Token: 0x06007A41 RID: 31297 RVA: 0x002B8ED0 File Offset: 0x002B70D0
		private void MergeNearbyNodes(Map map, RiverNode root)
		{
			TileMutatorWorker_RiverDelta.<>c__DisplayClass27_0 CS$<>8__locals1;
			CS$<>8__locals1.markedForRemoval = new HashSet<RiverNode>();
			List<Vector3> list = new List<Vector3>();
			Queue<RiverNode> queue = new Queue<RiverNode>();
			queue.Enqueue(root);
			while (!queue.Empty<RiverNode>())
			{
				RiverNode riverNode = queue.Dequeue();
				bool flag = false;
				foreach (Vector3 vector in list)
				{
					if ((riverNode.end - vector).magnitude < 25f)
					{
						riverNode.end = vector;
						foreach (RiverNode node in riverNode.childNodes)
						{
							TileMutatorWorker_RiverDelta.<MergeNearbyNodes>g__RecursivelyMarkForRemoval|27_0(node, ref CS$<>8__locals1);
						}
						flag = true;
					}
				}
				if (!flag)
				{
					list.Add(riverNode.end);
					foreach (RiverNode item in riverNode.childNodes.InRandomOrder(null))
					{
						queue.Enqueue(item);
					}
				}
			}
			foreach (RiverNode item2 in CS$<>8__locals1.markedForRemoval)
			{
				map.waterInfo.riverGraph.Remove(item2);
			}
		}

		// Token: 0x06007A42 RID: 31298 RVA: 0x002B9068 File Offset: 0x002B7268
		[CompilerGenerated]
		internal static void <MergeNearbyNodes>g__RecursivelyMarkForRemoval|27_0(RiverNode node, ref TileMutatorWorker_RiverDelta.<>c__DisplayClass27_0 A_1)
		{
			A_1.markedForRemoval.Add(node);
			foreach (RiverNode node2 in node.childNodes)
			{
				TileMutatorWorker_RiverDelta.<MergeNearbyNodes>g__RecursivelyMarkForRemoval|27_0(node2, ref A_1);
			}
		}

		// Token: 0x04004D28 RID: 19752
		private const float MinWidth = 1.5f;

		// Token: 0x04004D29 RID: 19753
		private const float DeltaCurveFrequency = 0.02f;

		// Token: 0x04004D2A RID: 19754
		private const float DeltaCurveAmplitude = 10f;

		// Token: 0x04004D2B RID: 19755
		private const float DeltaMergeDist = 25f;

		// Token: 0x04004D2C RID: 19756
		private const float DeltaSplitChance = 0.9f;

		// Token: 0x04004D2D RID: 19757
		private const float MinSegmentLength = 30f;

		// Token: 0x04004D2E RID: 19758
		private const float MaxSegmentLength = 70f;

		// Token: 0x04004D2F RID: 19759
		private const float MinSplitAngle = 15f;

		// Token: 0x04004D30 RID: 19760
		private const float MaxSplitAngle = 30f;

		// Token: 0x04004D31 RID: 19761
		private const float MinInitialLength = 0.2f;

		// Token: 0x04004D32 RID: 19762
		private const float MaxInitialLength = 0.5f;

		// Token: 0x04004D33 RID: 19763
		private const float ExtraWidthAngle = 5f;
	}
}
