using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB7 RID: 3511
	public class ThoughtWorker_Precept_HighLife : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06006025 RID: 24613 RVA: 0x0022E6E8 File Offset: 0x0022C8E8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return false;
			}
			float num = (float)(Find.TickManager.TicksGame - p.mindState.lastTakeRecreationalDrugTick) / 60000f;
			if (num > 1f && this.def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < this.def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			if (num < 1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (num < 2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num < 11f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}

		// Token: 0x06006026 RID: 24614 RVA: 0x0022E7A4 File Offset: 0x0022C9A4
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			return new ThoughtWorker_Precept_HighLife.<GetDescriptionArgs>d__6(-2);
		}

		// Token: 0x040035E1 RID: 13793
		private const float DaysSatisfied = 0.75f;

		// Token: 0x040035E2 RID: 13794
		private const float DaysNoBonus = 1f;

		// Token: 0x040035E3 RID: 13795
		private const float DaysMissing = 2f;

		// Token: 0x040035E4 RID: 13796
		private const float DaysMissing_Major = 11f;

		// Token: 0x040035E5 RID: 13797
		public static readonly SimpleCurve MoodOffsetFromDaysSinceLastDrugCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.75f, 3f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(2f, -1f),
				true
			},
			{
				new CurvePoint(11f, -10f),
				true
			}
		};
	}
}
