using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200168D RID: 5773
	public class RitualBehaviorWorker_ChildBirth : RitualBehaviorWorker
	{
		// Token: 0x060090B2 RID: 37042 RVA: 0x00351553 File Offset: 0x0034F753
		public RitualBehaviorWorker_ChildBirth()
		{
		}

		// Token: 0x060090B3 RID: 37043 RVA: 0x0035155B File Offset: 0x0034F75B
		public RitualBehaviorWorker_ChildBirth(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x170018FB RID: 6395
		// (get) Token: 0x060090B4 RID: 37044 RVA: 0x00351564 File Offset: 0x0034F764
		public override bool ChecksReservations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170018FC RID: 6396
		// (get) Token: 0x060090B5 RID: 37045 RVA: 0x00351567 File Offset: 0x0034F767
		public override string descriptionOverride
		{
			get
			{
				if (Find.Storyteller.difficulty.babiesAreHealthy)
				{
					return "ChildBirthAlwaysHealthy_Desc".Translate();
				}
				return null;
			}
		}

		// Token: 0x060090B6 RID: 37046 RVA: 0x0035158C File Offset: 0x0034F78C
		public override void Tick(LordJob_Ritual ritual)
		{
			Pawn pawn = ritual.assignments.FirstAssignedPawn("mother");
			Hediff_Labor hediff_Labor;
			if (pawn == null)
			{
				hediff_Labor = null;
			}
			else
			{
				Pawn_HealthTracker health = pawn.health;
				if (health == null)
				{
					hediff_Labor = null;
				}
				else
				{
					HediffSet hediffSet = health.hediffSet;
					hediff_Labor = ((hediffSet != null) ? hediffSet.GetFirstHediff<Hediff_Labor>() : null);
				}
			}
			Hediff_Labor hediff_Labor2 = hediff_Labor;
			float value;
			if (hediff_Labor2 != null)
			{
				value = hediff_Labor2.Progress;
			}
			else
			{
				Hediff_LaborPushing hediff_LaborPushing;
				if (pawn == null)
				{
					hediff_LaborPushing = null;
				}
				else
				{
					Pawn_HealthTracker health2 = pawn.health;
					if (health2 == null)
					{
						hediff_LaborPushing = null;
					}
					else
					{
						HediffSet hediffSet2 = health2.hediffSet;
						hediff_LaborPushing = ((hediffSet2 != null) ? hediffSet2.GetFirstHediff<Hediff_LaborPushing>() : null);
					}
				}
				Hediff_LaborPushing hediff_LaborPushing2 = hediff_LaborPushing;
				if (hediff_LaborPushing2 != null)
				{
					value = hediff_LaborPushing2.Progress;
				}
				else
				{
					value = 1f;
				}
			}
			ritual.progressBarOverride = new float?(value);
			base.Tick(ritual);
		}

		// Token: 0x060090B7 RID: 37047 RVA: 0x00351625 File Offset: 0x0034F825
		public override string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return "UntilBabyIsBorn".Translate();
		}

		// Token: 0x060090B8 RID: 37048 RVA: 0x00351638 File Offset: 0x0034F838
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			IntVec3 bedSleepingSlotPosFor = RestUtility.GetBedSleepingSlotPosFor(assignments.FirstAssignedPawn("mother"), (Building_Bed)target.Thing);
			return new LordJob_Ritual_ChildBirth(target, ritual, obligation, this.def.stages, assignments, null, new IntVec3?(bedSleepingSlotPosFor));
		}

		// Token: 0x060090B9 RID: 37049 RVA: 0x00351680 File Offset: 0x0034F880
		public override bool ShouldInitAsSpectator(Pawn pawn, RitualRoleAssignments assignments)
		{
			Pawn pawn2 = assignments.FirstAssignedPawn("mother");
			if (pawn2 == null)
			{
				return false;
			}
			using (List<DirectPawnRelation>.Enumerator enumerator = LovePartnerRelationUtility.ExistingLovePartners(pawn2, false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.otherPawn == pawn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060090BA RID: 37050 RVA: 0x003516EC File Offset: 0x0034F8EC
		public override bool TargetStillAllowed(TargetInfo selectedTarget, LordJob_Ritual ritual)
		{
			if (!base.TargetStillAllowed(selectedTarget, ritual))
			{
				return false;
			}
			Pawn pawn = ritual.assignments.FirstAssignedPawn("mother");
			Pawn pawn2 = ritual.assignments.FirstAssignedPawn("doctor");
			Building_Bed building_Bed = (Building_Bed)selectedTarget.Thing;
			ReservationManager reservationManager = building_Bed.Map.reservationManager;
			bool flag = reservationManager.ReservedBy(building_Bed, pawn, null);
			bool flag2 = reservationManager.ReservedBy(building_Bed, pawn2, null);
			return (ritual.StageIndex < 2 || flag2 || flag) && (!flag || PregnancyUtility.IsUsableBedFor(pawn, pawn, building_Bed)) && (!flag2 || PregnancyUtility.IsUsableBedFor(pawn, pawn2, building_Bed)) && RestUtility.CanUseBedNow(building_Bed, pawn, false, false, null);
		}
	}
}
