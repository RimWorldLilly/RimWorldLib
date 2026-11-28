using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002209 RID: 8713
	public class QuestNode_DestroyOrPassToWorld : QuestNode
	{
		// Token: 0x0600D842 RID: 55362 RVA: 0x00502F55 File Offset: 0x00501155
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D843 RID: 55363 RVA: 0x00502F58 File Offset: 0x00501158
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_DestroyThingsOrPassToWorld questPart_DestroyThingsOrPassToWorld = new QuestPart_DestroyThingsOrPassToWorld();
			questPart_DestroyThingsOrPassToWorld.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_DestroyThingsOrPassToWorld.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_DestroyThingsOrPassToWorld);
		}

		// Token: 0x040089A7 RID: 35239
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x040089A8 RID: 35240
		public SlateRef<IEnumerable<Thing>> things;
	}
}
