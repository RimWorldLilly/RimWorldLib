using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200081D RID: 2077
	public class JobDriver_PickupToHold : JobDriver
	{
		// Token: 0x0600414F RID: 16719 RVA: 0x0019EB3C File Offset: 0x0019CD3C
		public static bool TryMakePreToilReservations(JobDriver driver, bool errorOnFailed)
		{
			driver.pawn.Map.pawnDestinationReservationManager.Reserve(driver.pawn, driver.job, driver.job.GetTarget(TargetIndex.A).Cell);
			return driver.pawn.Reserve(driver.job.GetTarget(TargetIndex.A), driver.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x0019EBA0 File Offset: 0x0019CDA0
		public static IEnumerable<Toil> Toils(JobDriver driver, TargetIndex HeldItem = TargetIndex.A, bool subtractNumTakenFromJobCount = true)
		{
			JobDriver_PickupToHold.<Toils>d__2 <Toils>d__ = new JobDriver_PickupToHold.<Toils>d__2(-2);
			<Toils>d__.<>3__driver = driver;
			<Toils>d__.<>3__HeldItem = HeldItem;
			<Toils>d__.<>3__subtractNumTakenFromJobCount = subtractNumTakenFromJobCount;
			return <Toils>d__;
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x0019EBBE File Offset: 0x0019CDBE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return JobDriver_PickupToHold.TryMakePreToilReservations(this, errorOnFailed);
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x0019EBC7 File Offset: 0x0019CDC7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			return JobDriver_PickupToHold.Toils(this, TargetIndex.A, true);
		}

		// Token: 0x04002B25 RID: 11045
		private const TargetIndex HeldItemInd = TargetIndex.A;
	}
}
