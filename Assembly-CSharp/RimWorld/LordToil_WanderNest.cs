using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D08 RID: 3336
	public class LordToil_WanderNest : LordToil
	{
		// Token: 0x06005DE7 RID: 24039 RVA: 0x00225F00 File Offset: 0x00224100
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.WanderNest);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}
	}
}
