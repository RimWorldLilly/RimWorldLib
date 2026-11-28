using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002179 RID: 8569
	public class QuestNode_IsTrueOrUnset : QuestNode
	{
		// Token: 0x0600D63A RID: 54842 RVA: 0x004F9628 File Offset: 0x004F7828
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate).GetValueOrDefault(true))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D63B RID: 54843 RVA: 0x004F967C File Offset: 0x004F787C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate).GetValueOrDefault(true))
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

		// Token: 0x04008778 RID: 34680
		public SlateRef<bool?> value;

		// Token: 0x04008779 RID: 34681
		public QuestNode node;

		// Token: 0x0400877A RID: 34682
		public QuestNode elseNode;
	}
}
