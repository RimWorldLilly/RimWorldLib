using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200172D RID: 5933
	public class RitualTargetFilter_SelectedThing : RitualTargetFilter
	{
		// Token: 0x060093F2 RID: 37874 RVA: 0x0035EEEA File Offset: 0x0035D0EA
		public RitualTargetFilter_SelectedThing()
		{
		}

		// Token: 0x060093F3 RID: 37875 RVA: 0x0035EEF2 File Offset: 0x0035D0F2
		public RitualTargetFilter_SelectedThing(RitualTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x060093F4 RID: 37876 RVA: 0x0035EEFB File Offset: 0x0035D0FB
		public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
		{
			rejectionReason = null;
			return true;
		}

		// Token: 0x060093F5 RID: 37877 RVA: 0x0035EF01 File Offset: 0x0035D101
		public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
		{
			return selectedTarget;
		}

		// Token: 0x060093F6 RID: 37878 RVA: 0x0035EF04 File Offset: 0x0035D104
		public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
		{
			return Enumerable.Empty<string>();
		}
	}
}
