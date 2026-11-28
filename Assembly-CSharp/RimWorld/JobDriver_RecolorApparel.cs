using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A63 RID: 2659
	public class JobDriver_RecolorApparel : JobDriver
	{
		// Token: 0x0600503D RID: 20541 RVA: 0x001E3AA0 File Offset: 0x001E1CA0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.C), this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			Thing thing = this.job.GetTarget(TargetIndex.C).Thing;
			if (thing != null && thing.def.hasInteractionCell && !this.pawn.ReserveSittableOrSpot(thing.InteractionCell, this.job, errorOnFailed))
			{
				return false;
			}
			int num = this.job.GetTargetQueue(TargetIndex.B).Count;
			foreach (LocalTargetInfo target in this.job.GetTargetQueue(TargetIndex.A))
			{
				int num2 = Mathf.Min(num, target.Thing.stackCount);
				if (!this.pawn.Reserve(target, this.job, 1, num2, null, errorOnFailed, false))
				{
					return false;
				}
				num -= num2;
				if (num2 <= 0)
				{
					break;
				}
			}
			return true;
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x001E3BB0 File Offset: 0x001E1DB0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RecolorApparel.<MakeNewToils>d__5 <MakeNewToils>d__ = new JobDriver_RecolorApparel.<MakeNewToils>d__5(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003133 RID: 12595
		public const TargetIndex DyeInd = TargetIndex.A;

		// Token: 0x04003134 RID: 12596
		public const TargetIndex ApparelInd = TargetIndex.B;

		// Token: 0x04003135 RID: 12597
		public const TargetIndex StylingStationInd = TargetIndex.C;

		// Token: 0x04003136 RID: 12598
		public const int WorkTimeTicks = 1000;
	}
}
