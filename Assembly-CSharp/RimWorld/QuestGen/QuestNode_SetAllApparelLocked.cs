using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002235 RID: 8757
	public class QuestNode_SetAllApparelLocked : QuestNode
	{
		// Token: 0x0600D8D1 RID: 55505 RVA: 0x005056D3 File Offset: 0x005038D3
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8D2 RID: 55506 RVA: 0x005056D8 File Offset: 0x005038D8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_SetAllApparelLocked questPart_SetAllApparelLocked = new QuestPart_SetAllApparelLocked();
			questPart_SetAllApparelLocked.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_SetAllApparelLocked.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_SetAllApparelLocked);
		}

		// Token: 0x04008A79 RID: 35449
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A7A RID: 35450
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
