using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B1F RID: 2847
	public abstract class JobGiver_ManTurrets : ThinkNode_JobGiver
	{
		// Token: 0x06005411 RID: 21521 RVA: 0x001F46C4 File Offset: 0x001F28C4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_ManTurrets jobGiver_ManTurrets = (JobGiver_ManTurrets)base.DeepCopy(resolve);
			jobGiver_ManTurrets.maxDistFromPoint = this.maxDistFromPoint;
			return jobGiver_ManTurrets;
		}

		// Token: 0x06005412 RID: 21522 RVA: 0x001F46E0 File Offset: 0x001F28E0
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_ManTurrets.<>c__DisplayClass2_0 CS$<>8__locals1 = new JobGiver_ManTurrets.<>c__DisplayClass2_0();
			CS$<>8__locals1.pawn = pawn;
			Thing thing = GenClosest.ClosestThingReachable(this.GetRoot(CS$<>8__locals1.pawn), CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.InteractionCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), this.maxDistFromPoint, new Predicate<Thing>(CS$<>8__locals1.<TryGiveJob>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.ManTurret, thing);
				job.expiryInterval = 2000;
				job.checkOverrideOnExpire = true;
				return job;
			}
			return null;
		}

		// Token: 0x06005413 RID: 21523
		protected abstract IntVec3 GetRoot(Pawn pawn);

		// Token: 0x04003257 RID: 12887
		public float maxDistFromPoint = -1f;
	}
}
