using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009CE RID: 2510
	public class JobDriver_FixBrokenDownBuilding : JobDriver
	{
		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x06004C34 RID: 19508 RVA: 0x001D694C File Offset: 0x001D4B4C
		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x06004C35 RID: 19509 RVA: 0x001D6974 File Offset: 0x001D4B74
		private Thing Components
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x001D6998 File Offset: 0x001D4B98
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Building, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Components, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x001D69EB File Offset: 0x001D4BEB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FixBrokenDownBuilding.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_FixBrokenDownBuilding.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003001 RID: 12289
		private const TargetIndex BuildingInd = TargetIndex.A;

		// Token: 0x04003002 RID: 12290
		private const TargetIndex ComponentInd = TargetIndex.B;

		// Token: 0x04003003 RID: 12291
		private const int TicksDuration = 1000;
	}
}
