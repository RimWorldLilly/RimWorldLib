using System;

namespace Verse.AI
{
	// Token: 0x020008D6 RID: 2262
	public class ThinkNode_ConditionalAnimalShouldDig : ThinkNode_Conditional
	{
		// Token: 0x060045D4 RID: 17876 RVA: 0x001B76E0 File Offset: 0x001B58E0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.playerSettings.animalDig;
		}
	}
}
