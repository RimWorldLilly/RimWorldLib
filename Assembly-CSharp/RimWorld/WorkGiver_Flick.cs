using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C51 RID: 3153
	public class WorkGiver_Flick : WorkGiver_Scanner
	{
		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06005982 RID: 22914 RVA: 0x0020ED99 File Offset: 0x0020CF99
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005983 RID: 22915 RVA: 0x0020ED9C File Offset: 0x0020CF9C
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06005984 RID: 22916 RVA: 0x0020ED9F File Offset: 0x0020CF9F
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_Flick.<PotentialWorkThingsGlobal>d__3 <PotentialWorkThingsGlobal>d__ = new WorkGiver_Flick.<PotentialWorkThingsGlobal>d__3(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x06005985 RID: 22917 RVA: 0x0020EDAF File Offset: 0x0020CFAF
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Flick);
		}

		// Token: 0x06005986 RID: 22918 RVA: 0x0020EDC9 File Offset: 0x0020CFC9
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06005987 RID: 22919 RVA: 0x0020EDFA File Offset: 0x0020CFFA
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Flick, t);
		}
	}
}
