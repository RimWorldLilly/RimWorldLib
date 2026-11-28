using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D8A RID: 3466
	public class ThinkNode_Duty : ThinkNode
	{
		// Token: 0x06005F9D RID: 24477 RVA: 0x0022CAB4 File Offset: 0x0022ACB4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " doing ThinkNode_Duty with no Lord.");
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.duty == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " doing ThinkNode_Duty with no duty.");
				return ThinkResult.NoJob;
			}
			ThinkResult result = this.subNodes[(int)pawn.mindState.duty.def.index].TryIssueJobPackage(pawn, jobParams);
			result = lord.Notify_DutyResult(result, pawn, jobParams);
			if (result.Job != null)
			{
				result.Job.lord = pawn.GetLord();
				result.Job.source = pawn.mindState.duty.source;
				result.Job.dutyTag = pawn.mindState.duty.tag;
			}
			return result;
		}

		// Token: 0x06005F9E RID: 24478 RVA: 0x0022CBA4 File Offset: 0x0022ADA4
		protected override void ResolveSubnodes()
		{
			foreach (DutyDef dutyDef in DefDatabase<DutyDef>.AllDefs)
			{
				dutyDef.thinkNode.ResolveSubnodesAndRecur();
				this.subNodes.Add(dutyDef.thinkNode.DeepCopy(true));
			}
		}
	}
}
