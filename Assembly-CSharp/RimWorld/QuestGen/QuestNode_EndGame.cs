using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200220F RID: 8719
	public class QuestNode_EndGame : QuestNode
	{
		// Token: 0x0600D854 RID: 55380 RVA: 0x005034A2 File Offset: 0x005016A2
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D855 RID: 55381 RVA: 0x005034A8 File Offset: 0x005016A8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_EndGame endGame = new QuestPart_EndGame();
			endGame.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			endGame.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.introText = x;
			}, QuestGenUtility.MergeRules(this.introTextRules.GetValue(slate), this.introText.GetValue(slate), "root"));
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				endGame.endingText = x;
			}, QuestGenUtility.MergeRules(this.endingTextRules.GetValue(slate), this.endingText.GetValue(slate), "root"));
			QuestGen.quest.AddPart(endGame);
		}

		// Token: 0x040089C1 RID: 35265
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089C2 RID: 35266
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x040089C3 RID: 35267
		public SlateRef<string> introText;

		// Token: 0x040089C4 RID: 35268
		public SlateRef<string> endingText;

		// Token: 0x040089C5 RID: 35269
		public SlateRef<RulePack> introTextRules;

		// Token: 0x040089C6 RID: 35270
		public SlateRef<RulePack> endingTextRules;

		// Token: 0x040089C7 RID: 35271
		private const string RootSymbol = "root";
	}
}
