using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023D8 RID: 9176
	public class TransportersArrivalAction_GiveGift : TransportersArrivalAction
	{
		// Token: 0x17002840 RID: 10304
		// (get) Token: 0x0600E4B2 RID: 58546 RVA: 0x0054F549 File Offset: 0x0054D749
		public override bool GeneratesMap
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E4B3 RID: 58547 RVA: 0x0054F54C File Offset: 0x0054D74C
		public TransportersArrivalAction_GiveGift()
		{
		}

		// Token: 0x0600E4B4 RID: 58548 RVA: 0x0054F554 File Offset: 0x0054D754
		public TransportersArrivalAction_GiveGift(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x0600E4B5 RID: 58549 RVA: 0x0054F563 File Offset: 0x0054D763
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x0600E4B6 RID: 58550 RVA: 0x0054F57C File Offset: 0x0054D77C
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_GiveGift.CanGiveGiftTo(pods, this.settlement);
		}

		// Token: 0x0600E4B7 RID: 58551 RVA: 0x0054F5CC File Offset: 0x0054D7CC
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			for (int i = 0; i < transporters.Count; i++)
			{
				for (int j = 0; j < transporters[i].innerContainer.Count; j++)
				{
					Pawn pawn = transporters[i].innerContainer[j] as Pawn;
					if (pawn != null)
					{
						if (pawn.RaceProps.Humanlike)
						{
							Pawn arg;
							if (pawn.HomeFaction == this.settlement.Faction)
							{
								GenGuest.AddHealthyPrisonerReleasedThoughts(pawn);
							}
							else if (PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.TryRandomElement(out arg))
							{
								Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SoldSlave, arg.Named(HistoryEventArgsNames.Doer)), true);
							}
						}
						else if (pawn.RaceProps.Animal && pawn.relations != null)
						{
							Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null);
							if (firstDirectRelationPawn != null && firstDirectRelationPawn.needs.mood != null)
							{
								pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Bond, firstDirectRelationPawn);
								firstDirectRelationPawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoldMyBondedAnimalMood, null, null);
							}
						}
					}
					transporters[i].innerContainer[j].Notify_AbandonedAtTile(tile);
				}
			}
			FactionGiftUtility.GiveGift(transporters, this.settlement);
		}

		// Token: 0x0600E4B8 RID: 58552 RVA: 0x0054F720 File Offset: 0x0054D920
		public static FloatMenuAcceptanceReport CanGiveGiftTo(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && pawn.IsQuestLodger())
					{
						return false;
					}
				}
			}
			return settlement != null && settlement.Spawned && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.HasMap;
		}

		// Token: 0x0600E4B9 RID: 58553 RVA: 0x0054F7E0 File Offset: 0x0054D9E0
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement.Faction == Faction.OfPlayer)
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
			return TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_GiveGift>(() => TransportersArrivalAction_GiveGift.CanGiveGiftTo(pods, settlement), () => new TransportersArrivalAction_GiveGift(settlement), "GiveGiftViaTransportPods".Translate(settlement.Faction.Name, FactionGiftUtility.GetGoodwillChange(pods, settlement).ToStringWithSign()), launchAction, settlement.Tile, delegate(Action action)
			{
				TradeRequestComp tradeReqComp = settlement.GetComponent<TradeRequestComp>();
				if (tradeReqComp != null && tradeReqComp.ActiveRequest && pods.Any((IThingHolder p) => p.GetDirectlyHeldThings().Contains(tradeReqComp.requestThingDef)))
				{
					Find.WindowStack.Add(new Dialog_MessageBox("GiveGiftViaTransportPodsTradeRequestWarning".Translate(), "Yes".Translate(), delegate()
					{
						action();
					}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
					return;
				}
				action();
			});
		}

		// Token: 0x0400916A RID: 37226
		private Settlement settlement;
	}
}
