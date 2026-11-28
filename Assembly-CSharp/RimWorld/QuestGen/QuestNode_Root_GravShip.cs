using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002272 RID: 8818
	public class QuestNode_Root_GravShip : QuestNode
	{
		// Token: 0x0600D9CC RID: 55756 RVA: 0x0050CF2C File Offset: 0x0050B12C
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive;
		}

		// Token: 0x0600D9CD RID: 55757 RVA: 0x0050CF38 File Offset: 0x0050B138
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			string inSignalEnable = QuestGen.slate.Get<string>("inSignal", null, false);
			QuestPart_SubquestGenerator_Gravcores questPart_SubquestGenerator_Gravcores = new QuestPart_SubquestGenerator_Gravcores
			{
				inSignalEnable = inSignalEnable,
				maxActiveSubquests = 3
			};
			questPart_SubquestGenerator_Gravcores.subquestDefs.AddRange(this.subquestDefs);
			quest.AddPart(questPart_SubquestGenerator_Gravcores);
			if (Faction.OfMechanoids != null)
			{
				quest.Delay(2700000, delegate
				{
					Quest quest = quest;
					LetterDef neutralEvent = LetterDefOf.NeutralEvent;
					string inSignal = null;
					string chosenPawnSignal = null;
					Faction relatedFaction = null;
					MapParent useColonistsOnMap = null;
					bool useColonistsFromCaravanArg = false;
					QuestPart.SignalListenMode signalListenMode = QuestPart.SignalListenMode.OngoingOnly;
					IEnumerable<object> lookTargets = null;
					bool filterDeadPawnsFromLookTargets = false;
					string label = "LetterLabelMechhiveWarning".Translate();
					quest.Letter(neutralEvent, inSignal, chosenPawnSignal, relatedFaction, useColonistsOnMap, useColonistsFromCaravanArg, signalListenMode, lookTargets, filterDeadPawnsFromLookTargets, "LetterTextMechhiveWarning".Translate(), null, label, null, null);
				}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			}
		}

		// Token: 0x04008B3C RID: 35644
		private const int MechhiveWarningDelayTicks = 2700000;

		// Token: 0x04008B3D RID: 35645
		private List<QuestScriptDef> subquestDefs = new List<QuestScriptDef>();
	}
}
