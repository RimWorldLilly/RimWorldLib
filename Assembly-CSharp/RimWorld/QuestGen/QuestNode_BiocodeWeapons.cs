using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002201 RID: 8705
	public class QuestNode_BiocodeWeapons : QuestNode
	{
		// Token: 0x0600D829 RID: 55337 RVA: 0x0050299A File Offset: 0x00500B9A
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D82A RID: 55338 RVA: 0x005029A0 File Offset: 0x00500BA0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_BiocodeWeapons questPart_BiocodeWeapons = new QuestPart_BiocodeWeapons();
			questPart_BiocodeWeapons.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_BiocodeWeapons.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_BiocodeWeapons);
		}

		// Token: 0x04008987 RID: 35207
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008988 RID: 35208
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
