using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C01 RID: 3073
	public abstract class WorkGiver_EnterBuilding : WorkGiver_Scanner
	{
		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x060057E0 RID: 22496 RVA: 0x00208F05 File Offset: 0x00207105
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x060057E1 RID: 22497 RVA: 0x00208F08 File Offset: 0x00207108
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (pawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
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
			if (building_Enterable.SelectedPawn != pawn)
			{
				return false;
			}
			AcceptanceReport acceptanceReport = building_Enterable.CanAcceptPawn(pawn);
			if (!acceptanceReport.Accepted)
			{
				if (!acceptanceReport.Reason.NullOrEmpty())
				{
					JobFailReason.Is(acceptanceReport.Reason.CapitalizeFirst(), null);
				}
				return false;
			}
			return true;
		}

		// Token: 0x060057E2 RID: 22498 RVA: 0x00208F98 File Offset: 0x00207198
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!(t is Building_Enterable))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.EnterBuilding, t);
		}
	}
}
