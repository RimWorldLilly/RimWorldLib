using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D79 RID: 3449
	public class ThinkNode_ConditionalInBed : ThinkNode_Conditional
	{
		// Token: 0x06005F77 RID: 24439 RVA: 0x0022C593 File Offset: 0x0022A793
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
