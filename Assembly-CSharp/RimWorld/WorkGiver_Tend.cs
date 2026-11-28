using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C48 RID: 3144
	public class WorkGiver_Tend : WorkGiver_Scanner
	{
		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x0600595F RID: 22879 RVA: 0x0020E977 File Offset: 0x0020CB77
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x0020E97A File Offset: 0x0020CB7A
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06005961 RID: 22881 RVA: 0x0020E97D File Offset: 0x0020CB7D
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06005962 RID: 22882 RVA: 0x0020E986 File Offset: 0x0020CB86
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWithAnyHediff;
		}

		// Token: 0x06005963 RID: 22883 RVA: 0x0020E998 File Offset: 0x0020CB98
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && WorkGiver_Tend.GoodLayingStatusForTend(pawn2, pawn) && HealthAIUtility.ShouldBeTendedNowByPlayer(pawn2) && pawn.CanReserve(pawn2, 1, -1, null, forced) && (!pawn2.IsMutant || pawn2.mutant.Def.entitledToMedicalCare) && (!pawn2.InAggroMentalState || pawn2.health.hediffSet.HasHediff(HediffDefOf.Scaria, false));
		}

		// Token: 0x06005964 RID: 22884 RVA: 0x0020EA0F File Offset: 0x0020CC0F
		public static bool GoodLayingStatusForTend(Pawn patient, Pawn doctor)
		{
			if (patient == doctor)
			{
				return true;
			}
			if (patient.RaceProps.Humanlike)
			{
				return patient.InBed();
			}
			return patient.GetPosture() > PawnPosture.Standing;
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x0020EA34 File Offset: 0x0020CC34
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing thing = HealthAIUtility.FindBestMedicine(pawn, pawn2, false);
			if (thing != null && thing.SpawnedParentOrMe != thing)
			{
				return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2, thing, thing.SpawnedParentOrMe);
			}
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2, thing);
			}
			return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2);
		}
	}
}
