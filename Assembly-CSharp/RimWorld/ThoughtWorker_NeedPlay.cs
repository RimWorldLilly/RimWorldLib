using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF3 RID: 3571
	public class ThoughtWorker_NeedPlay : ThoughtWorker
	{
		// Token: 0x060060CC RID: 24780 RVA: 0x00230908 File Offset: 0x0022EB08
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.play == null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.play.CurCategory)
			{
			case PlayCategory.Empty:
				return ThoughtState.ActiveAtStage(0);
			case PlayCategory.VeryLow:
				return ThoughtState.ActiveAtStage(1);
			case PlayCategory.Low:
				return ThoughtState.ActiveAtStage(2);
			case PlayCategory.Satisfied:
				return ThoughtState.Inactive;
			case PlayCategory.High:
				return ThoughtState.ActiveAtStage(3);
			case PlayCategory.Extreme:
				return ThoughtState.ActiveAtStage(4);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
