using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A0B RID: 2571
	public class JobDriver_Radiotalking : JobDriver
	{
		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x06004DB3 RID: 19891 RVA: 0x001DB6E6 File Offset: 0x001D98E6
		public Building_CommsConsole CommsConsole
		{
			get
			{
				return (Building_CommsConsole)base.TargetThingA;
			}
		}

		// Token: 0x06004DB4 RID: 19892 RVA: 0x001DB6F3 File Offset: 0x001D98F3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x001DB711 File Offset: 0x001D9911
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Radiotalking.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_Radiotalking.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
