using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020008A6 RID: 2214
	public class PawnPath : IDisposable
	{
		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x0600447A RID: 17530 RVA: 0x001ACEF0 File Offset: 0x001AB0F0
		public bool Found
		{
			get
			{
				return this.totalCostInt >= 0f;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x0600447B RID: 17531 RVA: 0x001ACF02 File Offset: 0x001AB102
		public bool Finished
		{
			get
			{
				return this.NodesLeftCount <= 0;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x0600447C RID: 17532 RVA: 0x001ACF10 File Offset: 0x001AB110
		public float TotalCost
		{
			get
			{
				return this.totalCostInt;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x0600447D RID: 17533 RVA: 0x001ACF18 File Offset: 0x001AB118
		public int NodesLeftCount
		{
			get
			{
				return this.curNodeIndex + 1;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x0600447E RID: 17534 RVA: 0x001ACF22 File Offset: 0x001AB122
		public int NodesConsumedCount
		{
			get
			{
				return this.nodes.Count - this.NodesLeftCount;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x0600447F RID: 17535 RVA: 0x001ACF36 File Offset: 0x001AB136
		public bool UsedRegionHeuristics
		{
			get
			{
				return this.usedRegionHeuristics;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004480 RID: 17536 RVA: 0x001ACF3E File Offset: 0x001AB13E
		public List<IntVec3> NodesReversed
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06004481 RID: 17537 RVA: 0x001ACF46 File Offset: 0x001AB146
		public IntVec3 FirstNode
		{
			get
			{
				List<IntVec3> list = this.nodes;
				return list[list.Count - 1];
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004482 RID: 17538 RVA: 0x001ACF5B File Offset: 0x001AB15B
		public IntVec3 LastNode
		{
			get
			{
				return this.nodes[0];
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004483 RID: 17539 RVA: 0x001ACF69 File Offset: 0x001AB169
		public static PawnPath NotFound
		{
			get
			{
				return new PawnPath
				{
					totalCostInt = -1f
				};
			}
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x001ACF7C File Offset: 0x001AB17C
		internal void Initialize(NativeList<IntVec3> points, int cost)
		{
			if (!this.Found)
			{
				Log.Warning("Calling initialize on invalid path");
				return;
			}
			this.inUse = true;
			for (int i = 0; i < points.Length; i++)
			{
				this.nodes.Add(points[i]);
			}
			this.curNodeIndex = this.nodes.Count - 1;
			this.totalCostInt = (float)cost;
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x001ACFE3 File Offset: 0x001AB1E3
		public void AddNode(IntVec3 nodePosition)
		{
			this.nodes.Add(nodePosition);
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x001ACFF1 File Offset: 0x001AB1F1
		public void Dispose()
		{
			this.ReleaseToPool();
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x001ACFFC File Offset: 0x001AB1FC
		public void ReleaseToPool()
		{
			if (!this.Found)
			{
				return;
			}
			this.totalCostInt = 0f;
			this.usedRegionHeuristics = false;
			this.nodes.Clear();
			this.inUse = false;
			if (this.pool != null)
			{
				this.pool.ReturnPath(this);
			}
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x001AD04A File Offset: 0x001AB24A
		public IntVec3 ConsumeNextNode()
		{
			IntVec3 result = this.Peek(1);
			this.curNodeIndex--;
			return result;
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x001AD061 File Offset: 0x001AB261
		public bool BacktrackNode()
		{
			if (this.curNodeIndex >= this.nodes.Count - 1)
			{
				return false;
			}
			this.curNodeIndex++;
			return true;
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x001AD089 File Offset: 0x001AB289
		public IntVec3 Peek(int nodesAhead)
		{
			return this.nodes[this.curNodeIndex - nodesAhead];
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x001AD0A0 File Offset: 0x001AB2A0
		public override string ToString()
		{
			if (!this.Found)
			{
				return "PawnPath(not found)";
			}
			if (!this.inUse)
			{
				return "PawnPath(not in use)";
			}
			string[] array = new string[6];
			array[0] = "PawnPath(nodeCount= ";
			array[1] = this.nodes.Count.ToString();
			int num = 2;
			string text;
			if (this.nodes.Count <= 0)
			{
				text = "";
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
				defaultInterpolatedStringHandler.AppendLiteral(" first=");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.FirstNode);
				defaultInterpolatedStringHandler.AppendLiteral(" last=");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(this.LastNode);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			array[num] = text;
			array[3] = " cost=";
			array[4] = this.totalCostInt.ToString();
			array[5] = " )";
			return string.Concat(array);
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x001AD16C File Offset: 0x001AB36C
		public void DrawPath(Pawn pathingPawn)
		{
			if (!this.Found)
			{
				return;
			}
			float y = AltitudeLayer.Item.AltitudeFor();
			if (this.NodesLeftCount > 0)
			{
				for (int i = 0; i < this.NodesLeftCount - 1; i++)
				{
					Vector3 a = this.Peek(i).ToVector3Shifted();
					a.y = y;
					Vector3 b = this.Peek(i + 1).ToVector3Shifted();
					b.y = y;
					GenDraw.DrawLineBetween(a, b);
				}
				if (pathingPawn != null)
				{
					Vector3 drawPos = pathingPawn.DrawPos;
					drawPos.y = y;
					Vector3 b2 = this.Peek(0).ToVector3Shifted();
					b2.y = y;
					if ((drawPos - b2).sqrMagnitude > 0.01f)
					{
						GenDraw.DrawLineBetween(drawPos, b2);
					}
				}
			}
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x001AD233 File Offset: 0x001AB433
		public List<IntVec3> PeekNextCells(int count, int curWaypointIndexOffset = 0)
		{
			this.PeekNextCells(count, PawnPath.tmpPeekCells, curWaypointIndexOffset);
			return PawnPath.tmpPeekCells;
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x001AD248 File Offset: 0x001AB448
		public void PeekNextCells(int count, List<IntVec3> outCells, int offset = 1)
		{
			outCells.Clear();
			if (this.Finished)
			{
				return;
			}
			int num = Math.Max(this.curNodeIndex - offset - count, 0);
			for (int i = this.curNodeIndex - offset; i >= num; i--)
			{
				outCells.Add(this.nodes[i]);
			}
		}

		// Token: 0x04002C55 RID: 11349
		private readonly List<IntVec3> nodes = new List<IntVec3>(128);

		// Token: 0x04002C56 RID: 11350
		private float totalCostInt;

		// Token: 0x04002C57 RID: 11351
		private int curNodeIndex;

		// Token: 0x04002C58 RID: 11352
		private bool usedRegionHeuristics;

		// Token: 0x04002C59 RID: 11353
		private bool inUse;

		// Token: 0x04002C5A RID: 11354
		internal PawnPathPool pool;

		// Token: 0x04002C5B RID: 11355
		private static readonly List<IntVec3> tmpPeekCells = new List<IntVec3>();
	}
}
