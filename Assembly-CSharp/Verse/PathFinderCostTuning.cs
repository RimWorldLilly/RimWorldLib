using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000363 RID: 867
	public struct PathFinderCostTuning : IEquatable<PathFinderCostTuning>
	{
		// Token: 0x06001AA3 RID: 6819 RVA: 0x000A32FC File Offset: 0x000A14FC
		public static PathFinderCostTuning For(Pawn pawn)
		{
			PathFinderCostTuning defaultTuning = PathFinderCostTuning.DefaultTuning;
			if (pawn.WaterCellCost != null)
			{
				defaultTuning.costWater = pawn.WaterCellCost.Value;
			}
			if (!pawn.RaceProps.ToolUser || pawn.HostileTo(Faction.OfPlayer))
			{
				defaultTuning.costBlueprintFrame = 0;
			}
			return defaultTuning;
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x000A3358 File Offset: 0x000A1558
		public PathFinderCostTuning(int costBlockedWallBase, float costBlockedWallExtraPerHitPoint, int costBlockedWallExtraForNaturalWalls, int costBlockedDoor, float costBlockedDoorPerHitPoint, int costOffLordWalkGrid, int costDanger, int costBlueprintFrame, int costWater = -1)
		{
			this.costBlockedWallBase = costBlockedWallBase;
			this.costBlockedWallExtraPerHitPoint = costBlockedWallExtraPerHitPoint;
			this.costBlockedWallExtraForNaturalWalls = costBlockedWallExtraForNaturalWalls;
			this.costBlockedDoor = costBlockedDoor;
			this.costBlockedDoorPerHitPoint = costBlockedDoorPerHitPoint;
			this.costOffLordWalkGrid = costOffLordWalkGrid;
			this.costDanger = costDanger;
			this.costBlueprintFrame = costBlueprintFrame;
			this.costWater = costWater;
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x000A33AC File Offset: 0x000A15AC
		public bool Equals(PathFinderCostTuning other)
		{
			return this.costBlockedWallBase == other.costBlockedWallBase && this.costBlockedWallExtraPerHitPoint.Equals(other.costBlockedWallExtraPerHitPoint) && this.costBlockedWallExtraForNaturalWalls == other.costBlockedWallExtraForNaturalWalls && this.costBlockedDoor == other.costBlockedDoor && this.costBlockedDoorPerHitPoint.Equals(other.costBlockedDoorPerHitPoint) && this.costOffLordWalkGrid == other.costOffLordWalkGrid && this.costDanger == other.costDanger && this.costBlueprintFrame == other.costBlueprintFrame && this.costWater == other.costWater;
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x000A3444 File Offset: 0x000A1644
		public override bool Equals(object obj)
		{
			if (obj is PathFinderCostTuning)
			{
				PathFinderCostTuning other = (PathFinderCostTuning)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x000A346C File Offset: 0x000A166C
		public override int GetHashCode()
		{
			return HashCode.Combine<int, int>(HashCode.Combine<int, float, int, int, float, int, int, int>(this.costBlockedWallBase, this.costBlockedWallExtraPerHitPoint, this.costBlockedWallExtraForNaturalWalls, this.costBlockedDoor, this.costBlockedDoorPerHitPoint, this.costOffLordWalkGrid, this.costDanger, this.costBlueprintFrame), this.costWater);
		}

		// Token: 0x040015AA RID: 5546
		private const int Cost_BlockedWallBase = 70;

		// Token: 0x040015AB RID: 5547
		private const float Cost_BlockedWallExtraPerHitPoint = 0.2f;

		// Token: 0x040015AC RID: 5548
		private const int Cost_BlockedWallExtraForNaturalWalls = 0;

		// Token: 0x040015AD RID: 5549
		private const int Cost_BlockedDoor = 50;

		// Token: 0x040015AE RID: 5550
		private const float Cost_BlockedDoorPerHitPoint = 0.2f;

		// Token: 0x040015AF RID: 5551
		private const int Cost_OffLordWalkGrid = 70;

		// Token: 0x040015B0 RID: 5552
		private const int Cost_Danger = 300;

		// Token: 0x040015B1 RID: 5553
		private const int Cost_BlueprintFrame = 800;

		// Token: 0x040015B2 RID: 5554
		public int costBlockedWallBase;

		// Token: 0x040015B3 RID: 5555
		public float costBlockedWallExtraPerHitPoint;

		// Token: 0x040015B4 RID: 5556
		public int costBlockedWallExtraForNaturalWalls;

		// Token: 0x040015B5 RID: 5557
		public int costBlockedDoor;

		// Token: 0x040015B6 RID: 5558
		public float costBlockedDoorPerHitPoint;

		// Token: 0x040015B7 RID: 5559
		public int costOffLordWalkGrid;

		// Token: 0x040015B8 RID: 5560
		public int costDanger;

		// Token: 0x040015B9 RID: 5561
		public int costWater;

		// Token: 0x040015BA RID: 5562
		public int costBlueprintFrame;

		// Token: 0x040015BB RID: 5563
		public static readonly PathFinderCostTuning DefaultTuning = new PathFinderCostTuning(70, 0.2f, 0, 50, 0.2f, 70, 300, 800, -1);
	}
}
