using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A26 RID: 2598
	public class JobDriver_DismissTrader : JobDriver
	{
		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06004E7F RID: 20095 RVA: 0x001DDE6C File Offset: 0x001DC06C
		private Pawn Trader
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x001DDE79 File Offset: 0x001DC079
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Trader, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x001DDE9C File Offset: 0x001DC09C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_DismissTrader.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_DismissTrader.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
