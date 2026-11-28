using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A84 RID: 2692
	public class JobDriver_TradeWithPawn : JobDriver
	{
		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x0600512F RID: 20783 RVA: 0x001E6498 File Offset: 0x001E4698
		private Pawn Trader
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x001E64A5 File Offset: 0x001E46A5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Trader, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x001E64C8 File Offset: 0x001E46C8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TradeWithPawn.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_TradeWithPawn.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
