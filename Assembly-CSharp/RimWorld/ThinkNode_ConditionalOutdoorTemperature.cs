using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5A RID: 3418
	public class ThinkNode_ConditionalOutdoorTemperature : ThinkNode_Conditional
	{
		// Token: 0x06005F33 RID: 24371 RVA: 0x0022BBBC File Offset: 0x00229DBC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Position.UsesOutdoorTemperature(pawn.Map);
		}
	}
}
