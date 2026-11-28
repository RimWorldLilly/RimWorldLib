using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023DD RID: 9181
	public class TransportersArrivalAction_VisitSite : TransportersArrivalAction
	{
		// Token: 0x17002843 RID: 10307
		// (get) Token: 0x0600E4D6 RID: 58582 RVA: 0x00550010 File Offset: 0x0054E210
		public override bool GeneratesMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E4D7 RID: 58583 RVA: 0x00550013 File Offset: 0x0054E213
		public TransportersArrivalAction_VisitSite()
		{
		}

		// Token: 0x0600E4D8 RID: 58584 RVA: 0x0055001B File Offset: 0x0054E21B
		public TransportersArrivalAction_VisitSite(Site site, PawnsArrivalModeDef arrivalMode)
		{
			this.site = site;
			this.arrivalMode = arrivalMode;
		}

		// Token: 0x0600E4D9 RID: 58585 RVA: 0x00550031 File Offset: 0x0054E231
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.arrivalMode, "arrivalMode");
		}

		// Token: 0x0600E4DA RID: 58586 RVA: 0x0055005C File Offset: 0x0054E25C
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.site != null && this.site.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_VisitSite.CanVisit(pods, this.site);
		}

		// Token: 0x0600E4DB RID: 58587 RVA: 0x005500AA File Offset: 0x0054E2AA
		public override bool ShouldUseLongEvent(List<ActiveTransporterInfo> pods, PlanetTile tile)
		{
			return !this.site.HasMap;
		}

		// Token: 0x0600E4DC RID: 58588 RVA: 0x005500BC File Offset: 0x0054E2BC
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			Thing lookTarget = TransportersArrivalActionUtility.GetLookTarget(transporters);
			bool flag = !this.site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(this.site.Tile, this.site.PreferredMapSize, null, null, false);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsInMapWherePlayerLanded".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
			}
			if (this.site.Faction != null && this.site.Faction != Faction.OfPlayer && this.site.MainSitePartDef.considerEnteringAsAttack)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this.site.Faction, Faction.OfPlayer.GoodwillToMakeHostile(this.site.Faction), true, true, HistoryEventDefOf.AttackedSettlement, null);
			}
			if (transporters.IsShuttle())
			{
				Messages.Message("MessageShuttleArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
			}
			else
			{
				Messages.Message("MessageTransportPodsArrived".Translate(), lookTarget, MessageTypeDefOf.TaskCompletion, true);
			}
			this.arrivalMode.Worker.TravellingTransportersArrived(transporters, orGenerateMap);
		}

		// Token: 0x0600E4DD RID: 58589 RVA: 0x0055020C File Offset: 0x0054E40C
		public static FloatMenuAcceptanceReport CanVisit(IEnumerable<IThingHolder> pods, Site site)
		{
			if (site == null || !site.Spawned)
			{
				return false;
			}
			if (!TransportersArrivalActionUtility.AnyNonDownedColonist(pods))
			{
				return false;
			}
			if (site.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(site.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x0600E4DE RID: 58590 RVA: 0x00550272 File Offset: 0x0054E472
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Site site)
		{
			TransportersArrivalAction_VisitSite.<GetFloatMenuOptions>d__11 <GetFloatMenuOptions>d__ = new TransportersArrivalAction_VisitSite.<GetFloatMenuOptions>d__11(-2);
			<GetFloatMenuOptions>d__.<>3__launchAction = launchAction;
			<GetFloatMenuOptions>d__.<>3__pods = pods;
			<GetFloatMenuOptions>d__.<>3__site = site;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x04009172 RID: 37234
		private Site site;

		// Token: 0x04009173 RID: 37235
		private PawnsArrivalModeDef arrivalMode;
	}
}
