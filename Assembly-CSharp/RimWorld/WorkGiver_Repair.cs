using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C39 RID: 3129
	public class WorkGiver_Repair : WorkGiver_Scanner
	{
		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x0600590B RID: 22795 RVA: 0x0020DC73 File Offset: 0x0020BE73
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x0600590C RID: 22796 RVA: 0x0020DC7C File Offset: 0x0020BE7C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600590D RID: 22797 RVA: 0x0020DC7F File Offset: 0x0020BE7F
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600590E RID: 22798 RVA: 0x0020DC82 File Offset: 0x0020BE82
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction);
		}

		// Token: 0x0600590F RID: 22799 RVA: 0x0020DC9A File Offset: 0x0020BE9A
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction).Count == 0;
		}

		// Token: 0x06005910 RID: 22800 RVA: 0x0020DCBC File Offset: 0x0020BEBC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!RepairUtility.PawnCanRepairNow(pawn, t))
			{
				return false;
			}
			Building building = t as Building;
			if (pawn.Faction == Faction.OfPlayer && !pawn.Map.areaManager.Home[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
				return false;
			}
			return pawn.CanReserve(building, 1, -1, null, forced) && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) == null && (!building.def.mineable || building.Map.designationManager.DesignationAt(building.Position, DesignationDefOf.Mine) == null) && (!building.def.mineable || building.Map.designationManager.DesignationAt(building.Position, DesignationDefOf.MineVein) == null) && !building.IsBurning();
		}

		// Token: 0x06005911 RID: 22801 RVA: 0x0020DDA3 File Offset: 0x0020BFA3
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Repair, t);
		}
	}
}
