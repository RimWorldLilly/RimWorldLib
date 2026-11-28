using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CF8 RID: 3320
	public class LordToil_Sleep : LordToil
	{
		// Token: 0x06005DA9 RID: 23977 RVA: 0x002250D0 File Offset: 0x002232D0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.SleepForever);
			}
		}
	}
}
