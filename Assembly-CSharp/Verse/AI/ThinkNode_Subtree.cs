using System;

namespace Verse.AI
{
	// Token: 0x020008DF RID: 2271
	public class ThinkNode_Subtree : ThinkNode
	{
		// Token: 0x060045EB RID: 17899 RVA: 0x001B7C04 File Offset: 0x001B5E04
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Subtree thinkNode_Subtree = (ThinkNode_Subtree)base.DeepCopy(false);
			thinkNode_Subtree.treeDef = this.treeDef;
			if (resolve)
			{
				thinkNode_Subtree.ResolveSubnodesAndRecur();
				thinkNode_Subtree.subtreeNode = thinkNode_Subtree.subNodes[this.subNodes.IndexOf(this.subtreeNode)];
			}
			return thinkNode_Subtree;
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x001B7C56 File Offset: 0x001B5E56
		protected override void ResolveSubnodes()
		{
			this.subtreeNode = this.treeDef.thinkRoot.DeepCopy(true);
			this.subNodes.Add(this.subtreeNode);
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x001B7C80 File Offset: 0x001B5E80
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			return this.subtreeNode.TryIssueJobPackage(pawn, jobParams);
		}

		// Token: 0x04002D6F RID: 11631
		private ThinkTreeDef treeDef;

		// Token: 0x04002D70 RID: 11632
		[Unsaved(false)]
		public ThinkNode subtreeNode;
	}
}
