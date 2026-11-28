using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200177D RID: 6013
	public class SitePartWorker_Turrets : SitePartWorker
	{
		// Token: 0x0600959B RID: 38299 RVA: 0x003664C7 File Offset: 0x003646C7
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x0600959C RID: 38300 RVA: 0x003664E0 File Offset: 0x003646E0
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.mortarsCount = Rand.RangeInclusive(0, 1);
			sitePartParams.turretsCount = Mathf.Clamp(Mathf.RoundToInt(sitePartParams.threatPoints / ThingDefOf.Turret_MiniTurret.building.combatPower), 2, 11);
			return sitePartParams;
		}

		// Token: 0x0600959D RID: 38301 RVA: 0x0036652C File Offset: 0x0036472C
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string threatsInfo = this.GetThreatsInfo(part.parms, part.site.Faction);
			outExtraDescriptionRules.Add(new Rule_String("threatsInfo", threatsInfo));
		}

		// Token: 0x0600959E RID: 38302 RVA: 0x0036656D File Offset: 0x0036476D
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + this.GetThreatsInfo(sitePart.parms, site.Faction);
		}

		// Token: 0x0600959F RID: 38303 RVA: 0x00366594 File Offset: 0x00364794
		private string GetThreatsInfo(SitePartParams parms, Faction faction)
		{
			this.threatsTmp.Clear();
			int num = parms.mortarsCount + 1;
			if (parms.turretsCount != 0)
			{
				string value;
				if (parms.turretsCount == 1)
				{
					value = "Turret".Translate();
				}
				else
				{
					value = "Turrets".Translate();
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.turretsCount.ToString(), value));
			}
			if (parms.mortarsCount != 0)
			{
				string value;
				if (parms.mortarsCount == 1)
				{
					value = "Mortar".Translate();
				}
				else
				{
					value = "Mortars".Translate();
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.mortarsCount.ToString(), value));
			}
			if (num != 0)
			{
				string value;
				if (faction == null)
				{
					value = ((num == 1) ? "Enemy".Translate() : "Enemies".Translate());
				}
				else
				{
					value = ((num == 1) ? faction.def.pawnSingular : faction.def.pawnsPlural);
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(num.ToString(), value));
			}
			return this.threatsTmp.ToCommaList(true, false);
		}

		// Token: 0x04005A19 RID: 23065
		private const int MinTurrets = 2;

		// Token: 0x04005A1A RID: 23066
		private const int MaxTurrets = 11;

		// Token: 0x04005A1B RID: 23067
		private List<string> threatsTmp = new List<string>();
	}
}
