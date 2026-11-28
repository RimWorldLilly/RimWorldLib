using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D91 RID: 3473
	public abstract class ThoughtWorker_Precept : ThoughtWorker
	{
		// Token: 0x06005FC2 RID: 24514
		protected abstract ThoughtState ShouldHaveThought(Pawn p);

		// Token: 0x06005FC3 RID: 24515 RVA: 0x0022D3F4 File Offset: 0x0022B5F4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Ideo ideo = p.Ideo;
			if (ideo == null)
			{
				return ThoughtState.Inactive;
			}
			Precept firstPreceptAllowingSituationalThought = ideo.GetFirstPreceptAllowingSituationalThought(this.def);
			if (firstPreceptAllowingSituationalThought != null && !firstPreceptAllowingSituationalThought.def.enabledForNPCFactions && !p.CountsAsNonNPCForPrecepts())
			{
				return ThoughtState.Inactive;
			}
			if (!ideo.cachedPossibleSituationalThoughts.Contains(this.def))
			{
				return ThoughtState.Inactive;
			}
			return this.ShouldHaveThought(p);
		}
	}
}
