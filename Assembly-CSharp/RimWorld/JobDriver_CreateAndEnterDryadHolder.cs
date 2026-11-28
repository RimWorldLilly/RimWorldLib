using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BD3 RID: 3027
	public abstract class JobDriver_CreateAndEnterDryadHolder : JobDriver
	{
		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x0600570F RID: 22287 RVA: 0x00204EFE File Offset: 0x002030FE
		private CompTreeConnection TreeComp
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x06005710 RID: 22288 RVA: 0x00204F15 File Offset: 0x00203115
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005711 RID: 22289 RVA: 0x00204F18 File Offset: 0x00203118
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CreateAndEnterDryadHolder.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_CreateAndEnterDryadHolder.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06005712 RID: 22290
		public abstract Toil EnterToil();

		// Token: 0x0400332B RID: 13099
		private const int TicksToCreate = 200;
	}
}
