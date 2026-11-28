using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D4E RID: 3406
	public class ThinkNode_ConditionalHasVoluntarilyJoinableLord : ThinkNode_Conditional
	{
		// Token: 0x06005F1A RID: 24346 RVA: 0x0022B994 File Offset: 0x00229B94
		protected override bool Satisfied(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_VoluntarilyJoinable;
		}
	}
}
