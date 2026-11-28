using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200217B RID: 8571
	public class QuestNode_Less : QuestNode
	{
		// Token: 0x0600D640 RID: 54848 RVA: 0x004F9788 File Offset: 0x004F7988
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D641 RID: 54849 RVA: 0x004F97DC File Offset: 0x004F79DC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value1.GetValue(slate) < this.value2.GetValue(slate))
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

		// Token: 0x0400877E RID: 34686
		public SlateRef<double> value1;

		// Token: 0x0400877F RID: 34687
		public SlateRef<double> value2;

		// Token: 0x04008780 RID: 34688
		public QuestNode node;

		// Token: 0x04008781 RID: 34689
		public QuestNode elseNode;
	}
}
