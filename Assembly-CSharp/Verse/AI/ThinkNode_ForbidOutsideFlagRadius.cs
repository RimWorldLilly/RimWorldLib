using System;

namespace Verse.AI
{
	// Token: 0x020008DA RID: 2266
	public class ThinkNode_ForbidOutsideFlagRadius : ThinkNode_Priority
	{
		// Token: 0x060045DD RID: 17885 RVA: 0x001B7836 File Offset: 0x001B5A36
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ForbidOutsideFlagRadius thinkNode_ForbidOutsideFlagRadius = (ThinkNode_ForbidOutsideFlagRadius)base.DeepCopy(resolve);
			thinkNode_ForbidOutsideFlagRadius.maxDistToSquadFlag = this.maxDistToSquadFlag;
			return thinkNode_ForbidOutsideFlagRadius;
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x001B7850 File Offset: 0x001B5A50
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			ThinkResult result;
			try
			{
				if (this.maxDistToSquadFlag > 0f)
				{
					if (pawn.mindState.maxDistToSquadFlag > 0f)
					{
						Log.Error("Squad flag was not reset properly; raiders may behave strangely");
					}
					pawn.mindState.maxDistToSquadFlag = this.maxDistToSquadFlag;
				}
				result = base.TryIssueJobPackage(pawn, jobParams);
			}
			finally
			{
				pawn.mindState.maxDistToSquadFlag = -1f;
			}
			return result;
		}

		// Token: 0x04002D5A RID: 11610
		public float maxDistToSquadFlag = -1f;
	}
}
