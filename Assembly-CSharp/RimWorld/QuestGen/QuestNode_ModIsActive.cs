using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F1 RID: 8689
	public class QuestNode_ModIsActive : QuestNode
	{
		// Token: 0x0600D7F7 RID: 55287 RVA: 0x00502020 File Offset: 0x00500220
		protected override bool TestRunInt(Slate slate)
		{
			if (ModsConfig.IsActive(this.modID.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7F8 RID: 55288 RVA: 0x0050206D File Offset: 0x0050026D
		protected override void RunInt()
		{
			if (ModsConfig.IsActive(this.modID.GetValue(QuestGen.slate)))
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

		// Token: 0x04008963 RID: 35171
		public SlateRef<string> modID;

		// Token: 0x04008964 RID: 35172
		public QuestNode node;

		// Token: 0x04008965 RID: 35173
		public QuestNode elseNode;
	}
}
