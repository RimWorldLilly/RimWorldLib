using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200113F RID: 4415
	public class GameCondition_Drought : GameCondition
	{
		// Token: 0x06006EE6 RID: 28390 RVA: 0x0026DC35 File Offset: 0x0026BE35
		public override void GameConditionTick()
		{
			this.HandleEndDueToRain();
		}

		// Token: 0x06006EE7 RID: 28391 RVA: 0x0026DC3E File Offset: 0x0026BE3E
		protected bool HandleEndDueToRain()
		{
			if (GenTicks.IsTickInterval(60) && base.SingleMap.weatherManager.curWeather.rainRate > 0f)
			{
				this.End();
				return true;
			}
			return false;
		}

		// Token: 0x040046EE RID: 18158
		public const float PlantGrowthRateFactor = 0.3f;
	}
}
