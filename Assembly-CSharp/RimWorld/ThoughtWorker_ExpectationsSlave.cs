using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1F RID: 3615
	public class ThoughtWorker_ExpectationsSlave : ThoughtWorker
	{
		// Token: 0x0600612B RID: 24875 RVA: 0x00231FF4 File Offset: 0x002301F4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
			if (expectationDef == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(this.GetThoughtStageForExpectation(expectationDef));
		}

		// Token: 0x0600612C RID: 24876 RVA: 0x0023202B File Offset: 0x0023022B
		private int GetThoughtStageForExpectation(ExpectationDef expectation)
		{
			if (expectation == ExpectationDefOf.ExtremelyLow)
			{
				return 0;
			}
			if (expectation == ExpectationDefOf.VeryLow)
			{
				return 1;
			}
			if (expectation == ExpectationDefOf.Low)
			{
				return 2;
			}
			return 3;
		}
	}
}
