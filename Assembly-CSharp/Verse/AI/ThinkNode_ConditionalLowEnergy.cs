using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008CD RID: 2253
	public class ThinkNode_ConditionalLowEnergy : ThinkNode_Conditional
	{
		// Token: 0x060045C0 RID: 17856 RVA: 0x001B75B1 File Offset: 0x001B57B1
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			return ((needs != null) ? needs.energy : null) != null && pawn.needs.energy.IsLowEnergySelfShutdown;
		}
	}
}
