using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001770 RID: 6000
	public class SitePartWorker_ConditionCauser : SitePartWorker
	{
		// Token: 0x0600956B RID: 38251 RVA: 0x003659EC File Offset: 0x00363BEC
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			int worldRange = sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange;
			return base.GetPostProcessedThreatLabel(site, sitePart) + " (" + "ConditionCauserRadius".Translate(worldRange) + ")";
		}

		// Token: 0x0600956C RID: 38252 RVA: 0x00365A48 File Offset: 0x00363C48
		public override void Init(Site site, SitePart sitePart)
		{
			sitePart.conditionCauser = ThingMaker.MakeThing(sitePart.def.conditionCauserDef, null);
			sitePart.conditionCauser.SetFaction(site.Faction, null);
			CompCauseGameCondition compCauseGameCondition = sitePart.conditionCauser.TryGetComp<CompCauseGameCondition>();
			compCauseGameCondition.RandomizeSettings(site);
			compCauseGameCondition.LinkWithSite(sitePart.site);
		}

		// Token: 0x0600956D RID: 38253 RVA: 0x00365A9B File Offset: 0x00363C9B
		public override void SitePartWorkerTick(SitePart sitePart)
		{
			if (!sitePart.conditionCauser.DestroyedOrNull() && !sitePart.conditionCauser.Spawned)
			{
				sitePart.conditionCauser.DoTick();
			}
		}

		// Token: 0x0600956E RID: 38254 RVA: 0x00365AC2 File Offset: 0x00363CC2
		public override void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
			base.PostDrawExtraSelectionOverlays(sitePart);
			GenDraw.DrawWorldRadiusRing(sitePart.site.Tile, sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange, null);
		}

		// Token: 0x0600956F RID: 38255 RVA: 0x00365AF4 File Offset: 0x00363CF4
		public override void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
			base.Notify_SiteMapAboutToBeRemoved(sitePart);
			if (!sitePart.conditionCauser.DestroyedOrNull() && sitePart.conditionCauser.Spawned && sitePart.conditionCauser.Map == sitePart.site.Map)
			{
				sitePart.conditionCauser.DeSpawn(DestroyMode.Vanish);
				sitePart.conditionCauserWasSpawned = false;
			}
		}

		// Token: 0x06009570 RID: 38256 RVA: 0x00365B4D File Offset: 0x00363D4D
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			slate.Set<Thing>("conditionCauser", part.conditionCauser, false);
			outExtraDescriptionRules.Add(new Rule_String("problemCauserLabel", part.conditionCauser.Label));
		}
	}
}
