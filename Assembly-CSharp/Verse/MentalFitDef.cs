using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000193 RID: 403
	public class MentalFitDef : Def
	{
		// Token: 0x06000B5B RID: 2907 RVA: 0x0003D6DF File Offset: 0x0003B8DF
		public override IEnumerable<string> ConfigErrors()
		{
			MentalFitDef.<ConfigErrors>d__3 <ConfigErrors>d__ = new MentalFitDef.<ConfigErrors>d__3(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0003D6F0 File Offset: 0x0003B8F0
		public float CalculateMTBDays(Pawn pawn)
		{
			if (!this.developmentalStageFilter.Has(pawn.DevelopmentalStage))
			{
				return float.PositiveInfinity;
			}
			if (pawn.needs.mood == null)
			{
				return float.PositiveInfinity;
			}
			SimpleCurve simpleCurve = this.mtbDaysMoodCurve;
			if (simpleCurve == null)
			{
				return float.PositiveInfinity;
			}
			return simpleCurve.Evaluate(pawn.needs.mood.CurLevelPercentage);
		}

		// Token: 0x040009B8 RID: 2488
		public MentalStateDef mentalState;

		// Token: 0x040009B9 RID: 2489
		public SimpleCurve mtbDaysMoodCurve;

		// Token: 0x040009BA RID: 2490
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;
	}
}
