using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001156 RID: 4438
	public class GameCondition_Windy : GameCondition
	{
		// Token: 0x06006F7D RID: 28541 RVA: 0x0026FCE8 File Offset: 0x0026DEE8
		public override float MinWindSpeed()
		{
			return 1.5f;
		}

		// Token: 0x06006F7E RID: 28542 RVA: 0x0026FCEF File Offset: 0x0026DEEF
		public override float WeatherCommonalityFactor(WeatherDef weather, Map map)
		{
			if (weather == WeatherDefOf.Fog || weather == WeatherDefOf.FoggyRain)
			{
				return 0f;
			}
			return 1f;
		}
	}
}
