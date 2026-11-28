using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CDB RID: 3291
	public class LordToil_GorehulkAssault : LordToil
	{
		// Token: 0x06005D1B RID: 23835 RVA: 0x002227B4 File Offset: 0x002209B4
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.GorehulkAssault);
			}
		}
	}
}
