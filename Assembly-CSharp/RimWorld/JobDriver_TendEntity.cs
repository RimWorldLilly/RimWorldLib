using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009EC RID: 2540
	public class JobDriver_TendEntity : JobDriver
	{
		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x06004CF2 RID: 19698 RVA: 0x001D8814 File Offset: 0x001D6A14
		private Thing Platform
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x06004CF3 RID: 19699 RVA: 0x001D881C File Offset: 0x001D6A1C
		private Pawn InnerPawn
		{
			get
			{
				Building_HoldingPlatform building_HoldingPlatform = this.Platform as Building_HoldingPlatform;
				if (building_HoldingPlatform == null)
				{
					return null;
				}
				return building_HoldingPlatform.HeldPawn;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x06004CF4 RID: 19700 RVA: 0x001D8834 File Offset: 0x001D6A34
		private Thing MedicineUsed
		{
			get
			{
				return this.job.targetB.Thing;
			}
		}

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06004CF5 RID: 19701 RVA: 0x001D8846 File Offset: 0x001D6A46
		protected bool IsMedicineInDoctorInventory
		{
			get
			{
				return this.MedicineUsed != null && this.pawn.inventory.Contains(this.MedicineUsed);
			}
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x001D8868 File Offset: 0x001D6A68
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usesMedicine, "usesMedicine", false, false);
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x001D8882 File Offset: 0x001D6A82
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usesMedicine = (this.MedicineUsed != null);
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x001D889C File Offset: 0x001D6A9C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (this.usesMedicine)
			{
				int num = this.pawn.Map.reservationManager.CanReserveStack(this.pawn, this.MedicineUsed, 10, null, false);
				if (num <= 0)
				{
					return false;
				}
				int stackCount = Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(this.InnerPawn));
				if (!this.pawn.Reserve(this.MedicineUsed, this.job, 10, stackCount, null, errorOnFailed, false))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x001D8940 File Offset: 0x001D6B40
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TendEntity.<MakeNewToils>d__15 <MakeNewToils>d__ = new JobDriver_TendEntity.<MakeNewToils>d__15(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x001D8950 File Offset: 0x001D6B50
		public override string GetReport()
		{
			return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.InnerPawn, this.job.targetB, this.job.targetC);
		}

		// Token: 0x04003039 RID: 12345
		private bool usesMedicine;

		// Token: 0x0400303A RID: 12346
		private const TargetIndex PlatformIndex = TargetIndex.A;

		// Token: 0x0400303B RID: 12347
		private const TargetIndex MedicineIndex = TargetIndex.B;

		// Token: 0x0400303C RID: 12348
		private const int MaxMedicineReservations = 10;
	}
}
