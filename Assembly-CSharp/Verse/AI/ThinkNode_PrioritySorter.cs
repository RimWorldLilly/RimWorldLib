using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020008DC RID: 2268
	public class ThinkNode_PrioritySorter : ThinkNode
	{
		// Token: 0x060045E0 RID: 17888 RVA: 0x001B78D7 File Offset: 0x001B5AD7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_PrioritySorter thinkNode_PrioritySorter = (ThinkNode_PrioritySorter)base.DeepCopy(resolve);
			thinkNode_PrioritySorter.minPriority = this.minPriority;
			return thinkNode_PrioritySorter;
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x001B78F4 File Offset: 0x001B5AF4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkNode_PrioritySorter.workingNodes.Clear();
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				ThinkNode_PrioritySorter.workingNodes.Insert(Rand.Range(0, ThinkNode_PrioritySorter.workingNodes.Count - 1), this.subNodes[i]);
			}
			while (ThinkNode_PrioritySorter.workingNodes.Count > 0)
			{
				float num = 0f;
				int num2 = -1;
				for (int j = 0; j < ThinkNode_PrioritySorter.workingNodes.Count; j++)
				{
					float num3 = 0f;
					try
					{
						num3 = ThinkNode_PrioritySorter.workingNodes[j].GetPriority(pawn);
					}
					catch (Exception ex)
					{
						string str = "Exception in ";
						Type type = base.GetType();
						Log.Error(str + ((type != null) ? type.ToString() : null) + " GetPriority: " + ex.ToString());
					}
					if (num3 > 0f && num3 >= this.minPriority && num3 > num)
					{
						num = num3;
						num2 = j;
					}
				}
				if (num2 == -1)
				{
					break;
				}
				ThinkResult result = ThinkResult.NoJob;
				try
				{
					result = ThinkNode_PrioritySorter.workingNodes[num2].TryIssueJobPackage(pawn, jobParams);
				}
				catch (Exception ex2)
				{
					string str2 = "Exception in ";
					Type type2 = base.GetType();
					Log.Error(str2 + ((type2 != null) ? type2.ToString() : null) + " TryIssueJobPackage: " + ex2.ToString());
				}
				if (result.IsValid)
				{
					return result;
				}
				ThinkNode_PrioritySorter.workingNodes.RemoveAt(num2);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x04002D6B RID: 11627
		public float minPriority;

		// Token: 0x04002D6C RID: 11628
		private static List<ThinkNode> workingNodes = new List<ThinkNode>();
	}
}
