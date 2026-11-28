using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A14 RID: 2580
	public class JobDriver_ApplyTechprint : JobDriver
	{
		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06004DEB RID: 19947 RVA: 0x001DC0F8 File Offset: 0x001DA2F8
		protected Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06004DEC RID: 19948 RVA: 0x001DC120 File Offset: 0x001DA320
		protected Thing Techprint
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06004DED RID: 19949 RVA: 0x001DC141 File Offset: 0x001DA341
		protected CompTechprint TechprintComp
		{
			get
			{
				return this.Techprint.TryGetComp<CompTechprint>();
			}
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x001DC150 File Offset: 0x001DA350
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Techprint, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x001DC1A3 File Offset: 0x001DA3A3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_ApplyTechprint.<MakeNewToils>d__11 <MakeNewToils>d__ = new JobDriver_ApplyTechprint.<MakeNewToils>d__11(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003086 RID: 12422
		private const TargetIndex ResearchBenchInd = TargetIndex.A;

		// Token: 0x04003087 RID: 12423
		private const TargetIndex TechprintInd = TargetIndex.B;

		// Token: 0x04003088 RID: 12424
		private const TargetIndex HaulCell = TargetIndex.C;

		// Token: 0x04003089 RID: 12425
		private const int Duration = 600;
	}
}
