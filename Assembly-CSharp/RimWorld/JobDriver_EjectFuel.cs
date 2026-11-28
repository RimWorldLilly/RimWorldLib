using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A29 RID: 2601
	public class JobDriver_EjectFuel : JobDriver
	{
		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x06004E8B RID: 20107 RVA: 0x001DE14C File Offset: 0x001DC34C
		private Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06004E8C RID: 20108 RVA: 0x001DE160 File Offset: 0x001DC360
		private CompRefuelable Refuelable
		{
			get
			{
				CompRefuelable result;
				if ((result = this.compRefuelable) == null)
				{
					result = (this.compRefuelable = this.job.targetA.Thing.TryGetComp<CompRefuelable>());
				}
				return result;
			}
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x001DE195 File Offset: 0x001DC395
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x001DE1B8 File Offset: 0x001DC3B8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EjectFuel.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_EjectFuel.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030BC RID: 12476
		public const int EjectTicks = 120;

		// Token: 0x040030BD RID: 12477
		private CompRefuelable compRefuelable;
	}
}
