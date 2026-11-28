using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008FC RID: 2300
	public class JobGiver_WanderInRoofedCellsInPen : JobGiver_Wander
	{
		// Token: 0x0600464A RID: 17994 RVA: 0x001B8F8D File Offset: 0x001B718D
		public JobGiver_WanderInRoofedCellsInPen()
		{
			this.wanderRadius = 10f;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 cell, IntVec3 root) => cell.Roofed(pawn.Map));
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x001B8FC8 File Offset: 0x001B71C8
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			Map map = pawn.Map;
			if (!JobGiver_WanderInRoofedCellsInPen.ShouldSeekRoofedCells(pawn))
			{
				return IntVec3.Invalid;
			}
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
			if (currentPenOf == null)
			{
				return IntVec3.Invalid;
			}
			IntVec3 intVec = pawn.Position;
			if (!intVec.Roofed(map))
			{
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Clear();
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Add(pawn.GetRegion(RegionType.Set_Passable));
				intVec = JobGiver_WanderInRoofedCellsInPen.FindNearbyRoofedCellIn(pawn.Position, JobGiver_WanderInRoofedCellsInPen.wanderRootRegions, map);
			}
			if (!intVec.IsValid || !intVec.Roofed(map))
			{
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Clear();
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.AddRange(currentPenOf.PenState.ConnectedRegions);
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Shuffle<Region>();
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.TruncateToLength(4);
				intVec = JobGiver_WanderInRoofedCellsInPen.FindNearbyRoofedCellIn(pawn.Position, JobGiver_WanderInRoofedCellsInPen.wanderRootRegions, map);
			}
			JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Clear();
			if (!intVec.IsValid)
			{
				return IntVec3.Invalid;
			}
			return RCellFinder.RandomWanderDestFor(pawn, intVec, this.wanderRadius, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger), false);
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x001B90CA File Offset: 0x001B72CA
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x001B90D4 File Offset: 0x001B72D4
		private static bool ShouldSeekRoofedCells(Pawn pawn)
		{
			using (List<GameCondition>.Enumerator enumerator = pawn.Map.GameConditionManager.ActiveConditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.pennedAnimalsSeekShelter)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x001B913C File Offset: 0x001B733C
		private static IntVec3 FindNearbyRoofedCellIn(IntVec3 root, List<Region> regions, Map map)
		{
			JobGiver_WanderInRoofedCellsInPen.<>c__DisplayClass6_0 CS$<>8__locals1 = new JobGiver_WanderInRoofedCellsInPen.<>c__DisplayClass6_0();
			CS$<>8__locals1.map = map;
			IntVec3 result = IntVec3.Invalid;
			int num = int.MaxValue;
			using (List<Region>.Enumerator enumerator = regions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 intVec;
					if (enumerator.Current.TryFindRandomCellInRegion(new Predicate<IntVec3>(CS$<>8__locals1.<FindNearbyRoofedCellIn>g__SafeCell|0), out intVec))
					{
						int num2 = root.DistanceToSquared(intVec);
						if (num2 < num)
						{
							result = intVec;
							num = num2;
						}
						if (num <= 100)
						{
							return result;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04002D9D RID: 11677
		private const int PenRegionsSampleSize = 4;

		// Token: 0x04002D9E RID: 11678
		private static readonly List<Region> wanderRootRegions = new List<Region>();
	}
}
