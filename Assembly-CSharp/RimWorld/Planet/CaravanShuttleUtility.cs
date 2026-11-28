using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002390 RID: 9104
	public static class CaravanShuttleUtility
	{
		// Token: 0x0600E26D RID: 57965 RVA: 0x00540EF4 File Offset: 0x0053F0F4
		private static float GetCaravanShuttleMass(Caravan caravan)
		{
			int num = caravan.GetHashCode();
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				num = Gen.HashCombineInt(num, thing.GetHashCode());
			}
			foreach (Pawn pawn in caravan.pawns)
			{
				num = Gen.HashCombineInt(num, pawn.GetHashCode());
			}
			if (num == CaravanShuttleUtility.cachedCaravanItemsHash)
			{
				return CaravanShuttleUtility.cachedCaravanShuttleMass;
			}
			CaravanShuttleUtility.cachedCaravanItemsHash = num;
			CaravanShuttleUtility.cachedCaravanShuttleMass = CollectionsMassCalculator.MassUsage(caravan.pawns, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, true, false);
			CaravanShuttleUtility.cachedCaravanShuttleMass -= caravan.Shuttle.GetStatValue(StatDefOf.Mass, true, -1);
			return CaravanShuttleUtility.cachedCaravanShuttleMass;
		}

		// Token: 0x0600E26E RID: 57966 RVA: 0x00540FE8 File Offset: 0x0053F1E8
		public static float FuelInCaravan(Caravan caravan)
		{
			float num = 0f;
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				if (caravan.Shuttle.RefuelableComp.Props.fuelFilter.Allows(thing))
				{
					num += (float)thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x0600E26F RID: 57967 RVA: 0x00541064 File Offset: 0x0053F264
		public static void ConsumeFuelFromCaravanInventory(Caravan caravan, int fuelAmount)
		{
			int num = fuelAmount;
			foreach (Thing thing in CaravanInventoryUtility.AllInventoryItems(caravan))
			{
				if (caravan.Shuttle.RefuelableComp.Props.fuelFilter.Allows(thing))
				{
					Thing thing2 = thing.SplitOff(Mathf.Min(num, thing.stackCount));
					num -= thing2.stackCount;
				}
				if (num <= 0)
				{
					break;
				}
			}
		}

		// Token: 0x0600E270 RID: 57968 RVA: 0x005410F4 File Offset: 0x0053F2F4
		public static AcceptanceReport CanLaunchCaravanShuttle(Caravan caravan)
		{
			float caravanShuttleMass = CaravanShuttleUtility.GetCaravanShuttleMass(caravan);
			if (caravanShuttleMass > caravan.Shuttle.TransporterComp.MassCapacity)
			{
				return "CommandLaunchGroupFailOverMassCapacity".Translate() + ": " + "MassUsageString".Translate(caravanShuttleMass.ToString("F0"), caravan.Shuttle.TransporterComp.MassCapacity.ToString("F0"));
			}
			float fuelLevel = caravan.Shuttle.FuelLevel;
			return caravan.Shuttle.LaunchableComp.CanLaunch(new float?(fuelLevel));
		}

		// Token: 0x0600E271 RID: 57969 RVA: 0x00541199 File Offset: 0x0053F399
		public static bool IsCaravanShuttle(CompTransporter transporter)
		{
			return ((transporter != null) ? transporter.Shuttle : null) != null && transporter.parent.IsInCaravan();
		}

		// Token: 0x0600E272 RID: 57970 RVA: 0x005411B8 File Offset: 0x0053F3B8
		public static void LaunchShuttle(Caravan caravan, PlanetTile destinationTile, TransportersArrivalAction arrivalAction)
		{
			Find.WorldTargeter.StopTargeting();
			Building_PassengerShuttle shuttle = caravan.Shuttle;
			float fuelLevel = shuttle.FuelLevel;
			if (!shuttle.LaunchableComp.CanLaunch(new float?(fuelLevel)))
			{
				return;
			}
			int num = Find.WorldGrid.TraversalDistanceBetween(caravan.Tile, destinationTile, true, int.MaxValue, true);
			if (num > shuttle.LaunchableComp.MaxLaunchDistanceAtFuelLevel(fuelLevel, destinationTile.Layer))
			{
				return;
			}
			float amount = Mathf.Max(shuttle.LaunchableComp.FuelNeededToLaunchAtDist((float)num, destinationTile.Layer), 1f);
			shuttle.RefuelableComp.ConsumeFuel(amount);
			CaravanInventoryUtility.GetOwnerOf(caravan, shuttle).inventory.innerContainer.Remove(shuttle);
			ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
			activeTransporterInfo.sentTransporterDef = shuttle.TransporterComp.parent.def;
			CaravanShuttleUtility.LoadCaravanItemsIntoContainer(caravan.pawns, activeTransporterInfo.innerContainer);
			activeTransporterInfo.innerContainer.TryAddRangeOrTransfer(caravan.pawns, true, false);
			activeTransporterInfo.SetShuttle(shuttle);
			TravellingTransporters travellingTransporters = (TravellingTransporters)WorldObjectMaker.MakeWorldObject(shuttle.LaunchableComp.Props.worldObjectDef);
			travellingTransporters.SetFaction(Faction.OfPlayer);
			travellingTransporters.destinationTile = destinationTile;
			travellingTransporters.arrivalAction = arrivalAction;
			PlanetTile planetTile = caravan.Tile;
			if (planetTile.Layer != destinationTile.Layer)
			{
				planetTile = destinationTile.Layer.GetClosestTile_NewTemp(planetTile, false);
			}
			travellingTransporters.Tile = planetTile;
			travellingTransporters.AddTransporter(activeTransporterInfo, true);
			Find.WorldObjects.Add(travellingTransporters);
			shuttle.LaunchableComp.lastLaunchTick = Find.TickManager.TicksGame;
			INotifyLaunchableLaunch notifyLaunchableLaunch = shuttle as INotifyLaunchableLaunch;
			if (notifyLaunchableLaunch != null)
			{
				notifyLaunchableLaunch.Notify_LaunchableLaunched(shuttle.LaunchableComp);
			}
			caravan.Destroy();
			CameraJumper.TryJump(shuttle, CameraJumper.MovementMode.Pan);
		}

		// Token: 0x0600E273 RID: 57971 RVA: 0x00541378 File Offset: 0x0053F578
		public static void LoadCaravanItemsIntoContainer(IEnumerable<Pawn> caravanPawns, ThingOwner container)
		{
			foreach (Pawn pawn in caravanPawns)
			{
				int num = 1000;
				for (;;)
				{
					Pawn_InventoryTracker inventory = pawn.inventory;
					if (((inventory != null) ? inventory.FirstUnloadableThing.Thing : null) == null || num-- <= 0)
					{
						break;
					}
					ThingCount firstUnloadableThing = pawn.inventory.FirstUnloadableThing;
					pawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, container, firstUnloadableThing.Count, true);
				}
			}
		}

		// Token: 0x04009089 RID: 37001
		private static int cachedCaravanItemsHash = -1;

		// Token: 0x0400908A RID: 37002
		private static float cachedCaravanShuttleMass = -1f;
	}
}
