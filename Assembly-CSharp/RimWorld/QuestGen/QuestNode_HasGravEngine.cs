using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02002174 RID: 8564
	public class QuestNode_HasGravEngine : QuestNode
	{
		// Token: 0x0600D62B RID: 54827 RVA: 0x004F933C File Offset: 0x004F753C
		protected override bool TestRunInt(Slate slate)
		{
			if (GravshipUtility.PlayerHasGravEngine())
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D62C RID: 54828 RVA: 0x004F9372 File Offset: 0x004F7572
		protected override void RunInt()
		{
			if (GravshipUtility.PlayerHasGravEngine())
			{
				QuestNode questNode = this.node;
				if (questNode == null)
				{
					return;
				}
				questNode.Run();
				return;
			}
			else
			{
				QuestNode questNode2 = this.elseNode;
				if (questNode2 == null)
				{
					return;
				}
				questNode2.Run();
				return;
			}
		}

		// Token: 0x04008769 RID: 34665
		public QuestNode node;

		// Token: 0x0400876A RID: 34666
		public QuestNode elseNode;
	}
}
