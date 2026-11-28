using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C2B RID: 3115
	public class WorkGiver_RemovePaintBuilding : WorkGiver_Scanner
	{
		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x060058BD RID: 22717 RVA: 0x0020C812 File Offset: 0x0020AA12
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060058BE RID: 22718 RVA: 0x0020C815 File Offset: 0x0020AA15
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.RemovePaintBuilding);
		}

		// Token: 0x060058BF RID: 22719 RVA: 0x0020C82F File Offset: 0x0020AA2F
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_RemovePaintBuilding.<PotentialWorkThingsGlobal>d__3 <PotentialWorkThingsGlobal>d__ = new WorkGiver_RemovePaintBuilding.<PotentialWorkThingsGlobal>d__3(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x060058C0 RID: 22720 RVA: 0x0020C840 File Offset: 0x0020AA40
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return t.def.building != null && t.def.building.paintable && pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.RemovePaintBuilding) != null && pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) == null && !t.IsBurning() && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x060058C1 RID: 22721 RVA: 0x0020C8C4 File Offset: 0x0020AAC4
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.RemovePaintBuilding);
			job.AddQueuedTarget(TargetIndex.A, t);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = t.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(t.Map) && !c.Fogged(t.Map) && pawn.CanReach(c, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					List<Thing> thingList = c.GetThingList(t.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing = thingList[j];
						if (!job.targetQueueA.Contains(thing) && thing != t && this.HasJobOnThing(pawn, thing, forced))
						{
							job.AddQueuedTarget(TargetIndex.A, thing);
						}
					}
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
