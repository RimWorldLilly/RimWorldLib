using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CBA RID: 3258
	public class LordToil_PrepareCaravan_Pause : LordToil
	{
		// Token: 0x06005C88 RID: 23688 RVA: 0x0021FDAC File Offset: 0x0021DFAC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Pause);
			}
		}
	}
}
