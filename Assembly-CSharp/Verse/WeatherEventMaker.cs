using System;

namespace Verse
{
	// Token: 0x02000327 RID: 807
	public class WeatherEventMaker
	{
		// Token: 0x060018BE RID: 6334 RVA: 0x00094650 File Offset: 0x00092850
		public void WeatherEventMakerTick(Map map, float strength)
		{
			if (Rand.Value < 1f / this.averageInterval * strength)
			{
				WeatherEvent newEvent = (WeatherEvent)Activator.CreateInstance(this.eventClass, new object[]
				{
					map
				});
				map.weatherManager.eventHandler.AddEvent(newEvent);
			}
		}

		// Token: 0x04001488 RID: 5256
		public float averageInterval = 100f;

		// Token: 0x04001489 RID: 5257
		public Type eventClass;
	}
}
