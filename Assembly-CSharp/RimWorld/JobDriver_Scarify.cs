using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009E9 RID: 2537
	public class JobDriver_Scarify : JobDriver
	{
		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06004CDE RID: 19678 RVA: 0x001D841C File Offset: 0x001D661C
		protected Pawn Target
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004CDF RID: 19679 RVA: 0x001D8444 File Offset: 0x001D6644
		public static bool AvailableOnNow(Pawn pawn, BodyPartRecord part = null)
		{
			return pawn.RaceProps.Humanlike && (Faction.OfPlayerSilentFail == null || Faction.OfPlayer.ideos.AnyPreceptWithRequiredScars()) && (part == null || (!pawn.health.WouldDieAfterAddingHediff(HediffDefOf.Scarification, part, 1f) && !pawn.health.WouldLosePartAfterAddingHediff(HediffDefOf.Scarification, part, 1f)));
		}

		// Token: 0x06004CE0 RID: 19680 RVA: 0x001D84AE File Offset: 0x001D66AE
		public static IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn)
		{
			JobDriver_Scarify.<GetPartsToApplyOn>d__5 <GetPartsToApplyOn>d__ = new JobDriver_Scarify.<GetPartsToApplyOn>d__5(-2);
			<GetPartsToApplyOn>d__.<>3__pawn = pawn;
			return <GetPartsToApplyOn>d__;
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x001D84C0 File Offset: 0x001D66C0
		public static void Scarify(Pawn pawn, BodyPartRecord part)
		{
			if (!ModLister.CheckIdeology("Scarification"))
			{
				return;
			}
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation = lord.LordJob as LordJob_Ritual_Mutilation;
				if (lordJob_Ritual_Mutilation != null)
				{
					lordJob_Ritual_Mutilation.mutilatedPawns.Add(pawn);
				}
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Scarification, pawn, part);
			HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff.TryGetComp<HediffComp_GetsPermanent>();
			hediffComp_GetsPermanent.IsPermanent = true;
			hediffComp_GetsPermanent.SetPainCategory(JobDriver_Scarify.InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted e) => e.weight).category);
			pawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x001D8563 File Offset: 0x001D6763
		public static void CreateHistoryEventDef(Pawn pawn)
		{
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotScarified, pawn.Named(HistoryEventArgsNames.Doer)), true);
		}

		// Token: 0x06004CE3 RID: 19683 RVA: 0x001D8585 File Offset: 0x001D6785
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06004CE4 RID: 19684 RVA: 0x001D85A8 File Offset: 0x001D67A8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Scarify.<MakeNewToils>d__9 <MakeNewToils>d__ = new JobDriver_Scarify.<MakeNewToils>d__9(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04003034 RID: 12340
		private const TargetIndex TargetPawnIndex = TargetIndex.A;

		// Token: 0x04003035 RID: 12341
		public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[]
		{
			new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.8f),
			new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f)
		};
	}
}
