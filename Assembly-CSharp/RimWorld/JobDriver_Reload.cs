using System;
using System.Collections.Generic;
using RimWorld.Utility;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A68 RID: 2664
	public class JobDriver_Reload : JobDriver
	{
		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06005062 RID: 20578 RVA: 0x001E4000 File Offset: 0x001E2200
		private Thing Gear
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x001E4021 File Offset: 0x001E2221
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x001E4044 File Offset: 0x001E2244
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Reload.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_Reload.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x001E4054 File Offset: 0x001E2254
		private IEnumerable<Toil> ReloadAsMuchAsPossible(IReloadableComp reloadable)
		{
			JobDriver_Reload.<ReloadAsMuchAsPossible>d__6 <ReloadAsMuchAsPossible>d__ = new JobDriver_Reload.<ReloadAsMuchAsPossible>d__6(-2);
			<ReloadAsMuchAsPossible>d__.<>4__this = this;
			<ReloadAsMuchAsPossible>d__.<>3__reloadable = reloadable;
			return <ReloadAsMuchAsPossible>d__;
		}

		// Token: 0x04003141 RID: 12609
		private const TargetIndex GearInd = TargetIndex.A;

		// Token: 0x04003142 RID: 12610
		private const TargetIndex AmmoInd = TargetIndex.B;
	}
}
