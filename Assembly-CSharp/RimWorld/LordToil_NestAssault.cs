using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CEB RID: 3307
	public class LordToil_NestAssault : LordToil
	{
		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06005D5A RID: 23898 RVA: 0x00223754 File Offset: 0x00221954
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x06005D5B RID: 23899 RVA: 0x00223757 File Offset: 0x00221957
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005D5C RID: 23900 RVA: 0x0022375A File Offset: 0x0022195A
		public LordToil_NestAssault(bool attackDownedIfStarving = false, bool canPickUpOpportunisticWeapons = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
			this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
		}

		// Token: 0x06005D5D RID: 23901 RVA: 0x00223770 File Offset: 0x00221970
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06005D5E RID: 23902 RVA: 0x00223784 File Offset: 0x00221984
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].mindState != null)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.NestAssault);
					this.lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
					this.lord.ownedPawns[i].mindState.duty.pickupOpportunisticWeapon = this.canPickUpOpportunisticWeapons;
					CompCanBeDormant compCanBeDormant = this.lord.ownedPawns[i].TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
				}
			}
		}

		// Token: 0x0400350D RID: 13581
		private bool attackDownedIfStarving;

		// Token: 0x0400350E RID: 13582
		private bool canPickUpOpportunisticWeapons;
	}
}
