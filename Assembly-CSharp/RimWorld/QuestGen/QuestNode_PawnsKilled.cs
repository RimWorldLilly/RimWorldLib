using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002227 RID: 8743
	public class QuestNode_PawnsKilled : QuestNode
	{
		// Token: 0x0600D8A5 RID: 55461 RVA: 0x00504DCC File Offset: 0x00502FCC
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600D8A6 RID: 55462 RVA: 0x00504DF8 File Offset: 0x00502FF8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MapParent parent = slate.Get<Map>("map", null, false).Parent;
			string text = QuestGen.GenerateNewSignal("PawnOfRaceKilled", true);
			QuestPart_PawnsKilled questPart_PawnsKilled = new QuestPart_PawnsKilled();
			questPart_PawnsKilled.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_PawnsKilled.race = this.race.GetValue(slate);
			questPart_PawnsKilled.requiredInstigatorFaction = Faction.OfPlayer;
			questPart_PawnsKilled.count = this.count.GetValue(slate);
			questPart_PawnsKilled.mapParent = parent;
			questPart_PawnsKilled.outSignalPawnKilled = text;
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_PawnsKilled);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_PawnsKilled.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_PawnsKilled);
			QuestPart_PawnsAvailable questPart_PawnsAvailable = new QuestPart_PawnsAvailable();
			questPart_PawnsAvailable.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			if (!this.outSignalPawnsNotAvailable.GetValue(slate).NullOrEmpty())
			{
				questPart_PawnsAvailable.outSignalPawnsNotAvailable = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalPawnsNotAvailable.GetValue(slate));
			}
			questPart_PawnsAvailable.race = this.race.GetValue(slate);
			questPart_PawnsAvailable.requiredCount = this.count.GetValue(slate);
			questPart_PawnsAvailable.mapParent = parent;
			questPart_PawnsAvailable.inSignalDecrement = text;
			QuestGen.quest.AddPart(questPart_PawnsAvailable);
		}

		// Token: 0x04008A51 RID: 35409
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x04008A52 RID: 35410
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x04008A53 RID: 35411
		[NoTranslate]
		public SlateRef<string> outSignalPawnsNotAvailable;

		// Token: 0x04008A54 RID: 35412
		public SlateRef<ThingDef> race;

		// Token: 0x04008A55 RID: 35413
		public SlateRef<int> count;

		// Token: 0x04008A56 RID: 35414
		public QuestNode node;

		// Token: 0x04008A57 RID: 35415
		private const string PawnOfRaceKilledSignal = "PawnOfRaceKilled";
	}
}
