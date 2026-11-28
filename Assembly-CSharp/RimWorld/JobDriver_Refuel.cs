using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A64 RID: 2660
	public class JobDriver_Refuel : JobDriver
	{
		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06005041 RID: 20545 RVA: 0x001E3C74 File Offset: 0x001E1E74
		protected Thing Refuelable
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x06005042 RID: 20546 RVA: 0x001E3C95 File Offset: 0x001E1E95
		protected CompRefuelable RefuelableComp
		{
			get
			{
				return this.Refuelable.TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06005043 RID: 20547 RVA: 0x001E3CA4 File Offset: 0x001E1EA4
		protected Thing Fuel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x001E3CC8 File Offset: 0x001E1EC8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Refuelable, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Fuel, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x001E3D1B File Offset: 0x001E1F1B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Refuel.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_Refuel.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003137 RID: 12599
		private const TargetIndex RefuelableInd = TargetIndex.A;

		// Token: 0x04003138 RID: 12600
		private const TargetIndex FuelInd = TargetIndex.B;

		// Token: 0x04003139 RID: 12601
		public const int RefuelingDuration = 240;
	}
}
