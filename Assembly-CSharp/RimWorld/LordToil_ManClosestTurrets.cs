using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CE8 RID: 3304
	public class LordToil_ManClosestTurrets : LordToil
	{
		// Token: 0x06005D4E RID: 23886 RVA: 0x002235C8 File Offset: 0x002217C8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ManClosestTurret, this.lord.ownedPawns[i].Position, -1f);
			}
		}
	}
}
