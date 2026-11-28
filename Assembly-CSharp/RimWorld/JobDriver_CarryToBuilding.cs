using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A19 RID: 2585
	public class JobDriver_CarryToBuilding : JobDriver
	{
		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06004E1A RID: 19994 RVA: 0x001DCC78 File Offset: 0x001DAE78
		private Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x06004E1B RID: 19995 RVA: 0x001DCCA0 File Offset: 0x001DAEA0
		private Building_Enterable Building
		{
			get
			{
				return (Building_Enterable)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x001DCCC6 File Offset: 0x001DAEC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x001DCCE9 File Offset: 0x001DAEE9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CarryToBuilding.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_CarryToBuilding.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400309A RID: 12442
		private const TargetIndex BuildingInd = TargetIndex.A;

		// Token: 0x0400309B RID: 12443
		private const TargetIndex TakeeInd = TargetIndex.B;
	}
}
