using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001139 RID: 4409
	public class GameCondition_ClimateCycle : GameCondition
	{
		// Token: 0x06006EAF RID: 28335 RVA: 0x0026D09A File Offset: 0x0026B29A
		public override void Init()
		{
			this.ticksOffset = ((Rand.Value < 0.5f) ? 0 : 7200000);
		}

		// Token: 0x06006EB0 RID: 28336 RVA: 0x0026D0B6 File Offset: 0x0026B2B6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksOffset, "ticksOffset", 0, false);
		}

		// Token: 0x06006EB1 RID: 28337 RVA: 0x0026D0D0 File Offset: 0x0026B2D0
		public override float TemperatureOffset()
		{
			return Mathf.Sin((GenDate.YearsPassedFloat + (float)this.ticksOffset / 3600000f) / 4f * 3.1415927f * 2f) * 20f;
		}

		// Token: 0x040046CF RID: 18127
		private int ticksOffset;

		// Token: 0x040046D0 RID: 18128
		private const float PeriodYears = 4f;

		// Token: 0x040046D1 RID: 18129
		private const float MaxTempOffset = 20f;
	}
}
