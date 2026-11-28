using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023E5 RID: 9189
	public static class WorldPawnsUtility
	{
		// Token: 0x0600E573 RID: 58739 RVA: 0x00553819 File Offset: 0x00551A19
		public static bool IsWorldPawn(this Pawn p)
		{
			return Find.WorldPawns.Contains(p);
		}
	}
}
