using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D59 RID: 3417
	public class ThinkNode_ConditionalDangerousTemperature : ThinkNode_Conditional
	{
		// Token: 0x06005F31 RID: 24369 RVA: 0x0022BB90 File Offset: 0x00229D90
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature);
		}
	}
}
