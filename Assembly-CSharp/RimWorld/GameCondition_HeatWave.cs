using System;

namespace RimWorld
{
	// Token: 0x02001137 RID: 4407
	public class GameCondition_HeatWave : GameCondition
	{
		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x06006EA9 RID: 28329 RVA: 0x0026D054 File Offset: 0x0026B254
		public override int TransitionTicks
		{
			get
			{
				return 12000;
			}
		}

		// Token: 0x06006EAA RID: 28330 RVA: 0x0026D05B File Offset: 0x0026B25B
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 17f);
		}

		// Token: 0x040046CD RID: 18125
		private const float MaxTempOffset = 17f;
	}
}
