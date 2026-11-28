using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002171 RID: 8561
	public class QuestNode_Greater : QuestNode
	{
		// Token: 0x0600D622 RID: 54818 RVA: 0x004F9160 File Offset: 0x004F7360
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D623 RID: 54819 RVA: 0x004F91B4 File Offset: 0x004F73B4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) > this.value2.GetValue(slate))
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

		// Token: 0x0400875E RID: 34654
		public SlateRef<double> value1;

		// Token: 0x0400875F RID: 34655
		public SlateRef<double> value2;

		// Token: 0x04008760 RID: 34656
		public QuestNode node;

		// Token: 0x04008761 RID: 34657
		public QuestNode elseNode;
	}
}
