using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AAE RID: 2734
	public class JobDriver_RemoveApparel : JobDriver
	{
		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06005262 RID: 21090 RVA: 0x001EB2B0 File Offset: 0x001E94B0
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x001EB2D6 File Offset: 0x001E94D6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x001EB2F0 File Offset: 0x001E94F0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x001EB2F3 File Offset: 0x001E94F3
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x001EB31A File Offset: 0x001E951A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_RemoveApparel.<MakeNewToils>d__7 <MakeNewToils>d__ = new JobDriver_RemoveApparel.<MakeNewToils>d__7(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x040031CE RID: 12750
		private int duration;

		// Token: 0x040031CF RID: 12751
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
