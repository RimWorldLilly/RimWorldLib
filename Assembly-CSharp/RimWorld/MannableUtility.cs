using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E7 RID: 6375
	public static class MannableUtility
	{
		// Token: 0x06009FC4 RID: 40900 RVA: 0x0039414C File Offset: 0x0039234C
		public static Thing MannedThing(this Pawn pawn)
		{
			if (pawn.Dead)
			{
				return null;
			}
			Thing lastMannedThing = pawn.mindState.lastMannedThing;
			if (lastMannedThing == null || lastMannedThing.TryGetComp<CompMannable>().ManningPawn != pawn)
			{
				return null;
			}
			return lastMannedThing;
		}
	}
}
