using System;

namespace Verse.AI
{
	// Token: 0x020008BF RID: 2239
	public abstract class ThinkNode_Conditional : ThinkNode_Priority
	{
		// Token: 0x0600459B RID: 17819 RVA: 0x001B7209 File Offset: 0x001B5409
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_Conditional thinkNode_Conditional = (ThinkNode_Conditional)base.DeepCopy(resolve);
			thinkNode_Conditional.invert = this.invert;
			return thinkNode_Conditional;
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x001B7223 File Offset: 0x001B5423
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (this.Satisfied(pawn) == !this.invert)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x0600459D RID: 17821
		protected abstract bool Satisfied(Pawn pawn);

		// Token: 0x04002D4F RID: 11599
		public bool invert;
	}
}
