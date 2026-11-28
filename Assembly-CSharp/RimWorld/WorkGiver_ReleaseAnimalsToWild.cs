using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BB7 RID: 2999
	public class WorkGiver_ReleaseAnimalsToWild : WorkGiver_Scanner
	{
		// Token: 0x06005681 RID: 22145 RVA: 0x002027AE File Offset: 0x002009AE
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_ReleaseAnimalsToWild.<PotentialWorkThingsGlobal>d__0 <PotentialWorkThingsGlobal>d__ = new WorkGiver_ReleaseAnimalsToWild.<PotentialWorkThingsGlobal>d__0(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06005682 RID: 22146 RVA: 0x002027BE File Offset: 0x002009BE
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06005683 RID: 22147 RVA: 0x002027C1 File Offset: 0x002009C1
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.ReleaseAnimalToWild);
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x002027DC File Offset: 0x002009DC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.IsAnimal)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild) == null)
			{
				return false;
			}
			if (pawn.Faction != t.Faction)
			{
				return false;
			}
			if (pawn2.InAggroMentalState || pawn2.Dead)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			IntVec3 intVec;
			if (!JobDriver_ReleaseAnimalToWild.TryFindClosestOutsideCell(t.Position, t.Map, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), pawn, out intVec))
			{
				JobFailReason.Is("NoReachableOutsideCell".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x00202887 File Offset: 0x00200A87
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ReleaseAnimalToWild, t);
			job.count = 1;
			return job;
		}
	}
}
