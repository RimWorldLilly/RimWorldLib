using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A2F RID: 2607
	public class JobDriver_EnterTransporter : JobDriver
	{
		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x06004EB6 RID: 20150 RVA: 0x001DE99C File Offset: 0x001DCB9C
		public CompTransporter Transporter
		{
			get
			{
				Thing thing = this.job.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x06004EB7 RID: 20151 RVA: 0x001DE9D0 File Offset: 0x001DCBD0
		public CompShuttle Shuttle
		{
			get
			{
				Thing thing = this.job.GetTarget(this.TransporterInd).Thing;
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompShuttle>();
			}
		}

		// Token: 0x06004EB8 RID: 20152 RVA: 0x001DEA02 File Offset: 0x001DCC02
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004EB9 RID: 20153 RVA: 0x001DEA05 File Offset: 0x001DCC05
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EnterTransporter.<MakeNewToils>d__6 <MakeNewToils>d__ = new JobDriver_EnterTransporter.<MakeNewToils>d__6(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030C9 RID: 12489
		private TargetIndex TransporterInd = TargetIndex.A;
	}
}
