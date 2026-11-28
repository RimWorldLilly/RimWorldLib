using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200217A RID: 8570
	public class QuestNode_IsZero : QuestNode
	{
		// Token: 0x0600D63D RID: 54845 RVA: 0x004F96D8 File Offset: 0x004F78D8
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == 0.0)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D63E RID: 54846 RVA: 0x004F972C File Offset: 0x004F792C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == 0.0)
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

		// Token: 0x0400877B RID: 34683
		public SlateRef<double> value;

		// Token: 0x0400877C RID: 34684
		public QuestNode node;

		// Token: 0x0400877D RID: 34685
		public QuestNode elseNode;
	}
}
