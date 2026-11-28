using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009ED RID: 2541
	public class JobDriver_TendPatient : JobDriver
	{
		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06004CFF RID: 19711 RVA: 0x001D8A3F File Offset: 0x001D6C3F
		protected Thing MedicineUsed
		{
			get
			{
				return this.job.targetB.Thing;
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06004D00 RID: 19712 RVA: 0x001D8A51 File Offset: 0x001D6C51
		protected Pawn Deliveree
		{
			get
			{
				return this.job.targetA.Pawn;
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x001D8A63 File Offset: 0x001D6C63
		protected bool IsMedicineInDoctorInventory
		{
			get
			{
				return this.MedicineUsed != null && this.pawn.inventory.Contains(this.MedicineUsed);
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06004D02 RID: 19714 RVA: 0x001D8A85 File Offset: 0x001D6C85
		protected Pawn_InventoryTracker MedicineHolderInventory
		{
			get
			{
				Thing medicineUsed = this.MedicineUsed;
				return ((medicineUsed != null) ? medicineUsed.ParentHolder : null) as Pawn_InventoryTracker;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x06004D03 RID: 19715 RVA: 0x001D8A9E File Offset: 0x001D6C9E
		protected Pawn OtherPawnMedicineHolder
		{
			get
			{
				return this.job.targetC.Pawn;
			}
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x001D8AB0 File Offset: 0x001D6CB0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usesMedicine, "usesMedicine", false, false);
			Scribe_Values.Look<PathEndMode>(ref this.pathEndMode, "pathEndMode", PathEndMode.None, false);
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x001D8ADC File Offset: 0x001D6CDC
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usesMedicine = (this.MedicineUsed != null);
			if (this.Deliveree == this.pawn)
			{
				this.pathEndMode = PathEndMode.OnCell;
				return;
			}
			if (this.Deliveree.InBed())
			{
				this.pathEndMode = PathEndMode.InteractionCell;
				return;
			}
			if (this.Deliveree != this.pawn)
			{
				this.pathEndMode = PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x001D8B40 File Offset: 0x001D6D40
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (this.MedicineUsed != null)
			{
				int num = this.pawn.Map.reservationManager.CanReserveStack(this.pawn, this.MedicineUsed, 10, null, false);
				if (num <= 0 || !this.pawn.Reserve(this.MedicineUsed, this.job, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(this.Deliveree)), null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x001D8BE0 File Offset: 0x001D6DE0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TendPatient.<MakeNewToils>d__19 <MakeNewToils>d__ = new JobDriver_TendPatient.<MakeNewToils>d__19(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D08 RID: 19720 RVA: 0x001D8BF0 File Offset: 0x001D6DF0
		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.Faction != Faction.OfPlayer && this.pawn == this.Deliveree)
			{
				this.pawn.jobs.CheckForJobOverride(0f, true);
			}
		}

		// Token: 0x06004D09 RID: 19721 RVA: 0x001D8C50 File Offset: 0x001D6E50
		public static List<Toil> CollectMedicineToils(Pawn doctor, Pawn patient, Job job, Toil gotoToil, out Toil reserveMedicine)
		{
			JobDriver_TendPatient.<>c__DisplayClass22_0 CS$<>8__locals1 = new JobDriver_TendPatient.<>c__DisplayClass22_0();
			CS$<>8__locals1.doctor = doctor;
			JobDriver_TendPatient.tmpCollectToils.Clear();
			CS$<>8__locals1.medicineUsed = job.targetB.Thing;
			JobDriver_TendPatient.<>c__DisplayClass22_0 CS$<>8__locals2 = CS$<>8__locals1;
			Thing medicineUsed = CS$<>8__locals1.medicineUsed;
			CS$<>8__locals2.medicineHolderInventory = (((medicineUsed != null) ? medicineUsed.ParentHolder : null) as Pawn_InventoryTracker);
			CS$<>8__locals1.otherPawnMedicineHolder = job.targetC.Pawn;
			reserveMedicine = Toils_Tend.ReserveMedicine(TargetIndex.B, patient).FailOnDespawnedNullOrForbidden(TargetIndex.B);
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Jump.JumpIf(gotoToil, () => CS$<>8__locals1.medicineUsed != null && CS$<>8__locals1.doctor.inventory.Contains(CS$<>8__locals1.medicineUsed)));
			Toil toil = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch, false).FailOn(delegate()
			{
				Pawn otherPawnMedicineHolder = CS$<>8__locals1.otherPawnMedicineHolder;
				Pawn_InventoryTracker medicineHolderInventory2 = CS$<>8__locals1.medicineHolderInventory;
				return otherPawnMedicineHolder != ((medicineHolderInventory2 != null) ? medicineHolderInventory2.pawn : null) || CS$<>8__locals1.otherPawnMedicineHolder.IsForbidden(CS$<>8__locals1.doctor);
			});
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Haul.CheckItemCarriedByOtherPawn(CS$<>8__locals1.medicineUsed, TargetIndex.C, toil));
			JobDriver_TendPatient.tmpCollectToils.Add(reserveMedicine);
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B));
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Tend.PickupMedicine(TargetIndex.B, patient).FailOnDestroyedOrNull(TargetIndex.B));
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Haul.CheckForGetOpportunityDuplicate(reserveMedicine, TargetIndex.B, TargetIndex.None, true, null));
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_Jump.Jump(gotoToil));
			JobDriver_TendPatient.tmpCollectToils.Add(toil);
			JobDriver_TendPatient.tmpCollectToils.Add(Toils_General.Wait(25, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.C, false, -0.5f));
			List<Toil> list = JobDriver_TendPatient.tmpCollectToils;
			Thing medicineUsed2 = CS$<>8__locals1.medicineUsed;
			ThingOwner innerContainer = CS$<>8__locals1.doctor.inventory.innerContainer;
			Pawn_InventoryTracker medicineHolderInventory = CS$<>8__locals1.medicineHolderInventory;
			list.Add(Toils_Haul.TakeFromOtherInventory(medicineUsed2, innerContainer, (medicineHolderInventory != null) ? medicineHolderInventory.innerContainer : null, Medicine.GetMedicineCountToFullyHeal(patient), TargetIndex.B));
			return JobDriver_TendPatient.tmpCollectToils;
		}

		// Token: 0x06004D0A RID: 19722 RVA: 0x001D8DE0 File Offset: 0x001D6FE0
		public static Toil FindMoreMedicineToil(Pawn doctor, Pawn patient, TargetIndex medicineIndex, Job job, Toil reserveMedicine)
		{
			Toil toil = ToilMaker.MakeToil("FindMoreMedicineToil");
			toil.initAction = delegate()
			{
				if (job.GetTarget(medicineIndex).Thing.DestroyedOrNull())
				{
					Thing thing = HealthAIUtility.FindBestMedicine(doctor, patient, false);
					if (thing != null)
					{
						job.SetTarget(medicineIndex, thing);
						doctor.jobs.curDriver.JumpToToil(reserveMedicine);
					}
				}
			};
			return toil;
		}

		// Token: 0x0400303D RID: 12349
		private bool usesMedicine;

		// Token: 0x0400303E RID: 12350
		private PathEndMode pathEndMode;

		// Token: 0x0400303F RID: 12351
		public const int BaseTendDuration = 600;

		// Token: 0x04003040 RID: 12352
		private const int TicksBetweenSelfTendMotes = 100;

		// Token: 0x04003041 RID: 12353
		private const TargetIndex MedicineIndex = TargetIndex.B;

		// Token: 0x04003042 RID: 12354
		private const TargetIndex MedicineHolderIndex = TargetIndex.C;

		// Token: 0x04003043 RID: 12355
		private static List<Toil> tmpCollectToils = new List<Toil>();
	}
}
