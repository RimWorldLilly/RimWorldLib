using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A6D RID: 2669
	public class JobDriver_Research : JobDriver
	{
		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06005086 RID: 20614 RVA: 0x001E4609 File Offset: 0x001E2809
		private ResearchProjectDef Project
		{
			get
			{
				return Find.ResearchManager.GetProject(null);
			}
		}

		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06005087 RID: 20615 RVA: 0x001E4616 File Offset: 0x001E2816
		private Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)base.TargetThingA;
			}
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x001E4624 File Offset: 0x001E2824
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed, false) && (!this.ResearchBench.def.hasInteractionCell || this.pawn.ReserveSittableOrSpot(this.ResearchBench.InteractionCell, this.job, errorOnFailed));
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x001E4687 File Offset: 0x001E2887
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Research.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_Research.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003148 RID: 12616
		private const int JobEndInterval = 4000;
	}
}
