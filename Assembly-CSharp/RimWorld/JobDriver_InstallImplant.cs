using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000A45 RID: 2629
	public class JobDriver_InstallImplant : JobDriver
	{
		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06004F5B RID: 20315 RVA: 0x001E09CC File Offset: 0x001DEBCC
		private Pawn TargetPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x06004F5C RID: 20316 RVA: 0x001E09F4 File Offset: 0x001DEBF4
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x001E0A18 File Offset: 0x001DEC18
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.TargetPawn, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x001E0A6B File Offset: 0x001DEC6B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_InstallImplant.<MakeNewToils>d__8 <MakeNewToils>d__ = new JobDriver_InstallImplant.<MakeNewToils>d__8(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x001E0A7C File Offset: 0x001DEC7C
		private void Install()
		{
			CompTargetEffect_InstallImplantInOtherPawn compTargetEffect_InstallImplantInOtherPawn = this.Item.TryGetComp<CompTargetEffect_InstallImplantInOtherPawn>();
			if (compTargetEffect_InstallImplantInOtherPawn == null)
			{
				return;
			}
			BodyPartRecord bodyPartRecord = this.TargetPawn.RaceProps.body.GetPartsWithDef(compTargetEffect_InstallImplantInOtherPawn.Props.bodyPart).FirstOrFallback(null);
			if (bodyPartRecord == null)
			{
				return;
			}
			Hediff firstHediffOfDef = this.TargetPawn.health.hediffSet.GetFirstHediffOfDef(compTargetEffect_InstallImplantInOtherPawn.Props.hediffDef, false);
			if (firstHediffOfDef == null && !compTargetEffect_InstallImplantInOtherPawn.Props.requiresExistingHediff)
			{
				this.TargetPawn.health.AddHediff(compTargetEffect_InstallImplantInOtherPawn.Props.hediffDef, bodyPartRecord, null, null);
			}
			else if (compTargetEffect_InstallImplantInOtherPawn.Props.canUpgrade)
			{
				((Hediff_Level)firstHediffOfDef).ChangeLevel(1);
			}
			if (this.TargetPawn.Map == Find.CurrentMap && compTargetEffect_InstallImplantInOtherPawn.Props.soundOnUsed != null)
			{
				compTargetEffect_InstallImplantInOtherPawn.Props.soundOnUsed.PlayOneShot(SoundInfo.InMap(this.TargetPawn, MaintenanceType.None));
			}
			this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
		}

		// Token: 0x040030F1 RID: 12529
		private const TargetIndex TargetPawnInd = TargetIndex.A;

		// Token: 0x040030F2 RID: 12530
		private const TargetIndex ItemInd = TargetIndex.B;

		// Token: 0x040030F3 RID: 12531
		private const int DurationTicks = 600;
	}
}
