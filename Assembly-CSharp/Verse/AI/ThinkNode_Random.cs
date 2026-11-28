using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020008DD RID: 2269
	public class ThinkNode_Random : ThinkNode
	{
		// Token: 0x060045E4 RID: 17892 RVA: 0x001B7A8C File Offset: 0x001B5C8C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_Random.tempList.Clear();
			for (int i = 0; i < this.subNodes.Count; i++)
			{
				ThinkNode_Random.tempList.Add(this.subNodes[i]);
			}
			ThinkNode_Random.tempList.Shuffle<ThinkNode>();
			for (int j = 0; j < ThinkNode_Random.tempList.Count; j++)
			{
				ThinkResult result = ThinkNode_Random.tempList[j].TryIssueJobPackage(pawn, jobParams);
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04002D6D RID: 11629
		private static List<ThinkNode> tempList = new List<ThinkNode>();
	}
}
