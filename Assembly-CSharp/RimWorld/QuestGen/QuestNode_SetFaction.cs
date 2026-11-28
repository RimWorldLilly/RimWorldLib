using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002238 RID: 8760
	public class QuestNode_SetFaction : QuestNode
	{
		// Token: 0x0600D8DA RID: 55514 RVA: 0x005057F7 File Offset: 0x005039F7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8DB RID: 55515 RVA: 0x005057FC File Offset: 0x005039FC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_SetFaction questPart_SetFaction = new QuestPart_SetFaction();
			questPart_SetFaction.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetFaction.faction = this.faction.GetValue(slate);
			questPart_SetFaction.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_SetFaction);
		}

		// Token: 0x04008A7D RID: 35453
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A7E RID: 35454
		public SlateRef<Faction> faction;

		// Token: 0x04008A7F RID: 35455
		public SlateRef<IEnumerable<Thing>> things;
	}
}
