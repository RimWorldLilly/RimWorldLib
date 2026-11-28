using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002248 RID: 8776
	public class QuestNode_TradeRequest_Initiate : QuestNode
	{
		// Token: 0x0600D914 RID: 55572 RVA: 0x00506740 File Offset: 0x00504940
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_InitiateTradeRequest part = new QuestPart_InitiateTradeRequest
			{
				settlement = this.settlement.GetValue(slate),
				requestedThingDef = this.requestedThingDef.GetValue(slate),
				requestedCount = this.requestedThingCount.GetValue(slate),
				requestDuration = this.duration.GetValue(slate),
				keepAfterQuestEnds = false,
				inSignal = slate.Get<string>("inSignal", null, false)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x0600D915 RID: 55573 RVA: 0x005067C8 File Offset: 0x005049C8
		protected override bool TestRunInt(Slate slate)
		{
			return this.settlement.GetValue(slate) != null && this.requestedThingCount.GetValue(slate) > 0 && this.requestedThingDef.GetValue(slate) != null && this.duration.GetValue(slate) > 0;
		}

		// Token: 0x04008ABA RID: 35514
		public SlateRef<Settlement> settlement;

		// Token: 0x04008ABB RID: 35515
		public SlateRef<ThingDef> requestedThingDef;

		// Token: 0x04008ABC RID: 35516
		public SlateRef<int> requestedThingCount;

		// Token: 0x04008ABD RID: 35517
		public SlateRef<int> duration;
	}
}
