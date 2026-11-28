using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CE2 RID: 3298
	public class LordToil_HuntDownColonists : LordToil
	{
		// Token: 0x06005D3B RID: 23867 RVA: 0x002232A4 File Offset: 0x002214A4
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].mindState != null)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.HuntDownColonists);
					CompCanBeDormant compCanBeDormant = this.lord.ownedPawns[i].TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
				}
			}
		}
	}
}
