using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200218A RID: 8586
	public class QuestNode_ExpansionActive : QuestNode
	{
		// Token: 0x0600D66F RID: 54895 RVA: 0x004FA834 File Offset: 0x004F8A34
		protected override bool TestRunInt(Slate slate)
		{
			if (ModsConfig.IsActive(this.modID.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D670 RID: 54896 RVA: 0x004FA884 File Offset: 0x004F8A84
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (ModsConfig.IsActive(this.modID.GetValue(slate)))
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

		// Token: 0x040087C1 RID: 34753
		[NoTranslate]
		public SlateRef<string> modID;

		// Token: 0x040087C2 RID: 34754
		public QuestNode node;

		// Token: 0x040087C3 RID: 34755
		public QuestNode elseNode;
	}
}
