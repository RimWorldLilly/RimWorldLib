using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016DE RID: 5854
	public class RitualOutcomeEffectWorker_GiveMemoryBelievers : RitualOutcomeEffectWorker
	{
		// Token: 0x06009279 RID: 37497 RVA: 0x00357AE0 File Offset: 0x00355CE0
		public RitualOutcomeEffectWorker_GiveMemoryBelievers()
		{
		}

		// Token: 0x0600927A RID: 37498 RVA: 0x00357AE8 File Offset: 0x00355CE8
		public RitualOutcomeEffectWorker_GiveMemoryBelievers(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600927B RID: 37499 RVA: 0x00357AF4 File Offset: 0x00355CF4
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				if (pawn.Ideo == jobRitual.Ritual.ideo)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(base.MakeMemory(pawn, jobRitual, null), null);
				}
			}
		}
	}
}
