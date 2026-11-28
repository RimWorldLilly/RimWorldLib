using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000D07 RID: 3335
	public class LordToil_WanderNearby : LordToil
	{
		// Token: 0x06005DE6 RID: 24038 RVA: 0x00225E4C File Offset: 0x0022404C
		public override void UpdateAllDuties()
		{
			if (this.cell == null && this.lord.ownedPawns.Any<Pawn>())
			{
				this.cell = new IntVec3?(this.lord.ownedPawns[0].Position);
			}
			if (this.cell == null)
			{
				return;
			}
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(DutyDefOf.WanderClose, this.cell.Value, -1f);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x04003545 RID: 13637
		private IntVec3? cell;
	}
}
