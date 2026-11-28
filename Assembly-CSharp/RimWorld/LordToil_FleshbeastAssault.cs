using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CDA RID: 3290
	public class LordToil_FleshbeastAssault : LordToil
	{
		// Token: 0x06005D19 RID: 23833 RVA: 0x00222748 File Offset: 0x00220948
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.FleshbeastAssault);
			}
		}
	}
}
