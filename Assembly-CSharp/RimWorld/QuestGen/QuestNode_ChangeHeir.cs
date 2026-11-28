using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002206 RID: 8710
	public class QuestNode_ChangeHeir : QuestNode
	{
		// Token: 0x0600D839 RID: 55353 RVA: 0x00502D7C File Offset: 0x00500F7C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_ChangeHeir questPart_ChangeHeir = new QuestPart_ChangeHeir();
			questPart_ChangeHeir.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_ChangeHeir.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_ChangeHeir.holder = this.holder.GetValue(slate);
			questPart_ChangeHeir.heir = this.heir.GetValue(slate);
			QuestGen.quest.AddPart(questPart_ChangeHeir);
		}

		// Token: 0x0600D83A RID: 55354 RVA: 0x00502E17 File Offset: 0x00501017
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400899B RID: 35227
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400899C RID: 35228
		public SlateRef<Faction> faction;

		// Token: 0x0400899D RID: 35229
		public SlateRef<Thing> factionOf;

		// Token: 0x0400899E RID: 35230
		public SlateRef<Pawn> holder;

		// Token: 0x0400899F RID: 35231
		public SlateRef<Pawn> heir;
	}
}
