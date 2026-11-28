using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C2D RID: 3117
	public class WorkGiver_RemovePaintFloor : WorkGiver_Scanner
	{
		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x060058CB RID: 22731 RVA: 0x0020CE05 File Offset: 0x0020B005
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060058CC RID: 22732 RVA: 0x0020CE08 File Offset: 0x0020B008
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.RemovePaintFloor);
		}

		// Token: 0x060058CD RID: 22733 RVA: 0x0020CE22 File Offset: 0x0020B022
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			WorkGiver_RemovePaintFloor.<PotentialWorkCellsGlobal>d__3 <PotentialWorkCellsGlobal>d__ = new WorkGiver_RemovePaintFloor.<PotentialWorkCellsGlobal>d__3(-2);
			<PotentialWorkCellsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkCellsGlobal>d__;
		}

		// Token: 0x060058CE RID: 22734 RVA: 0x0020CE34 File Offset: 0x0020B034
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			if (!pawn.Map.terrainGrid.TerrainAt(c).isPaintable)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.RemovePaintFloor) == null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.PaintFloor) != null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
			{
				return false;
			}
			Building firstBuilding = c.GetFirstBuilding(pawn.Map);
			TerrainDef terrainDef = pawn.Map.terrainGrid.UnderTerrainAt(c);
			return (firstBuilding == null || firstBuilding.def.terrainAffordanceNeeded == null || terrainDef == null || terrainDef.affordances.Contains(firstBuilding.def.terrainAffordanceNeeded)) && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, forced);
		}

		// Token: 0x060058CF RID: 22735 RVA: 0x0020CF10 File Offset: 0x0020B110
		public override Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.RemovePaintFloor);
			job.AddQueuedTarget(TargetIndex.A, cell);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = cell + GenRadial.RadialPattern[i];
				if (c.InBounds(pawn.Map) && !c.Fogged(pawn.Map) && pawn.CanReach(c, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !job.targetQueueA.Contains(c))
				{
					job.AddQueuedTarget(TargetIndex.A, c);
					if (job.GetTargetQueue(TargetIndex.A).Count >= 10)
					{
						break;
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}
	}
}
