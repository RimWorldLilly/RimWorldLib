using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200222B RID: 8747
	public class QuestNode_ReleaseParalyzedAnimals : QuestNode
	{
		// Token: 0x0600D8B3 RID: 55475 RVA: 0x0050520A File Offset: 0x0050340A
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8B4 RID: 55476 RVA: 0x00505210 File Offset: 0x00503410
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> value = this.pawns.GetValue(slate);
			if (value.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_ReleaseParalyzedAnimals questPart_ReleaseParalyzedAnimals = new QuestPart_ReleaseParalyzedAnimals();
			questPart_ReleaseParalyzedAnimals.pawns.AddRange(value);
			QuestGen.quest.AddPart(questPart_ReleaseParalyzedAnimals);
		}

		// Token: 0x04008A62 RID: 35426
		public SlateRef<IEnumerable<Pawn>> pawns;
	}
}
