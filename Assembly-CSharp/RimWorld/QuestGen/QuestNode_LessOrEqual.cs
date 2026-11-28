using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200217C RID: 8572
	public class QuestNode_LessOrEqual : QuestNode
	{
		// Token: 0x0600D643 RID: 54851 RVA: 0x004F9838 File Offset: 0x004F7A38
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) <= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D644 RID: 54852 RVA: 0x004F988C File Offset: 0x004F7A8C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) <= this.value2.GetValue(slate))
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		// Token: 0x04008782 RID: 34690
		public SlateRef<double> value1;

		// Token: 0x04008783 RID: 34691
		public SlateRef<double> value2;

		// Token: 0x04008784 RID: 34692
		public QuestNode node;

		// Token: 0x04008785 RID: 34693
		public QuestNode elseNode;
	}
}
