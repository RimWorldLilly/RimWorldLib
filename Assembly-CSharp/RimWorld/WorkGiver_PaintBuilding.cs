using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C2A RID: 3114
	public class WorkGiver_PaintBuilding : WorkGiver_Scanner
	{
		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x060058B5 RID: 22709 RVA: 0x0020C3A5 File Offset: 0x0020A5A5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x0020C3A8 File Offset: 0x0020A5A8
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.PaintBuilding);
		}

		// Token: 0x060058B7 RID: 22711 RVA: 0x0020C3C2 File Offset: 0x0020A5C2
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_PaintBuilding.<PotentialWorkThingsGlobal>d__3 <PotentialWorkThingsGlobal>d__ = new WorkGiver_PaintBuilding.<PotentialWorkThingsGlobal>d__3(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x060058B8 RID: 22712 RVA: 0x0020C3D4 File Offset: 0x0020A5D4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			AcceptanceReport report = this.ShouldPaintThing(pawn, t, forced, true);
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

		// Token: 0x060058B9 RID: 22713 RVA: 0x0020C414 File Offset: 0x0020A614
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			WorkGiver_PaintBuilding.tmpDye.Clear();
			WorkGiver_PaintBuilding.tmpDye = PaintUtility.FindNearbyDyes(pawn, forced);
			int stackCountFromThingList = ThingUtility.GetStackCountFromThingList(WorkGiver_PaintBuilding.tmpDye);
			if (!WorkGiver_PaintBuilding.tmpDye.Any<Thing>())
			{
				return null;
			}
			WorkGiver_PaintBuilding.tmpDye.SortBy((Thing x) => x.Position.DistanceToSquared(t.Position));
			int num = 0;
			Job job = JobMaker.MakeJob(JobDefOf.PaintBuilding);
			job.AddQueuedTarget(TargetIndex.A, t);
			job.AddQueuedTarget(TargetIndex.B, WorkGiver_PaintBuilding.tmpDye[num]);
			job.countQueue = new List<int>
			{
				1
			};
			int num2 = Mathf.Min(10, stackCountFromThingList);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = t.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(t.Map) && !c.Fogged(t.Map) && pawn.CanReach(c, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					List<Thing> thingList = c.GetThingList(t.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (thing != t && this.ShouldPaintThing(pawn, thing, forced, false) && !job.targetQueueA.Contains(thing))
						{
							job.AddQueuedTarget(TargetIndex.A, thing);
							List<int> countQueue = job.countQueue;
							int num3 = countQueue[0];
							countQueue[0] = num3 + 1;
							if (job.countQueue[0] >= WorkGiver_PaintBuilding.tmpDye[num].stackCount)
							{
								num++;
								if (num >= WorkGiver_PaintBuilding.tmpDye.Count)
								{
									break;
								}
								job.AddQueuedTarget(TargetIndex.B, WorkGiver_PaintBuilding.tmpDye[num]);
							}
						}
					}
					if (job.GetTargetQueue(TargetIndex.A).Count >= num2 || num >= WorkGiver_PaintBuilding.tmpDye.Count)
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

		// Token: 0x060058BA RID: 22714 RVA: 0x0020C680 File Offset: 0x0020A880
		private AcceptanceReport ShouldPaintThing(Pawn pawn, Thing t, bool forced, bool checkDye)
		{
			if (t.def.building == null || !t.def.building.paintable)
			{
				return false;
			}
			Designation designation = pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.PaintBuilding);
			if (((designation != null) ? designation.colorDef : null) == null)
			{
				return false;
			}
			Building building = t as Building;
			if (building != null && building.PaintColorDef == designation.colorDef)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.PaintBuilding) == null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.RemovePaintBuilding) != null)
			{
				return false;
			}
			if (t.IsBurning())
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
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

		// Token: 0x04003352 RID: 13138
		private static List<Thing> tmpDye = new List<Thing>();
	}
}
