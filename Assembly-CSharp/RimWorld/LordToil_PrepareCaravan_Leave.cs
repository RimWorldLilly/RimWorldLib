using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CB9 RID: 3257
	public class LordToil_PrepareCaravan_Leave : LordToil
	{
		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06005C81 RID: 23681 RVA: 0x0021FCB8 File Offset: 0x0021DEB8
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06005C82 RID: 23682 RVA: 0x0021FCBB File Offset: 0x0021DEBB
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x06005C83 RID: 23683 RVA: 0x0021FCC7 File Offset: 0x0021DEC7
		public override bool AllowRestingInBed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06005C84 RID: 23684 RVA: 0x0021FCCA File Offset: 0x0021DECA
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005C85 RID: 23685 RVA: 0x0021FCCD File Offset: 0x0021DECD
		public LordToil_PrepareCaravan_Leave(IntVec3 exitSpot)
		{
			this.exitSpot = exitSpot;
		}

		// Token: 0x06005C86 RID: 23686 RVA: 0x0021FCDC File Offset: 0x0021DEDC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.TravelOrWait, this.exitSpot, -1f);
				pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
			}
		}

		// Token: 0x06005C87 RID: 23687 RVA: 0x0021FD4C File Offset: 0x0021DF4C
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				GatherAnimalsAndSlavesForCaravanUtility.CheckArrived(this.lord, this.lord.ownedPawns, this.exitSpot, "ReadyToExitMap", (Pawn x) => true, null);
			}
		}

		// Token: 0x040034C8 RID: 13512
		private IntVec3 exitSpot;
	}
}
