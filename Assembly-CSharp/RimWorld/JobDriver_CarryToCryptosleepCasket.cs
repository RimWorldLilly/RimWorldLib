using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A1D RID: 2589
	public class JobDriver_CarryToCryptosleepCasket : JobDriver
	{
		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06004E3A RID: 20026 RVA: 0x001DD0F4 File Offset: 0x001DB2F4
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06004E3B RID: 20027 RVA: 0x001DD11C File Offset: 0x001DB31C
		protected Building_CryptosleepCasket DropPod
		{
			get
			{
				return (Building_CryptosleepCasket)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004E3C RID: 20028 RVA: 0x001DD144 File Offset: 0x001DB344
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DropPod, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E3D RID: 20029 RVA: 0x001DD197 File Offset: 0x001DB397
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CarryToCryptosleepCasket.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_CarryToCryptosleepCasket.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x001DD1A7 File Offset: 0x001DB3A7
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				this.Takee
			};
		}

		// Token: 0x040030A4 RID: 12452
		private const TargetIndex TakeeInd = TargetIndex.A;

		// Token: 0x040030A5 RID: 12453
		private const TargetIndex DropPodInd = TargetIndex.B;
	}
}
