using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200216E RID: 8558
	public class QuestNode_Equal : QuestNode
	{
		// Token: 0x0600D61B RID: 54811 RVA: 0x004F8F60 File Offset: 0x004F7160
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D61C RID: 54812 RVA: 0x004F8FC8 File Offset: 0x004F71C8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestNodeEqualUtility.Equal(this.value1.GetValue(slate), this.value2.GetValue(slate), this.compareAs.GetValue(slate)))
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

		// Token: 0x04008755 RID: 34645
		[NoTranslate]
		public SlateRef<object> value1;

		// Token: 0x04008756 RID: 34646
		[NoTranslate]
		public SlateRef<object> value2;

		// Token: 0x04008757 RID: 34647
		public SlateRef<Type> compareAs;

		// Token: 0x04008758 RID: 34648
		public QuestNode node;

		// Token: 0x04008759 RID: 34649
		public QuestNode elseNode;
	}
}
