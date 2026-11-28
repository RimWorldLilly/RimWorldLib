using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001776 RID: 6006
	public class SitePartWorker_Manhunters : SitePartWorker
	{
		// Token: 0x0600957C RID: 38268 RVA: 0x00365D98 File Offset: 0x00363F98
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x0600957D RID: 38269 RVA: 0x00365DB0 File Offset: 0x00363FB0
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			if (ManhunterPackGenStepUtility.TryGetAnimalsKind(sitePartParams.threatPoints, tile, out sitePartParams.animalKind))
			{
				sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, sitePartParams.animalKind.combatPower);
			}
			return sitePartParams;
		}

		// Token: 0x0600957E RID: 38270 RVA: 0x00365DF8 File Offset: 0x00363FF8
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int animalsCount = this.GetAnimalsCount(part.parms);
			string output = GenLabel.BestKindLabel(part.parms.animalKind, Gender.None, true, animalsCount);
			outExtraDescriptionRules.Add(new Rule_String("count", animalsCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("kindLabel", output));
			outExtraDescriptionConstants.Add("count", animalsCount.ToString());
		}

		// Token: 0x0600957F RID: 38271 RVA: 0x00365E6C File Offset: 0x0036406C
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			int animalsCount = this.GetAnimalsCount(sitePart.parms);
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(animalsCount.ToString(), GenLabel.BestKindLabel(sitePart.parms.animalKind, Gender.None, true, animalsCount));
		}

		// Token: 0x06009580 RID: 38272 RVA: 0x00365ED0 File Offset: 0x003640D0
		private int GetAnimalsCount(SitePartParams parms)
		{
			return AggressiveAnimalIncidentUtility.GetAnimalsCount(parms.animalKind, parms.threatPoints);
		}
	}
}
