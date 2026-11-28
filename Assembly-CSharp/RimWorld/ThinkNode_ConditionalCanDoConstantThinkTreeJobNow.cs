using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D66 RID: 3430
	public class ThinkNode_ConditionalCanDoConstantThinkTreeJobNow : ThinkNode_Conditional
	{
		// Token: 0x06005F4E RID: 24398 RVA: 0x0022BF84 File Offset: 0x0022A184
		protected override bool Satisfied(Pawn pawn)
		{
			return (!pawn.Downed || pawn.health.CanCrawl) && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake();
		}
	}
}
