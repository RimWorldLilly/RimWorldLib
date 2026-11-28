using System;

namespace Verse.AI
{
	// Token: 0x020008DE RID: 2270
	public class ThinkNode_Tagger : ThinkNode_Priority
	{
		// Token: 0x060045E7 RID: 17895 RVA: 0x001B7B25 File Offset: 0x001B5D25
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Tagger thinkNode_Tagger = (ThinkNode_Tagger)base.DeepCopy(resolve);
			thinkNode_Tagger.tagToGive = this.tagToGive;
			return thinkNode_Tagger;
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x001B7B40 File Offset: 0x001B5D40
		public override float GetPriority(Pawn pawn)
		{
			if (this.priority >= 0f)
			{
				return this.priority;
			}
			if (this.subNodes.Any<ThinkNode>())
			{
				return this.subNodes[0].GetPriority(pawn);
			}
			Log.ErrorOnce("ThinkNode_PrioritySorter has child node which didn't give a priority: " + ((this != null) ? this.ToString() : null), this.GetHashCode());
			return 0f;
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x001B7BA8 File Offset: 0x001B5DA8
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result = base.TryIssueJobPackage(pawn, jobParams);
			if (result.IsValid && result.Tag == null)
			{
				result = new ThinkResult(result.Job, result.SourceNode, new JobTag?(this.tagToGive), false);
			}
			return result;
		}

		// Token: 0x04002D6E RID: 11630
		private JobTag tagToGive;
	}
}
