using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200094D RID: 2381
	public class LordToil_ExitMapRandom : LordToil
	{
		// Token: 0x0600491F RID: 18719 RVA: 0x001C93EC File Offset: 0x001C75EC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.ExitMapRandom);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}
	}
}
