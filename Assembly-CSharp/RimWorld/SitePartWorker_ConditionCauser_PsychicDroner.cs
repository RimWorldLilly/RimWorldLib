using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001773 RID: 6003
	public class SitePartWorker_ConditionCauser_PsychicDroner : SitePartWorker_ConditionCauser
	{
		// Token: 0x06009576 RID: 38262 RVA: 0x00365C20 File Offset: 0x00363E20
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string label = part.conditionCauser.TryGetComp<CompCauseGameCondition_PsychicEmanation>().gender.GetLabel(false);
			outExtraDescriptionRules.Add(new Rule_String("affectedGender", label));
		}
	}
}
