using System;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000366 RID: 870
	[BurstCompile(CompileSynchronously = true)]
	public struct PathFinderJob : IJob
	{
		// Token: 0x06001AAB RID: 6827 RVA: 0x000A3510 File Offset: 0x000A1710
		[BurstCompile]
		public void Execute()
		{
			this.calcGrid.Clear<PathFinderJob.CalcNode>();
			this.frontier.Clear();
			this.path.Clear();
			this.result.Value = default(PathFinderJob.ResultData);
			int num = this.indices.CellToIndex(this.destCell);
			bool flag = this.destRect.Area == 1;
			int num2 = 0;
			this.InitalizeSearch();
			while (this.frontier.Count > 0)
			{
				int num3;
				float a;
				this.frontier.Dequeue(out num3, out a);
				if (Mathf.Approximately(a, this.calcGrid[num3].fCost) && this.calcGrid[num3].status != PathFinderJob.CalcNode.Status.Closed)
				{
					IntVec3 intVec = this.indices.IndexToCell(num3);
					if (flag)
					{
						if (num3 == num)
						{
							this.FinalizePath(num3);
							return;
						}
					}
					else if (this.destRect.Contains(intVec) && !this.excludedRectIndices.Contains(num3))
					{
						this.FinalizePath(num3);
						return;
					}
					if (num2 >= this.indices.NumGridCells)
					{
						return;
					}
					CellConnection connections = this.connectivity[num3];
					int num4 = connections.BitLoopEndIndex();
					for (int i = 0; i < num4; i++)
					{
						CellConnection cellConnection = CellConnectionExtensions.FlagFromBitIndex[i];
						if (connections.HasBit(cellConnection))
						{
							IntVec3 intVec2 = intVec + CellConnectionExtensions.OffsetFromBitIndex[i];
							int num5 = this.indices.CellToIndex(intVec2);
							if (this.calcGrid[num5].status != PathFinderJob.CalcNode.Status.Closed)
							{
								int num6 = this.IndexCost(num5);
								if (num6 < 10000)
								{
									if (cellConnection.Diagonal())
									{
										int num7 = this.indices.CellToIndex(new IntVec3(intVec2.x, 0, intVec.z));
										int num8 = this.indices.CellToIndex(new IntVec3(intVec.x, 0, intVec2.z));
										if (this.IndexCost(num7) >= 10000 || this.IndexCost(num8) >= 10000 || this.buildings.IsSet(num7) || (!this.traverseParams.canBashFences && this.fences.IsSet(num7)) || this.buildings.IsSet(num8) || (!this.traverseParams.canBashFences && this.fences.IsSet(num8)))
										{
											goto IL_3F7;
										}
									}
									if (cellConnection.Diagonal())
									{
										num6 += this.moveTicksDiagonal;
									}
									else
									{
										num6 += this.moveTicksCardinal;
									}
									int num9 = num6 + this.calcGrid[num3].gCost;
									PathFinderJob.CalcNode.Status status = this.calcGrid[num5].status;
									if (status != PathFinderJob.CalcNode.Status.None)
									{
										int num10 = 0;
										if (status == PathFinderJob.CalcNode.Status.Closed)
										{
											num10 = Mathf.CeilToInt((float)this.moveTicksCardinal * 0.8f);
										}
										if (this.calcGrid[num5].gCost <= num9 + num10)
										{
											goto IL_3F7;
										}
									}
									if (status == PathFinderJob.CalcNode.Status.None)
									{
										IntVec3 intVec3 = this.destCell - intVec2;
										if (intVec3.x != 0 || intVec3.z != 0)
										{
											int num11 = math.abs(intVec3.x);
											int num12 = math.abs(intVec3.z);
											float num13 = 1f * (float)(num11 + num12) + -0.58579004f * (float)math.min(num11, num12);
											PathFinderJob.CalcNode value = this.calcGrid[num5];
											value.hCost = num13 * 13f * this.heuristicStrength;
											this.calcGrid[num5] = value;
										}
									}
									float num14 = (float)num9 + this.calcGrid[num5].hCost;
									if (num14 < 0f)
									{
										num14 = 0f;
									}
									PathFinderJob.CalcNode value2 = this.calcGrid[num5];
									value2.gCost = num9;
									value2.fCost = num14;
									value2.parentIndex = num3;
									value2.status = PathFinderJob.CalcNode.Status.Open;
									this.calcGrid[num5] = value2;
									this.frontier.Enqueue(num5, num14);
								}
							}
						}
						IL_3F7:;
					}
					num2++;
					PathFinderJob.CalcNode value3 = this.calcGrid[num3];
					value3.status = PathFinderJob.CalcNode.Status.Closed;
					this.calcGrid[num3] = value3;
				}
			}
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x000A3954 File Offset: 0x000A1B54
		private int IndexCost(int index)
		{
			if (this.providerCost[index] == 65535)
			{
				return 10000;
			}
			return math.max(this.grid[index], (int)this.providerCost[index]) + (this.blocked[index] ? 175 : 0);
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x000A39B0 File Offset: 0x000A1BB0
		private void InitalizeSearch()
		{
			int num = this.indices.CellToIndex(this.start);
			PathFinderJob.CalcNode value = this.calcGrid[num];
			value.parentIndex = num;
			value.status = PathFinderJob.CalcNode.Status.Open;
			this.calcGrid[num] = value;
			this.frontier.Enqueue(num, 0f);
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x000A3A0C File Offset: 0x000A1C0C
		private void FinalizePath(int finalIndex)
		{
			int num = finalIndex;
			for (;;)
			{
				IntVec3 intVec = this.indices.IndexToCell(num);
				this.path.Add(intVec);
				int parentIndex = this.calcGrid[num].parentIndex;
				if (num == parentIndex)
				{
					break;
				}
				num = parentIndex;
			}
			PathFinderJob.ResultData value;
			value.pathCost = this.calcGrid[finalIndex].gCost;
			this.result.Value = value;
		}

		// Token: 0x040015BC RID: 5564
		private const int Cost_Blocked = 175;

		// Token: 0x040015BD RID: 5565
		public CellIndices indices;

		// Token: 0x040015BE RID: 5566
		public float heuristicStrength;

		// Token: 0x040015BF RID: 5567
		public int moveTicksCardinal;

		// Token: 0x040015C0 RID: 5568
		public int moveTicksDiagonal;

		// Token: 0x040015C1 RID: 5569
		public IntVec3 start;

		// Token: 0x040015C2 RID: 5570
		public IntVec3 destCell;

		// Token: 0x040015C3 RID: 5571
		public CellRect destRect;

		// Token: 0x040015C4 RID: 5572
		public PathFinder.UnmanagedGridTraverseParams traverseParams;

		// Token: 0x040015C5 RID: 5573
		public NativeArray<PathFinderJob.CalcNode> calcGrid;

		// Token: 0x040015C6 RID: 5574
		public NativePriorityQueue<int, float, FloatMinComparer> frontier;

		// Token: 0x040015C7 RID: 5575
		public NativeList<IntVec3> path;

		// Token: 0x040015C8 RID: 5576
		public NativeReference<PathFinderJob.ResultData> result;

		// Token: 0x040015C9 RID: 5577
		[ReadOnly]
		public NativeArray<int>.ReadOnly grid;

		// Token: 0x040015CA RID: 5578
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly providerCost;

		// Token: 0x040015CB RID: 5579
		[ReadOnly]
		public NativeArray<bool>.ReadOnly blocked;

		// Token: 0x040015CC RID: 5580
		[ReadOnly]
		public NativeList<int> excludedRectIndices;

		// Token: 0x040015CD RID: 5581
		[ReadOnly]
		public NativeArray<CellConnection>.ReadOnly connectivity;

		// Token: 0x040015CE RID: 5582
		[ReadOnly]
		public NativeBitArray.ReadOnly fences;

		// Token: 0x040015CF RID: 5583
		[ReadOnly]
		public NativeBitArray.ReadOnly buildings;

		// Token: 0x040015D0 RID: 5584
		private bool passAllDestroyableThings;

		// Token: 0x040015D1 RID: 5585
		private bool passWater;

		// Token: 0x0200269A RID: 9882
		public struct CalcNode
		{
			// Token: 0x04009E0D RID: 40461
			public int gCost;

			// Token: 0x04009E0E RID: 40462
			public float hCost;

			// Token: 0x04009E0F RID: 40463
			public float fCost;

			// Token: 0x04009E10 RID: 40464
			public int parentIndex;

			// Token: 0x04009E11 RID: 40465
			public PathFinderJob.CalcNode.Status status;

			// Token: 0x02003F4D RID: 16205
			public enum Status : byte
			{
				// Token: 0x0400F992 RID: 63890
				None,
				// Token: 0x0400F993 RID: 63891
				Open,
				// Token: 0x0400F994 RID: 63892
				Closed
			}
		}

		// Token: 0x0200269B RID: 9883
		public struct ResultData
		{
			// Token: 0x04009E12 RID: 40466
			public int pathCost;
		}
	}
}
