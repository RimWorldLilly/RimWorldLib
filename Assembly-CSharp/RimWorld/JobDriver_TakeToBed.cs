using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A82 RID: 2690
	public class JobDriver_TakeToBed : JobDriver
	{
		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06005114 RID: 20756 RVA: 0x001E5DE4 File Offset: 0x001E3FE4
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06005115 RID: 20757 RVA: 0x001E5E0C File Offset: 0x001E400C
		protected Building_Bed DropBed
		{
			get
			{
				return (Building_Bed)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06005116 RID: 20758 RVA: 0x001E5E34 File Offset: 0x001E4034
		private bool TakeeRescued
		{
			get
			{
				return this.Takee.RaceProps.Humanlike && this.job.def != JobDefOf.Arrest && !this.Takee.IsPrisonerOfColony && (!this.Takee.ageTracker.CurLifeStage.alwaysDowned || HealthAIUtility.ShouldSeekMedicalRest(this.Takee));
			}
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x001E5E98 File Offset: 0x001E4098
		public override string GetReport()
		{
			if (this.job.def == JobDefOf.Rescue && !this.TakeeRescued)
			{
				return "TakingToBed".Translate(this.Takee);
			}
			return base.GetReport();
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x001E5ED8 File Offset: 0x001E40D8
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.Takee.ClearAllReservations(true);
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DropBed, this.job, this.DropBed.SleepingSlotsCount, 0, null, errorOnFailed, false);
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x001E5F41 File Offset: 0x001E4141
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_TakeToBed.<MakeNewToils>d__10 <MakeNewToils>d__ = new JobDriver_TakeToBed.<MakeNewToils>d__10(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x001E5F54 File Offset: 0x001E4154
		private void CheckMakeTakeePrisoner()
		{
			if (this.job.def.makeTargetPrisoner)
			{
				if (this.Takee.guest.Released)
				{
					this.Takee.guest.Released = false;
					this.Takee.guest.SetExclusiveInteraction(PrisonerInteractionModeDefOf.MaintainOnly);
					GenGuest.RemoveHealthyPrisonerReleasedThoughts(this.Takee);
				}
				if (!this.Takee.IsPrisonerOfColony)
				{
					this.Takee.guest.CapturedBy(Faction.OfPlayer, this.pawn);
				}
			}
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x001E5FE0 File Offset: 0x001E41E0
		private void CheckMakeTakeeGuest()
		{
			if (!this.job.def.makeTargetPrisoner && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan() && this.Takee.DevelopmentalStage != DevelopmentalStage.Baby)
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Guest);
				QuestUtility.SendQuestTargetSignals(this.Takee.questTags, "Rescued", this.Takee.Named("SUBJECT"));
			}
		}

		// Token: 0x04003179 RID: 12665
		private const TargetIndex TakeeIndex = TargetIndex.A;

		// Token: 0x0400317A RID: 12666
		private const TargetIndex BedIndex = TargetIndex.B;
	}
}
