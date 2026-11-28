using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C32 RID: 3122
	public class WorkGiver_PlantSeed : WorkGiver_Scanner
	{
		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x060058E0 RID: 22752 RVA: 0x0020D31E File Offset: 0x0020B51E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Seed);
			}
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x0020D327 File Offset: 0x0020B527
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.IdeologyActive && !ModsConfig.BiotechActive && !ModsConfig.AnomalyActive && !ModsConfig.OdysseyActive;
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x0020D348 File Offset: 0x0020B548
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompPlantable compPlantable = t.TryGetComp<CompPlantable>();
			if (compPlantable == null || !pawn.CanReserve(t, 1, 1, null, forced))
			{
				return false;
			}
			List<IntVec3> plantCells = compPlantable.PlantCells;
			for (int i = 0; i < plantCells.Count; i++)
			{
				if (!plantCells[i].IsForbidden(pawn) && pawn.CanReach(plantCells[i], PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (compPlantable.Props.plantDefToSpawn.plant.vacuumResistant || plantCells[i].GetVacuum(t.Map) < 0.5f))
				{
					Plant plant = plantCells[i].GetPlant(t.Map);
					if (plant == null || this.CanDoCutJob(pawn, plant, forced))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060058E3 RID: 22755 RVA: 0x0020D40C File Offset: 0x0020B60C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompPlantable compPlantable = t.TryGetComp<CompPlantable>();
			if (compPlantable == null)
			{
				return null;
			}
			List<IntVec3> plantCells = compPlantable.PlantCells;
			for (int i = 0; i < plantCells.Count; i++)
			{
				if (!plantCells[i].IsForbidden(pawn) && pawn.CanReach(plantCells[i], PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (compPlantable.Props.plantDefToSpawn.plant.vacuumResistant || plantCells[i].GetVacuum(t.Map) < 0.5f))
				{
					Plant plant = plantCells[i].GetPlant(t.Map);
					if (plant == null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.PlantSeed, t, plantCells[i]);
						job.playerForced = forced;
						job.plantDefToSow = compPlantable.Props.plantDefToSpawn;
						job.count = 1;
						return job;
					}
					if (this.CanDoCutJob(pawn, plant, forced))
					{
						return JobMaker.MakeJob(JobDefOf.CutPlant, plant);
					}
				}
			}
			return null;
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x0020D510 File Offset: 0x0020B710
		private bool CanDoCutJob(Pawn pawn, Thing plant, bool forced)
		{
			return pawn.CanReserve(plant, 1, -1, null, forced) && PlantUtility.PawnWillingToCutPlant_Job(plant, pawn);
		}
	}
}
