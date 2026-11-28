using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022C5 RID: 8901
	public class QuestNode_RandomRaid : QuestNode
	{
		// Token: 0x0600DB8F RID: 56207 RVA: 0x0051D29C File Offset: 0x0051B49C
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.allowViolentQuests && slate.Exists("map", false) && (slate.Exists("enemyFaction", false) || this.faction.GetValue(slate) != null);
		}

		// Token: 0x0600DB90 RID: 56208 RVA: 0x0051D2EC File Offset: 0x0051B4EC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			float val = QuestGen.slate.Get<float>("points", 0f, false);
			Faction faction = this.faction.GetValue(slate) ?? QuestGen.slate.Get<Faction>("enemyFaction", null, false);
			QuestPart_RandomRaid randomRaid = new QuestPart_RandomRaid();
			randomRaid.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			randomRaid.mapParent = map.Parent;
			randomRaid.faction = faction;
			randomRaid.pointsRange = QuestNode_RandomRaid.RaidPointsRandomFactor * val;
			randomRaid.useCurrentThreatPoints = this.useCurrentThreatPoints.GetValue(slate);
			randomRaid.currentThreatPointsFactor = this.currentThreatPointsFactor.GetValue(slate).GetValueOrDefault(1f);
			if (this.arrivalMode.GetValue(slate) != null)
			{
				randomRaid.arrivalMode = this.arrivalMode.GetValue(slate);
			}
			if (!this.customLetterLabel.GetValue(slate).NullOrEmpty() || this.customLetterLabelRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					randomRaid.customLetterLabel = x;
				}, QuestGenUtility.MergeRules(this.customLetterLabelRules.GetValue(slate), this.customLetterLabel.GetValue(slate), "root"));
			}
			if (!this.customLetterText.GetValue(slate).NullOrEmpty() || this.customLetterTextRules.GetValue(slate) != null)
			{
				QuestGen.AddTextRequest("root", delegate(string x)
				{
					randomRaid.customLetterText = x;
				}, QuestGenUtility.MergeRules(this.customLetterTextRules.GetValue(slate), this.customLetterText.GetValue(slate), "root"));
			}
			QuestGen.quest.AddPart(randomRaid);
		}

		// Token: 0x04008CB3 RID: 36019
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008CB4 RID: 36020
		public SlateRef<Faction> faction;

		// Token: 0x04008CB5 RID: 36021
		public SlateRef<bool> useCurrentThreatPoints;

		// Token: 0x04008CB6 RID: 36022
		public SlateRef<float?> currentThreatPointsFactor;

		// Token: 0x04008CB7 RID: 36023
		public SlateRef<PawnsArrivalModeDef> arrivalMode;

		// Token: 0x04008CB8 RID: 36024
		public SlateRef<string> customLetterLabel;

		// Token: 0x04008CB9 RID: 36025
		public SlateRef<string> customLetterText;

		// Token: 0x04008CBA RID: 36026
		public SlateRef<RulePack> customLetterLabelRules;

		// Token: 0x04008CBB RID: 36027
		public SlateRef<RulePack> customLetterTextRules;

		// Token: 0x04008CBC RID: 36028
		private const string RootSymbol = "root";

		// Token: 0x04008CBD RID: 36029
		private static readonly FloatRange RaidPointsRandomFactor = new FloatRange(0.9f, 1.1f);
	}
}
