using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C18 RID: 3096
	public class WorkGiver_HaulMechToCharger : WorkGiver_Scanner
	{
		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x0600584F RID: 22607 RVA: 0x0020AA2A File Offset: 0x00208C2A
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06005850 RID: 22608 RVA: 0x0020AA33 File Offset: 0x00208C33
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x06005851 RID: 22609 RVA: 0x0020AA4C File Offset: 0x00208C4C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Haul mech to recharger"))
			{
				return false;
			}
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.Spawned || !pawn2.RaceProps.IsMechanoid || !pawn2.IsColonyMech)
			{
				return false;
			}
			if (pawn2.needs.energy != null)
			{
				if (!pawn2.Downed && !pawn2.needs.energy.IsLowEnergySelfShutdown)
				{
					return false;
				}
				MechanitorControlGroup mechControlGroup = pawn2.GetMechControlGroup();
				if (mechControlGroup != null && mechControlGroup.WorkMode == MechWorkModeDefOf.SelfShutdown)
				{
					return false;
				}
				if (pawn2.needs.energy.CurLevel >= JobGiver_GetEnergy.GetMaxRechargeLimit(pawn2))
				{
					return false;
				}
			}
			return pawn2.CurJobDef != JobDefOf.MechCharge && pawn.CanReserve(t, 1, -1, null, forced) && JobGiver_GetEnergy_Charger.GetClosestCharger(pawn2, pawn, forced) != null;
		}

		// Token: 0x06005852 RID: 22610 RVA: 0x0020AB1C File Offset: 0x00208D1C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Building_MechCharger closestCharger = JobGiver_GetEnergy_Charger.GetClosestCharger(pawn2, pawn, forced);
			Job job = JobMaker.MakeJob(JobDefOf.HaulMechToCharger, pawn2, closestCharger, closestCharger.InteractionCell);
			job.count = 1;
			return job;
		}
	}
}
