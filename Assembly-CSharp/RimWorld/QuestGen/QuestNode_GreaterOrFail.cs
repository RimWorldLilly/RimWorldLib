using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002173 RID: 8563
	public class QuestNode_GreaterOrFail : QuestNode
	{
		// Token: 0x0600D628 RID: 54824 RVA: 0x004F92C0 File Offset: 0x004F74C0
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) > this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600D629 RID: 54825 RVA: 0x004F92F4 File Offset: 0x004F74F4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x04008766 RID: 34662
		public SlateRef<double> value1;

		// Token: 0x04008767 RID: 34663
		public SlateRef<double> value2;

		// Token: 0x04008768 RID: 34664
		public QuestNode node;
	}
}
