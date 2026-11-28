using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D06 RID: 3334
	public class LordToil_WanderClose : LordToil
	{
		// Token: 0x06005DE3 RID: 24035 RVA: 0x00225DD5 File Offset: 0x00223FD5
		public LordToil_WanderClose(IntVec3 location)
		{
			this.location = location;
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x00225DE4 File Offset: 0x00223FE4
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.WanderClose, this.location, -1f);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04003544 RID: 13636
		private IntVec3 location;
	}
}
