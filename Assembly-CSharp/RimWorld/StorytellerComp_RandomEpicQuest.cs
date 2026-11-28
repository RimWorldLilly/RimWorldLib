using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E1 RID: 4577
	public class StorytellerComp_RandomEpicQuest : StorytellerComp_OnOffCycle
	{
		// Token: 0x06007204 RID: 29188 RVA: 0x0027EA0D File Offset: 0x0027CC0D
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			StorytellerComp_RandomEpicQuest.<MakeIntervalIncidents>d__0 <MakeIntervalIncidents>d__ = new StorytellerComp_RandomEpicQuest.<MakeIntervalIncidents>d__0(-2);
			<MakeIntervalIncidents>d__.<>4__this = this;
			<MakeIntervalIncidents>d__.<>3__target = target;
			return <MakeIntervalIncidents>d__;
		}

		// Token: 0x06007205 RID: 29189 RVA: 0x0027EA24 File Offset: 0x0027CC24
		public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			IncidentParms parms = base.GenerateParms(incCat, target);
			QuestScriptDef questScriptDef;
			if ((from x in DefDatabase<QuestScriptDef>.AllDefs
			where x.IsEpic && x.CanRun(parms.points, target)
			select x).TryRandomElement(out questScriptDef))
			{
				parms.questScriptDef = questScriptDef;
			}
			return parms;
		}
	}
}
