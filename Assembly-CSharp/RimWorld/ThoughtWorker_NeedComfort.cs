using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DED RID: 3565
	public class ThoughtWorker_NeedComfort : ThoughtWorker
	{
		// Token: 0x060060C0 RID: 24768 RVA: 0x00230554 File Offset: 0x0022E754
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.comfort == null)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.comfort.CurCategory)
			{
			case ComfortCategory.Uncomfortable:
				return ThoughtState.ActiveAtStage(0);
			case ComfortCategory.Normal:
				return ThoughtState.Inactive;
			case ComfortCategory.Comfortable:
				return ThoughtState.ActiveAtStage(1);
			case ComfortCategory.VeryComfortable:
				return ThoughtState.ActiveAtStage(2);
			case ComfortCategory.ExtremelyComfortable:
				return ThoughtState.ActiveAtStage(3);
			case ComfortCategory.LuxuriantlyComfortable:
				return ThoughtState.ActiveAtStage(4);
			default:
				throw new NotImplementedException();
			}
		}
	}
}
