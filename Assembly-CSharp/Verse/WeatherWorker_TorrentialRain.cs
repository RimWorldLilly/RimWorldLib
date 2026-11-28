using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200032A RID: 810
	public class WeatherWorker_TorrentialRain : WeatherWorker
	{
		// Token: 0x060018D0 RID: 6352 RVA: 0x00094E01 File Offset: 0x00093001
		public WeatherWorker_TorrentialRain(WeatherDef def) : base(def)
		{
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x00094E0A File Offset: 0x0009300A
		public override void OnWeatherStart(Map map)
		{
			base.OnWeatherStart(map);
			GenSpawn.Spawn(ThingDefOf.TorrentialRainFlood, map.Center, map, WipeMode.Vanish);
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x00094E26 File Offset: 0x00093026
		public override void OnWeatherEnd(Map map)
		{
			base.OnWeatherEnd(map);
			Thing thing = map.listerThings.ThingsOfDef(ThingDefOf.TorrentialRainFlood).FirstOrDefault<Thing>();
			if (thing == null)
			{
				return;
			}
			thing.Destroy(DestroyMode.Vanish);
		}
	}
}
