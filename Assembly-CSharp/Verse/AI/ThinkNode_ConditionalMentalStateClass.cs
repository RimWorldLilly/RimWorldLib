using System;

namespace Verse.AI
{
	// Token: 0x020008C1 RID: 2241
	public class ThinkNode_ConditionalMentalStateClass : ThinkNode_Conditional
	{
		// Token: 0x060045A2 RID: 17826 RVA: 0x001B7292 File Offset: 0x001B5492
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStateClass thinkNode_ConditionalMentalStateClass = (ThinkNode_ConditionalMentalStateClass)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStateClass.stateClass = this.stateClass;
			return thinkNode_ConditionalMentalStateClass;
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x001B72AC File Offset: 0x001B54AC
		protected override bool Satisfied(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			return mentalState != null && this.stateClass.IsInstanceOfType(mentalState);
		}

		// Token: 0x04002D51 RID: 11601
		public Type stateClass;
	}
}
