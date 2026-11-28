using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023D6 RID: 9174
	public class TransportersArrivalAction_AttackSettlement : TransportersArrivalAction
	{
		// Token: 0x1700283E RID: 10302
		// (get) Token: 0x0600E4A1 RID: 58529 RVA: 0x0054F0DC File Offset: 0x0054D2DC
		public override bool GeneratesMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E4A2 RID: 58530 RVA: 0x0054F0DF File Offset: 0x0054D2DF
		public TransportersArrivalAction_AttackSettlement()
		{
		}

		// Token: 0x0600E4A3 RID: 58531 RVA: 0x0054F0E7 File Offset: 0x0054D2E7
		public TransportersArrivalAction_AttackSettlement(Settlement settlement, PawnsArrivalModeDef arrivalMode)
		{
			this.settlement = settlement;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x0600E4A4 RID: 58532 RVA: 0x0054F0FD File Offset: 0x0054D2FD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x0600E4A5 RID: 58533 RVA: 0x0054F128 File Offset: 0x0054D328
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
			return TransportersArrivalAction_AttackSettlement.CanAttack(pods, this.settlement);
		}

		// Token: 0x0600E4A6 RID: 58534 RVA: 0x0054F176 File Offset: 0x0054D376
		public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return !this.settlement.HasMap;
		}

		// Token: 0x0600E4A7 RID: 58535 RVA: 0x0054F188 File Offset: 0x0054D388
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
			bool flag = !this.settlement.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.settlement.Tile, null, null);
			TaggedString label = "LetterLabelCaravanEnteredEnemyBase".Translate();
			TaggedString text = "LetterTransportPodsLandedInEnemyBase".Translate(this.settlement.Label).CapitalizeFirst();
			SettlementUtility.AffectRelationsOnAttacked(this.settlement, ref text);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(orGenerateMap.mapPawns.AllPawns, ref label, ref text, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, lookTarget, this.settlement.Faction, null, null, null, 0, true);
			this.arrivalMode.Worker.TravellingTransportersArrived(transporters, orGenerateMap);
		}

		// Token: 0x0600E4A8 RID: 58536 RVA: 0x0054F278 File Offset: 0x0054D478
		public static FloatMenuAcceptanceReport CanAttack(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement == null || !settlement.Spawned || !settlement.Attackable)
			{
				return false;
			}
			if (!TransportersArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			if (settlement.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailReasonAndMessage("EnterCooldownBlocksEntering".Translate(), "MessageEnterCooldownBlocksEntering".Translate(settlement.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x0600E4A9 RID: 58537 RVA: 0x0054F2F5 File Offset: 0x0054D4F5
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			TransportersArrivalAction_AttackSettlement.<GetFloatMenuOptions>d__11 <GetFloatMenuOptions>d__ = new TransportersArrivalAction_AttackSettlement.<GetFloatMenuOptions>d__11(-2);
			<GetFloatMenuOptions>d__.<>3__launchAction = launchAction;
			<GetFloatMenuOptions>d__.<>3__pods = pods;
			<GetFloatMenuOptions>d__.<>3__settlement = settlement;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x04009165 RID: 37221
		private Settlement settlement;

		// Token: 0x04009166 RID: 37222
		private PawnsArrivalModeDef arrivalMode;
	}
}
