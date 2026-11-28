using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001771 RID: 6001
	public class SitePartWorker_ConditionCauser_WeatherController : SitePartWorker_ConditionCauser
	{
		// Token: 0x06009572 RID: 38258 RVA: 0x00365B90 File Offset: 0x00363D90
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			WeatherDef weather = part.conditionCauser.TryGetComp<CompCauseGameCondition_ForceWeather>().weather;
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("weather", weather));
		}
	}
}
