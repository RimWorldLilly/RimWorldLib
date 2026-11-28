using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002232 RID: 8754
	public class QuestNode_RequirementsToAcceptResearch : QuestNode
	{
		// Token: 0x0600D8C8 RID: 55496 RVA: 0x00505580 File Offset: 0x00503780
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.quest.AddPart(new QuestPart_RequirementsToAcceptResearch
			{
				project = this.reserach.GetValue(slate)
			});
		}

		// Token: 0x0600D8C9 RID: 55497 RVA: 0x005055B4 File Offset: 0x005037B4
		protected override bool TestRunInt(Slate slate)
		{
			ResearchProjectDef value = this.reserach.GetValue(slate);
			return value == null || value.IsFinished;
		}

		// Token: 0x04008A73 RID: 35443
		public SlateRef<ResearchProjectDef> reserach;
	}
}
