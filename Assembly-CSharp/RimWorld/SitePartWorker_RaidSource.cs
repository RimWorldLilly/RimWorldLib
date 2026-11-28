using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200177A RID: 6010
	public class SitePartWorker_RaidSource : SitePartWorker_Outpost
	{
		// Token: 0x0600958F RID: 38287 RVA: 0x003661AC File Offset: 0x003643AC
		public override void SitePartWorkerTick(SitePart sitePart)
		{
			base.SitePartWorkerTick(sitePart);
			if (sitePart.lastRaidTick == -1 || (float)Find.TickManager.TicksGame > (float)sitePart.lastRaidTick + 90000f)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome && sitePart.site.IsHashIntervalTick(2500) && Rand.MTBEventOccurs(QuestTuning.PointsToRaidSourceRaidsMTBDaysCurve.Evaluate(sitePart.parms.threatPoints), 60000f, 2500f))
					{
						this.StartRaid(maps[i], sitePart);
					}
				}
			}
		}

		// Token: 0x06009590 RID: 38288 RVA: 0x00366250 File Offset: 0x00364450
		private void StartRaid(Map map, SitePart sitePart)
		{
			IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, map);
			incidentParms.forced = true;
			incidentParms.points *= 0.6f;
			if (IncidentDefOf.RaidEnemy.Worker.CanFireNow(incidentParms))
			{
				IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms);
				sitePart.lastRaidTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06009591 RID: 38289 RVA: 0x003662B8 File Offset: 0x003644B8
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int enemiesCount = base.GetEnemiesCount(part.site, part.parms);
			float num = QuestTuning.PointsToRaidSourceRaidsMTBDaysCurve.Evaluate(part.parms.threatPoints);
			outExtraDescriptionRules.Add(new Rule_String("enemiesCount", enemiesCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("mtbDays", ((int)(num * 60000f)).ToStringTicksToPeriod(true, false, false, true, false)));
			outExtraDescriptionRules.Add(new Rule_String("enemiesLabel", base.GetEnemiesLabel(part.site, enemiesCount)));
		}
	}
}
