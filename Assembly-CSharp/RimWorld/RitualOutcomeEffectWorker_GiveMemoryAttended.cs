using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016DC RID: 5852
	public class RitualOutcomeEffectWorker_GiveMemoryAttended : RitualOutcomeEffectWorker
	{
		// Token: 0x06009271 RID: 37489 RVA: 0x0035787E File Offset: 0x00355A7E
		public RitualOutcomeEffectWorker_GiveMemoryAttended()
		{
		}

		// Token: 0x06009272 RID: 37490 RVA: 0x00357886 File Offset: 0x00355A86
		public RitualOutcomeEffectWorker_GiveMemoryAttended(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06009273 RID: 37491 RVA: 0x00357890 File Offset: 0x00355A90
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			if (progress < 1f)
			{
				return;
			}
			foreach (KeyValuePair<Pawn, int> keyValuePair in totalPresence)
			{
				keyValuePair.Key.needs.mood.thoughts.memories.TryGainMemory(base.MakeMemory(keyValuePair.Key, jobRitual, null), null);
			}
		}
	}
}
