using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002175 RID: 8565
	public class QuestNode_IsInList : QuestNode
	{
		// Token: 0x0600D62E RID: 54830 RVA: 0x004F93A4 File Offset: 0x004F75A4
		protected override bool TestRunInt(Slate slate)
		{
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D62F RID: 54831 RVA: 0x004F9400 File Offset: 0x004F7600
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (QuestGenUtility.IsInList(slate, this.name.GetValue(slate), this.value.GetValue(slate)))
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

		// Token: 0x0400876B RID: 34667
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400876C RID: 34668
		public SlateRef<object> value;

		// Token: 0x0400876D RID: 34669
		public QuestNode node;

		// Token: 0x0400876E RID: 34670
		public QuestNode elseNode;
	}
}
