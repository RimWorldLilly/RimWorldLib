using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BCE RID: 3022
	public class WorkGiver_Replant : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x060056FE RID: 22270 RVA: 0x00204B0A File Offset: 0x00202D0A
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

		// Token: 0x060056FF RID: 22271 RVA: 0x00204B14 File Offset: 0x00202D14
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Blueprint_Install blueprint_Install = t as Blueprint_Install;
			if (blueprint_Install != null)
			{
				ThingDef thingDef = blueprint_Install.def.entityDefToBuild as ThingDef;
				if (thingDef != null && thingDef.plant != null)
				{
					if (GenConstruct.FirstBlockingThing(blueprint_Install, pawn) != null)
					{
						return GenConstruct.HandleBlockingThingJob(blueprint_Install, pawn, forced);
					}
					Thing thing;
					AcceptanceReport report = thingDef.CanEverPlantAt(t.Position, t.Map, out thing, true, true, false);
					if (!report)
					{
						JobFailReason.Is(report.Reason, null);
						return null;
					}
					if (!thingDef.CanNowPlantAt(t.Position, t.Map, true))
					{
						return null;
					}
					if (thingDef.plant.interferesWithRoof && t.Position.Roofed(t.Map))
					{
						JobFailReason.Is("BlockedByRoof".Translate(), null);
						return null;
					}
					Thing miniToInstallOrBuildingToReinstall = blueprint_Install.MiniToInstallOrBuildingToReinstall;
					IThingHolder parentHolder = miniToInstallOrBuildingToReinstall.ParentHolder;
					if (parentHolder != null)
					{
						Pawn_CarryTracker pawn_CarryTracker = parentHolder as Pawn_CarryTracker;
						if (pawn_CarryTracker != null)
						{
							JobFailReason.Is("BeingCarriedBy".Translate(pawn_CarryTracker.pawn), null);
							return null;
						}
					}
					if (miniToInstallOrBuildingToReinstall.IsForbidden(pawn))
					{
						JobFailReason.Is(WorkGiver_ConstructDeliverResources.ForbiddenLowerTranslated, null);
						return null;
					}
					if (!pawn.CanReach(miniToInstallOrBuildingToReinstall, PathEndMode.ClosestTouch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						JobFailReason.Is(WorkGiver_ConstructDeliverResources.NoPathTranslated, null);
						return null;
					}
					if (!pawn.CanReserve(miniToInstallOrBuildingToReinstall, 1, -1, null, false))
					{
						Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(miniToInstallOrBuildingToReinstall, pawn, null);
						if (pawn2 != null)
						{
							JobFailReason.Is("ReservedBy".Translate(pawn2.LabelShort, pawn2), null);
						}
						return null;
					}
					Job job = JobMaker.MakeJob(JobDefOf.Replant);
					job.targetA = miniToInstallOrBuildingToReinstall;
					job.targetB = blueprint_Install;
					job.plantDefToSow = thingDef;
					job.count = 1;
					job.haulMode = HaulMode.ToContainer;
					return job;
				}
			}
			return null;
		}
	}
}
