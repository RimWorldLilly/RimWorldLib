using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000800 RID: 2048
	public class JobDriver_BabyPassive : JobDriver
	{
		// Token: 0x0600409B RID: 16539 RVA: 0x0019B9AA File Offset: 0x00199BAA
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x0019B9AD File Offset: 0x00199BAD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			return new JobDriver_BabyPassive.<MakeNewToils>d__1(-2);
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0019B9B6 File Offset: 0x00199BB6
		private static Toil Passive()
		{
			Toil toil = ToilMaker.MakeToil("Passive");
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}
	}
}
