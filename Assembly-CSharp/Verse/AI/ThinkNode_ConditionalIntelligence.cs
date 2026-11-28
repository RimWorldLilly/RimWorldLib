using System;

namespace Verse.AI
{
	// Token: 0x020008C8 RID: 2248
	public class ThinkNode_ConditionalIntelligence : ThinkNode_Conditional
	{
		// Token: 0x060045B4 RID: 17844 RVA: 0x001B743E File Offset: 0x001B563E
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalIntelligence thinkNode_ConditionalIntelligence = (ThinkNode_ConditionalIntelligence)base.DeepCopy(resolve);
			thinkNode_ConditionalIntelligence.minIntelligence = this.minIntelligence;
			return thinkNode_ConditionalIntelligence;
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x001B7458 File Offset: 0x001B5658
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.intelligence >= this.minIntelligence;
		}

		// Token: 0x04002D55 RID: 11605
		public Intelligence minIntelligence = Intelligence.ToolUser;
	}
}
