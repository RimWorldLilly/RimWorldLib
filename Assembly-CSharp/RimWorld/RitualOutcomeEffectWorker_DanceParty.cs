using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E7 RID: 5863
	public class RitualOutcomeEffectWorker_DanceParty : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x060092AF RID: 37551 RVA: 0x00359A01 File Offset: 0x00357C01
		public RitualOutcomeEffectWorker_DanceParty()
		{
		}

		// Token: 0x060092B0 RID: 37552 RVA: 0x00359A09 File Offset: 0x00357C09
		public RitualOutcomeEffectWorker_DanceParty(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092B1 RID: 37553 RVA: 0x00359A14 File Offset: 0x00357C14
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive && Rand.Chance(0.1f))
			{
				float statFactorFromList = HediffDefOf.WorkFocus.stages[0].statOffsets.GetStatFactorFromList(StatDefOf.WorkSpeedGlobal);
				extraOutcomeDesc = "RitualOutcomeExtraDesc_DancePartyWorkFocus".Translate((((statFactorFromList > 0f) ? "+" : "") + statFactorFromList.ToStringPercent()).Named("PERCENTAGE"));
				foreach (Pawn pawn in totalPresence.Keys)
				{
					pawn.health.AddHediff(HediffDefOf.WorkFocus, null, null, null);
				}
			}
		}

		// Token: 0x040058C9 RID: 22729
		public const float WorkFocusChance = 0.1f;
	}
}
