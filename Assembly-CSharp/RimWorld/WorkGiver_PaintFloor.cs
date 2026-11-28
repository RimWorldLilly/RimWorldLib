using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C2C RID: 3116
	public class WorkGiver_PaintFloor : WorkGiver_Scanner
	{
		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x060058C3 RID: 22723 RVA: 0x0020CA07 File Offset: 0x0020AC07
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060058C4 RID: 22724 RVA: 0x0020CA0A File Offset: 0x0020AC0A
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.PaintFloor);
		}

		// Token: 0x060058C5 RID: 22725 RVA: 0x0020CA24 File Offset: 0x0020AC24
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			WorkGiver_PaintFloor.<PotentialWorkCellsGlobal>d__3 <PotentialWorkCellsGlobal>d__ = new WorkGiver_PaintFloor.<PotentialWorkCellsGlobal>d__3(-2);
			<PotentialWorkCellsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkCellsGlobal>d__;
		}

		// Token: 0x060058C6 RID: 22726 RVA: 0x0020CA34 File Offset: 0x0020AC34
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			AcceptanceReport report = this.ShouldPaintCell(pawn, c, forced, true);
			if (!report)
			{
				if (!report.Reason.NullOrEmpty())
				{
					JobFailReason.Is(report.Reason, null);
				}
				return false;
			}
			return true;
		}

		// Token: 0x060058C7 RID: 22727 RVA: 0x0020CA74 File Offset: 0x0020AC74
		public override Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			WorkGiver_PaintFloor.tmpDye.Clear();
			WorkGiver_PaintFloor.tmpDye = PaintUtility.FindNearbyDyes(pawn, forced);
			int stackCountFromThingList = ThingUtility.GetStackCountFromThingList(WorkGiver_PaintFloor.tmpDye);
			if (!WorkGiver_PaintFloor.tmpDye.Any<Thing>())
			{
				return null;
			}
			WorkGiver_PaintFloor.tmpDye.SortBy((Thing x) => x.Position.DistanceToSquared(cell));
			int num = 0;
			Job job = JobMaker.MakeJob(JobDefOf.PaintFloor);
			job.AddQueuedTarget(TargetIndex.A, cell);
			job.AddQueuedTarget(TargetIndex.B, WorkGiver_PaintFloor.tmpDye[num]);
			job.countQueue = new List<int>
			{
				1
			};
			int num2 = Mathf.Min(10, stackCountFromThingList);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = cell + GenRadial.RadialPattern[i];
				if (c.InBounds(pawn.Map) && !c.Fogged(pawn.Map) && pawn.CanReach(c, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					if (this.ShouldPaintCell(pawn, c, forced, false))
					{
						if (job.targetQueueA.Contains(c))
						{
							goto IL_1C6;
						}
						job.AddQueuedTarget(TargetIndex.A, c);
						List<int> countQueue = job.countQueue;
						int num3 = countQueue[0];
						countQueue[0] = num3 + 1;
						if (job.countQueue[0] >= WorkGiver_PaintFloor.tmpDye[num].stackCount)
						{
							num++;
							if (num >= WorkGiver_PaintFloor.tmpDye.Count)
							{
								break;
							}
							job.AddQueuedTarget(TargetIndex.B, WorkGiver_PaintFloor.tmpDye[num]);
						}
					}
					if (job.GetTargetQueue(TargetIndex.A).Count >= num2)
					{
						break;
					}
				}
				IL_1C6:;
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
			{
				job.targetQueueA.SortBy((LocalTargetInfo targ) => targ.Cell.DistanceToSquared(pawn.Position));
			}
			return job;
		}

		// Token: 0x060058C8 RID: 22728 RVA: 0x0020CC84 File Offset: 0x0020AE84
		private AcceptanceReport ShouldPaintCell(Pawn pawn, IntVec3 c, bool forced, bool checkDye)
		{
			if (!pawn.Map.terrainGrid.TerrainAt(c).isPaintable)
			{
				return false;
			}
			Designation designation = pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.PaintFloor);
			if (((designation != null) ? designation.colorDef : null) == null)
			{
				return false;
			}
			if (pawn.Map.terrainGrid.ColorAt(c) == designation.colorDef)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.PaintFloor) == null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.RemoveFloor) != null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationAt(c, DesignationDefOf.RemovePaintFloor) != null)
			{
				return false;
			}
			if (!pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, forced))
			{
				return false;
			}
			if (checkDye)
			{
				List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye);
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].IsForbidden(pawn) && pawn.CanReserveAndReach(list[i], PathEndMode.ClosestTouch, Danger.Deadly, 1, 1, null, forced))
					{
						return true;
					}
				}
				return "NoIngredient".Translate(ThingDefOf.Dye);
			}
			return true;
		}

		// Token: 0x04003353 RID: 13139
		private static List<Thing> tmpDye = new List<Thing>();
	}
}
