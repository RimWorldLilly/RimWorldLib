using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001D25 RID: 7461
	public class FloatMenuOptionProvider_DraftedTend : FloatMenuOptionProvider
	{
		// Token: 0x1700234D RID: 9037
		// (get) Token: 0x0600C114 RID: 49428 RVA: 0x004662FF File Offset: 0x004644FF
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700234E RID: 9038
		// (get) Token: 0x0600C115 RID: 49429 RVA: 0x00466302 File Offset: 0x00464502
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700234F RID: 9039
		// (get) Token: 0x0600C116 RID: 49430 RVA: 0x00466305 File Offset: 0x00464505
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002350 RID: 9040
		// (get) Token: 0x0600C117 RID: 49431 RVA: 0x00466308 File Offset: 0x00464508
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002351 RID: 9041
		// (get) Token: 0x0600C118 RID: 49432 RVA: 0x0046630B File Offset: 0x0046450B
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002352 RID: 9042
		// (get) Token: 0x0600C119 RID: 49433 RVA: 0x0046630E File Offset: 0x0046450E
		protected override bool CanSelfTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C11A RID: 49434 RVA: 0x00466311 File Offset: 0x00464511
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.FirstSelectedPawn.IsMutant || context.FirstSelectedPawn.mutant.Def.canTend;
		}

		// Token: 0x0600C11B RID: 49435 RVA: 0x00466337 File Offset: 0x00464537
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider_DraftedTend.<GetOptionsFor>d__13 <GetOptionsFor>d__ = new FloatMenuOptionProvider_DraftedTend.<GetOptionsFor>d__13(-2);
			<GetOptionsFor>d__.<>4__this = this;
			<GetOptionsFor>d__.<>3__clickedPawn = clickedPawn;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}

		// Token: 0x0600C11C RID: 49436 RVA: 0x00466358 File Offset: 0x00464558
		private bool IsValidTendTarget(Pawn doctor, Pawn patient)
		{
			return (doctor.Drafted || patient == doctor) && (patient.Downed || (!patient.HostileTo(doctor.Faction) && (patient.IsColonist || patient.IsQuestLodger() || patient.IsPrisonerOfColony || patient.IsSlaveOfColony || (patient.Faction == Faction.OfPlayer && patient.IsAnimal) || (patient.IsColonySubhuman && patient.mutant.Def.entitledToMedicalCare))));
		}

		// Token: 0x0600C11D RID: 49437 RVA: 0x004663E4 File Offset: 0x004645E4
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Building_HoldingPlatform holdingPlatform = clickedThing as Building_HoldingPlatform;
			if (holdingPlatform == null)
			{
				return null;
			}
			Pawn heldPawn = holdingPlatform.HeldPawn;
			if (heldPawn == null)
			{
				return null;
			}
			if (!HealthAIUtility.ShouldBeTendedNowByPlayer(heldPawn))
			{
				return null;
			}
			if (context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor))
			{
				return new FloatMenuOption("CannotTend".Translate(heldPawn) + ": " + "CannotPrioritizeWorkTypeDisabled".Translate(WorkTypeDefOf.Doctor.gerundLabel), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.CanReach(heldPawn, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotTend".Translate(heldPawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Thing medicine = HealthAIUtility.FindBestMedicine(context.FirstSelectedPawn, heldPawn, false);
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Tend".Translate(heldPawn.LabelShort), delegate()
			{
				JobDef tendEntity = JobDefOf.TendEntity;
				LocalTargetInfo targetA = holdingPlatform;
				Thing medicine = medicine;
				Job job = JobMaker.MakeJob(tendEntity, targetA, (medicine != null) ? medicine : LocalTargetInfo.Invalid);
				job.count = 1;
				job.draftedTend = true;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, holdingPlatform, "ReservedBy", null);
		}
	}
}
