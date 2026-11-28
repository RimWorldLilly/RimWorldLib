using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B9C RID: 2972
	internal class JobGiver_FireStartingSpree : ThinkNode_JobGiver
	{
		// Token: 0x06005603 RID: 22019 RVA: 0x001FFDBF File Offset: 0x001FDFBF
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_FireStartingSpree jobGiver_FireStartingSpree = (JobGiver_FireStartingSpree)base.DeepCopy(resolve);
			jobGiver_FireStartingSpree.waitTicks = this.waitTicks;
			return jobGiver_FireStartingSpree;
		}

		// Token: 0x06005604 RID: 22020 RVA: 0x001FFDDC File Offset: 0x001FDFDC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.mindState.nextMoveOrderIsWait)
			{
				Job job = JobMaker.MakeJob(JobDefOf.Wait_Wander);
				job.expiryInterval = this.waitTicks.RandomInRange;
				pawn.mindState.nextMoveOrderIsWait = false;
				return job;
			}
			if (Rand.Value < 0.75f)
			{
				Thing thing = this.TryFindRandomIgniteTarget(pawn);
				if (thing != null)
				{
					pawn.mindState.nextMoveOrderIsWait = true;
					return JobMaker.MakeJob(JobDefOf.Ignite, thing);
				}
			}
			IntVec3 c = RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 10f, null, Danger.Deadly, false);
			if (c.IsValid)
			{
				pawn.mindState.nextMoveOrderIsWait = true;
				return JobMaker.MakeJob(JobDefOf.GotoWander, c);
			}
			return null;
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x001FFE90 File Offset: 0x001FE090
		private Thing TryFindRandomIgniteTarget(Pawn pawn)
		{
			Region region;
			if (!CellFinder.TryFindClosestRegionWith(pawn.GetRegion(RegionType.Set_Passable), TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), (Region candidateRegion) => !candidateRegion.IsForbiddenEntirely(pawn), 100, out region, RegionType.Set_Passable))
			{
				return null;
			}
			JobGiver_FireStartingSpree.potentialTargets.Clear();
			List<Thing> allThings = region.ListerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && thing.FlammableNow && !thing.IsBurning() && !thing.OccupiedRect().Contains(pawn.Position))
				{
					JobGiver_FireStartingSpree.potentialTargets.Add(thing);
				}
			}
			if (JobGiver_FireStartingSpree.potentialTargets.NullOrEmpty<Thing>())
			{
				return null;
			}
			return JobGiver_FireStartingSpree.potentialTargets.RandomElement<Thing>();
		}

		// Token: 0x04003304 RID: 13060
		private IntRange waitTicks = new IntRange(80, 140);

		// Token: 0x04003305 RID: 13061
		private const float FireStartChance = 0.75f;

		// Token: 0x04003306 RID: 13062
		private static List<Thing> potentialTargets = new List<Thing>();
	}
}
