using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C30 RID: 3120
	public class WorkGiver_PatientGoToBedTreatment : WorkGiver_PatientGoToBedRecuperate
	{
		// Token: 0x060058D6 RID: 22742 RVA: 0x0020D077 File Offset: 0x0020B277
		public override Job NonScanJob(Pawn pawn)
		{
			if (!HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
			{
				return null;
			}
			if (!WorkGiver_PatientGoToBedTreatment.AnyAvailableDoctorFor(pawn))
			{
				return null;
			}
			return base.NonScanJob(pawn);
		}

		// Token: 0x060058D7 RID: 22743 RVA: 0x0020D094 File Offset: 0x0020B294
		public static bool AnyAvailableDoctorFor(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return false;
			}
			List<Pawn> list = mapHeld.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn2 = list[i];
				if (pawn2 != pawn && (pawn2.RaceProps.Humanlike || pawn2.IsColonyMechPlayerControlled) && !pawn2.Downed && pawn2.Awake() && !pawn2.InBed() && !pawn2.InMentalState && !pawn2.IsPrisoner && pawn2.workSettings != null && pawn2.workSettings.EverWork && pawn2.workSettings.WorkIsActive(WorkTypeDefOf.Doctor) && pawn2.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && pawn2.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
