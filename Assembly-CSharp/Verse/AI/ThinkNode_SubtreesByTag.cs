using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	// Token: 0x020008E0 RID: 2272
	public class ThinkNode_SubtreesByTag : ThinkNode
	{
		// Token: 0x060045EF RID: 17903 RVA: 0x001B7C97 File Offset: 0x001B5E97
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_SubtreesByTag thinkNode_SubtreesByTag = (ThinkNode_SubtreesByTag)base.DeepCopy(resolve);
			thinkNode_SubtreesByTag.insertTag = this.insertTag;
			return thinkNode_SubtreesByTag;
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x001B7CB1 File Offset: 0x001B5EB1
		protected override void ResolveSubnodes()
		{
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x001B7CB4 File Offset: 0x001B5EB4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.matchedTrees == null)
			{
				this.matchedTrees = new List<ThinkTreeDef>();
				foreach (ThinkTreeDef thinkTreeDef in DefDatabase<ThinkTreeDef>.AllDefs)
				{
					if (thinkTreeDef.insertTag == this.insertTag)
					{
						this.matchedTrees.Add(thinkTreeDef);
					}
				}
				this.matchedTrees = (from tDef in this.matchedTrees
				orderby tDef.insertPriority descending
				select tDef).ToList<ThinkTreeDef>();
			}
			for (int i = 0; i < this.matchedTrees.Count; i++)
			{
				ThinkResult result = this.matchedTrees[i].thinkRoot.TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04002D71 RID: 11633
		[NoTranslate]
		public string insertTag;

		// Token: 0x04002D72 RID: 11634
		[Unsaved(false)]
		private List<ThinkTreeDef> matchedTrees;
	}
}
