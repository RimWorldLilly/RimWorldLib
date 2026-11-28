using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC1 RID: 3265
	public class LordToil_AssaultColony : LordToil
	{
		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x06005CA0 RID: 23712 RVA: 0x002202F9 File Offset: 0x0021E4F9
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005CA1 RID: 23713 RVA: 0x002202FC File Offset: 0x0021E4FC
		public LordToil_AssaultColony(bool attackDownedIfStarving = false, bool canPickUpOpportunisticWeapons = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
			this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x06005CA2 RID: 23714 RVA: 0x00220312 File Offset: 0x0021E512
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x00220315 File Offset: 0x0021E515
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06005CA4 RID: 23716 RVA: 0x00220328 File Offset: 0x0021E528
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.mindState != null)
				{
					PawnDuty duty = pawn.mindState.duty;
					if (((duty != null) ? duty.def : null) != DutyDefOf.AssaultColony)
					{
						pawn.mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
						pawn.mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
						pawn.mindState.duty.pickupOpportunisticWeapon = this.canPickUpOpportunisticWeapons;
						CompCanBeDormant compCanBeDormant = pawn.TryGetComp<CompCanBeDormant>();
						if (compCanBeDormant != null)
						{
							compCanBeDormant.WakeUp();
						}
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x040034CD RID: 13517
		private bool attackDownedIfStarving;

		// Token: 0x040034CE RID: 13518
		private bool canPickUpOpportunisticWeapons;
	}
}
