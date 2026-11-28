using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D58 RID: 3416
	public class ThinkNode_ConditionalBleeding : ThinkNode_Conditional
	{
		// Token: 0x06005F2F RID: 24367 RVA: 0x0022BB6C File Offset: 0x00229D6C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.health.hediffSet.BleedRateTotal > 0.001f;
		}
	}
}
