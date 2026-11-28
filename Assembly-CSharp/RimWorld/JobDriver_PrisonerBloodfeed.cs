using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B49 RID: 2889
	public class JobDriver_PrisonerBloodfeed : JobDriver
	{
		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x0600549A RID: 21658 RVA: 0x001F70D4 File Offset: 0x001F52D4
		protected Pawn Prisoner
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x001F70EB File Offset: 0x001F52EB
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x001F710E File Offset: 0x001F530E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_PrisonerBloodfeed.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_PrisonerBloodfeed.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0400328C RID: 12940
		public const float BloodLoss = 0.4499f;

		// Token: 0x0400328D RID: 12941
		public const int WaitTicks = 120;

		// Token: 0x0400328E RID: 12942
		private const float HemogenGain = 0.2f;

		// Token: 0x0400328F RID: 12943
		private const float NutritionGain = 0.1f;
	}
}
