using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02002226 RID: 8742
	public class QuestNode_PawnsArrive : QuestNode
	{
		// Token: 0x0600D8A2 RID: 55458 RVA: 0x00504AD9 File Offset: 0x00502CD9
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x0600D8A3 RID: 55459 RVA: 0x00504AEC File Offset: 0x00502CEC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			PawnsArrivalModeDef pawnsArrivalModeDef = this.arrivalMode.GetValue(slate) ?? PawnsArrivalModeDefOf.EdgeWalkIn;
			QuestPart_PawnsArrive pawnsArrive = new QuestPart_PawnsArrive();
			pawnsArrive.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			pawnsArrive.pawns.AddRange(this.pawns.GetValue(slate));
			pawnsArrive.arrivalMode = pawnsArrivalModeDef;
			pawnsArrive.joinPlayer = this.joinPlayer.GetValue(slate);
			pawnsArrive.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			pawnsArrive.customLetterDef = this.customLetterDef.GetValue(slate);
			if (pawnsArrivalModeDef.walkIn)
			{
				pawnsArrive.spawnNear = (this.walkInSpot.GetValue(slate) ?? (QuestGen.slate.Get<IntVec3?>("walkInSpot", null, false) ?? IntVec3.Invalid));
			}
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					pawnsArrive.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			QuestGen.quest.AddPart(pawnsArrive);
			if (this.isSingleReward.GetValue(slate))
			{
				QuestPart_Choice questPart_Choice = new QuestPart_Choice();
				questPart_Choice.inSignalChoiceUsed = pawnsArrive.inSignal;
				QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
				choice.questParts.Add(pawnsArrive);
				foreach (Pawn pawn in pawnsArrive.pawns)
				{
					choice.rewards.Add(new Reward_Pawn
					{
						pawn = pawn,
						detailsHidden = this.rewardDetailsHidden.GetValue(slate)
					});
				}
				questPart_Choice.choices.Add(choice);
				QuestGen.quest.AddPart(questPart_Choice);
			}
		}

		// Token: 0x04008A44 RID: 35396
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A45 RID: 35397
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04008A46 RID: 35398
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x04008A47 RID: 35399
		public SlateRef<bool> joinPlayer;

		// Token: 0x04008A48 RID: 35400
		public SlateRef<IntVec3?> walkInSpot;

		// Token: 0x04008A49 RID: 35401
		public SlateRef<string> customLetterLabel;

		// Token: 0x04008A4A RID: 35402
		public SlateRef<string> customLetterText;

		// Token: 0x04008A4B RID: 35403
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04008A4C RID: 35404
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04008A4D RID: 35405
		public SlateRef<LetterDef> customLetterDef;

		// Token: 0x04008A4E RID: 35406
		public SlateRef<bool> isSingleReward;

		// Token: 0x04008A4F RID: 35407
		public SlateRef<bool> rewardDetailsHidden;

		// Token: 0x04008A50 RID: 35408
		private const string RootSymbol = "root";
	}
}
