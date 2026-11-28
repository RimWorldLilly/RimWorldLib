using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D52 RID: 3410
	public class ThinkNode_ConditionalAnimalWrongSeason : ThinkNode_Conditional
	{
		// Token: 0x06005F23 RID: 24355 RVA: 0x0022BAB4 File Offset: 0x00229CB4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsAnimal && !pawn.Map.mapTemperature.SeasonAcceptableFor(pawn.def, 0f);
		}
	}
}
