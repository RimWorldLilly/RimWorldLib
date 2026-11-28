using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E8 RID: 5864
	public class RitualOutcomeEffectWorker_Blinding : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x060092B2 RID: 37554 RVA: 0x00359AF4 File Offset: 0x00357CF4
		public RitualOutcomeEffectWorker_Blinding()
		{
		}

		// Token: 0x060092B3 RID: 37555 RVA: 0x00359AFC File Offset: 0x00357CFC
		public RitualOutcomeEffectWorker_Blinding(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092B4 RID: 37556 RVA: 0x00359B08 File Offset: 0x00357D08
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (ModsConfig.RoyaltyActive && outcome.Positive && (outcome.BestPositiveOutcome(jobRitual) || Rand.Chance(0.5f)))
			{
				Pawn pawn = ((LordJob_Ritual_Mutilation)jobRitual).mutilatedPawns[0];
				extraOutcomeDesc = "RitualOutcomeExtraDesc_BlindingPsylink".Translate(pawn.Named("PAWN"));
				List<Ability> existingAbils = pawn.abilities.AllAbilitiesForReading.ToList<Ability>();
				pawn.ChangePsylinkLevel(1, true);
				Ability ability = pawn.abilities.AllAbilitiesForReading.FirstOrDefault((Ability a) => !existingAbils.Contains(a));
				if (ability != null)
				{
					extraOutcomeDesc += " " + "RitualOutcomeExtraDesc_BlindingPsylinkAbility".Translate(ability.def.LabelCap, pawn.Named("PAWN"));
				}
			}
		}

		// Token: 0x040058CA RID: 22730
		public const float PsylinkGainChance = 0.5f;
	}
}
