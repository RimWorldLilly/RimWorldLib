using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001784 RID: 6020
	public class TransportersArrivalAction_TransportShip : TransportersArrivalAction
	{
		// Token: 0x17001991 RID: 6545
		// (get) Token: 0x060095C6 RID: 38342 RVA: 0x00366E6C File Offset: 0x0036506C
		public override bool GeneratesMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060095C7 RID: 38343 RVA: 0x00366E6F File Offset: 0x0036506F
		public TransportersArrivalAction_TransportShip()
		{
		}

		// Token: 0x060095C8 RID: 38344 RVA: 0x00366E82 File Offset: 0x00365082
		public TransportersArrivalAction_TransportShip(MapParent mapParent, TransportShip transportShip)
		{
			this.mapParent = mapParent;
			this.transportShip = transportShip;
		}

		// Token: 0x060095C9 RID: 38345 RVA: 0x00366EA3 File Offset: 0x003650A3
		public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return !this.mapParent.HasMap;
		}

		// Token: 0x060095CA RID: 38346 RVA: 0x00366EB4 File Offset: 0x003650B4
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			if (this.transportShip == null || this.transportShip.Disposed)
			{
				Log.Error("Trying to arrive in a null or disposed transport ship.");
				return;
			}
			if (this.mapParent.Destroyed)
			{
				new TransportersArrivalAction_FormCaravan().Arrived(transporters, tile);
				return;
			}
			bool flag = !this.mapParent.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, null, null);
			if (!this.cell.IsValid)
			{
				this.cell = DropCellFinder.GetBestShuttleLandingSpot(orGenerateMap, Faction.OfPlayer);
			}
			LookTargets lookTargets = new LookTargets(this.cell, orGenerateMap);
			if (!this.cell.IsValid)
			{
				Log.Error("Could not find cell for transport ship arrival.");
				return;
			}
			Settlement settlement = orGenerateMap.Parent as Settlement;
			if (settlement != null && settlement.Faction != Faction.OfPlayer)
			{
				TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
				TaggedString text = "LetterShuttleLandedInEnemyBase".Translate(settlement.Label).CapitalizeFirst();
				SettlementUtility.AffectRelationsOnAttacked(settlement, ref text);
				if (flag)
				{
					Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
				}
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTargets, settlement.Faction, null, null, null, 0, true);
			}
			transporters[0].innerContainer.Remove(this.transportShip.shipThing);
			this.transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(transporters[0].innerContainer, true, true);
			this.transportShip.ArriveAt(this.cell, this.mapParent);
			TransportShipDef shipDef = this.transportShip.shipThing.TryGetComp<CompShuttle>().Props.shipDef;
			if (shipDef != null && shipDef.playerShuttle)
			{
				ShipJob_Unload shipJob_Unload = (ShipJob_Unload)ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload);
				shipJob_Unload.dropMode = TransportShipDropMode.PawnsOnly;
				this.transportShip.AddJob(shipJob_Unload);
			}
			Messages.Message("MessageShuttleArrived".Translate(), lookTargets, MessageTypeDefOf.TaskCompletion, true);
		}

		// Token: 0x060095CB RID: 38347 RVA: 0x003670D4 File Offset: 0x003652D4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
		}

		// Token: 0x04005A1D RID: 23069
		public MapParent mapParent;

		// Token: 0x04005A1E RID: 23070
		public TransportShip transportShip;

		// Token: 0x04005A1F RID: 23071
		public IntVec3 cell = IntVec3.Invalid;
	}
}
