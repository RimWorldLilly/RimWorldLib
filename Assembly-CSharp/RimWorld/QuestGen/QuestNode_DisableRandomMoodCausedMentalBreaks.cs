using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200220C RID: 8716
	public class QuestNode_DisableRandomMoodCausedMentalBreaks : QuestNode
	{
		// Token: 0x0600D84B RID: 55371 RVA: 0x005030A6 File Offset: 0x005012A6
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D84C RID: 55372 RVA: 0x005030AC File Offset: 0x005012AC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_DisableRandomMoodCausedMentalBreaks questPart_DisableRandomMoodCausedMentalBreaks = new QuestPart_DisableRandomMoodCausedMentalBreaks();
			questPart_DisableRandomMoodCausedMentalBreaks.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_DisableRandomMoodCausedMentalBreaks.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_DisableRandomMoodCausedMentalBreaks.pawns.AddRange(value);
			QuestGen.quest.AddPart(questPart_DisableRandomMoodCausedMentalBreaks);
		}

		// Token: 0x040089AC RID: 35244
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040089AD RID: 35245
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040089AE RID: 35246
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
