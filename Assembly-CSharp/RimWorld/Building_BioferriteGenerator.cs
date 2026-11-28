using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A9F RID: 6815
	public class Building_BioferriteGenerator : Building
	{
		// Token: 0x17001E9F RID: 7839
		// (get) Token: 0x0600AC0A RID: 44042 RVA: 0x003CC020 File Offset: 0x003CA220
		private CompHeatPusherPowered HeatPusher
		{
			get
			{
				CompHeatPusherPowered result;
				if ((result = this.heatPusher) == null)
				{
					result = (this.heatPusher = base.GetComp<CompHeatPusherPowered>());
				}
				return result;
			}
		}

		// Token: 0x0600AC0B RID: 44043 RVA: 0x003CC046 File Offset: 0x003CA246
		public override bool IsWorking()
		{
			return this.HeatPusher.ShouldPushHeatNow;
		}

		// Token: 0x04006411 RID: 25617
		[Unsaved(false)]
		private CompHeatPusherPowered heatPusher;
	}
}
