using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022D7 RID: 8919
	public class WorldPath : IDisposable
	{
		// Token: 0x1700263E RID: 9790
		// (get) Token: 0x0600DBE8 RID: 56296 RVA: 0x0051E8DA File Offset: 0x0051CADA
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x1700263F RID: 9791
		// (get) Token: 0x0600DBE9 RID: 56297 RVA: 0x0051E8EC File Offset: 0x0051CAEC
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x17002640 RID: 9792
		// (get) Token: 0x0600DBEA RID: 56298 RVA: 0x0051E8F4 File Offset: 0x0051CAF4
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x17002641 RID: 9793
		// (get) Token: 0x0600DBEB RID: 56299 RVA: 0x0051E8FE File Offset: 0x0051CAFE
		public List<PlanetTile> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x17002642 RID: 9794
		// (get) Token: 0x0600DBEC RID: 56300 RVA: 0x0051E906 File Offset: 0x0051CB06
		public PlanetTile FirstNode
		{
			get
			{
				List<PlanetTile> list = this.nodes;
				return list[list.Count - 1];
			}
		}

		// Token: 0x17002643 RID: 9795
		// (get) Token: 0x0600DBED RID: 56301 RVA: 0x0051E91B File Offset: 0x0051CB1B
		public PlanetTile LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x17002644 RID: 9796
		// (get) Token: 0x0600DBEE RID: 56302 RVA: 0x0051E929 File Offset: 0x0051CB29
		public PlanetLayer Layer
		{
			get
			{
				return this.layer;
			}
		}

		// Token: 0x17002645 RID: 9797
		// (get) Token: 0x0600DBEF RID: 56303 RVA: 0x0051E931 File Offset: 0x0051CB31
		public int NodeCount
		{
			get
			{
				return this.nodes.Count;
			}
		}

		// Token: 0x17002646 RID: 9798
		// (get) Token: 0x0600DBF0 RID: 56304 RVA: 0x0051E93E File Offset: 0x0051CB3E
		public static WorldPath NotFound
		{
			get
			{
				return WorldPathPool.NotFoundPath;
			}
		}

		// Token: 0x0600DBF1 RID: 56305 RVA: 0x0051E945 File Offset: 0x0051CB45
		public void AddNodeAtStart(PlanetTile tile)
		{
			this.nodes.Add(tile);
		}

		// Token: 0x0600DBF2 RID: 56306 RVA: 0x0051E954 File Offset: 0x0051CB54
		public void SetupFound(float totalCost, PlanetLayer planetLayer)
		{
			if (this == WorldPath.NotFound)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Calling SetupFound with totalCost=");
				defaultInterpolatedStringHandler.AppendFormatted<float>(totalCost);
				defaultInterpolatedStringHandler.AppendLiteral(" on WorldPath.NotFound");
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.layer = planetLayer;
			this.totalCostInt = totalCost;
			this.curNodeIndex = this.nodes.Count - 1;
		}

		// Token: 0x0600DBF3 RID: 56307 RVA: 0x0051E9C1 File Offset: 0x0051CBC1
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x0600DBF4 RID: 56308 RVA: 0x0051E9C9 File Offset: 0x0051CBC9
		public void ReleaseToPool()
		{
			if (this != WorldPath.NotFound)
			{
				this.totalCostInt = 0f;
				this.nodes.Clear();
				this.inUse = false;
				this.layer = null;
			}
		}

		// Token: 0x0600DBF5 RID: 56309 RVA: 0x0051E9F7 File Offset: 0x0051CBF7
		public static WorldPath NewNotFound()
		{
			return new WorldPath
			{
				totalCostInt = -1f
			};
		}

		// Token: 0x0600DBF6 RID: 56310 RVA: 0x0051EA09 File Offset: 0x0051CC09
		public PlanetTile ConsumeNextNode()
		{
			PlanetTile result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x0600DBF7 RID: 56311 RVA: 0x0051EA20 File Offset: 0x0051CC20
		public PlanetTile Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x0600DBF8 RID: 56312 RVA: 0x0051EA38 File Offset: 0x0051CC38
		public override string ToString()
		{
			if (!this.Found)
			{
				return "WorldPath(not found)";
			}
			if (!this.inUse)
			{
				return "WorldPath(not in use)";
			}
			return string.Concat(new string[]
			{
				"WorldPath(nodeCount= ",
				this.nodes.Count.ToString(),
				(this.nodes.Count > 0) ? (" first=" + this.FirstNode.ToString() + " last=" + this.LastNode.ToString()) : "",
				" cost=",
				this.totalCostInt.ToString(),
				" )"
			});
		}

		// Token: 0x0600DBF9 RID: 56313 RVA: 0x0051EAF8 File Offset: 0x0051CCF8
		public void DrawPath(Caravan pathingCaravan)
		{
			if (!this.Found)
			{
				return;
			}
			if (this.NodesLeftCount > 0)
			{
				WorldGrid worldGrid = Find.WorldGrid;
				float d = 0.08f;
				for (int i = 0; i < this.NodesLeftCount - 1; i++)
				{
					Vector3 a = worldGrid.GetTileCenter(this.Peek(i));
					Vector3 vector = worldGrid.GetTileCenter(this.Peek(i + 1));
					a += a.normalized * d;
					vector += vector.normalized * d;
					GenDraw.DrawWorldLineBetween(a, vector, 1f);
				}
				if (pathingCaravan != null)
				{
					Vector3 a2 = pathingCaravan.DrawPos;
					Vector3 vector2 = worldGrid.GetTileCenter(this.Peek(0));
					a2 += a2.normalized * d;
					vector2 += vector2.normalized * d;
					if ((a2 - vector2).sqrMagnitude > 0.005f)
					{
						GenDraw.DrawWorldLineBetween(a2, vector2, 1f);
					}
				}
			}
		}

		// Token: 0x04008D1F RID: 36127
		private readonly List<PlanetTile> nodes = new List<PlanetTile>(128);

		// Token: 0x04008D20 RID: 36128
		private float totalCostInt;

		// Token: 0x04008D21 RID: 36129
		private int curNodeIndex;

		// Token: 0x04008D22 RID: 36130
		private PlanetLayer layer;

		// Token: 0x04008D23 RID: 36131
		public bool inUse;
	}
}
