using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002172 RID: 8562
	public class QuestNode_GreaterOrEqual : QuestNode
	{
		// Token: 0x0600D625 RID: 54821 RVA: 0x004F9210 File Offset: 0x004F7410
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) >= this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D626 RID: 54822 RVA: 0x004F9264 File Offset: 0x004F7464
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) >= this.value2.GetValue(slate))
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

		// Token: 0x04008762 RID: 34658
		public SlateRef<double> value1;

		// Token: 0x04008763 RID: 34659
		public SlateRef<double> value2;

		// Token: 0x04008764 RID: 34660
		public QuestNode node;

		// Token: 0x04008765 RID: 34661
		public QuestNode elseNode;
	}
}
