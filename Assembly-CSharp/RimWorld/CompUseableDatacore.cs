using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020019D3 RID: 6611
	public class CompUseableDatacore : CompUsable
	{
		// Token: 0x0600A66C RID: 42604 RVA: 0x003B5908 File Offset: 0x003B3B08
		public override LocalTargetInfo GetExtraTarget(Pawn pawn)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.ResearchBench), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing thing) => pawn.CanReserve(thing, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x0600A66D RID: 42605 RVA: 0x003B5974 File Offset: 0x003B3B74
		public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
		{
			AcceptanceReport result = base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
			if (!result.Accepted)
			{
				return result;
			}
			if (!this.GetExtraTarget(p).HasThing)
			{
				return "NoResearchBench".Translate();
			}
			return true;
		}

		// Token: 0x0600A66E RID: 42606 RVA: 0x003B59BD File Offset: 0x003B3BBD
		public override void UsedBy(Pawn p)
		{
			base.UsedBy(p);
			Find.History.Notify_MechanoidDatacoreReadOrLost();
		}

		// Token: 0x0600A66F RID: 42607 RVA: 0x003B59D0 File Offset: 0x003B3BD0
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			Find.History.Notify_MechanoidDatacoreReadOrLost();
		}

		// Token: 0x0600A670 RID: 42608 RVA: 0x003B59E4 File Offset: 0x003B3BE4
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			base.Notify_AbandonedAtTile(tile);
			Find.History.Notify_MechanoidDatacoreReadOrLost();
		}
	}
}
