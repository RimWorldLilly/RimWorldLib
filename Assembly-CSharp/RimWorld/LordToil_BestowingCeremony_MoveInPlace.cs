using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C5C RID: 3164
	public class LordToil_BestowingCeremony_MoveInPlace : LordToil
	{
		// Token: 0x060059C5 RID: 22981 RVA: 0x002106E1 File Offset: 0x0020E8E1
		public LordToil_BestowingCeremony_MoveInPlace(IntVec3 spot, Pawn target)
		{
			this.spot = spot;
			this.target = target;
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x002106F8 File Offset: 0x0020E8F8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.BestowingCeremony_MoveInPlace, this.spot, -1f);
				pawnDuty.locomotion = LocomotionUrgency.Walk;
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04003373 RID: 13171
		public bool? pass;

		// Token: 0x04003374 RID: 13172
		public IntVec3 spot;

		// Token: 0x04003375 RID: 13173
		public Pawn target;
	}
}
