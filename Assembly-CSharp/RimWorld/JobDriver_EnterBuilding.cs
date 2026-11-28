using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A2C RID: 2604
	public class JobDriver_EnterBuilding : JobDriver
	{
		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x001DE69E File Offset: 0x001DC89E
		private Building_Enterable Building
		{
			get
			{
				return (Building_Enterable)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x001DE6B5 File Offset: 0x001DC8B5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x001DE6D8 File Offset: 0x001DC8D8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EnterBuilding.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_EnterBuilding.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030C6 RID: 12486
		public const int EnterDelay = 60;
	}
}
