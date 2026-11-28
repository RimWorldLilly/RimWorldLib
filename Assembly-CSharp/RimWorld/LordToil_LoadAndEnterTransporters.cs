using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CE7 RID: 3303
	public class LordToil_LoadAndEnterTransporters : LordToil
	{
		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06005D4B RID: 23883 RVA: 0x00223550 File Offset: 0x00221750
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x00223553 File Offset: 0x00221753
		public LordToil_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x0022356C File Offset: 0x0022176C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.LoadAndEnterTransporters);
				pawnDuty.transportersGroup = this.transportersGroup;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04003509 RID: 13577
		public int transportersGroup = -1;
	}
}
