using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEB RID: 3563
	public class ThoughtWorker_NeedRest : ThoughtWorker
	{
		// Token: 0x060060BC RID: 24764 RVA: 0x00230458 File Offset: 0x0022E658
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.rest == null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.rest.CurCategory)
			{
			case RestCategory.Rested:
				return ThoughtState.Inactive;
			case RestCategory.Tired:
				return ThoughtState.ActiveAtStage(0);
			case RestCategory.VeryTired:
				return ThoughtState.ActiveAtStage(1);
			case RestCategory.Exhausted:
				return ThoughtState.ActiveAtStage(2);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
