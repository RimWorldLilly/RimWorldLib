using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D85 RID: 3461
	public class ThinkNode_ConditionalRevenantState : ThinkNode_Conditional
	{
		// Token: 0x06005F93 RID: 24467 RVA: 0x0022C8EC File Offset: 0x0022AAEC
		protected override bool Satisfied(Pawn pawn)
		{
			CompRevenant compRevenant = pawn.TryGetComp<CompRevenant>();
			return compRevenant != null && compRevenant.revenantState == this.state;
		}

		// Token: 0x040035CF RID: 13775
		public RevenantState state;
	}
}
