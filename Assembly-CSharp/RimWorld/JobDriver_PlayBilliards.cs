using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009F6 RID: 2550
	public class JobDriver_PlayBilliards : JobDriver
	{
		// Token: 0x06004D3C RID: 19772 RVA: 0x001D9D3F File Offset: 0x001D7F3F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed, false);
		}

		// Token: 0x06004D3D RID: 19773 RVA: 0x001D9D71 File Offset: 0x001D7F71
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PlayBilliards.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_PlayBilliards.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x001D9D84 File Offset: 0x001D7F84
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}

		// Token: 0x04003054 RID: 12372
		private const int ShotDuration = 600;
	}
}
