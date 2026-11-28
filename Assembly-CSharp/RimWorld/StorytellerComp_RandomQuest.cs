using System;

namespace RimWorld
{
	// Token: 0x020011E6 RID: 4582
	public class StorytellerComp_RandomQuest : StorytellerComp_OnOffCycle
	{
		// Token: 0x06007211 RID: 29201 RVA: 0x0027EC54 File Offset: 0x0027CE54
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms incidentParms = base.GenerateParms(incCat, target);
			incidentParms.questScriptDef = NaturalRandomQuestChooser.ChooseNaturalRandomQuest(incidentParms.points, target);
			return incidentParms;
		}
	}
}
