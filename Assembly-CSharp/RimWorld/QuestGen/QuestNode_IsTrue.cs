using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002178 RID: 8568
	public class QuestNode_IsTrue : QuestNode
	{
		// Token: 0x0600D637 RID: 54839 RVA: 0x004F959B File Offset: 0x004F779B
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D638 RID: 54840 RVA: 0x004F95D8 File Offset: 0x004F77D8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate))
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

		// Token: 0x04008775 RID: 34677
		public SlateRef<bool> value;

		// Token: 0x04008776 RID: 34678
		public QuestNode node;

		// Token: 0x04008777 RID: 34679
		public QuestNode elseNode;
	}
}
