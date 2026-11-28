using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC2 RID: 3266
	public class LordToil_AssaultColonyBossgroup : LordToil
	{
		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x06005CA5 RID: 23717 RVA: 0x002203EE File Offset: 0x0021E5EE
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06005CA6 RID: 23718 RVA: 0x002203F1 File Offset: 0x0021E5F1
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x002203FC File Offset: 0x0021E5FC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].RaceProps.dutyBoss != null)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.lord.ownedPawns[i].RaceProps.dutyBoss);
				}
				else
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
			}
		}
	}
}
