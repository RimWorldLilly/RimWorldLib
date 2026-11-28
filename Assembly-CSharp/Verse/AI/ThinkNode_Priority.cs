using System;

namespace Verse.AI
{
	// Token: 0x020008D8 RID: 2264
	public class ThinkNode_Priority : ThinkNode
	{
		// Token: 0x060045D8 RID: 17880 RVA: 0x001B770C File Offset: 0x001B590C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			int count = this.subNodes.Count;
			for (int i = 0; i < count; i++)
			{
				ThinkResult result = ThinkResult.NoJob;
				try
				{
					result = this.subNodes[i].TryIssueJobPackage(pawn, jobParams);
				}
				catch (Exception ex)
				{
					string str = "Exception in ";
					Type type = base.GetType();
					Log.Error(str + ((type != null) ? type.ToString() : null) + " TryIssueJobPackage: " + ex.ToString());
				}
				if (result.IsValid)
				{
					return result;
				}
			}
			return ThinkResult.NoJob;
		}
	}
}
