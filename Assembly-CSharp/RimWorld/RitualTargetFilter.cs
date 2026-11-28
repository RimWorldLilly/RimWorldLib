using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001727 RID: 5927
	public abstract class RitualTargetFilter : IExposable
	{
		// Token: 0x060093D1 RID: 37841 RVA: 0x0035E8BF File Offset: 0x0035CABF
		public RitualTargetFilter()
		{
		}

		// Token: 0x060093D2 RID: 37842 RVA: 0x0035E8C7 File Offset: 0x0035CAC7
		public RitualTargetFilter(RitualTargetFilterDef def)
		{
			this.def = def;
		}

		// Token: 0x060093D3 RID: 37843
		public abstract bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason);

		// Token: 0x060093D4 RID: 37844
		public abstract TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget);

		// Token: 0x060093D5 RID: 37845
		public abstract IEnumerable<string> GetTargetInfos(TargetInfo initiator);

		// Token: 0x060093D6 RID: 37846 RVA: 0x0035E8D6 File Offset: 0x0035CAD6
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualTargetFilterDef>(ref this.def, "def");
		}

		// Token: 0x04005959 RID: 22873
		public RitualTargetFilterDef def;
	}
}
