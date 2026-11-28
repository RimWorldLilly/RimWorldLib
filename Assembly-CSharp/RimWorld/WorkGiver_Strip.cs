using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C3E RID: 3134
	public class WorkGiver_Strip : WorkGiver_Scanner
	{
		// Token: 0x0600592C RID: 22828 RVA: 0x0020E14C File Offset: 0x0020C34C
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_Strip.<PotentialWorkThingsGlobal>d__0 <PotentialWorkThingsGlobal>d__ = new WorkGiver_Strip.<PotentialWorkThingsGlobal>d__0(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x0600592D RID: 22829 RVA: 0x0020E15C File Offset: 0x0020C35C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x0600592E RID: 22830 RVA: 0x0020E15F File Offset: 0x0020C35F
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x0020E162 File Offset: 0x0020C362
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Strip);
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x0020E17C File Offset: 0x0020C37C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return t.Map.designationManager.DesignationOn(t, DesignationDefOf.Strip) != null && pawn.CanReserve(t, 1, -1, null, forced) && StrippableUtility.CanBeStrippedByColony(t);
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x0020E1B7 File Offset: 0x0020C3B7
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Strip, t);
		}
	}
}
