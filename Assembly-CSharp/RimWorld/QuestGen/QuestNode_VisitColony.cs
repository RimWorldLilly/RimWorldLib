using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021E6 RID: 8678
	public class QuestNode_VisitColony : QuestNode
	{
		// Token: 0x0600D7CC RID: 55244 RVA: 0x00501860 File Offset: 0x004FFA60
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D7CD RID: 55245 RVA: 0x00501864 File Offset: 0x004FFA64
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate).EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			QuestPart_VisitColony questPart_VisitColony = new QuestPart_VisitColony();
			questPart_VisitColony.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_VisitColony.pawns.AddRange(this.pawns.GetValue(slate));
			questPart_VisitColony.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_VisitColony.faction = this.faction.GetValue(slate);
			questPart_VisitColony.durationTicks = this.durationTicks.GetValue(slate);
			QuestGen.quest.AddPart(questPart_VisitColony);
		}

		// Token: 0x04008940 RID: 35136
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008941 RID: 35137
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04008942 RID: 35138
		public SlateRef<Faction> faction;

		// Token: 0x04008943 RID: 35139
		public SlateRef<int?> durationTicks;
	}
}
