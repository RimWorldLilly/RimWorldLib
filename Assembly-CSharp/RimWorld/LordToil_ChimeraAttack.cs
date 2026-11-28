using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CCA RID: 3274
	public class LordToil_ChimeraAttack : LordToil
	{
		// Token: 0x06005CD7 RID: 23767 RVA: 0x002212FC File Offset: 0x0021F4FC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.ChimeraAttack);
			}
		}
	}
}
