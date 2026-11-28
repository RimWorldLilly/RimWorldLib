using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200221D RID: 8733
	public class QuestNode_LeaveOnCleanup : QuestNode
	{
		// Token: 0x0600D887 RID: 55431 RVA: 0x005042A6 File Offset: 0x005024A6
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D888 RID: 55432 RVA: 0x005042AC File Offset: 0x005024AC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_Leave questPart_Leave = new QuestPart_Leave();
			questPart_Leave.pawns.AddRange(value);
			questPart_Leave.sendStandardLetter = (this.sendStandardLetter.GetValue(slate) ?? questPart_Leave.sendStandardLetter);
			questPart_Leave.leaveOnCleanup = true;
			questPart_Leave.inSignalRemovePawn = this.inSignalRemovePawn.GetValue(slate);
			QuestGen.quest.AddPart(questPart_Leave);
		}

		// Token: 0x04008A13 RID: 35347
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04008A14 RID: 35348
		public SlateRef<bool?> sendStandardLetter;

		// Token: 0x04008A15 RID: 35349
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;
	}
}
