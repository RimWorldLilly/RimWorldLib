using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E9 RID: 5865
	public class RitualOutcomeEffectWorker_Consumable : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x060092B5 RID: 37557 RVA: 0x00359BF8 File Offset: 0x00357DF8
		public RitualOutcomeEffectWorker_Consumable()
		{
		}

		// Token: 0x060092B6 RID: 37558 RVA: 0x00359C00 File Offset: 0x00357E00
		public RitualOutcomeEffectWorker_Consumable(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092B7 RID: 37559 RVA: 0x00359C0C File Offset: 0x00357E0C
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive && Rand.Chance(outcome.BestPositiveOutcome(jobRitual) ? 1f : 0.5f))
			{
				Pawn inspiredPawn = (from p in totalPresence.Keys
				where !p.Inspired && DefDatabase<InspirationDef>.AllDefsListForReading.Any((InspirationDef i) => i.Worker.InspirationCanOccur(p))
				select p).RandomElementWithFallback(null);
				if (inspiredPawn == null)
				{
					return;
				}
				InspirationDef inspirationDef = (from i in DefDatabase<InspirationDef>.AllDefsListForReading
				where i.Worker.InspirationCanOccur(inspiredPawn)
				select i).RandomElementWithFallback(null);
				if (inspirationDef == null)
				{
					Log.Error("Could not find inspiration for pawn " + inspiredPawn.Name.ToStringFull);
					return;
				}
				if (!inspiredPawn.mindState.inspirationHandler.TryStartInspiration(inspirationDef, null, false))
				{
					Log.Error("Inspiring " + inspiredPawn.Name.ToStringFull + " failed, but the inspiration worker claimed it can occur!");
					return;
				}
				extraOutcomeDesc = "RitualOutcomeExtraDesc_ConsumableInspiration".Translate(inspiredPawn.Named("PAWN"), inspirationDef.LabelCap.Named("INSPIRATION"), jobRitual.Ritual.Label.Named("RITUAL")).CapitalizeFirst() + " " + inspiredPawn.Inspiration.LetterText;
			}
		}

		// Token: 0x040058CB RID: 22731
		public const float InspirationGainChance = 0.5f;

		// Token: 0x040058CC RID: 22732
		public const float InspirationGainChanceBestOutcome = 1f;
	}
}
