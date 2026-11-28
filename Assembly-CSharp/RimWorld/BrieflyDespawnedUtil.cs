using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B35 RID: 6965
	public static class BrieflyDespawnedUtil
	{
		// Token: 0x0600B2B5 RID: 45749 RVA: 0x003EECA5 File Offset: 0x003ECEA5
		public static bool BrieflyDespawned(this Pawn pawn)
		{
			return !pawn.Spawned && pawn.ParentHolder is PawnFlyer;
		}
	}
}
