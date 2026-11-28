using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	// Token: 0x02002388 RID: 9096
	[StaticConstructorOnStartup]
	public static class CaravanFormingUtility
	{
		// Token: 0x0600E226 RID: 57894 RVA: 0x0053EB15 File Offset: 0x0053CD15
		public static void FormAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, PlanetTile exitFromTile, PlanetTile directionTile, PlanetTile destinationTile)
		{
			CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, true);
		}

		// Token: 0x0600E227 RID: 57895 RVA: 0x0053EB24 File Offset: 0x0053CD24
		public static void StartFormingCaravan(List<Pawn> pawns, List<Pawn> downedPawns, Faction faction, List<TransferableOneWay> transferables, IntVec3 meetingPoint, IntVec3 exitSpot, PlanetTile startingTile, PlanetTile destinationTile)
		{
			if (!startingTile.Valid)
			{
				Log.Error("Can't start forming caravan because startingTile is invalid.");
				return;
			}
			if (!pawns.Any<Pawn>())
			{
				Log.Error("Can't start forming caravan with 0 pawns.");
				return;
			}
			if (pawns.Any((Pawn x) => x.Downed))
			{
				Log.Warning("Forming a caravan with a downed pawn. This shouldn't happen because we have to create a Lord.");
			}
			List<TransferableOneWay> list = transferables.ToList<TransferableOneWay>();
			list.RemoveAll((TransferableOneWay x) => x.CountToTransfer <= 0 || !x.HasAnyThing || x.AnyThing is Pawn);
			for (int i = 0; i < pawns.Count; i++)
			{
				Lord lord = pawns[i].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawns[i], PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordJob_FormAndSendCaravan lordJob = new LordJob_FormAndSendCaravan(list, downedPawns, meetingPoint, exitSpot, startingTile, destinationTile);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, pawns[0].MapHeld, pawns);
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn = pawns[j];
				if (pawn.Spawned)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x0600E228 RID: 57896 RVA: 0x0053EC54 File Offset: 0x0053CE54
		public static void StopFormingCaravan(Lord lord)
		{
			CaravanFormingUtility.tmpPawnsInCancelledCaravan.Clear();
			CaravanFormingUtility.tmpPawnsInCancelledCaravan.AddRange(lord.ownedPawns);
			bool isPlayerHome = lord.Map.IsPlayerHome;
			CaravanFormingUtility.SetToUnloadEverything(lord);
			lord.lordManager.RemoveLord(lord);
			if (isPlayerHome)
			{
				CaravanFormingUtility.LeadAnimalsToPen(CaravanFormingUtility.tmpPawnsInCancelledCaravan);
			}
			CaravanFormingUtility.tmpPawnsInCancelledCaravan.Clear();
		}

		// Token: 0x0600E229 RID: 57897 RVA: 0x0053ECB0 File Offset: 0x0053CEB0
		public static void LeadAnimalsToPen(List<Pawn> pawns)
		{
			CaravanFormingUtility.tmpRopers.Clear();
			CaravanFormingUtility.tmpNeedRoping.Clear();
			foreach (Pawn pawn in pawns)
			{
				if (pawn.Spawned && !pawn.Downed && !pawn.Dead && !pawn.Drafted)
				{
					if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
					{
						CaravanFormingUtility.tmpNeedRoping.Add(pawn);
					}
					else if (pawn.IsColonist)
					{
						CaravanFormingUtility.tmpRopers.Add(pawn);
					}
				}
			}
			if (CaravanFormingUtility.tmpNeedRoping.Any<Pawn>() && CaravanFormingUtility.tmpRopers.Any<Pawn>())
			{
				using (List<Pawn>.Enumerator enumerator = CaravanFormingUtility.tmpNeedRoping.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn ropee = enumerator.Current;
						if (!ropee.roping.IsRoped)
						{
							CaravanFormingUtility.tmpRopers.MinBy((Pawn p) => p.Position.DistanceToSquared(ropee.Position)).roping.RopePawn(ropee);
						}
					}
				}
				CaravanFormingUtility.StartReturnedLord((from p in CaravanFormingUtility.tmpRopers
				where p.roping.IsRopingOthers
				select p).Concat(CaravanFormingUtility.tmpNeedRoping).ToList<Pawn>());
			}
			CaravanFormingUtility.tmpRopers.Clear();
			CaravanFormingUtility.tmpNeedRoping.Clear();
		}

		// Token: 0x0600E22A RID: 57898 RVA: 0x0053EE40 File Offset: 0x0053D040
		private static void StartReturnedLord(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Lord lord = pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordJob_ReturnedCaravan lordJob = new LordJob_ReturnedCaravan(pawns[0].Position);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, pawns[0].MapHeld, pawns);
			foreach (Pawn pawn2 in pawns)
			{
				if (pawn2.Spawned)
				{
					pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x0600E22B RID: 57899 RVA: 0x0053EF20 File Offset: 0x0053D120
		public static void RemovePawnFromCaravan(Pawn pawn, Lord lord, bool removeFromDowned = true)
		{
			bool flag = false;
			pawn.inventory.DropAllPackingCaravanThings();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn2 = lord.ownedPawns[i];
				if (pawn2 != pawn && CaravanUtility.IsOwner(pawn2, Faction.OfPlayer))
				{
					flag = true;
					break;
				}
			}
			bool flag2 = true;
			TaggedString taggedString = "MessagePawnLostWhileFormingCaravan".Translate(pawn).CapitalizeFirst();
			if (!flag)
			{
				CaravanFormingUtility.StopFormingCaravan(lord);
				taggedString += " " + "MessagePawnLostWhileFormingCaravan_AllLost".Translate();
			}
			else
			{
				pawn.inventory.UnloadEverything = true;
				if (lord.ownedPawns.Contains(pawn))
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedByPlayerAction, null);
					flag2 = false;
				}
				LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lord.LordJob as LordJob_FormAndSendCaravan;
				if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.downedPawns.Contains(pawn))
				{
					if (!removeFromDowned)
					{
						flag2 = false;
					}
					else
					{
						lordJob_FormAndSendCaravan.downedPawns.Remove(pawn);
					}
				}
				if (pawn.jobs != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			}
			if (flag2)
			{
				Messages.Message(taggedString, pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0600E22C RID: 57900 RVA: 0x0053F04C File Offset: 0x0053D24C
		public static void ForceCaravanDepart(Lord lord)
		{
			LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = (LordJob_FormAndSendCaravan)lord.LordJob;
			for (int i = lord.ownedPawns.Count - 1; i >= 0; i--)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn.InMentalState)
				{
					lord.RemovePawn(pawn);
					pawn.inventory.UnloadEverything = true;
				}
			}
			if (lord.ownedPawns.Count == 0)
			{
				lord.Map.lordManager.RemoveLord(lord);
				Messages.Message("MessageForceSendingCaravan_AllLost".Translate(), MessageTypeDefOf.NeutralEvent, true);
				return;
			}
			for (int j = lord.ownedPawns.Count - 1; j >= 0; j--)
			{
				Pawn pawn2 = lord.ownedPawns[j];
				if (pawn2.Drafted)
				{
					pawn2.drafter.Drafted = false;
				}
				else if (pawn2.roping.RopedToSpot.IsValid)
				{
					pawn2.roping.UnropeFromSpot();
				}
			}
			List<Pawn> downedPawns = lordJob_FormAndSendCaravan.downedPawns;
			if (downedPawns != null)
			{
				downedPawns.Clear();
			}
			lord.ReceiveMemo("ForceDepartNow");
		}

		// Token: 0x0600E22D RID: 57901 RVA: 0x0053F15C File Offset: 0x0053D35C
		private static void SetToUnloadEverything(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				lord.ownedPawns[i].inventory.DropAllPackingCaravanThings();
				lord.ownedPawns[i].inventory.UnloadEverything = true;
			}
		}

		// Token: 0x0600E22E RID: 57902 RVA: 0x0053F1AC File Offset: 0x0053D3AC
		public static void TryAddItemBackToTransferables(Thing item, List<TransferableOneWay> transferables, int count)
		{
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(item, transferables, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay != null && !transferableOneWay.things.Contains(item))
			{
				transferableOneWay.things.Add(item);
			}
			if (transferableOneWay != null)
			{
				transferableOneWay.AdjustTo(transferableOneWay.ClampAmount(transferableOneWay.CountToTransfer + count));
			}
		}

		// Token: 0x0600E22F RID: 57903 RVA: 0x0053F1F8 File Offset: 0x0053D3F8
		public static List<Thing> AllReachableColonyItems(Map map, bool allowEvenIfOutsideHomeArea = false, bool allowEvenIfReserved = false, bool canMinify = false)
		{
			CaravanFormingUtility.<>c__DisplayClass16_0 CS$<>8__locals1 = new CaravanFormingUtility.<>c__DisplayClass16_0();
			CS$<>8__locals1.canMinify = canMinify;
			CS$<>8__locals1.allowEvenIfOutsideHomeArea = allowEvenIfOutsideHomeArea;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.allowEvenIfReserved = allowEvenIfReserved;
			CaravanFormingUtility.tmpThings.Clear();
			CS$<>8__locals1.map.listerThings.GetAllThings(CaravanFormingUtility.tmpThings, new Predicate<Thing>(CS$<>8__locals1.<AllReachableColonyItems>g__IsValidCaravanItem|0), true);
			return CaravanFormingUtility.tmpThings;
		}

		// Token: 0x0600E230 RID: 57904 RVA: 0x0053F258 File Offset: 0x0053D458
		public static List<Pawn> AllSendablePawns(Map map, bool allowEvenIfDowned = false, bool allowEvenIfInMentalState = false, bool allowEvenIfPrisonerNotSecure = false, bool allowCapturableDownedPawns = false, bool allowLodgers = false, int allowLoadAndEnterTransportersLordForGroupID = -1)
		{
			List<Pawn> list = new List<Pawn>();
			IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((allowEvenIfDowned || !pawn.Downed) && (allowEvenIfInMentalState || !pawn.InMentalState) && (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || (allowCapturableDownedPawns && CaravanFormingUtility.CanListAsAutoCapturable(pawn))) && pawn.RaceProps.allowedOnCaravan && !pawn.IsQuestHelper() && (!pawn.IsQuestLodger() || allowLodgers) && (allowEvenIfPrisonerNotSecure || !pawn.IsPrisoner || pawn.guest.PrisonerIsSecure) && (pawn.GetLord() == null || pawn.GetLord().LordJob is LordJob_VoluntarilyJoinable || pawn.GetLord().LordJob.IsCaravanSendable || (allowLoadAndEnterTransportersLordForGroupID >= 0 && pawn.GetLord().LordJob is LordJob_LoadAndEnterTransporters && ((LordJob_LoadAndEnterTransporters)pawn.GetLord().LordJob).transportersGroup == allowLoadAndEnterTransportersLordForGroupID)))
				{
					list.Add(pawn);
				}
			}
			return list;
		}

		// Token: 0x0600E231 RID: 57905 RVA: 0x0053F384 File Offset: 0x0053D584
		private static bool CanListAsAutoCapturable(Pawn p)
		{
			return p.Downed && !p.mindState.WillJoinColonyIfRescued && CaravanUtility.ShouldAutoCapture(p, Faction.OfPlayer);
		}

		// Token: 0x0600E232 RID: 57906 RVA: 0x0053F3A8 File Offset: 0x0053D5A8
		public static IEnumerable<Gizmo> GetGizmos(Pawn pawn)
		{
			CaravanFormingUtility.<GetGizmos>d__19 <GetGizmos>d__ = new CaravanFormingUtility.<GetGizmos>d__19(-2);
			<GetGizmos>d__.<>3__pawn = pawn;
			return <GetGizmos>d__;
		}

		// Token: 0x0600E233 RID: 57907 RVA: 0x0053F3B8 File Offset: 0x0053D5B8
		public static void LateJoinFormingCaravan(Pawn pawn, Lord lord)
		{
			Lord lord2 = pawn.GetLord();
			if (lord2 != null)
			{
				lord2.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
			}
			if (pawn.Downed)
			{
				((LordJob_FormAndSendCaravan)lord.LordJob).downedPawns.Add(pawn);
			}
			else
			{
				lord.AddPawn(pawn);
			}
			if (pawn.Spawned)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x0600E234 RID: 57908 RVA: 0x0053F420 File Offset: 0x0053D620
		public static bool IsFormingCaravan(this Pawn p)
		{
			Lord lord;
			return p.TryGetFormingCaravanLord(out lord);
		}

		// Token: 0x0600E235 RID: 57909 RVA: 0x0053F435 File Offset: 0x0053D635
		public static bool TryGetFormingCaravanLord(this Pawn p, out Lord lord)
		{
			lord = p.GetLord();
			return lord != null && lord.LordJob is LordJob_FormAndSendCaravan;
		}

		// Token: 0x0600E236 RID: 57910 RVA: 0x0053F454 File Offset: 0x0053D654
		public static bool IsFormingCaravanOrDownedPawnToBeTakenByCaravan(Pawn p)
		{
			return CaravanFormingUtility.GetFormAndSendCaravanLord(p) != null;
		}

		// Token: 0x0600E237 RID: 57911 RVA: 0x0053F460 File Offset: 0x0053D660
		public static Lord GetFormAndSendCaravanLord(Pawn p)
		{
			if (p.IsFormingCaravan())
			{
				return p.GetLord();
			}
			if (p.SpawnedOrAnyParentSpawned)
			{
				List<Lord> lords = p.MapHeld.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lords[i].LordJob as LordJob_FormAndSendCaravan;
					if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.downedPawns.Contains(p))
					{
						return lords[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600E238 RID: 57912 RVA: 0x0053F4D4 File Offset: 0x0053D6D4
		public static float CapacityLeft(LordJob_FormAndSendCaravan lordJob)
		{
			float num = CollectionsMassCalculator.MassUsageTransferables(lordJob.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			for (int i = 0; i < lordJob.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lordJob.lord.ownedPawns[i];
				CaravanFormingUtility.tmpCaravanPawns.Add(new ThingCount(pawn, pawn.stackCount, false));
			}
			num += CollectionsMassCalculator.MassUsage(CaravanFormingUtility.tmpCaravanPawns, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			float num2 = CollectionsMassCalculator.Capacity(CaravanFormingUtility.tmpCaravanPawns, null);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			return num2 - num;
		}

		// Token: 0x0600E239 RID: 57913 RVA: 0x0053F565 File Offset: 0x0053D765
		public static string AppendOverweightInfo(string text, float capacityLeft)
		{
			if (capacityLeft < 0f)
			{
				text += " (" + "OverweightLower".Translate() + ")";
			}
			return text;
		}

		// Token: 0x0600E23A RID: 57914 RVA: 0x0053F59C File Offset: 0x0053D79C
		public static bool AllItemsLoadedOntoCaravan(Lord lord, Map map)
		{
			bool flag = true;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn.IsColonist && pawn.mindState.lastJobTag != JobTag.WaitingForOthersToFinishGatheringItems)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
				for (int j = 0; j < allPawnsSpawned.Count; j++)
				{
					if (allPawnsSpawned[j].CurJob != null && allPawnsSpawned[j].jobs.curDriver is JobDriver_PrepareCaravan_GatherItems && allPawnsSpawned[j].CurJob.lord == lord)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600E23B RID: 57915 RVA: 0x0053F650 File Offset: 0x0053D850
		public static string GetForceDepartWarningMessage(LordJob_FormAndSendCaravan lordJob)
		{
			List<Transferable> list = new List<Transferable>();
			if (!CaravanFormingUtility.AllItemsLoadedOntoCaravan(lordJob.lord, lordJob.lord.Map))
			{
				foreach (TransferableOneWay transferableOneWay in lordJob.transferables)
				{
					if (transferableOneWay.CountToTransfer > 0)
					{
						list.Add(transferableOneWay);
					}
				}
			}
			List<ThingCount> list2 = new List<ThingCount>();
			List<Pawn> list3 = new List<Pawn>();
			foreach (Pawn pawn in lordJob.lord.ownedPawns)
			{
				if (pawn.Downed)
				{
					list3.Add(pawn);
				}
				else if (pawn.InMentalState)
				{
					list3.Add(pawn);
				}
				else
				{
					list2.Add(new ThingCount(pawn, pawn.stackCount, false));
				}
			}
			List<ThingCount> list4 = new List<ThingCount>();
			foreach (Pawn pawn2 in list3)
			{
				foreach (Thing thing in pawn2.inventory.innerContainer)
				{
					list4.Add(new ThingCount(thing, thing.stackCount, false));
				}
			}
			list4 = (from tc in list4
			group tc by tc.Thing.def into g
			select new ThingCount(g.First<ThingCount>().Thing, g.Sum((ThingCount tc) => tc.Count), true)).ToList<ThingCount>();
			TaggedString taggedString = "ConfirmForceDepartIntro".Translate();
			if (list.Count > 0)
			{
				taggedString += "\n\n" + "ConfirmForceDepartItemsStillLoading".Translate();
				taggedString += "\n\n" + (from i in list
				select i.LabelCap + " x" + i.CountToTransfer.ToString()).ToLineList("- ", false);
			}
			if (list3.Count > 0)
			{
				taggedString += "\n\n" + "ConfirmForceDepartPawnsNotLeaving".Translate();
				taggedString += "\n\n" + (from p in list3
				select p.LabelNoCountColored.ToString()).ToLineList("- ", false);
			}
			if (list4.Count > 0)
			{
				taggedString += "\n\n" + "ConfirmForceDepartItemsOnPawnsNotLeaving".Translate();
				taggedString += "\n\n" + (from i in list4
				select i.Thing.LabelCapNoCount + " x" + i.Count.ToString()).ToLineList("- ", false);
			}
			return taggedString;
		}

		// Token: 0x04009073 RID: 36979
		private static readonly Texture2D RemoveFromCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/RemoveFromCaravan", true);

		// Token: 0x04009074 RID: 36980
		private static readonly Texture2D AddToCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/AddToCaravan", true);

		// Token: 0x04009075 RID: 36981
		private static readonly Texture2D ForceDepartCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/ForceCaravanToDepart", true);

		// Token: 0x04009076 RID: 36982
		private static List<Pawn> tmpPawnsInCancelledCaravan = new List<Pawn>();

		// Token: 0x04009077 RID: 36983
		private static List<Pawn> tmpRopers = new List<Pawn>();

		// Token: 0x04009078 RID: 36984
		private static List<Pawn> tmpNeedRoping = new List<Pawn>();

		// Token: 0x04009079 RID: 36985
		private static readonly List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400907A RID: 36986
		private static List<ThingCount> tmpCaravanPawns = new List<ThingCount>();
	}
}
