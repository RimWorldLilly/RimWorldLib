using System;

namespace Verse.AI
{
	// Token: 0x020008D9 RID: 2265
	public class ThinkNode_FilterPriority : ThinkNode
	{
		// Token: 0x060045DA RID: 17882 RVA: 0x001B77A4 File Offset: 0x001B59A4
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_FilterPriority thinkNode_FilterPriority = (ThinkNode_FilterPriority)base.DeepCopy(resolve);
			thinkNode_FilterPriority.minPriority = this.minPriority;
			return thinkNode_FilterPriority;
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x001B77C0 File Offset: 0x001B59C0
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.subNodes[i].GetPriority(pawn) > this.minPriority)
				{
					ThinkResult result = this.subNodes[i].TryIssueJobPackage(pawn, jobParams);
					if (result.IsValid)
					{
						return result;
					}
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04002D59 RID: 11609
		public float minPriority = 0.5f;
	}
}
