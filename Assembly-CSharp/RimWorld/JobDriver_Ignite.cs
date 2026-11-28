using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A43 RID: 2627
	public class JobDriver_Ignite : JobDriver
	{
		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06004F52 RID: 20306 RVA: 0x001E0914 File Offset: 0x001DEB14
		public Thing TargetThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x001E0935 File Offset: 0x001DEB35
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x001E0938 File Offset: 0x001DEB38
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Ignite.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_Ignite.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030EF RID: 12527
		public const TargetIndex TargetInd = TargetIndex.A;
	}
}
