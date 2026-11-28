using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CD1 RID: 3281
	internal class LordToil_DefendTraderCaravan : LordToil_DefendPoint
	{
		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06005CEC RID: 23788 RVA: 0x00221934 File Offset: 0x0021FB34
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06005CED RID: 23789 RVA: 0x00221937 File Offset: 0x0021FB37
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x06005CEE RID: 23790 RVA: 0x00221943 File Offset: 0x0021FB43
		public LordToil_DefendTraderCaravan() : base(true)
		{
		}

		// Token: 0x06005CEF RID: 23791 RVA: 0x0022194C File Offset: 0x0021FB4C
		public LordToil_DefendTraderCaravan(IntVec3 defendPoint) : base(defendPoint, null, null)
		{
		}

		// Token: 0x06005CF0 RID: 23792 RVA: 0x00221974 File Offset: 0x0021FB74
		public override void UpdateAllDuties()
		{
			LordToilData_DefendPoint data = base.Data;
			Pawn pawn = TraderCaravanUtility.FindTrader(this.lord);
			if (pawn != null)
			{
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = this.lord.ownedPawns[i];
					switch (pawn2.GetTraderCaravanRole())
					{
					case TraderCaravanRole.Carrier:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Follow, pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					case TraderCaravanRole.Guard:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.defendPoint, data.defendRadius);
						break;
					case TraderCaravanRole.Chattel:
						pawn2.mindState.duty = new PawnDuty(DutyDefOf.Escort, pawn, 5f);
						pawn2.mindState.duty.locomotion = LocomotionUrgency.Walk;
						break;
					}
				}
				return;
			}
		}
	}
}
