using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A23 RID: 2595
	public class JobDriver_CreateXenogerm : JobDriver
	{
		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x06004E68 RID: 20072 RVA: 0x001DD989 File Offset: 0x001DBB89
		private Building_GeneAssembler Xenogerminator
		{
			get
			{
				return (Building_GeneAssembler)base.TargetThingA;
			}
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x001DD998 File Offset: 0x001DBB98
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Xenogerminator, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.Xenogerminator.InteractionCell, this.job, errorOnFailed);
		}

		// Token: 0x06004E6A RID: 20074 RVA: 0x001DD9E7 File Offset: 0x001DBBE7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CreateXenogerm.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_CreateXenogerm.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030B4 RID: 12468
		private const int JobEndInterval = 4000;
	}
}
