using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B6B RID: 2923
	public class JobGiver_TakeCountToInventory : ThinkNode_JobGiver
	{
		// Token: 0x0600551E RID: 21790 RVA: 0x001FA764 File Offset: 0x001F8964
		protected override Job TryGiveJob(Pawn pawn)
		{
			int toTake = Math.Max(this.count - pawn.inventory.Count(this.def), 0);
			if (toTake == 0)
			{
				return null;
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(this.def), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => x.stackCount >= toTake && !x.IsForbidden(pawn) && pawn.CanReserve(x, 10, toTake, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.TakeCountToInventory, thing);
				job.count = toTake;
				return job;
			}
			return null;
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x001FA824 File Offset: 0x001F8A24
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_TakeCountToInventory jobGiver_TakeCountToInventory = (JobGiver_TakeCountToInventory)base.DeepCopy(resolve);
			jobGiver_TakeCountToInventory.def = this.def;
			jobGiver_TakeCountToInventory.count = this.count;
			return jobGiver_TakeCountToInventory;
		}

		// Token: 0x040032C1 RID: 12993
		public ThingDef def;

		// Token: 0x040032C2 RID: 12994
		public int count = 1;
	}
}
