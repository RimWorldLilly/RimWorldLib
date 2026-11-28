using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CD2 RID: 3282
	public class LordToil_DevourerAssault : LordToil
	{
		// Token: 0x06005CF1 RID: 23793 RVA: 0x00221AA8 File Offset: 0x0021FCA8
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.DevourerAssault);
			}
		}
	}
}
