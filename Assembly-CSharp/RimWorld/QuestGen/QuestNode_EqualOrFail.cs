using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002170 RID: 8560
	public class QuestNode_EqualOrFail : QuestNode
	{
		// Token: 0x0600D61F RID: 54815 RVA: 0x004F90B4 File Offset: 0x004F72B4
		protected override bool TestRunInt(Slate slate)
		{
			return QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600D620 RID: 54816 RVA: 0x004F9104 File Offset: 0x004F7304
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x0400875A RID: 34650
		public SlateRef<object> value1;

		// Token: 0x0400875B RID: 34651
		public SlateRef<object> value2;

		// Token: 0x0400875C RID: 34652
		public SlateRef<Type> compareAs;

		// Token: 0x0400875D RID: 34653
		public QuestNode node;
	}
}
