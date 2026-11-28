using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D67 RID: 3431
	public class ThinkNode_ConditionalCanDoLordJobNow : ThinkNode_Conditional
	{
		// Token: 0x06005F50 RID: 24400 RVA: 0x0022BFC3 File Offset: 0x0022A1C3
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake() && (!pawn.Downed || pawn.DutyActiveWhenDown(false));
		}
	}
}
