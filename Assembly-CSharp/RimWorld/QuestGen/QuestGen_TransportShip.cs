using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002168 RID: 8552
	public static class QuestGen_TransportShip
	{
		// Token: 0x0600D603 RID: 54787 RVA: 0x004F89DC File Offset: 0x004F6BDC
		public static QuestPart_SetupTransportShip GenerateTransportShip(this Quest quest, TransportShipDef def, IEnumerable<Thing> contents, Thing shipThing, string inSignal = null)
		{
			QuestPart_SetupTransportShip questPart_SetupTransportShip = new QuestPart_SetupTransportShip();
			questPart_SetupTransportShip.inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetupTransportShip.transportShip = TransportShipMaker.MakeTransportShip(def, null, shipThing);
			List<Thing> items;
			if (contents == null)
			{
				items = null;
			}
			else
			{
				items = (from c in contents
				where !(c is Pawn)
				select c).ToList<Thing>();
			}
			questPart_SetupTransportShip.items = items;
			questPart_SetupTransportShip.pawns = ((contents != null) ? contents.OfType<Pawn>().ToList<Pawn>() : null);
			QuestPart_SetupTransportShip questPart_SetupTransportShip2 = questPart_SetupTransportShip;
			quest.AddPart(questPart_SetupTransportShip2);
			return questPart_SetupTransportShip2;
		}

		// Token: 0x0600D604 RID: 54788 RVA: 0x004F8A70 File Offset: 0x004F6C70
		public static QuestPart_SendTransportShipAwayOnCleanup SendTransportShipAwayOnCleanup(this Quest quest, TransportShip transportShip, bool unloadContents = false, TransportShipDropMode unsatisfiedDropMode = TransportShipDropMode.NonRequired)
		{
			QuestPart_SendTransportShipAwayOnCleanup questPart_SendTransportShipAwayOnCleanup = new QuestPart_SendTransportShipAwayOnCleanup
			{
				transportShip = transportShip,
				unloadContents = unloadContents,
				unsatisfiedDropMode = unsatisfiedDropMode
			};
			quest.AddPart(questPart_SendTransportShipAwayOnCleanup);
			return questPart_SendTransportShipAwayOnCleanup;
		}

		// Token: 0x0600D605 RID: 54789 RVA: 0x004F8AA0 File Offset: 0x004F6CA0
		public static QuestPart_AddShipJob AddShipJob(this Quest quest, TransportShip transportShip, ShipJobDef def, ShipJobStartMode startMode = ShipJobStartMode.Queue, string inSignal = null)
		{
			QuestPart_AddShipJob questPart_AddShipJob = new QuestPart_AddShipJob
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shipJobStartMode = startMode,
				transportShip = transportShip,
				shipJobDef = def
			};
			quest.AddPart(questPart_AddShipJob);
			return questPart_AddShipJob;
		}

		// Token: 0x0600D606 RID: 54790 RVA: 0x004F8AF0 File Offset: 0x004F6CF0
		public static QuestPart_AddShipJob_Unload AddShipJob_Unload(this Quest quest, TransportShip transportShip, ShipJobStartMode startMode = ShipJobStartMode.Queue, bool unforbidAll = true, string inSignal = null)
		{
			QuestPart_AddShipJob_Unload questPart_AddShipJob_Unload = new QuestPart_AddShipJob_Unload
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shipJobStartMode = startMode,
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.Unload,
				unforbidAll = unforbidAll
			};
			quest.AddPart(questPart_AddShipJob_Unload);
			return questPart_AddShipJob_Unload;
		}

		// Token: 0x0600D607 RID: 54791 RVA: 0x004F8B48 File Offset: 0x004F6D48
		public static QuestPart_AddShipJob_Arrive AddShipJob_Arrive(this Quest quest, TransportShip transportShip, MapParent mapParent, Pawn mapOfPawn = null, IntVec3? cell = null, ShipJobStartMode startMode = ShipJobStartMode.Queue, Faction factionForArrival = null, string inSignal = null)
		{
			PocketMapParent pocketMapParent = mapParent as PocketMapParent;
			if (pocketMapParent != null)
			{
				mapParent = pocketMapParent.sourceMap.Parent;
			}
			QuestPart_AddShipJob_Arrive questPart_AddShipJob_Arrive = new QuestPart_AddShipJob_Arrive
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shipJobStartMode = startMode,
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.Arrive,
				cell = (cell ?? IntVec3.Invalid),
				mapParent = mapParent,
				mapOfPawn = mapOfPawn,
				factionForArrival = factionForArrival
			};
			quest.AddPart(questPart_AddShipJob_Arrive);
			return questPart_AddShipJob_Arrive;
		}

		// Token: 0x0600D608 RID: 54792 RVA: 0x004F8BE8 File Offset: 0x004F6DE8
		public static QuestPart_AddShipJob_WaitTime AddShipJob_WaitTime(this Quest quest, TransportShip transportShip, int duration, bool leaveImmediatelyWhenSatisfied, List<Thing> sendAwayIfAllDespawned = null, string inSignal = null)
		{
			QuestPart_AddShipJob_WaitTime questPart_AddShipJob_WaitTime = new QuestPart_AddShipJob_WaitTime
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.WaitTime,
				leaveImmediatelyWhenSatisfied = leaveImmediatelyWhenSatisfied,
				sendAwayIfAllDespawned = sendAwayIfAllDespawned,
				duration = duration
			};
			quest.AddPart(questPart_AddShipJob_WaitTime);
			return questPart_AddShipJob_WaitTime;
		}

		// Token: 0x0600D609 RID: 54793 RVA: 0x004F8C48 File Offset: 0x004F6E48
		public static QuestPart_AddShipJob_WaitForever AddShipJob_WaitForever(this Quest quest, TransportShip transportShip, bool leaveImmediatelyWhenSatisfied, bool showGizmos, List<Thing> sendAwayIfAllDespawned = null, string inSignal = null)
		{
			QuestPart_AddShipJob_WaitForever questPart_AddShipJob_WaitForever = new QuestPart_AddShipJob_WaitForever
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.WaitForever,
				leaveImmediatelyWhenSatisfied = leaveImmediatelyWhenSatisfied,
				sendAwayIfAllDespawned = sendAwayIfAllDespawned
			};
			quest.AddPart(questPart_AddShipJob_WaitForever);
			return questPart_AddShipJob_WaitForever;
		}

		// Token: 0x0600D60A RID: 54794 RVA: 0x004F8CA4 File Offset: 0x004F6EA4
		public static QuestPart_AddShipJob_WaitSendable AddShipJob_WaitSendable(this Quest quest, TransportShip transportShip, MapParent destination, bool leaveImmeiatelyWhenSatisfied = false, bool targetPlayerSettlement = false, string inSignal = null)
		{
			QuestPart_AddShipJob_WaitSendable questPart_AddShipJob_WaitSendable = new QuestPart_AddShipJob_WaitSendable
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.WaitSendable,
				destination = destination,
				leaveImmediatelyWhenSatisfied = leaveImmeiatelyWhenSatisfied,
				targetPlayerSettlement = targetPlayerSettlement
			};
			quest.AddPart(questPart_AddShipJob_WaitSendable);
			return questPart_AddShipJob_WaitSendable;
		}

		// Token: 0x0600D60B RID: 54795 RVA: 0x004F8D04 File Offset: 0x004F6F04
		public static QuestPart_AddShipJob_FlyAway AddShipJob_FlyAway(this Quest quest, TransportShip transportShip, PlanetTile? destinationTile = null, TransportersArrivalAction arrivalAction = null, TransportShipDropMode dropMode = TransportShipDropMode.NonRequired, string inSignal = null)
		{
			QuestPart_AddShipJob_FlyAway questPart_AddShipJob_FlyAway = new QuestPart_AddShipJob_FlyAway
			{
				inSignal = (inSignal ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				transportShip = transportShip,
				shipJobDef = ShipJobDefOf.FlyAway,
				destinationTile = (destinationTile ?? PlanetTile.Invalid),
				arrivalAction = arrivalAction,
				dropMode = dropMode
			};
			quest.AddPart(questPart_AddShipJob_FlyAway);
			return questPart_AddShipJob_FlyAway;
		}
	}
}
