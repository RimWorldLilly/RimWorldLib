using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CEC RID: 3308
	public class LordToil_PanicFlee : LordToil
	{
		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x06005D5F RID: 23903 RVA: 0x0022385E File Offset: 0x00221A5E
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x06005D60 RID: 23904 RVA: 0x00223861 File Offset: 0x00221A61
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D61 RID: 23905 RVA: 0x00223864 File Offset: 0x00221A64
		public override void Init()
		{
			base.Init();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.InAggroMentalState && (!this.HasFleeingDuty(pawn) || pawn.mindState.duty.def == DutyDefOf.ExitMapRandom))
				{
					pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, false, null, false, false, false);
				}
			}
		}

		// Token: 0x06005D62 RID: 23906 RVA: 0x002238EC File Offset: 0x00221AEC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!this.HasFleeingDuty(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.ExitMapRandom);
				}
			}
		}

		// Token: 0x06005D63 RID: 23907 RVA: 0x00223944 File Offset: 0x00221B44
		private bool HasFleeingDuty(Pawn pawn)
		{
			return pawn.mindState.duty != null && (pawn.mindState.duty.def == DutyDefOf.ExitMapRandom || pawn.mindState.duty.def == DutyDefOf.Steal || pawn.mindState.duty.def == DutyDefOf.Kidnap);
		}
	}
}
