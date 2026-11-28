using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001133 RID: 4403
	public class GameCondition_ForceWeather : GameCondition
	{
		// Token: 0x06006E96 RID: 28310 RVA: 0x0026CC44 File Offset: 0x0026AE44
		public override void Init()
		{
			base.Init();
			if (this.weather == null)
			{
				this.weather = this.def.weatherDef;
			}
		}

		// Token: 0x06006E97 RID: 28311 RVA: 0x0026CC65 File Offset: 0x0026AE65
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<WeatherDef>(ref this.weather, "weather");
		}

		// Token: 0x06006E98 RID: 28312 RVA: 0x0026CC7D File Offset: 0x0026AE7D
		public override WeatherDef ForcedWeather()
		{
			return this.weather;
		}

		// Token: 0x06006E99 RID: 28313 RVA: 0x0026CC88 File Offset: 0x0026AE88
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			this.weather = (from def in DefDatabase<WeatherDef>.AllDefsListForReading
			where def.isBad && def.canOccurAsRandomForcedEvent
			select def).RandomElement<WeatherDef>();
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("forcedWeather", this.weather));
		}

		// Token: 0x040046CA RID: 18122
		public WeatherDef weather;
	}
}
