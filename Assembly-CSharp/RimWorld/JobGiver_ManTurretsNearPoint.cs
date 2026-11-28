using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B20 RID: 2848
	public class JobGiver_ManTurretsNearPoint : JobGiver_ManTurrets
	{
		// Token: 0x06005415 RID: 21525 RVA: 0x001F4784 File Offset: 0x001F2984
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.GetLord().CurLordToil.FlagLoc;
		}
	}
}
