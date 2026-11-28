using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BFE RID: 3070
	public class WorkGiver_DeepDrill : WorkGiver_Scanner
	{
		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x060057BA RID: 22458 RVA: 0x00207796 File Offset: 0x00205996
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.DeepDrill);
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x060057BB RID: 22459 RVA: 0x002077A2 File Offset: 0x002059A2
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x060057BC RID: 22460 RVA: 0x002077A5 File Offset: 0x002059A5
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060057BD RID: 22461 RVA: 0x002077A8 File Offset: 0x002059A8
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def == ThingDefOf.DeepDrill)
				{
					CompPowerTrader comp = building.GetComp<CompPowerTrader>();
					if ((comp == null || comp.PowerOn) && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Uninstall) == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060057BE RID: 22462 RVA: 0x0020781C File Offset: 0x00205A1C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Building building = t as Building;
			return building != null && pawn.CanReserve(building, 1, -1, null, forced) && building.TryGetComp<CompDeepDrill>().CanDrillNow() && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Uninstall) == null && !building.IsBurning();
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x0020788D File Offset: 0x00205A8D
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.OperateDeepDrill, t, 1500, true);
		}
	}
}
