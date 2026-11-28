using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A47 RID: 2631
	public class JobDriver_Insult : JobDriver
	{
		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x06004F6A RID: 20330 RVA: 0x001E0CEE File Offset: 0x001DEEEE
		private Pawn Target
		{
			get
			{
				return (Pawn)((Thing)this.pawn.CurJob.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x001E0D0B File Offset: 0x001DEF0B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x001E0D0E File Offset: 0x001DEF0E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Insult.<MakeNewToils>d__4 <MakeNewToils>d__ = new JobDriver_Insult.<MakeNewToils>d__4(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x001E0D1E File Offset: 0x001DEF1E
		private Toil InteractToil()
		{
			return Toils_General.Do(delegate
			{
				if (this.pawn.interactions.TryInteractWith(this.Target, InteractionDefOf.Insult))
				{
					MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
					if (mentalState_InsultingSpree != null)
					{
						mentalState_InsultingSpree.lastInsultTicks = Find.TickManager.TicksGame;
						if (mentalState_InsultingSpree.target == this.Target)
						{
							mentalState_InsultingSpree.insultedTargetAtLeastOnce = true;
						}
					}
				}
			});
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x001E0D31 File Offset: 0x001DEF31
		private Toil InsultingSpreeDelayToil()
		{
			Toil toil = ToilMaker.MakeToil("InsultingSpreeDelayToil");
			toil.initAction = new Action(this.<InsultingSpreeDelayToil>g__WaitAction|6_0);
			toil.tickIntervalAction = delegate(int delta)
			{
				this.<InsultingSpreeDelayToil>g__WaitAction|6_0();
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x001E0DDC File Offset: 0x001DEFDC
		[CompilerGenerated]
		private void <InsultingSpreeDelayToil>g__WaitAction|6_0()
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = this.pawn.MentalState as MentalState_InsultingSpree;
			if (mentalState_InsultingSpree == null || Find.TickManager.TicksGame - mentalState_InsultingSpree.lastInsultTicks >= 1200)
			{
				this.pawn.jobs.curDriver.ReadyForNextToil();
			}
		}

		// Token: 0x040030F8 RID: 12536
		private const TargetIndex TargetInd = TargetIndex.A;
	}
}
