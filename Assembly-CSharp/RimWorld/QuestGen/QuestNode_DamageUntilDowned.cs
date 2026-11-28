using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002208 RID: 8712
	public class QuestNode_DamageUntilDowned : QuestNode
	{
		// Token: 0x0600D83F RID: 55359 RVA: 0x00502EB6 File Offset: 0x005010B6
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D840 RID: 55360 RVA: 0x00502EBC File Offset: 0x005010BC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_DamageUntilDowned questPart_DamageUntilDowned = new QuestPart_DamageUntilDowned();
			questPart_DamageUntilDowned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DamageUntilDowned.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_DamageUntilDowned.allowBleedingWounds = this.allowBleedingWounds.GetValue(slate).GetValueOrDefault(true);
			QuestGen.quest.AddPart(questPart_DamageUntilDowned);
		}

		// Token: 0x040089A4 RID: 35236
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089A5 RID: 35237
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x040089A6 RID: 35238
		public SlateRef<bool?> allowBleedingWounds;
	}
}
