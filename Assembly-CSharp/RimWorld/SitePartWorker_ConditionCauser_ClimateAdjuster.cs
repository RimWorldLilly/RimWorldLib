using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001772 RID: 6002
	public class SitePartWorker_ConditionCauser_ClimateAdjuster : SitePartWorker_ConditionCauser
	{
		// Token: 0x06009574 RID: 38260 RVA: 0x00365BD4 File Offset: 0x00363DD4
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string output = part.conditionCauser.TryGetComp<CompCauseGameCondition_TemperatureOffset>().temperatureOffset.ToStringTemperatureOffset("F1");
			outExtraDescriptionRules.Add(new Rule_String("temperatureOffset", output));
		}
	}
}
