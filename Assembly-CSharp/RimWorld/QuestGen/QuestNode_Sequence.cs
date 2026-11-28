using System;
using System.Collections.Generic;

namespace RimWorld.QuestGen
{
	// Token: 0x0200218F RID: 8591
	public class QuestNode_Sequence : QuestNode
	{
		// Token: 0x0600D67E RID: 54910 RVA: 0x004FAC90 File Offset: 0x004F8E90
		protected override void RunInt()
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].Run();
			}
		}

		// Token: 0x0600D67F RID: 54911 RVA: 0x004FACC4 File Offset: 0x004F8EC4
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.nodes.Count; i++)
			{
				if (!this.nodes[i].TestRun(slate))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040087CF RID: 34767
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
