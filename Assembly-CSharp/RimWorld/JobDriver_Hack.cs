using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A3D RID: 2621
	public class JobDriver_Hack : JobDriver
	{
		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06004F1C RID: 20252 RVA: 0x001DFDA5 File Offset: 0x001DDFA5
		private Thing HackTarget
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06004F1D RID: 20253 RVA: 0x001DFDAD File Offset: 0x001DDFAD
		private CompHackable CompHacking
		{
			get
			{
				return this.HackTarget.TryGetComp<CompHackable>();
			}
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x001DFDBA File Offset: 0x001DDFBA
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.HackTarget, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x001DFDDD File Offset: 0x001DDFDD
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Hack.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_Hack.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
