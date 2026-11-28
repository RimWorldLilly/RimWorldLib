using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A6B RID: 2667
	public class JobDriver_RepairMechRemote : JobDriver_RepairMech
	{
		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x0600507A RID: 20602 RVA: 0x001E438D File Offset: 0x001E258D
		protected override bool Remote
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x001E4390 File Offset: 0x001E2590
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x001E4393 File Offset: 0x001E2593
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RepairMechRemote.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_RepairMechRemote.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
