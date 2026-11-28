using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AB6 RID: 2742
	public class JobGiver_LayEgg : ThinkNode_JobGiver
	{
		// Token: 0x0600528A RID: 21130 RVA: 0x001EBFA8 File Offset: 0x001EA1A8
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompEggLayer compEggLayer = pawn.TryGetComp<CompEggLayer>();
			if (compEggLayer == null || !compEggLayer.CanLayNow)
			{
				return null;
			}
			ThingDef singleDef = compEggLayer.NextEggType();
			PathEndMode peMode = PathEndMode.OnCell;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false, false, false, true);
			if (pawn.Faction == Faction.OfPlayer)
			{
				Thing bestEggBox = this.GetBestEggBox(pawn, peMode, traverseParms);
				if (bestEggBox != null)
				{
					return JobMaker.MakeJob(JobDefOf.LayEgg, bestEggBox);
				}
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(singleDef), peMode, traverseParms, 30f, (Thing x) => pawn.GetRoom(RegionType.Set_All) == null || x.GetRoom(RegionType.Set_All) == pawn.GetRoom(RegionType.Set_All), null, 0, -1, false, RegionType.Set_Passable, false, false);
			return JobMaker.MakeJob(JobDefOf.LayEgg, (thing != null) ? thing.Position : RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 5f, null, Danger.Some, false));
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x001EC0AC File Offset: 0x001EA2AC
		private Thing GetBestEggBox(Pawn pawn, PathEndMode peMode, TraverseParms tp)
		{
			JobGiver_LayEgg.<>c__DisplayClass4_0 CS$<>8__locals1 = new JobGiver_LayEgg.<>c__DisplayClass4_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.eggDef = CS$<>8__locals1.pawn.TryGetComp<CompEggLayer>().NextEggType();
			return GenClosest.ClosestThing_Regionwise_ReachablePrioritized(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForDef(ThingDefOf.EggBox), peMode, tp, 30f, new Predicate<Thing>(CS$<>8__locals1.<GetBestEggBox>g__IsUsableBox|0), new Func<Thing, float>(JobGiver_LayEgg.<GetBestEggBox>g__GetScore|4_1), 10, 30, false);
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x001EC130 File Offset: 0x001EA330
		[CompilerGenerated]
		internal static float <GetBestEggBox>g__GetScore|4_1(Thing thing)
		{
			CompEggContainer compEggContainer = thing.TryGetComp<CompEggContainer>();
			if (compEggContainer == null || compEggContainer.Full)
			{
				return 0f;
			}
			Thing containedThing = compEggContainer.ContainedThing;
			int? num = (containedThing != null) ? new int?(containedThing.stackCount) : null;
			return ((num != null) ? new float?((float)num.GetValueOrDefault() * 5f) : null).GetValueOrDefault(0.5f);
		}

		// Token: 0x040031D9 RID: 12761
		private const float LayRadius = 5f;

		// Token: 0x040031DA RID: 12762
		private const float MaxSearchRadius = 30f;

		// Token: 0x040031DB RID: 12763
		private const int MinSearchRegions = 10;
	}
}
