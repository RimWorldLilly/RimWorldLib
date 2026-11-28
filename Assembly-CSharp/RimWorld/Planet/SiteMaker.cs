using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023BF RID: 9151
	public static class SiteMaker
	{
		// Token: 0x0600E43C RID: 58428 RVA: 0x0054D036 File Offset: 0x0054B236
		public static Site MakeSite(SitePartDef sitePart, PlanetTile tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null, WorldObjectDef worldObjectDef = null)
		{
			return SiteMaker.MakeSite((sitePart != null) ? Gen.YieldSingle<SitePartDef>(sitePart) : null, tile, faction, ifHostileThenMustRemainHostile, threatPoints, worldObjectDef);
		}

		// Token: 0x0600E43D RID: 58429 RVA: 0x0054D050 File Offset: 0x0054B250
		public static Site MakeSite(IEnumerable<SitePartDef> siteParts, PlanetTile tile, Faction faction, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null, WorldObjectDef worldObjectDef = null)
		{
			float num = threatPoints ?? StorytellerUtility.DefaultSiteThreatPointsNow();
			List<SitePartDefWithParams> siteParts2;
			SiteMakerHelper.GenerateDefaultParams(num, tile, faction, siteParts, out siteParts2);
			Site site = SiteMaker.MakeSite(siteParts2, tile, faction, ifHostileThenMustRemainHostile, worldObjectDef);
			site.desiredThreatPoints = num;
			return site;
		}

		// Token: 0x0600E43E RID: 58430 RVA: 0x0054D098 File Offset: 0x0054B298
		public static Site MakeSite(IEnumerable<SitePartDefWithParams> siteParts, PlanetTile tile, Faction faction, bool ifHostileThenMustRemainHostile = true, WorldObjectDef worldObjectDef = null)
		{
			Site site = (Site)WorldObjectMaker.MakeWorldObject(worldObjectDef ?? WorldObjectDefOf.Site);
			site.Tile = tile;
			site.SetFaction(faction);
			if (ifHostileThenMustRemainHostile && faction != null && faction.HostileTo(Faction.OfPlayer))
			{
				site.factionMustRemainHostile = true;
			}
			if (siteParts != null)
			{
				foreach (SitePartDefWithParams sitePartDefWithParams in siteParts)
				{
					site.AddPart(new SitePart(site, sitePartDefWithParams.def, sitePartDefWithParams.parms));
				}
			}
			site.desiredThreatPoints = site.ActualThreatPoints;
			return site;
		}

		// Token: 0x0600E43F RID: 58431 RVA: 0x0054D140 File Offset: 0x0054B340
		public static Site TryMakeSite_SingleSitePart(IEnumerable<SitePartDef> singleSitePartCandidates, PlanetTile tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(singleSitePartCandidates, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}

		// Token: 0x0600E440 RID: 58432 RVA: 0x0054D16C File Offset: 0x0054B36C
		public static Site TryMakeSite_SingleSitePart(string singleSitePartTag, PlanetTile tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			SitePartDef sitePart;
			if (!SiteMakerHelper.TryFindSiteParams_SingleSitePart(singleSitePartTag, out sitePart, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(sitePart, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}

		// Token: 0x0600E441 RID: 58433 RVA: 0x0054D198 File Offset: 0x0054B398
		public static Site TryMakeSite_MultipleSiteParts(IEnumerable<IEnumerable<SitePartDef>> sitePartsCandidates, PlanetTile tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			List<SitePartDef> siteParts;
			if (!SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsCandidates, out siteParts, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}

		// Token: 0x0600E442 RID: 58434 RVA: 0x0054D1C4 File Offset: 0x0054B3C4
		public static Site TryMakeSite_MultipleSiteParts(List<string> sitePartsTags, PlanetTile tile, Faction faction = null, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			List<SitePartDef> siteParts;
			if (!SiteMakerHelper.TryFindSiteParams_MultipleSiteParts(sitePartsTags, out siteParts, out faction, faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}

		// Token: 0x0600E443 RID: 58435 RVA: 0x0054D1F0 File Offset: 0x0054B3F0
		public static Site TryMakeSite(IEnumerable<SitePartDef> siteParts, PlanetTile tile, bool disallowNonHostileFactions = true, Predicate<Faction> extraFactionValidator = null, bool ifHostileThenMustRemainHostile = true, float? threatPoints = null)
		{
			Faction faction;
			if (!SiteMakerHelper.TryFindRandomFactionFor(siteParts, out faction, disallowNonHostileFactions, extraFactionValidator))
			{
				return null;
			}
			return SiteMaker.MakeSite(siteParts, tile, faction, ifHostileThenMustRemainHostile, threatPoints, null);
		}
	}
}
