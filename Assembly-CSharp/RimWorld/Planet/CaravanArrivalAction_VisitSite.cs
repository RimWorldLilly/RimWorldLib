using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200237B RID: 9083
	public class CaravanArrivalAction_VisitSite : CaravanArrivalAction
	{
		// Token: 0x170027B0 RID: 10160
		// (get) Token: 0x0600E187 RID: 57735 RVA: 0x0053AE55 File Offset: 0x00539055
		public override string Label
		{
			get
			{
				return this.site.ApproachOrderString;
			}
		}

		// Token: 0x170027B1 RID: 10161
		// (get) Token: 0x0600E188 RID: 57736 RVA: 0x0053AE62 File Offset: 0x00539062
		public override string ReportString
		{
			get
			{
				return this.site.ApproachingReportString;
			}
		}

		// Token: 0x0600E189 RID: 57737 RVA: 0x0053AE6F File Offset: 0x0053906F
		public CaravanArrivalAction_VisitSite()
		{
		}

		// Token: 0x0600E18A RID: 57738 RVA: 0x0053AE77 File Offset: 0x00539077
		public CaravanArrivalAction_VisitSite(Site site)
		{
			this.site = site;
		}

		// Token: 0x0600E18B RID: 57739 RVA: 0x0053AE88 File Offset: 0x00539088
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.site != null && this.site.Tile != destinationTile)
			{
				return false;
			}
			return CaravanArrivalAction_VisitSite.CanVisit(caravan, this.site);
		}

		// Token: 0x0600E18C RID: 57740 RVA: 0x0053AED8 File Offset: 0x005390D8
		public override void Arrived(Caravan caravan)
		{
			if (!this.site.HasMap)
			{
				LongEventHandler.QueueLongEvent(delegate()
				{
					this.DoEnter(caravan, this.site);
				}, "GeneratingMapForNewEncounter", false, null, true, false, null);
				return;
			}
			this.DoEnter(caravan, this.site);
		}

		// Token: 0x0600E18D RID: 57741 RVA: 0x0053AF34 File Offset: 0x00539134
		private void DoEnter(Caravan caravan, Site site)
		{
			LookTargets lookTargets = new LookTargets(caravan.PawnsListForReading);
			bool draftColonists = site.Faction == null || site.Faction.HostileTo(Faction.OfPlayer);
			bool flag = !site.HasMap;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(site.Tile, site.PreferredMapSize, null, null, false);
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(orGenerateMap.mapPawns.AllPawns, "LetterRelatedPawnsSite".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, true, true);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("LetterCaravanEnteredMap".Translate(caravan.Label, site).CapitalizeFirst());
				LetterDef letterDef;
				LookTargets lookTargets2;
				this.AppendThreatInfo(stringBuilder, site, orGenerateMap, out letterDef, out lookTargets2);
				TaggedString taggedString = null;
				if (site.parts.Any((SitePart part) => part.def.considerEnteringAsAttack))
				{
					SettlementUtility.AffectRelationsOnAttacked(site, ref taggedString);
				}
				if (!taggedString.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						if (stringBuilder[stringBuilder.Length - 1] != '\n')
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLineTagged(taggedString);
				}
				List<HediffDef> list = null;
				foreach (SitePart sitePart in site.parts)
				{
					if (!sitePart.def.arrivedLetterHediffHyperlinks.NullOrEmpty<HediffDef>())
					{
						if (list == null)
						{
							list = new List<HediffDef>();
						}
						foreach (HediffDef item in sitePart.def.arrivedLetterHediffHyperlinks)
						{
							if (!list.Contains(item))
							{
								list.Add(item);
							}
						}
					}
				}
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter("LetterLabelCaravanEnteredMap".Translate(site), stringBuilder.ToString(), letterDef ?? LetterDefOf.NeutralEvent, lookTargets2.IsValid() ? lookTargets2 : lookTargets, null, null, null);
				choiceLetter.hyperlinkHediffDefs = list;
				Find.LetterStack.ReceiveLetter(choiceLetter, null, 0, true);
			}
			else
			{
				Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredMap".Translate(site), "LetterCaravanEnteredMap".Translate(caravan.Label, site).CapitalizeFirst(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null, 0, true);
			}
			CaravanEnterMapUtility.Enter(caravan, orGenerateMap, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, draftColonists, null);
		}

		// Token: 0x0600E18E RID: 57742 RVA: 0x0053B1F0 File Offset: 0x005393F0
		private void AppendThreatInfo(StringBuilder sb, Site site, Map map, out LetterDef letterDef, out LookTargets allLookTargets)
		{
			allLookTargets = new LookTargets();
			CaravanArrivalAction_VisitSite.tmpUsedDefs.Clear();
			CaravanArrivalAction_VisitSite.tmpDefs.Clear();
			for (int i = 0; i < site.parts.Count; i++)
			{
				CaravanArrivalAction_VisitSite.tmpDefs.Add(site.parts[i].def);
			}
			letterDef = null;
			for (int j = 0; j < CaravanArrivalAction_VisitSite.tmpDefs.Count; j++)
			{
				LetterDef letterDef2;
				LookTargets lookTargets;
				string arrivedLetterPart = CaravanArrivalAction_VisitSite.tmpDefs[j].Worker.GetArrivedLetterPart(map, out letterDef2, out lookTargets);
				if (arrivedLetterPart != null)
				{
					if (!CaravanArrivalAction_VisitSite.tmpUsedDefs.Contains(CaravanArrivalAction_VisitSite.tmpDefs[j]))
					{
						CaravanArrivalAction_VisitSite.tmpUsedDefs.Add(CaravanArrivalAction_VisitSite.tmpDefs[j]);
						if (sb.Length > 0)
						{
							sb.AppendLine();
							sb.AppendLine();
						}
						sb.Append(arrivedLetterPart);
					}
					if (letterDef == null)
					{
						letterDef = letterDef2;
					}
					if (lookTargets.IsValid())
					{
						allLookTargets = new LookTargets(allLookTargets.targets.Concat(lookTargets.targets));
					}
				}
			}
		}

		// Token: 0x0600E18F RID: 57743 RVA: 0x0053B301 File Offset: 0x00539501
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Site>(ref this.site, "site", false);
		}

		// Token: 0x0600E190 RID: 57744 RVA: 0x0053B31C File Offset: 0x0053951C
		public static FloatMenuAcceptanceReport CanVisit(Caravan caravan, Site site)
		{
			if (site == null || !site.Spawned)
			{
				return false;
			}
			if (site.EnterCooldownBlocksEntering())
			{
				return FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(site.EnterCooldownTicksLeft().ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return true;
		}

		// Token: 0x0600E191 RID: 57745 RVA: 0x0053B374 File Offset: 0x00539574
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, Site site)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_VisitSite>(() => CaravanArrivalAction_VisitSite.CanVisit(caravan, site), () => new CaravanArrivalAction_VisitSite(site), site.ApproachOrderString, caravan, site.Tile, site, null);
		}

		// Token: 0x04009040 RID: 36928
		private Site site;

		// Token: 0x04009041 RID: 36929
		private static List<SitePartDef> tmpDefs = new List<SitePartDef>();

		// Token: 0x04009042 RID: 36930
		private static List<SitePartDef> tmpUsedDefs = new List<SitePartDef>();
	}
}
