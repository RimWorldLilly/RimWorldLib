using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200094C RID: 2380
	public class LordToil_ExitMapNear : LordToil
	{
		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x0600491B RID: 18715 RVA: 0x001C9346 File Offset: 0x001C7546
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x0600491C RID: 18716 RVA: 0x001C9349 File Offset: 0x001C7549
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x001C934C File Offset: 0x001C754C
		public LordToil_ExitMapNear(IntVec3 near, float radius, LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false)
		{
			this.near = near;
			this.radius = radius;
			this.locomotion = locomotion;
			this.canDig = canDig;
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x001C9374 File Offset: 0x001C7574
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.ExitMapNearDutyTarget, this.near, this.radius);
				pawnDuty.locomotion = this.locomotion;
				pawnDuty.canDig = this.canDig;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04002EF1 RID: 12017
		private IntVec3 near;

		// Token: 0x04002EF2 RID: 12018
		private float radius;

		// Token: 0x04002EF3 RID: 12019
		private LocomotionUrgency locomotion;

		// Token: 0x04002EF4 RID: 12020
		private bool canDig;
	}
}
