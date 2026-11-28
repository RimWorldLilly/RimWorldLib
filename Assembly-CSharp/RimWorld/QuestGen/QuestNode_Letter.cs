using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200221F RID: 8735
	public class QuestNode_Letter : QuestNode
	{
		// Token: 0x0600D88D RID: 55437 RVA: 0x00504468 File Offset: 0x00502668
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D88E RID: 55438 RVA: 0x0050446C File Offset: 0x0050266C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Letter questPart_Letter = new QuestPart_Letter();
			questPart_Letter.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			LetterDef letterDef = this.letterDef.GetValue(slate) ?? LetterDefOf.NeutralEvent;
			if (typeof(ChoiceLetter).IsAssignableFrom(letterDef.letterClass))
			{
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter("error", "error", letterDef, QuestGenUtility.ToLookTargets(this.lookTargets, slate), this.relatedFaction.GetValue(slate), QuestGen.quest, null);
				questPart_Letter.letter = choiceLetter;
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.Label = x;
				}, QuestGenUtility.MergeRules(this.labelRules.GetValue(slate), this.label.GetValue(slate), "root"));
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					choiceLetter.Text = x;
				}, QuestGenUtility.MergeRules(this.textRules.GetValue(slate), this.text.GetValue(slate), "root"));
			}
			else
			{
				questPart_Letter.letter = LetterMaker.MakeLetter(letterDef);
				questPart_Letter.letter.lookTargets = QuestGenUtility.ToLookTargets(this.lookTargets, slate);
				questPart_Letter.letter.relatedFaction = this.relatedFaction.GetValue(slate);
			}
			questPart_Letter.chosenPawnSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.chosenPawnSignal.GetValue(slate));
			questPart_Letter.useColonistsOnMap = this.useColonistsOnMap.GetValue(slate);
			questPart_Letter.useColonistsFromCaravanArg = this.useColonistsFromCaravanArg.GetValue(slate);
			questPart_Letter.acceptedVisitorsSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.acceptedVisitorsSignal.GetValue(slate));
			questPart_Letter.visitors = this.visitors.GetValue(slate);
			questPart_Letter.signalListenMode = this.signalListenMode.GetValue(slate).GetValueOrDefault();
			questPart_Letter.filterDeadPawnsFromLookTargets = this.filterDeadPawnsFromLookTargets.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Letter);
		}

		// Token: 0x04008A1C RID: 35356
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A1D RID: 35357
		public SlateRef<Faction> relatedFaction;

		// Token: 0x04008A1E RID: 35358
		public SlateRef<LetterDef> letterDef;

		// Token: 0x04008A1F RID: 35359
		public SlateRef<string> label;

		// Token: 0x04008A20 RID: 35360
		public SlateRef<string> text;

		// Token: 0x04008A21 RID: 35361
		public SlateRef<RulePack> labelRules;

		// Token: 0x04008A22 RID: 35362
		public SlateRef<RulePack> textRules;

		// Token: 0x04008A23 RID: 35363
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x04008A24 RID: 35364
		public SlateRef<QuestPart.SignalListenMode?> signalListenMode;

		// Token: 0x04008A25 RID: 35365
		[NoTranslate]
		public SlateRef<string> chosenPawnSignal;

		// Token: 0x04008A26 RID: 35366
		public SlateRef<MapParent> useColonistsOnMap;

		// Token: 0x04008A27 RID: 35367
		public SlateRef<bool> useColonistsFromCaravanArg;

		// Token: 0x04008A28 RID: 35368
		[NoTranslate]
		public SlateRef<string> acceptedVisitorsSignal;

		// Token: 0x04008A29 RID: 35369
		public SlateRef<List<Pawn>> visitors;

		// Token: 0x04008A2A RID: 35370
		public SlateRef<bool> filterDeadPawnsFromLookTargets;

		// Token: 0x04008A2B RID: 35371
		private const string RootSymbol = "root";
	}
}
