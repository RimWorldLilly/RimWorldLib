using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C00 RID: 3072
	public class WorkGiver_EjectFuel : WorkGiver_Scanner
	{
		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x060057DA RID: 22490 RVA: 0x00208E57 File Offset: 0x00207057
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060057DB RID: 22491 RVA: 0x00208E5A File Offset: 0x0020705A
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_EjectFuel.<PotentialWorkThingsGlobal>d__2 <PotentialWorkThingsGlobal>d__ = new WorkGiver_EjectFuel.<PotentialWorkThingsGlobal>d__2(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x00208E6A File Offset: 0x0020706A
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.EjectFuel);
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x00208E84 File Offset: 0x00207084
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.EjectFuel) == null)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			CompRefuelable compRefuelable;
			if (!t.TryGetComp(out compRefuelable))
			{
				return false;
			}
			AcceptanceReport acceptanceReport = compRefuelable.CanEjectFuel();
			if (!acceptanceReport.Accepted)
			{
				JobFailReason.Is(acceptanceReport.Reason, null);
				return false;
			}
			return true;
		}

		// Token: 0x060057DE RID: 22494 RVA: 0x00208EEB File Offset: 0x002070EB
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.EjectFuel, t);
		}
	}
}
