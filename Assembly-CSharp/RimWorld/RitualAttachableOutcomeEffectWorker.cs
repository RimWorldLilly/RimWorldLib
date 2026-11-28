using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001657 RID: 5719
	public abstract class RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06008FE5 RID: 36837
		public abstract void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets);

		// Token: 0x06008FE6 RID: 36838 RVA: 0x0034E38C File Offset: 0x0034C58C
		public virtual AcceptanceReport CanApplyNow(Precept_Ritual ritual, Map map)
		{
			return true;
		}

		// Token: 0x040057E5 RID: 22501
		public RitualAttachableOutcomeEffectDef def;
	}
}
