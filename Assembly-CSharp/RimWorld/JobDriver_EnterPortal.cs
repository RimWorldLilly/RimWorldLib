using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A2E RID: 2606
	public class JobDriver_EnterPortal : JobDriver
	{
		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x06004EAF RID: 20143 RVA: 0x001DE76C File Offset: 0x001DC96C
		public MapPortal MapPortal
		{
			get
			{
				return base.TargetThingA as MapPortal;
			}
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x001DE779 File Offset: 0x001DC979
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x001DE77C File Offset: 0x001DC97C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EnterPortal.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_EnterPortal.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030C7 RID: 12487
		private TargetIndex PortalInd = TargetIndex.A;

		// Token: 0x040030C8 RID: 12488
		private const int EnterDelay = 90;
	}
}
