using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C28 RID: 3112
	public class WorkGiver_Open : WorkGiver_Scanner
	{
		// Token: 0x060058A8 RID: 22696 RVA: 0x0020C273 File Offset: 0x0020A473
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_Open.<PotentialWorkThingsGlobal>d__0 <PotentialWorkThingsGlobal>d__ = new WorkGiver_Open.<PotentialWorkThingsGlobal>d__0(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x060058A9 RID: 22697 RVA: 0x0020C283 File Offset: 0x0020A483
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Open);
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x060058AA RID: 22698 RVA: 0x0020C29D File Offset: 0x0020A49D
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x0020C2A0 File Offset: 0x0020A4A0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Open) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x0020C2D1 File Offset: 0x0020A4D1
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Open, t);
		}
	}
}
