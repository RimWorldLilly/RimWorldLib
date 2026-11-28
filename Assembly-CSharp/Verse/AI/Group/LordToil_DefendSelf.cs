using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000946 RID: 2374
	public class LordToil_DefendSelf : LordToil
	{
		// Token: 0x06004908 RID: 18696 RVA: 0x001C90C4 File Offset: 0x001C72C4
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Defend, this.lord.ownedPawns[i].Position, -1f);
				this.lord.ownedPawns[i].mindState.duty.radius = 28f;
			}
		}
	}
}
