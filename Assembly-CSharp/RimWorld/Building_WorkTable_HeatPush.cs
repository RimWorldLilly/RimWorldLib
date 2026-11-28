using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ADE RID: 6878
	public class Building_WorkTable_HeatPush : Building_WorkTable
	{
		// Token: 0x0600AF98 RID: 44952 RVA: 0x003DCF71 File Offset: 0x003DB171
		public override void UsedThisTick()
		{
			base.UsedThisTick();
			if (this.IsHashIntervalTick(30))
			{
				GenTemperature.PushHeat(this, this.def.building.heatPerTickWhileWorking * 30f);
			}
		}

		// Token: 0x0400655F RID: 25951
		private const int HeatPushInterval = 30;
	}
}
