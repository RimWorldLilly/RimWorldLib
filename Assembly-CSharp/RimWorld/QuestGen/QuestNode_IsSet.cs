using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002177 RID: 8567
	public class QuestNode_IsSet : QuestNode
	{
		// Token: 0x0600D634 RID: 54836 RVA: 0x004F94F0 File Offset: 0x004F76F0
		protected override bool TestRunInt(Slate slate)
		{
			if (slate.Exists(this.name.GetValue(slate), false))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D635 RID: 54837 RVA: 0x004F9540 File Offset: 0x004F7740
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGen.slate.Exists(this.name.GetValue(slate), false))
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

		// Token: 0x04008772 RID: 34674
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x04008773 RID: 34675
		public QuestNode node;

		// Token: 0x04008774 RID: 34676
		public QuestNode elseNode;
	}
}
