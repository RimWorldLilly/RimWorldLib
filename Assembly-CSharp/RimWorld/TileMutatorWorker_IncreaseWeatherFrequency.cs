using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132E RID: 4910
	public class TileMutatorWorker_IncreaseWeatherFrequency : TileMutatorWorker
	{
		// Token: 0x060079B2 RID: 31154 RVA: 0x002B5110 File Offset: 0x002B3310
		public TileMutatorWorker_IncreaseWeatherFrequency(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079B3 RID: 31155 RVA: 0x002B5119 File Offset: 0x002B3319
		public override void MutateWeatherCommonalityFor(WeatherDef weather, PlanetTile tile, ref float commonality)
		{
			if (this.def.weathersToAffect.Contains(weather))
			{
				commonality += this.def.weatherFrequencyOffset;
				commonality *= this.def.weatherFrequencyFactor;
			}
		}
	}
}
