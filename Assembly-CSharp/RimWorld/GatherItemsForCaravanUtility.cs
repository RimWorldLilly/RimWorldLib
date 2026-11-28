using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AEE RID: 2798
	public static class GatherItemsForCaravanUtility
	{
		// Token: 0x0600537D RID: 21373 RVA: 0x001F171C File Offset: 0x001EF91C
		public static Thing FindThingToHaul(Pawn p, Lord lord)
		{
			GatherItemsForCaravanUtility.neededItems.Clear();
			List<TransferableOneWay> transferables = ((LordJob_FormAndSendCaravan)lord.LordJob).transferables;
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (GatherItemsForCaravanUtility.CountLeftToTransfer(p, transferableOneWay, lord) > 0)
				{
					for (int j = 0; j < transferableOneWay.things.Count; j++)
					{
						GatherItemsForCaravanUtility.neededItems.Add(transferableOneWay.things[j]);
					}
				}
			}
			if (!GatherItemsForCaravanUtility.neededItems.Any<Thing>())
			{
				return null;
			}
			Thing result = GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.Touch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => GatherItemsForCaravanUtility.neededItems.Contains(x) && p.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, true);
			GatherItemsForCaravanUtility.neededItems.Clear();
			return result;
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x001F1810 File Offset: 0x001EFA10
		public static int CountLeftToTransfer(Pawn pawn, TransferableOneWay transferable, Lord lord)
		{
			if (transferable.CountToTransfer <= 0 || !transferable.HasAnyThing)
			{
				return 0;
			}
			return Mathf.Max(transferable.CountToTransfer - GatherItemsForCaravanUtility.TransferableCountHauledByOthers(pawn, transferable, lord), 0);
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x001F183C File Offset: 0x001EFA3C
		private static int TransferableCountHauledByOthers(Pawn pawn, TransferableOneWay transferable, Lord lord)
		{
			if (!transferable.HasAnyThing)
			{
				Log.Warning("Can't determine transferable count hauled by others because transferable has 0 things.");
				return 0;
			}
			IReadOnlyList<Pawn> allPawnsSpawned = lord.Map.mapPawns.AllPawnsSpawned;
			int num = 0;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if (pawn2 != pawn && pawn2.CurJob != null && pawn2.CurJob.def == JobDefOf.PrepareCaravan_GatherItems && pawn2.CurJob.lord == lord)
				{
					Thing toHaul = ((JobDriver_PrepareCaravan_GatherItems)pawn2.jobs.curDriver).ToHaul;
					if (transferable.things.Contains(toHaul) || TransferableUtility.TransferAsOne(transferable.AnyThing, toHaul, TransferAsOneMode.PodsOrCaravanPacking))
					{
						num += toHaul.stackCount;
					}
				}
			}
			return num;
		}

		// Token: 0x04003226 RID: 12838
		private static readonly HashSet<Thing> neededItems = new HashSet<Thing>();
	}
}
