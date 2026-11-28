using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A57 RID: 2647
	public class JobDriver_NociosphereDepart : JobDriver
	{
		// Token: 0x06004FDB RID: 20443 RVA: 0x001E25E1 File Offset: 0x001E07E1
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x001E25E4 File Offset: 0x001E07E4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			return new JobDriver_NociosphereDepart.<MakeNewToils>d__2(-2);
		}

		// Token: 0x04003116 RID: 12566
		private static readonly int LeaveTimeTicks = 720;
	}
}
