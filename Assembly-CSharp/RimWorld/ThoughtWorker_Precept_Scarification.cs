using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D97 RID: 3479
	public class ThoughtWorker_Precept_Scarification : ThoughtWorker_Precept
	{
		// Token: 0x06005FD3 RID: 24531 RVA: 0x0022D668 File Offset: 0x0022B868
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			int hediffCount = p.health.hediffSet.GetHediffCount(HediffDefOf.Scarification);
			int requiredScars = p.ideo.Ideo.RequiredScars;
			if (hediffCount >= requiredScars)
			{
				return this.ProcessedState(0, p);
			}
			if (hediffCount == 0)
			{
				return this.ProcessedState(1, p);
			}
			if (hediffCount < requiredScars)
			{
				return this.ProcessedState(2, p);
			}
			return false;
		}

		// Token: 0x06005FD4 RID: 24532 RVA: 0x0022D6D8 File Offset: 0x0022B8D8
		private ThoughtState ProcessedState(int index, Pawn p)
		{
			if (this.def.stages[index].baseMoodEffect < 0f && this.def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < this.def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(index);
		}
	}
}
