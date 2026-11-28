using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016EB RID: 5867
	public class RitualOutcomeEffectWorker_Duel : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x060092BC RID: 37564 RVA: 0x00359F52 File Offset: 0x00358152
		public RitualOutcomeEffectWorker_Duel()
		{
		}

		// Token: 0x060092BD RID: 37565 RVA: 0x00359F5A File Offset: 0x0035815A
		public RitualOutcomeEffectWorker_Duel(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092BE RID: 37566 RVA: 0x00359F63 File Offset: 0x00358163
		protected override bool OutcomePossible(RitualOutcomePossibility chance, LordJob_Ritual ritual)
		{
			if (!chance.BestPositiveOutcome(ritual))
			{
				return true;
			}
			return ((LordJob_Ritual_Duel)ritual).duelists.Any((Pawn d) => d.Dead);
		}

		// Token: 0x060092BF RID: 37567 RVA: 0x00359FA0 File Offset: 0x003581A0
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive)
			{
				float amount = outcome.BestPositiveOutcome(jobRitual) ? 0.5f : 0.25f;
				float xp = outcome.BestPositiveOutcome(jobRitual) ? 5000f : 2500f;
				float xp2 = outcome.BestPositiveOutcome(jobRitual) ? 2000f : 1000f;
				LordJob_Ritual_Duel lordJob_Ritual_Duel = (LordJob_Ritual_Duel)jobRitual;
				foreach (Pawn pawn in totalPresence.Keys)
				{
					if (lordJob_Ritual_Duel.duelists.Contains(pawn))
					{
						pawn.skills.Learn(SkillDefOf.Melee, xp, false, false);
					}
					else
					{
						pawn.skills.Learn(SkillDefOf.Melee, xp2, false, false);
						if (pawn.needs.joy != null)
						{
							pawn.needs.joy.GainJoy(amount, JoyKindDefOf.Social);
						}
					}
				}
			}
		}

		// Token: 0x040058D0 RID: 22736
		public const float RecreationGainGood = 0.25f;

		// Token: 0x040058D1 RID: 22737
		public const float RecreationGainBest = 0.5f;

		// Token: 0x040058D2 RID: 22738
		public const float MeleeXPGainParticipantsGood = 2500f;

		// Token: 0x040058D3 RID: 22739
		public const float MeleeXPGainSpectatorsGood = 1000f;

		// Token: 0x040058D4 RID: 22740
		public const float MeleeXPGainParticipantsBest = 5000f;

		// Token: 0x040058D5 RID: 22741
		public const float MeleeXPGainSpectatorsBest = 2000f;
	}
}
