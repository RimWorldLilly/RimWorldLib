using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CD8 RID: 3288
	public class LordToil_ExitMapAndDefendSelf : LordToil
	{
		// Token: 0x06005D0D RID: 23821 RVA: 0x00222204 File Offset: 0x00220404
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
			}
		}
	}
}
