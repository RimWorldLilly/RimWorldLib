using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000326 RID: 806
	public class WeatherEventHandler
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x00094593 File Offset: 0x00092793
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x0009459B File Offset: 0x0009279B
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x000945B0 File Offset: 0x000927B0
		public void WeatherEventHandlerTick()
		{
			for (int i = this.liveEvents.Count - 1; i >= 0; i--)
			{
				this.liveEvents[i].WeatherEventTick();
				if (this.liveEvents[i].Expired)
				{
					this.liveEvents.RemoveAt(i);
				}
			}
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00094608 File Offset: 0x00092808
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}

		// Token: 0x04001487 RID: 5255
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();
	}
}
