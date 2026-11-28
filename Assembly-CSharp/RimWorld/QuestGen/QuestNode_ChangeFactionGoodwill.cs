using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002203 RID: 8707
	public class QuestNode_ChangeFactionGoodwill : QuestNode
	{
		// Token: 0x0600D82F RID: 55343 RVA: 0x00502A27 File Offset: 0x00500C27
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D830 RID: 55344 RVA: 0x00502A2C File Offset: 0x00500C2C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_FactionGoodwillChange questPart_FactionGoodwillChange = new QuestPart_FactionGoodwillChange();
			questPart_FactionGoodwillChange.change = this.change.GetValue(slate);
			questPart_FactionGoodwillChange.faction = (this.faction.GetValue(slate) ?? this.factionOf.GetValue(slate).Faction);
			questPart_FactionGoodwillChange.canSendHostilityLetter = this.canSendLetter.GetValue(slate).GetValueOrDefault(true);
			questPart_FactionGoodwillChange.canSendMessage = this.canSendMessage.GetValue(slate).GetValueOrDefault(true);
			questPart_FactionGoodwillChange.ensureMakesHostile = this.ensureHostile.GetValue(slate);
			questPart_FactionGoodwillChange.historyEvent = this.reason.GetValue(slate);
			questPart_FactionGoodwillChange.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_FactionGoodwillChange);
		}

		// Token: 0x04008989 RID: 35209
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400898A RID: 35210
		public SlateRef<Faction> faction;

		// Token: 0x0400898B RID: 35211
		public SlateRef<Thing> factionOf;

		// Token: 0x0400898C RID: 35212
		public SlateRef<int> change;

		// Token: 0x0400898D RID: 35213
		public SlateRef<bool?> canSendLetter;

		// Token: 0x0400898E RID: 35214
		public SlateRef<bool?> canSendMessage;

		// Token: 0x0400898F RID: 35215
		public SlateRef<bool> ensureHostile;

		// Token: 0x04008990 RID: 35216
		public SlateRef<HistoryEventDef> reason;
	}
}
