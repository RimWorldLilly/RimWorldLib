using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200036A RID: 874
	[BurstCompile(CompileSynchronously = true)]
	public struct PathGridJob : IJobParallelFor
	{
		// Token: 0x06001AC7 RID: 6855 RVA: 0x000A4A34 File Offset: 0x000A2C34
		[BurstCompile]
		public void Execute(int index)
		{
			this.passAllDestroyableThings = (this.traverseParams.mode == TraverseMode.PassAllDestroyableThings || this.traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater);
			this.passWater = (this.traverseParams.mode != TraverseMode.NoPassClosedDoorsOrWater && this.traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater);
			if (!this.CellIsPassable(index))
			{
				this.grid[index] = 10000;
				return;
			}
			this.grid[index] = this.CostForCell(index);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x000A4ABC File Offset: 0x000A2CBC
		private bool DestroyableBuilding(int index)
		{
			return this.pathGridDirect[index] >= 10000 && ((this.traverseParams.fenceBlocked && this.traverseParams.canBashFences && this.fence.IsSet(index)) || (this.building.IsSet(index) && this.buildingDestroyable.IsSet(index) && ((this.traverseParams.mode == TraverseMode.PassAllDestroyablePlayerOwnedThings && this.player.IsSet(index)) || this.passAllDestroyableThings)));
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x000A4B4C File Offset: 0x000A2D4C
		private bool CellIsPassable(int index)
		{
			return (this.pathGridDirect[index] < 10000 || this.DestroyableBuilding(index)) && (this.custom.Length <= 0 || this.custom[index] < 10000) && (this.passWater || !this.water.IsSet(index)) && (!this.traverseParams.fenceBlocked || this.traverseParams.canBashFences || !this.fence.IsSet(index));
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x000A4BE0 File Offset: 0x000A2DE0
		private int CostForCell(int index)
		{
			int num;
			if (this.DestroyableBuilding(index))
			{
				num = this.tuning.costBlockedWallBase + Mathf.RoundToInt((float)this.buildingHitPoints[index] * this.tuning.costBlockedWallExtraPerHitPoint);
			}
			else if (this.tuning.costWater >= 0 && this.water.IsSet(index))
			{
				num = this.tuning.costWater;
			}
			else
			{
				num = this.pathGridDirect[index];
			}
			if (this.tuning.costWater <= 0 || !this.water.IsSet(index))
			{
				num += (int)this.perceptualCost[index];
			}
			if (this.avoidGrid.Length > 0)
			{
				num += (int)(this.avoidGrid[index] * 8);
			}
			if (this.allowedGrid.Length > 0 && !this.allowedGrid.IsSet(index))
			{
				num += 600;
			}
			if (this.traverseParams.fenceBlocked && this.fence.IsSet(index) && this.passAllDestroyableThings)
			{
				num += this.tuning.costBlockedDoor + Mathf.RoundToInt((float)this.buildingHitPoints[index] * this.tuning.costBlockedDoorPerHitPoint);
			}
			IntVec3 c = this.indicies[index];
			if (this.traverseParams.targetBuildable.Area == 0 || !this.traverseParams.targetBuildable.Contains(c))
			{
				num += (int)this.factionCosts[index];
			}
			if (this.traverseParams.avoidPersistentDanger && this.persistentDanger.IsSet(index))
			{
				num += this.tuning.costDanger;
			}
			else if (this.traverseParams.avoidDarknessDanger && this.darknessDanger.IsSet(index))
			{
				num += this.tuning.costDanger;
			}
			if (this.lordGrid.Length > 0 && !this.lordGrid.IsSet(index))
			{
				num += this.tuning.costOffLordWalkGrid;
			}
			if (this.traverseParams.avoidFog && this.fogged.IsSet(index))
			{
				num += 600;
			}
			if (this.custom.Length > 0)
			{
				num += (int)this.custom[index];
			}
			return num;
		}

		// Token: 0x040015F7 RID: 5623
		public const int Cost_OutsideAllowedArea = 600;

		// Token: 0x040015F8 RID: 5624
		public const int Cost_AvoidFog = 600;

		// Token: 0x040015F9 RID: 5625
		[ReadOnly]
		public PathFinderCostTuning tuning;

		// Token: 0x040015FA RID: 5626
		[ReadOnly]
		public PathFinder.UnmanagedGridTraverseParams traverseParams;

		// Token: 0x040015FB RID: 5627
		[ReadOnly]
		public CellIndices indicies;

		// Token: 0x040015FC RID: 5628
		[ReadOnly]
		public NativeArray<int>.ReadOnly pathGridDirect;

		// Token: 0x040015FD RID: 5629
		[ReadOnly]
		public NativeBitArray.ReadOnly allowedGrid;

		// Token: 0x040015FE RID: 5630
		[ReadOnly]
		public NativeBitArray.ReadOnly building;

		// Token: 0x040015FF RID: 5631
		[ReadOnly]
		public NativeBitArray.ReadOnly buildingDestroyable;

		// Token: 0x04001600 RID: 5632
		[ReadOnly]
		public NativeBitArray.ReadOnly fence;

		// Token: 0x04001601 RID: 5633
		[ReadOnly]
		public NativeBitArray.ReadOnly lordGrid;

		// Token: 0x04001602 RID: 5634
		[ReadOnly]
		public NativeBitArray.ReadOnly water;

		// Token: 0x04001603 RID: 5635
		[ReadOnly]
		public NativeBitArray.ReadOnly darknessDanger;

		// Token: 0x04001604 RID: 5636
		[ReadOnly]
		public NativeBitArray.ReadOnly persistentDanger;

		// Token: 0x04001605 RID: 5637
		[ReadOnly]
		public NativeBitArray.ReadOnly fogged;

		// Token: 0x04001606 RID: 5638
		[ReadOnly]
		public NativeBitArray.ReadOnly player;

		// Token: 0x04001607 RID: 5639
		[ReadOnly]
		public NativeArray<byte>.ReadOnly avoidGrid;

		// Token: 0x04001608 RID: 5640
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly factionCosts;

		// Token: 0x04001609 RID: 5641
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly buildingHitPoints;

		// Token: 0x0400160A RID: 5642
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly perceptualCost;

		// Token: 0x0400160B RID: 5643
		[ReadOnly]
		public NativeArray<ushort>.ReadOnly custom;

		// Token: 0x0400160C RID: 5644
		public NativeArray<int> grid;

		// Token: 0x0400160D RID: 5645
		private bool passAllDestroyableThings;

		// Token: 0x0400160E RID: 5646
		private bool passWater;
	}
}
