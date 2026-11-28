using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021FA RID: 8698
	public class QuestNode_AddHediff : QuestNode
	{
		// Token: 0x0600D814 RID: 55316 RVA: 0x005024BB File Offset: 0x005006BB
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D815 RID: 55317 RVA: 0x005024C0 File Offset: 0x005006C0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null || this.hediffDef.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_AddHediff questPart_AddHediff = new QuestPart_AddHediff();
			questPart_AddHediff.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_AddHediff.hediffDef = this.hediffDef.GetValue(slate);
			questPart_AddHediff.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_AddHediff.checkDiseaseContractChance = this.checkDiseaseContractChance.GetValue(slate);
			if (this.partsToAffect.GetValue(slate) != null)
			{
				questPart_AddHediff.partsToAffect = new List<BodyPartDef>();
				questPart_AddHediff.partsToAffect.AddRange(this.partsToAffect.GetValue(slate));
			}
			questPart_AddHediff.addToHyperlinks = this.addToHyperlinks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_AddHediff);
		}

		// Token: 0x0400896F RID: 35183
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008970 RID: 35184
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04008971 RID: 35185
		public SlateRef<HediffDef> hediffDef;

		// Token: 0x04008972 RID: 35186
		public SlateRef<IEnumerable<BodyPartDef>> partsToAffect;

		// Token: 0x04008973 RID: 35187
		public SlateRef<bool> checkDiseaseContractChance;

		// Token: 0x04008974 RID: 35188
		public SlateRef<bool> addToHyperlinks;
	}
}
