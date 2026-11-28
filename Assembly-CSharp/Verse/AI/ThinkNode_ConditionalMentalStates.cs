using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x020008C2 RID: 2242
	public class ThinkNode_ConditionalMentalStates : ThinkNode_Conditional
	{
		// Token: 0x060045A5 RID: 17829 RVA: 0x001B72D9 File Offset: 0x001B54D9
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalMentalStates thinkNode_ConditionalMentalStates = (ThinkNode_ConditionalMentalStates)base.DeepCopy(resolve);
			thinkNode_ConditionalMentalStates.states = this.states;
			return thinkNode_ConditionalMentalStates;
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x001B72F3 File Offset: 0x001B54F3
		protected override bool Satisfied(Pawn pawn)
		{
			return this.states.Contains(pawn.MentalStateDef);
		}

		// Token: 0x04002D52 RID: 11602
		public List<MentalStateDef> states;
	}
}
