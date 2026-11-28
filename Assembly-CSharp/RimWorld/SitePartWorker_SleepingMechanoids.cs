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
	// Token: 0x0200177B RID: 6011
	public class SitePartWorker_SleepingMechanoids : SitePartWorker
	{
		// Token: 0x06009593 RID: 38291 RVA: 0x00366357 File Offset: 0x00364557
		public override bool IsAvailable()
		{
			return base.IsAvailable() && Faction.OfMechanoids != null;
		}

		// Token: 0x06009594 RID: 38292 RVA: 0x0036636B File Offset: 0x0036456B
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x06009595 RID: 38293 RVA: 0x00366384 File Offset: 0x00364584
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int mechanoidsCount = this.GetMechanoidsCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("count", mechanoidsCount.ToString()));
			outExtraDescriptionConstants.Add("count", mechanoidsCount.ToString());
		}

		// Token: 0x06009596 RID: 38294 RVA: 0x003663DC File Offset: 0x003645DC
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetMechanoidsCount(site, sitePart.parms), "Enemies".Translate());
		}

		// Token: 0x06009597 RID: 38295 RVA: 0x00366430 File Offset: 0x00364630
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, FactionDefOf.Mechanoid.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null));
			return sitePartParams;
		}

		// Token: 0x06009598 RID: 38296 RVA: 0x0036645C File Offset: 0x0036465C
		private int GetMechanoidsCount(Site site, SitePartParams parms)
		{
			return PawnGroupMakerUtility.GeneratePawnKindsExample(new PawnGroupMakerParms
			{
				tile = site.Tile,
				faction = Faction.OfMechanoids,
				groupKind = PawnGroupKindDefOf.Combat,
				points = parms.threatPoints,
				seed = new int?(SleepingMechanoidsSitePartUtility.GetPawnGroupMakerSeed(parms))
			}).Count<PawnKindDef>();
		}
	}
}
