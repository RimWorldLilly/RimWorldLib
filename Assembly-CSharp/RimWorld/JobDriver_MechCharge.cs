using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AA3 RID: 2723
	public class JobDriver_MechCharge : JobDriver
	{
		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x06005201 RID: 20993 RVA: 0x001E8DC9 File Offset: 0x001E6FC9
		public Building_MechCharger Charger
		{
			get
			{
				return (Building_MechCharger)this.job.targetA.Thing;
			}
		}

		// Token: 0x06005202 RID: 20994 RVA: 0x001E8DE0 File Offset: 0x001E6FE0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Charger, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x001E8E08 File Offset: 0x001E7008
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_MechCharge.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_MechCharge.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040031B1 RID: 12721
		private const TargetIndex ChargerInd = TargetIndex.A;
	}
}
