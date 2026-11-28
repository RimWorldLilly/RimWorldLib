using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Utility;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BDA RID: 3034
	public class JobGiver_Reload : ThinkNode_JobGiver
	{
		// Token: 0x0600572E RID: 22318 RVA: 0x002052D8 File Offset: 0x002034D8
		public override float GetPriority(Pawn pawn)
		{
			return 5.9f;
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x002052E0 File Offset: 0x002034E0
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			IReloadableComp reloadableComp = ReloadableUtility.FindSomeReloadableComponent(pawn, false);
			if (reloadableComp == null)
			{
				return null;
			}
			if (pawn.carryTracker.AvailableStackSpace(reloadableComp.AmmoDef) < reloadableComp.MinAmmoNeeded(true))
			{
				return null;
			}
			List<Thing> list = ReloadableUtility.FindEnoughAmmo(pawn, pawn.Position, reloadableComp, false);
			if (list.NullOrEmpty<Thing>())
			{
				return null;
			}
			return JobGiver_Reload.MakeReloadJob(reloadableComp, list);
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x00205350 File Offset: 0x00203550
		public static Job MakeReloadJob(IReloadableComp reloadable, List<Thing> chosenAmmo)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Reload, reloadable.ReloadableThing);
			job.targetQueueB = (from t in chosenAmmo
			select new LocalTargetInfo(t)).ToList<LocalTargetInfo>();
			job.count = chosenAmmo.Sum((Thing t) => t.stackCount);
			job.count = Math.Min(job.count, reloadable.MaxAmmoNeeded(true));
			return job;
		}

		// Token: 0x0400332E RID: 13102
		private const bool ForceReloadWhenLookingForWork = false;
	}
}
