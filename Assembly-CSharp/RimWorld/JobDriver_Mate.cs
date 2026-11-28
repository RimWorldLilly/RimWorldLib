using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009B5 RID: 2485
	public class JobDriver_Mate : JobDriver
	{
		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x06004B3B RID: 19259 RVA: 0x001D2DB8 File Offset: 0x001D0FB8
		private Pawn Female
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x001D2DDE File Offset: 0x001D0FDE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x001D2DE1 File Offset: 0x001D0FE1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Mate.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_Mate.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FBB RID: 12219
		private const int MateDuration = 500;

		// Token: 0x04002FBC RID: 12220
		private const TargetIndex FemInd = TargetIndex.A;

		// Token: 0x04002FBD RID: 12221
		public const int TicksBetweenHeartMotes = 100;
	}
}
