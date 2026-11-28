using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200218D RID: 8589
	public class QuestNode_RandomNode : QuestNode
	{
		// Token: 0x0600D677 RID: 54903 RVA: 0x004FAA88 File Offset: 0x004F8C88
		protected override bool TestRunInt(Slate slate)
		{
			QuestNode questNode = this.GetNodesCanRun(slate).FirstOrDefault<QuestNode>();
			if (questNode == null)
			{
				return false;
			}
			questNode.TestRun(slate);
			return true;
		}

		// Token: 0x0600D678 RID: 54904 RVA: 0x004FAAB0 File Offset: 0x004F8CB0
		protected override void RunInt()
		{
			QuestNode questNode;
			if (!this.GetNodesCanRun(QuestGen.slate).TryRandomElementByWeight((QuestNode e) => e.SelectionWeight(QuestGen.slate), out questNode))
			{
				return;
			}
			questNode.Run();
		}

		// Token: 0x0600D679 RID: 54905 RVA: 0x004FAAF7 File Offset: 0x004F8CF7
		private IEnumerable<QuestNode> GetNodesCanRun(Slate slate)
		{
			QuestNode_RandomNode.<GetNodesCanRun>d__3 <GetNodesCanRun>d__ = new QuestNode_RandomNode.<GetNodesCanRun>d__3(-2);
			<GetNodesCanRun>d__.<>4__this = this;
			<GetNodesCanRun>d__.<>3__slate = slate;
			return <GetNodesCanRun>d__;
		}

		// Token: 0x040087CB RID: 34763
		public List<QuestNode> nodes = new List<QuestNode>();
	}
}
