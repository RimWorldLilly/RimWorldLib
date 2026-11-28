using System;

namespace Verse.AI
{
	// Token: 0x020008D7 RID: 2263
	public class ThinkNode_ConditionalAnimalShouldForage : ThinkNode_Conditional
	{
		// Token: 0x060045D6 RID: 17878 RVA: 0x001B76F5 File Offset: 0x001B58F5
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.playerSettings.animalForage;
		}
	}
}
