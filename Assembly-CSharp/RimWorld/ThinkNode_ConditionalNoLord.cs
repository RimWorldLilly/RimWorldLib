using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D54 RID: 3412
	public class ThinkNode_ConditionalNoLord : ThinkNode_Conditional
	{
		// Token: 0x06005F27 RID: 24359 RVA: 0x0022BAF9 File Offset: 0x00229CF9
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.GetLord() == null;
		}
	}
}
