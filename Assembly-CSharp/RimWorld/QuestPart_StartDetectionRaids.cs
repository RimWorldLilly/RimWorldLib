using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200108D RID: 4237
	public class QuestPart_StartDetectionRaids : QuestPart
	{
		// Token: 0x06006A2C RID: 27180 RVA: 0x00255D94 File Offset: 0x00253F94
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				TimedDetectionRaids component = this.worldObject.GetComponent<TimedDetectionRaids>();
				int ticks = 240000;
				if (this.delayRangeHours != null)
				{
					ticks = (int)(this.delayRangeHours.Value.RandomInRange * 2500f);
					component.delayRangeHours = this.delayRangeHours.Value;
				}
				if (this.firstRaidDelayTicks != null)
				{
					ticks = this.firstRaidDelayTicks.Value;
				}
				component.StartDetectionCountdown(ticks, -1);
				component.SetNotifiedSilently();
			}
		}

		// Token: 0x06006A2D RID: 27181 RVA: 0x00255E30 File Offset: 0x00254030
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
			Scribe_Values.Look<FloatRange?>(ref this.delayRangeHours, "delayRangeHours", null, false);
			Scribe_Values.Look<int?>(ref this.firstRaidDelayTicks, "firstRaidDelayTicks", null, false);
		}

		// Token: 0x04004375 RID: 17269
		public string inSignal;

		// Token: 0x04004376 RID: 17270
		public FloatRange? delayRangeHours;

		// Token: 0x04004377 RID: 17271
		public WorldObject worldObject;

		// Token: 0x04004378 RID: 17272
		public int? firstRaidDelayTicks;
	}
}
