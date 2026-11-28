using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A73 RID: 2675
	public class JobDriver_RevenantWander : JobDriver
	{
		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x060050B1 RID: 20657 RVA: 0x001E4E14 File Offset: 0x001E3014
		private CompRevenant Comp
		{
			get
			{
				return this.pawn.TryGetComp<CompRevenant>();
			}
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x001E4E21 File Offset: 0x001E3021
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x001E4E24 File Offset: 0x001E3024
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RevenantWander.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_RevenantWander.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003159 RID: 12633
		private const int SmearMTBDays = 2;

		// Token: 0x0400315A RID: 12634
		private const int MinSmearInterval = 60000;
	}
}
