using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D8B RID: 3467
	public class ThinkNode_DutyConstant : ThinkNode
	{
		// Token: 0x06005FA0 RID: 24480 RVA: 0x0022CC14 File Offset: 0x0022AE14
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " doing ThinkNode_DutyConstant with no Lord.");
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.duty == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " doing ThinkNode_DutyConstant with no duty.");
				return ThinkResult.NoJob;
			}
			if (this.dutyDefToSubNode == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " has null dutyDefToSubNode. Recovering by calling ResolveSubnodes() (though that should have been called already).");
				this.ResolveSubnodes();
			}
			int num = this.dutyDefToSubNode[pawn.mindState.duty.def];
			if (num < 0)
			{
				return ThinkResult.NoJob;
			}
			ThinkResult result = this.subNodes[num].TryIssueJobPackage(pawn, jobParams);
			result = lord.Notify_DutyConstantResult(result, pawn, jobParams);
			if (result.Job != null)
			{
				result.Job.lord = pawn.GetLord();
				result.Job.source = pawn.mindState.duty.source;
				result.Job.dutyTag = pawn.mindState.duty.tag;
			}
			return result;
		}

		// Token: 0x06005FA1 RID: 24481 RVA: 0x0022CD40 File Offset: 0x0022AF40
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_DutyConstant thinkNode_DutyConstant = (ThinkNode_DutyConstant)base.DeepCopy(resolve);
			if (this.dutyDefToSubNode != null)
			{
				thinkNode_DutyConstant.dutyDefToSubNode = new DefMap<DutyDef, int>();
				thinkNode_DutyConstant.dutyDefToSubNode.SetAll(-1);
				foreach (DutyDef def in DefDatabase<DutyDef>.AllDefs)
				{
					thinkNode_DutyConstant.dutyDefToSubNode[def] = this.dutyDefToSubNode[def];
				}
			}
			return thinkNode_DutyConstant;
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x0022CDCC File Offset: 0x0022AFCC
		protected override void ResolveSubnodes()
		{
			this.dutyDefToSubNode = new DefMap<DutyDef, int>();
			this.dutyDefToSubNode.SetAll(-1);
			foreach (DutyDef dutyDef in DefDatabase<DutyDef>.AllDefs)
			{
				if (dutyDef.constantThinkNode != null)
				{
					this.dutyDefToSubNode[dutyDef] = this.subNodes.Count;
					dutyDef.constantThinkNode.ResolveSubnodesAndRecur();
					this.subNodes.Add(dutyDef.constantThinkNode.DeepCopy(true));
				}
			}
		}

		// Token: 0x040035D3 RID: 13779
		private DefMap<DutyDef, int> dutyDefToSubNode;
	}
}
