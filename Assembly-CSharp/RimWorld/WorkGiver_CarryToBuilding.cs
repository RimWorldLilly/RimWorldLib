using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BF3 RID: 3059
	public abstract class WorkGiver_CarryToBuilding : WorkGiver_Scanner
	{
		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x06005784 RID: 22404 RVA: 0x00206E49 File Offset: 0x00205049
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x00206E4C File Offset: 0x0020504C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserveAndReach(t, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			Building_Enterable building_Enterable = t as Building_Enterable;
			if (building_Enterable == null)
			{
				return false;
			}
			Pawn selectedPawn = building_Enterable.SelectedPawn;
			return selectedPawn != null && selectedPawn.Map == pawn.Map && building_Enterable.CanAcceptPawn(selectedPawn) && (selectedPawn.IsPrisonerOfColony || selectedPawn.Downed || !selectedPawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) || (this.def.workType != null && selectedPawn.WorkTypeIsDisabled(this.def.workType))) && pawn.CanReserveAndReach(selectedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, forced);
		}

		// Token: 0x06005786 RID: 22406 RVA: 0x00206F18 File Offset: 0x00205118
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_Enterable building_Enterable = t as Building_Enterable;
			if (building_Enterable == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.CarryToBuilding, building_Enterable, building_Enterable.SelectedPawn);
			job.count = 1;
			return job;
		}
	}
}
