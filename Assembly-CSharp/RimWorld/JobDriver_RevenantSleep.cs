using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A72 RID: 2674
	public class JobDriver_RevenantSleep : JobDriver
	{
		// Token: 0x060050AB RID: 20651 RVA: 0x001E4D40 File Offset: 0x001E2F40
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060050AC RID: 20652 RVA: 0x001E4D43 File Offset: 0x001E2F43
		private CompRevenant Comp
		{
			get
			{
				return this.pawn.TryGetComp<CompRevenant>();
			}
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x001E4D50 File Offset: 0x001E2F50
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RevenantSleep.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_RevenantSleep.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
