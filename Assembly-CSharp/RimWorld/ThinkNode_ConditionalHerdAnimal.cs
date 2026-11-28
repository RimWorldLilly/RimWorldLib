using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D55 RID: 3413
	public class ThinkNode_ConditionalHerdAnimal : ThinkNode_Conditional
	{
		// Token: 0x06005F29 RID: 24361 RVA: 0x0022BB0C File Offset: 0x00229D0C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.RaceProps.herdAnimal;
		}
	}
}
