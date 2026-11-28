using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D6C RID: 3436
	public class ThinkNode_ConditionalAnyColonistTryingToExitMap : ThinkNode_Conditional
	{
		// Token: 0x06005F5B RID: 24411 RVA: 0x0022C0CC File Offset: 0x0022A2CC
		protected override bool Satisfied(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return false;
			}
			foreach (Pawn pawn2 in mapHeld.mapPawns.FreeColonistsSpawned)
			{
				Job curJob = pawn2.CurJob;
				if (curJob != null && curJob.exitMapOnArrival)
				{
					return true;
				}
			}
			return false;
		}
	}
}
