using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A41 RID: 2625
	public class JobDriver_Hunt : JobDriver
	{
		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06004F43 RID: 20291 RVA: 0x001E0634 File Offset: 0x001DE834
		public Pawn Victim
		{
			get
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06004F44 RID: 20292 RVA: 0x001E066C File Offset: 0x001DE86C
		private Corpse Corpse
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Corpse;
			}
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x001E0692 File Offset: 0x001DE892
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.jobStartTick, "jobStartTick", 0, false);
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x001E06AC File Offset: 0x001DE8AC
		public override string GetReport()
		{
			if (this.Victim != null)
			{
				return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.Victim);
			}
			return base.GetReport();
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x001E06DD File Offset: 0x001DE8DD
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x001E0700 File Offset: 0x001DE900
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Hunt.<MakeNewToils>d__13 <MakeNewToils>d__ = new JobDriver_Hunt.<MakeNewToils>d__13(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x001E0710 File Offset: 0x001DE910
		private Toil StartCollectCorpseToil()
		{
			Toil toil = ToilMaker.MakeToil("StartCollectCorpseToil");
			toil.initAction = delegate()
			{
				if (this.Victim == null)
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.Hunted, new object[]
				{
					this.pawn,
					this.Victim
				});
				Corpse corpse = this.Victim.Corpse;
				if (corpse == null || !this.pawn.CanReserveAndReach(corpse, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, false))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					return;
				}
				corpse.SetForbidden(false, true);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(corpse, this.pawn, this.Map, StoragePriority.Unstored, this.pawn.Faction, out c, true))
				{
					this.pawn.Reserve(corpse, this.job, 1, -1, null, true, false);
					this.pawn.Reserve(c, this.job, 1, -1, null, true, false);
					this.job.SetTarget(TargetIndex.B, c);
					this.job.SetTarget(TargetIndex.A, corpse);
					this.job.count = 1;
					this.job.haulMode = HaulMode.ToCellStorage;
					return;
				}
				this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
			};
			return toil;
		}

		// Token: 0x040030E9 RID: 12521
		private int jobStartTick = -1;

		// Token: 0x040030EA RID: 12522
		private const TargetIndex VictimInd = TargetIndex.A;

		// Token: 0x040030EB RID: 12523
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x040030EC RID: 12524
		private const TargetIndex StoreCellInd = TargetIndex.B;

		// Token: 0x040030ED RID: 12525
		private const int MaxHuntTicks = 5000;

		// Token: 0x040030EE RID: 12526
		public const float MaxRangeFactor = 0.95f;
	}
}
