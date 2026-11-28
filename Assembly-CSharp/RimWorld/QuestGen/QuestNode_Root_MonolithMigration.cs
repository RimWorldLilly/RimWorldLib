using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002282 RID: 8834
	public class QuestNode_Root_MonolithMigration : QuestNode
	{
		// Token: 0x0600DA35 RID: 55861 RVA: 0x005118FC File Offset: 0x0050FAFC
		protected override bool TestRunInt(Slate slate)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!Find.Anomaly.GenerateMonolith)
			{
				return false;
			}
			Map map = QuestGen_Get.GetMap(false, null, false);
			return map != null && !map.listerThings.AnyThingWithDef(ThingDefOf.VoidMonolith);
		}

		// Token: 0x0600DA36 RID: 55862 RVA: 0x0051194C File Offset: 0x0050FB4C
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			quest.AcceptanceRequirementNotSpace(map.Parent);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_Unknown());
			questPart_Choice.choices.Add(choice);
			slate.Set<bool>("askerIsNull", true, false);
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("monolithMap");
			QuestUtility.AddQuestTag(ref map.Parent.questTags, questTagToAdd);
			string startSpawnMonolithSignal = QuestGen.GenerateNewSignal("SpawnMonolith", true);
			quest.Delay(QuestNode_Root_MonolithMigration.SpawnDelayRangeTicks.RandomInRange, delegate
			{
				quest.SignalPass(null, null, startSpawnMonolithSignal);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.AddPart(new QuestPart_SpawnMonolith(startSpawnMonolithSignal));
			quest.Delay(EffecterDefOf.VoidStructureIncoming.maintainTicks, delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, startSpawnMonolithSignal, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x04008B80 RID: 35712
		public static readonly IntRange SpawnDelayRangeTicks = new IntRange(2500, 5000);
	}
}
