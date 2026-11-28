using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D05 RID: 3333
	public static class GiveItemsToPawnUtility
	{
		// Token: 0x06005DDC RID: 24028 RVA: 0x00225C28 File Offset: 0x00223E28
		public static Thing FindItemToGive(Pawn hauler, ThingDef thingDef)
		{
			return GenClosest.ClosestThingReachable(hauler.Position, hauler.Map, ThingRequest.ForDef(thingDef), PathEndMode.Touch, TraverseParms.For(hauler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(hauler) && hauler.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06005DDD RID: 24029 RVA: 0x00225C90 File Offset: 0x00223E90
		public static bool IsWaitingForItems(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.CurLordToil is IWaitForItemsLordToil;
		}

		// Token: 0x06005DDE RID: 24030 RVA: 0x00225CB7 File Offset: 0x00223EB7
		public static int ItemCountWaitingFor(Pawn pawn)
		{
			if (!GiveItemsToPawnUtility.IsWaitingForItems(pawn))
			{
				return -1;
			}
			return ((IWaitForItemsLordToil)pawn.GetLord().CurLordToil).CountRemaining;
		}

		// Token: 0x06005DDF RID: 24031 RVA: 0x00225CD8 File Offset: 0x00223ED8
		public static int ItemCountBeingHauled(Pawn requester)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = requester.Map.mapPawns.AllPawnsSpawned;
			Lord lord = requester.GetLord();
			int num = 0;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if (pawn.CurJob != null)
				{
					JobDriver_GiveToPawn jobDriver_GiveToPawn = pawn.jobs.curDriver as JobDriver_GiveToPawn;
					if (jobDriver_GiveToPawn != null && jobDriver_GiveToPawn.job.lord == lord)
					{
						num += jobDriver_GiveToPawn.CountBeingHauled;
					}
				}
			}
			return num;
		}

		// Token: 0x06005DE0 RID: 24032 RVA: 0x00225D53 File Offset: 0x00223F53
		public static int ItemCountLeftToCollect(Pawn requester)
		{
			return GiveItemsToPawnUtility.ItemCountWaitingFor(requester) - GiveItemsToPawnUtility.ItemCountBeingHauled(requester);
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x00225D62 File Offset: 0x00223F62
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptionsForPawn(Pawn requester, Pawn current, ThingDef requestedThingDef, int countRequired)
		{
			GiveItemsToPawnUtility.<GetFloatMenuOptionsForPawn>d__5 <GetFloatMenuOptionsForPawn>d__ = new GiveItemsToPawnUtility.<GetFloatMenuOptionsForPawn>d__5(-2);
			<GetFloatMenuOptionsForPawn>d__.<>3__requester = requester;
			<GetFloatMenuOptionsForPawn>d__.<>3__current = current;
			<GetFloatMenuOptionsForPawn>d__.<>3__requestedThingDef = requestedThingDef;
			<GetFloatMenuOptionsForPawn>d__.<>3__countRequired = countRequired;
			return <GetFloatMenuOptionsForPawn>d__;
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x00225D88 File Offset: 0x00223F88
		public static int GetCountRemaining(Pawn pawn, ThingDef requestedThingDef, int requestedThingCount)
		{
			int num = 0;
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i].def == requestedThingDef)
				{
					num += innerContainer[i].stackCount;
				}
			}
			return requestedThingCount - num;
		}
	}
}
