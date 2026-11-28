using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200094B RID: 2379
	public class LordToil_ExitMapTraderFighting : LordToil
	{
		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06004917 RID: 18711 RVA: 0x001C92AF File Offset: 0x001C74AF
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06004918 RID: 18712 RVA: 0x001C92B2 File Offset: 0x001C74B2
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x001C92B8 File Offset: 0x001C74B8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				TraderCaravanRole traderCaravanRole = pawn.GetTraderCaravanRole();
				if (traderCaravanRole == TraderCaravanRole.Carrier || traderCaravanRole == TraderCaravanRole.Chattel)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBest);
					pawn.mindState.duty.locomotion = LocomotionUrgency.Jog;
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapBestAndDefendSelf);
				}
			}
		}
	}
}
