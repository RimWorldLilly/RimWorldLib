using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001778 RID: 6008
	public class SitePartWorker_Outpost : SitePartWorker
	{
		// Token: 0x06009586 RID: 38278 RVA: 0x00365F40 File Offset: 0x00364140
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x06009587 RID: 38279 RVA: 0x00365F58 File Offset: 0x00364158
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int enemiesCount = this.GetEnemiesCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("enemiesCount", enemiesCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("enemiesLabel", this.GetEnemiesLabel(part.site, enemiesCount)));
		}

		// Token: 0x06009588 RID: 38280 RVA: 0x00365FB8 File Offset: 0x003641B8
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.Faction.IsPlayer)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetEnemiesCount(site, sitePart.parms), "Enemies".Translate());
		}

		// Token: 0x06009589 RID: 38281 RVA: 0x0036601C File Offset: 0x0036421C
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Settlement, null));
			sitePartParams.lootMarketValue = SitePartWorker_Outpost.ThreatPointsLootMarketValue.Evaluate(sitePartParams.threatPoints);
			return sitePartParams;
		}

		// Token: 0x0600958A RID: 38282 RVA: 0x0036606C File Offset: 0x0036426C
		protected int GetEnemiesCount(Site site, SitePartParams parms)
		{
			return PawnGroupMakerUtility.GeneratePawnKindsExample(new PawnGroupMakerParms
			{
				tile = site.Tile,
				faction = site.Faction,
				groupKind = PawnGroupKindDefOf.Settlement,
				points = parms.threatPoints,
				inhabitants = true,
				seed = new int?(OutpostSitePartUtility.GetPawnGroupMakerSeed(parms))
			}).Count<PawnKindDef>();
		}

		// Token: 0x0600958B RID: 38283 RVA: 0x003660D0 File Offset: 0x003642D0
		protected string GetEnemiesLabel(Site site, int enemiesCount)
		{
			if (site.Faction == null)
			{
				return (enemiesCount == 1) ? "Enemy".Translate() : "Enemies".Translate();
			}
			if (enemiesCount != 1)
			{
				return site.Faction.def.pawnsPlural;
			}
			return site.Faction.def.pawnSingular;
		}

		// Token: 0x04005A18 RID: 23064
		public static readonly SimpleCurve ThreatPointsLootMarketValue = new SimpleCurve
		{
			{
				new CurvePoint(100f, 200f),
				true
			},
			{
				new CurvePoint(250f, 450f),
				true
			},
			{
				new CurvePoint(800f, 1000f),
				true
			},
			{
				new CurvePoint(10000f, 2000f),
				true
			}
		};
	}
}
