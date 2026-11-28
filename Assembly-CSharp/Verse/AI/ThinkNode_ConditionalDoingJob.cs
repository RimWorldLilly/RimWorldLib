using System;

namespace Verse.AI
{
	// Token: 0x020008D4 RID: 2260
	public class ThinkNode_ConditionalDoingJob : ThinkNode_Conditional
	{
		// Token: 0x060045CE RID: 17870 RVA: 0x001B7664 File Offset: 0x001B5864
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.CurJobDef == this.jobDef;
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x001B7674 File Offset: 0x001B5874
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalDoingJob thinkNode_ConditionalDoingJob = (ThinkNode_ConditionalDoingJob)base.DeepCopy(resolve);
			thinkNode_ConditionalDoingJob.jobDef = this.jobDef;
			return thinkNode_ConditionalDoingJob;
		}

		// Token: 0x04002D57 RID: 11607
		public JobDef jobDef;
	}
}
