using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C2 RID: 4546
	public class IncidentWorker_GiveQuest_Map : IncidentWorker_GiveQuest
	{
		// Token: 0x0600718B RID: 29067 RVA: 0x0027C998 File Offset: 0x0027AB98
		protected override void GiveQuest(IncidentParms parms, QuestScriptDef questDef)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", parms.points, false);
			slate.Set<Map>("map", (Map)parms.target, false);
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(questDef, slate);
			if (quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
		}
	}
}
