using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016EC RID: 5868
	public class RitualOutcomeEffectWorker_ChildBirth : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x060092C0 RID: 37568 RVA: 0x0035A0A4 File Offset: 0x003582A4
		public RitualOutcomeEffectWorker_ChildBirth()
		{
		}

		// Token: 0x060092C1 RID: 37569 RVA: 0x0035A0AC File Offset: 0x003582AC
		public RitualOutcomeEffectWorker_ChildBirth(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x17001932 RID: 6450
		// (get) Token: 0x060092C2 RID: 37570 RVA: 0x0035A0B5 File Offset: 0x003582B5
		public override bool ShowQuality
		{
			get
			{
				return !Find.Storyteller.difficulty.babiesAreHealthy;
			}
		}

		// Token: 0x17001933 RID: 6451
		// (get) Token: 0x060092C3 RID: 37571 RVA: 0x0035A0C9 File Offset: 0x003582C9
		public override string Description
		{
			get
			{
				if (!Find.Storyteller.difficulty.babiesAreHealthy)
				{
					return base.Description;
				}
				return "";
			}
		}

		// Token: 0x060092C4 RID: 37572 RVA: 0x0035A0E8 File Offset: 0x003582E8
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			if (progress == 0f)
			{
				return;
			}
			float quality = base.GetQuality(jobRitual, progress);
			RitualOutcomePossibility outcome = this.GetOutcome(quality, jobRitual);
			Pawn pawn = jobRitual.assignments.FirstAssignedPawn("mother");
			Pawn doctor = jobRitual.assignments.FirstAssignedPawn("doctor");
			Hediff_LaborPushing hediff_LaborPushing = (Hediff_LaborPushing)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnancyLaborPushing, false);
			float quality2 = quality;
			Precept_Ritual ritual = jobRitual.Ritual;
			GeneSet geneSet = hediff_LaborPushing.geneSet;
			PregnancyUtility.ApplyBirthOutcome(outcome, quality2, ritual, (geneSet != null) ? geneSet.GenesListForReading : null, hediff_LaborPushing.Mother ?? pawn, pawn, hediff_LaborPushing.Father, doctor, jobRitual, jobRitual.assignments, false);
		}

		// Token: 0x060092C5 RID: 37573 RVA: 0x0035A187 File Offset: 0x00358387
		public override RitualOutcomePossibility GetOutcome(float quality, LordJob_Ritual ritual)
		{
			if (Find.Storyteller.difficulty.babiesAreHealthy)
			{
				return this.def.BestOutcome;
			}
			return base.GetOutcome(quality, ritual);
		}
	}
}
