using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200222C RID: 8748
	public class QuestNode_RemoveEquipmentFromPawns : QuestNode
	{
		// Token: 0x0600D8B6 RID: 55478 RVA: 0x0050525E File Offset: 0x0050345E
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8B7 RID: 55479 RVA: 0x00505264 File Offset: 0x00503464
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_RemoveEquipmentFromPawns questPart_RemoveEquipmentFromPawns = new QuestPart_RemoveEquipmentFromPawns();
			questPart_RemoveEquipmentFromPawns.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_RemoveEquipmentFromPawns.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_RemoveEquipmentFromPawns);
		}

		// Token: 0x04008A63 RID: 35427
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A64 RID: 35428
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
