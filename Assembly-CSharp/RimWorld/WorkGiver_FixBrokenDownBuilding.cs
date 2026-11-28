using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C10 RID: 3088
	public class WorkGiver_FixBrokenDownBuilding : WorkGiver_Scanner
	{
		// Token: 0x06005828 RID: 22568 RVA: 0x00209EFC File Offset: 0x002080FC
		public static void ResetStaticData()
		{
			WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans = "NotInHomeArea".Translate();
			WorkGiver_FixBrokenDownBuilding.NoComponentsToRepairTrans = "NoComponentsToRepair".Translate();
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06005829 RID: 22569 RVA: 0x00209F26 File Offset: 0x00208126
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		// Token: 0x0600582A RID: 22570 RVA: 0x00209F2F File Offset: 0x0020812F
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.GetComponent<BreakdownManager>().brokenDownThings;
		}

		// Token: 0x0600582B RID: 22571 RVA: 0x00209F41 File Offset: 0x00208141
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.GetComponent<BreakdownManager>().brokenDownThings.Count == 0;
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x0600582C RID: 22572 RVA: 0x00209F5B File Offset: 0x0020815B
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600582D RID: 22573 RVA: 0x00209F5E File Offset: 0x0020815E
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600582E RID: 22574 RVA: 0x00209F64 File Offset: 0x00208164
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t as Building;
			if (building == null)
			{
				return false;
			}
			if (!building.def.building.repairable)
			{
				return false;
			}
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			if (!t.IsBrokenDown())
			{
				return false;
			}
			if (pawn.Faction == Faction.OfPlayer && !pawn.Map.areaManager.Home[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
				return false;
			}
			if (!pawn.CanReserve(building, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			if (building.IsBurning())
			{
				return false;
			}
			if (this.FindClosestComponent(pawn) == null)
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NoComponentsToRepairTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x0600582F RID: 22575 RVA: 0x0020A034 File Offset: 0x00208234
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Thing t2 = this.FindClosestComponent(pawn);
			Job job = JobMaker.MakeJob(JobDefOf.FixBrokenDownBuilding, t, t2);
			job.count = 1;
			return job;
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x0020A068 File Offset: 0x00208268
		private Thing FindClosestComponent(Pawn pawn)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.ComponentIndustrial), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x04003349 RID: 13129
		public static string NotInHomeAreaTrans;

		// Token: 0x0400334A RID: 13130
		private static string NoComponentsToRepairTrans;
	}
}
