using System;

namespace Verse.AI
{
	// Token: 0x020008C0 RID: 2240
	public class ThinkNode_ConditionalMentalState : ThinkNode_Conditional
	{
		// Token: 0x0600459F RID: 17823 RVA: 0x001B724D File Offset: 0x001B544D
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalState thinkNode_ConditionalMentalState = (ThinkNode_ConditionalMentalState)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalState.state = this.state;
			return thinkNode_ConditionalMentalState;
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x001B7268 File Offset: 0x001B5468
		protected override bool Satisfied(Pawn pawn)
		{
			MentalStateDef mentalStateDef = pawn.MentalStateDef;
			return mentalStateDef != null && mentalStateDef == this.state;
		}

		// Token: 0x04002D50 RID: 11600
		public MentalStateDef state;
	}
}
