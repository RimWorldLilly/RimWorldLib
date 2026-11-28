using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009DB RID: 2523
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06004C83 RID: 19587 RVA: 0x001D7274 File Offset: 0x001D5474
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x001D728C File Offset: 0x001D548C
		public override string GetReport()
		{
			if (this.TargetFire != null && this.TargetFire.parent != null)
			{
				return "ReportExtinguishingFireOn".Translate(this.TargetFire.parent.LabelCap, this.TargetFire.parent.Named("TARGET"));
			}
			return "ReportExtinguishingFire".Translate();
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x001D72F7 File Offset: 0x001D54F7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x001D72FA File Offset: 0x001D54FA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ExtinguishSelf.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_ExtinguishSelf.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
