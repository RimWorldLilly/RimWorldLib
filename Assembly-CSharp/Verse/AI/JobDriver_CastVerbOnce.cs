using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200082E RID: 2094
	public class JobDriver_CastVerbOnce : JobDriver
	{
		// Token: 0x0600419B RID: 16795 RVA: 0x0019FAC8 File Offset: 0x0019DCC8
		public override string GetReport()
		{
			string value;
			if (base.TargetA.HasThing)
			{
				value = base.TargetThingA.LabelCap;
			}
			else
			{
				value = "AreaLower".Translate();
			}
			if (this.job.verbToUse == null)
			{
				return null;
			}
			return "UsingVerb".Translate(this.job.verbToUse.ReportLabel, value);
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x0019FB3C File Offset: 0x0019DD3C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x0019FB3F File Offset: 0x0019DD3F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_CastVerbOnce.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_CastVerbOnce.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
