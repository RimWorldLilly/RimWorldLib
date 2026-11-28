using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200220A RID: 8714
	public class QuestNode_DestroyOrPassToWorldOnCleanup : QuestNode
	{
		// Token: 0x0600D845 RID: 55365 RVA: 0x00502FDB File Offset: 0x005011DB
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D846 RID: 55366 RVA: 0x00502FE0 File Offset: 0x005011E0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.things.GetValue(slate).EnumerableNullOrEmpty<Thing>())
			{
				return;
			}
			QuestPart_DestroyThingsOrPassToWorldOnCleanup questPart_DestroyThingsOrPassToWorldOnCleanup = new QuestPart_DestroyThingsOrPassToWorldOnCleanup();
			questPart_DestroyThingsOrPassToWorldOnCleanup.things.AddRange(this.things.GetValue(slate));
			QuestGen.quest.AddPart(questPart_DestroyThingsOrPassToWorldOnCleanup);
		}

		// Token: 0x040089A9 RID: 35241
		public SlateRef<IEnumerable<Thing>> things;
	}
}
