using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200217D RID: 8573
	public class QuestNode_LessOrFail : QuestNode
	{
		// Token: 0x0600D646 RID: 54854 RVA: 0x004F98E8 File Offset: 0x004F7AE8
		protected override bool TestRunInt(Slate slate)
		{
			return this.value1.GetValue(slate) < this.value2.GetValue(slate) && (this.node == null || this.node.TestRun(slate));
		}

		// Token: 0x0600D647 RID: 54855 RVA: 0x004F991C File Offset: 0x004F7B1C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate) && this.node != null)
			{
				this.node.Run();
			}
		}

		// Token: 0x04008786 RID: 34694
		public SlateRef<double> value1;

		// Token: 0x04008787 RID: 34695
		public SlateRef<double> value2;

		// Token: 0x04008788 RID: 34696
		public QuestNode node;
	}
}
