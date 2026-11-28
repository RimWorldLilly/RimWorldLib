using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A49 RID: 2633
	public class JobDriver_InvestigateMonolith : JobDriver
	{
		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06004F7F RID: 20351 RVA: 0x001E12D3 File Offset: 0x001DF4D3
		private Building_VoidMonolith VoidMonolith
		{
			get
			{
				return base.TargetThingA as Building_VoidMonolith;
			}
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x001E12E0 File Offset: 0x001DF4E0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.VoidMonolith, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.VoidMonolith.InteractionCell, this.job, errorOnFailed);
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x001E1334 File Offset: 0x001DF534
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_InvestigateMonolith.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_InvestigateMonolith.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040030FC RID: 12540
		private const int InvestigateSeconds = 5;
	}
}
