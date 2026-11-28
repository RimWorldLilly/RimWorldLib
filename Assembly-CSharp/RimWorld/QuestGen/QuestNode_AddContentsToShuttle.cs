using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F9 RID: 8697
	public class QuestNode_AddContentsToShuttle : QuestNode
	{
		// Token: 0x0600D811 RID: 55313 RVA: 0x00502431 File Offset: 0x00500631
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D812 RID: 55314 RVA: 0x00502434 File Offset: 0x00500634
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.contents.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_AddContentsToShuttle questPart_AddContentsToShuttle = new QuestPart_AddContentsToShuttle();
			questPart_AddContentsToShuttle.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			questPart_AddContentsToShuttle.shuttle = this.shuttle.GetValue(slate);
			questPart_AddContentsToShuttle.Things = this.contents.GetValue(slate);
			QuestGen.quest.AddPart(questPart_AddContentsToShuttle);
		}

		// Token: 0x0400896C RID: 35180
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400896D RID: 35181
		public SlateRef<Thing> shuttle;

		// Token: 0x0400896E RID: 35182
		public SlateRef<IEnumerable<Thing>> contents;
	}
}
