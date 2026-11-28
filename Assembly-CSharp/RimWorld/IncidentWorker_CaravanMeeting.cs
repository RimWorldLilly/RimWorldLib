using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001162 RID: 4450
	public class IncidentWorker_CaravanMeeting : IncidentWorker
	{
		// Token: 0x06006FD1 RID: 28625 RVA: 0x00271A74 File Offset: 0x0026FC74
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return parms.target is Map || (CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile) && this.TryFindFaction(out faction));
		}

		// Token: 0x06006FD2 RID: 28626 RVA: 0x00271AAC File Offset: 0x0026FCAC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (parms.target is Map)
			{
				return IncidentDefOf.TravelerGroup.Worker.TryExecute(parms);
			}
			Caravan caravan = (Caravan)parms.target;
			Faction faction;
			if (!this.TryFindFaction(out faction))
			{
				return false;
			}
			List<Pawn> list = this.GenerateCaravanPawns(faction);
			if (!list.Any<Pawn>())
			{
				Log.Error("IncidentWorker_CaravanMeeting could not generate any pawns.");
				return false;
			}
			Caravan metCaravan = CaravanMaker.MakeCaravan(list, faction, PlanetTile.Invalid, false);
			CameraJumper.TryJumpAndSelect(caravan, CameraJumper.MovementMode.Pan);
			DiaNode diaNode = new DiaNode("CaravanMeeting".Translate(caravan.Name, faction.NameColored, PawnUtility.PawnKindsToLineList(from x in metCaravan.PawnsListForReading
			select x.kindDef, "  - ")).Resolve().CapitalizeFirst());
			Pawn bestPlayerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, faction, metCaravan.TraderKind);
			if (metCaravan.CanTradeNow)
			{
				DiaOption diaOption = new DiaOption("CaravanMeeting_Trade".Translate());
				diaOption.action = delegate()
				{
					Find.WindowStack.Add(new Dialog_Trade(bestPlayerNegotiator, metCaravan, false));
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(metCaravan.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradingWithOtherCaravan".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, false, true);
				};
				if (bestPlayerNegotiator == null)
				{
					if (metCaravan.TraderKind.permitRequiredForTrading != null && !caravan.pawns.Any((Pawn p) => p.royalty != null && p.royalty.HasPermit(metCaravan.TraderKind.permitRequiredForTrading, faction)))
					{
						RoyalTitleDef royalTitleDef = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.First((RoyalTitleDef t) => t.permits != null && t.permits.Contains(metCaravan.TraderKind.permitRequiredForTrading));
						diaOption.Disable("CaravanMeeting_NoPermit".Translate(royalTitleDef.GetLabelForBothGenders(), faction).Resolve());
					}
					else
					{
						diaOption.Disable("CaravanMeeting_TradeIncapable".Translate());
					}
				}
				diaNode.options.Add(diaOption);
			}
			DiaOption diaOption2 = new DiaOption("CaravanMeeting_Attack".Translate());
			Action <>9__5;
			diaOption2.action = delegate()
			{
				Action action;
				if ((action = <>9__5) == null)
				{
					action = (<>9__5 = delegate()
					{
						Pawn t = caravan.PawnsListForReading[0];
						Faction.OfPlayer.TryAffectGoodwillWith(faction, Faction.OfPlayer.GoodwillToMakeHostile(faction), true, true, HistoryEventDefOf.AttackedCaravan, new GlobalTargetInfo?(t));
						Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(caravan, new IntVec3(100, 1, 100), WorldObjectDefOf.AttackedNonPlayerCaravan);
						map.Parent.SetFaction(faction);
						IntVec3 playerSpot;
						IntVec3 enemySpot;
						MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerSpot, out enemySpot);
						CaravanEnterMapUtility.Enter(caravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(playerSpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, true);
						List<Pawn> list2 = metCaravan.PawnsListForReading.ToList<Pawn>();
						CaravanEnterMapUtility.Enter(metCaravan, map, (Pawn p) => CellFinder.RandomClosewalkCellNear(enemySpot, map, 12, null), CaravanDropInventoryMode.DoNotDrop, false);
						LordMaker.MakeNewLord(faction, new LordJob_DefendAttackedTraderCaravan(list2[0].Position), map, list2);
						Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
						CameraJumper.TryJumpAndSelect(t, CameraJumper.MovementMode.Pan);
						PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(list2, "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
					});
				}
				LongEventHandler.QueueLongEvent(action, "GeneratingMapForNewEncounter", false, null, true, false, null);
			};
			diaOption2.resolveTree = true;
			diaNode.options.Add(diaOption2);
			DiaOption diaOption3 = new DiaOption("CaravanMeeting_MoveOn".Translate());
			diaOption3.action = delegate()
			{
				this.RemoveAllPawnsAndPassToWorld(metCaravan);
			};
			diaOption3.resolveTree = true;
			diaNode.options.Add(diaOption3);
			string title = "CaravanMeetingTitle".Translate(caravan.Label);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, faction, true, false, title));
			Find.Archive.Add(new ArchivedDialog(diaNode.text, title, faction));
			return true;
		}

		// Token: 0x06006FD3 RID: 28627 RVA: 0x00271DB7 File Offset: 0x0026FFB7
		private bool TryFindFaction(out Faction faction)
		{
			return (from x in Find.FactionManager.AllFactionsListForReading
			where !x.IsPlayer && !x.HostileTo(Faction.OfPlayer) && !x.Hidden && x.def.humanlikeFaction && !x.temporary && x.def.caravanTraderKinds.Any<TraderKindDef>() && !x.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()
			select x).TryRandomElement(out faction);
		}

		// Token: 0x06006FD4 RID: 28628 RVA: 0x00271DED File Offset: 0x0026FFED
		private List<Pawn> GenerateCaravanPawns(Faction faction)
		{
			return PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Trader,
				faction = faction,
				points = TraderCaravanUtility.GenerateGuardPoints(),
				dontUseSingleUseRocketLaunchers = true
			}, true).ToList<Pawn>();
		}

		// Token: 0x06006FD5 RID: 28629 RVA: 0x00271E24 File Offset: 0x00270024
		private void RemoveAllPawnsAndPassToWorld(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Find.WorldPawns.PassToWorld(pawnsListForReading[i], PawnDiscardDecideMode.Decide);
			}
			caravan.RemoveAllPawns();
		}

		// Token: 0x04004781 RID: 18305
		private const int MapSize = 100;
	}
}
