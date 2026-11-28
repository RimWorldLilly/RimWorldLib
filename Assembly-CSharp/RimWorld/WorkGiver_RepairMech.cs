using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C3B RID: 3131
	public class WorkGiver_RepairMech : WorkGiver_Scanner
	{
		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06005915 RID: 22805 RVA: 0x0020DE4B File Offset: 0x0020C04B
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x06005916 RID: 22806 RVA: 0x0020DE54 File Offset: 0x0020C054
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06005917 RID: 22807 RVA: 0x0020DE6C File Offset: 0x0020C06C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005918 RID: 22808 RVA: 0x0020DE6F File Offset: 0x0020C06F
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06005919 RID: 22809 RVA: 0x0020DE72 File Offset: 0x0020C072
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !MechanitorUtility.IsMechanitor(pawn);
		}

		// Token: 0x0600591A RID: 22810 RVA: 0x0020DE80 File Offset: 0x0020C080
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckBiotech("Repair mech"))
			{
				return false;
			}
			Pawn pawn2 = (Pawn)t;
			CompMechRepairable compMechRepairable = t.TryGetComp<CompMechRepairable>();
			return compMechRepairable != null && pawn2.RaceProps.IsMechanoid && !pawn2.InAggroMentalState && !pawn2.HostileTo(pawn) && pawn.CanReserve(t, 1, -1, null, forced) && !pawn2.IsBurning() && !pawn2.IsAttacking() && pawn2.needs.energy != null && MechRepairUtility.CanRepair(pawn2) && (forced || compMechRepairable.autoRepair);
		}

		// Token: 0x0600591B RID: 22811 RVA: 0x0020DF1C File Offset: 0x0020C11C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RepairMech, t);
		}
	}
}
