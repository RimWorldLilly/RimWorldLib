using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B6C RID: 2924
	public class JobGiver_TakeForInventoryStock : ThinkNode_JobGiver
	{
		// Token: 0x06005521 RID: 21793 RVA: 0x001FA85C File Offset: 0x001F8A5C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Find.TickManager.TicksGame < pawn.mindState.nextInventoryStockTick)
			{
				return null;
			}
			if (!pawn.inventoryStock.AnyThingsRequiredNow())
			{
				return null;
			}
			if (pawn.inventory.UnloadEverything)
			{
				return null;
			}
			foreach (InventoryStockEntry inventoryStockEntry in pawn.inventoryStock.stockEntries.Values)
			{
				if (pawn.inventory.Count(inventoryStockEntry.thingDef) < inventoryStockEntry.count)
				{
					Thing thing = this.FindThingFor(pawn, inventoryStockEntry.thingDef);
					if (thing != null)
					{
						Job job = JobMaker.MakeJob(JobDefOf.TakeCountToInventory, thing);
						int b = inventoryStockEntry.count - pawn.inventory.Count(thing.def);
						job.count = Mathf.Min(thing.stackCount, b);
						return job;
					}
				}
			}
			pawn.mindState.nextInventoryStockTick = Find.TickManager.TicksGame + Rand.Range(6000, 9000);
			return null;
		}

		// Token: 0x06005522 RID: 21794 RVA: 0x001FA97C File Offset: 0x001F8B7C
		private Thing FindThingFor(Pawn pawn, ThingDef thingDef)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(thingDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => this.ThingValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06005523 RID: 21795 RVA: 0x001FA9E9 File Offset: 0x001F8BE9
		private bool ThingValidator(Pawn pawn, Thing thing)
		{
			return !thing.IsForbidden(pawn) && pawn.CanReserve(thing, 10, -1, null, false);
		}

		// Token: 0x040032C3 RID: 12995
		private const int InventoryStockCheckIntervalMin = 6000;

		// Token: 0x040032C4 RID: 12996
		private const int InventoryStockCheckIntervalMax = 9000;
	}
}
