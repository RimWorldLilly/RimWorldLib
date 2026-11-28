using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000369 RID: 873
	public class PathGridDoorsBlockedJob : IJob
	{
		// Token: 0x06001AC4 RID: 6852 RVA: 0x000A46E8 File Offset: 0x000A28E8
		public void Execute()
		{
			this.providerCost.Clear<ushort>();
			this.blocked.Clear<bool>();
			foreach (Thing thing in this.doors)
			{
				Building_Door building_Door = thing as Building_Door;
				if (building_Door != null)
				{
					foreach (IntVec3 c in building_Door.OccupiedRect())
					{
						int index = this.map.cellIndices.CellToIndex(c);
						this.providerCost[index] = PathUtility.GetDoorCost(building_Door, this.traverseParams, this.pawn, new PathFinderCostTuning?(this.tuning));
					}
				}
			}
			if (this.pawn != null)
			{
				foreach (IPathFindCostProvider pathFindCostProvider in this.providers)
				{
					foreach (IntVec3 c2 in pathFindCostProvider.GetOccupiedRect())
					{
						int num = this.map.cellIndices.CellToIndex(c2);
						ref NativeArray<ushort> ptr = ref this.providerCost;
						int index2 = num;
						ptr[index2] += pathFindCostProvider.PathFindCostFor(this.pawn);
					}
				}
			}
			if (!PawnUtility.ShouldCollideWithPawns(this.pawn))
			{
				return;
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (pawn != this.pawn && this.CanBlockEver(pawn))
				{
					bool collideWithNonHostile = this.pawn.CurJob != null && (this.pawn.CurJob.collideWithPawns || this.pawn.CurJob.def.collideWithPawns || this.pawn.jobs.curDriver.collideWithPawns);
					if (PawnUtility.PawnBlockedBy(this.pawn, pawn, false, collideWithNonHostile, true, false))
					{
						this.blocked[this.map.cellIndices.CellToIndex(pawn.Position)] = true;
					}
				}
			}
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x000A4988 File Offset: 0x000A2B88
		private bool CanBlockEver(Pawn other)
		{
			if (other.Destroyed || other.Discarded || !other.Spawned)
			{
				return false;
			}
			if (this.blocked[this.map.cellIndices.CellToIndex(other.Position)])
			{
				return false;
			}
			if (!this.pawn.pather.BestPathHadPawnsInTheWayRecently())
			{
				int lengthManhattan = (other.Position - this.start).LengthManhattan;
				int lengthManhattan2 = (other.Position - this.dest).LengthManhattan;
				if (Mathf.Min(lengthManhattan, lengthManhattan2) > 32)
				{
					return false;
				}
			}
			return PawnUtility.ShouldCollideWithPawns(other);
		}

		// Token: 0x040015E9 RID: 5609
		private const int MinDistToCheckBlocked = 32;

		// Token: 0x040015EA RID: 5610
		public Map map;

		// Token: 0x040015EB RID: 5611
		public Pawn pawn;

		// Token: 0x040015EC RID: 5612
		public IntVec3 start;

		// Token: 0x040015ED RID: 5613
		public IntVec3 dest;

		// Token: 0x040015EE RID: 5614
		public TraverseParms traverseParams;

		// Token: 0x040015EF RID: 5615
		public PathFinderCostTuning tuning;

		// Token: 0x040015F0 RID: 5616
		public IReadOnlyList<Thing> doors;

		// Token: 0x040015F1 RID: 5617
		public IReadOnlyList<IPathFindCostProvider> providers;

		// Token: 0x040015F2 RID: 5618
		public IReadOnlyList<Pawn> pawns;

		// Token: 0x040015F3 RID: 5619
		public NativeArray<ushort> providerCost;

		// Token: 0x040015F4 RID: 5620
		public NativeArray<bool> blocked;

		// Token: 0x040015F5 RID: 5621
		private bool passAllDestroyableThings;

		// Token: 0x040015F6 RID: 5622
		private bool passWater;
	}
}
