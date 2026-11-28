using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002176 RID: 8566
	public class QuestNode_IsNull : QuestNode
	{
		// Token: 0x0600D631 RID: 54833 RVA: 0x004F9462 File Offset: 0x004F7662
		protected override bool TestRunInt(Slate slate)
		{
			if (this.value.GetValue(slate) == null)
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D632 RID: 54834 RVA: 0x004F94A0 File Offset: 0x004F76A0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.value.GetValue(slate) == null)
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

		// Token: 0x0400876F RID: 34671
		[NoTranslate]
		public SlateRef<object> value;

		// Token: 0x04008770 RID: 34672
		public QuestNode node;

		// Token: 0x04008771 RID: 34673
		public QuestNode elseNode;
	}
}
