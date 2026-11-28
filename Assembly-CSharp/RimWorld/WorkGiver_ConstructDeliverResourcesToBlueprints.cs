using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC6 RID: 3014
	public class WorkGiver_ConstructDeliverResourcesToBlueprints : WorkGiver_ConstructDeliverResources
	{
		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x060056CF RID: 22223 RVA: 0x0020425E File Offset: 0x0020245E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Blueprint);
			}
		}

		// Token: 0x060056D0 RID: 22224 RVA: 0x00204268 File Offset: 0x00202468
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Blueprint blueprint = t as Blueprint;
			if (blueprint == null)
			{
				return false;
			}
			ThingDef thingDef = blueprint.def.entityDefToBuild as ThingDef;
			if (thingDef != null && thingDef.plant != null)
			{
				return false;
			}
			if (!GenConstruct.CanTouchTargetFromValidCell(blueprint, pawn))
			{
				return false;
			}
			if (GenConstruct.FirstBlockingThing(blueprint, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(blueprint, pawn, forced) != null;
			}
			if (!GenConstruct.CanConstruct(blueprint, pawn, this.def.workType, forced, JobDefOf.HaulToContainer))
			{
				return false;
			}
			if (this.def.workType != WorkTypeDefOf.Construction && WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
			{
				return false;
			}
			if (WorkGiver_ConstructDeliverResourcesToBlueprints.CanDoRemoveExistingFloorWork(pawn, blueprint))
			{
				return true;
			}
			if (blueprint is Blueprint_Install)
			{
				return base.ResourceDeliverJobFor(pawn, blueprint, true, forced) != null;
			}
			if (blueprint.TotalMaterialCost().Count == 0)
			{
				return this.def.workType != WorkTypeDefOf.Hauling;
			}
			return GenConstruct.CanGetResources_NewTemp(blueprint, pawn, forced) && base.ResourceDeliverJobFor(pawn, blueprint, true, forced) != null;
		}

		// Token: 0x060056D1 RID: 22225 RVA: 0x00204368 File Offset: 0x00202568
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return null;
			}
			Blueprint blueprint = t as Blueprint;
			if (blueprint == null)
			{
				return null;
			}
			ThingDef thingDef = blueprint.def.entityDefToBuild as ThingDef;
			if (thingDef != null && thingDef.plant != null)
			{
				return null;
			}
			if (!GenConstruct.CanTouchTargetFromValidCell(blueprint, pawn))
			{
				return null;
			}
			if (GenConstruct.FirstBlockingThing(blueprint, pawn) != null)
			{
				return GenConstruct.HandleBlockingThingJob(blueprint, pawn, forced);
			}
			if (!GenConstruct.CanConstruct(blueprint, pawn, this.def.workType, forced, JobDefOf.HaulToContainer))
			{
				return null;
			}
			if (this.def.workType != WorkTypeDefOf.Construction && WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blueprint))
			{
				return null;
			}
			Job job = base.RemoveExistingFloorJob(pawn, blueprint);
			if (job != null)
			{
				return job;
			}
			Job job2 = base.ResourceDeliverJobFor(pawn, blueprint, true, forced);
			if (job2 != null)
			{
				return job2;
			}
			if (this.def.workType != WorkTypeDefOf.Hauling)
			{
				Job job3 = this.NoCostFrameMakeJobFor(blueprint);
				if (job3 != null)
				{
					return job3;
				}
			}
			return null;
		}

		// Token: 0x060056D2 RID: 22226 RVA: 0x00204448 File Offset: 0x00202648
		private static bool CanDoRemoveExistingFloorWork(Pawn pawn, Blueprint blue)
		{
			return WorkGiver_ConstructDeliverResources.ShouldRemoveExistingFloorFirst(pawn, blue) && pawn.CanReserve(blue.Position, 1, -1, ReservationLayerDefOf.Floor, false) && !pawn.WorkTypeIsDisabled(WorkGiverDefOf.ConstructRemoveFloors.workType);
		}

		// Token: 0x060056D3 RID: 22227 RVA: 0x00204487 File Offset: 0x00202687
		private Job NoCostFrameMakeJobFor(IConstructible c)
		{
			if (c is Blueprint_Install)
			{
				return null;
			}
			if (c is Blueprint && c.TotalMaterialCost().Count == 0)
			{
				Job job = JobMaker.MakeJob(JobDefOf.PlaceNoCostFrame);
				job.targetA = (Thing)c;
				return job;
			}
			return null;
		}
	}
}
