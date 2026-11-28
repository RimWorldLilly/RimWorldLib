using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200081C RID: 2076
	public class JobDriver_IdleWhileDespawned : JobDriver
	{
		// Token: 0x06004149 RID: 16713 RVA: 0x0019EAD4 File Offset: 0x0019CCD4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x0019EAD7 File Offset: 0x0019CCD7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_IdleWhileDespawned.<MakeNewToils>d__1 <MakeNewToils>d__ = new JobDriver_IdleWhileDespawned.<MakeNewToils>d__1(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x0019EAE7 File Offset: 0x0019CCE7
		private static Toil IdleWhileDespawned()
		{
			Toil toil = ToilMaker.MakeToil("IdleWhileDespawned");
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x0019EAFA File Offset: 0x0019CCFA
		public override string GetReport()
		{
			Log.ErrorOnce(this.pawn.ToStringSafe<Pawn>() + " reported job string for IdleWhileDespawned.  An inspectible despawned pawn should have a different job.", 1560125928);
			return base.GetReport();
		}
	}
}
