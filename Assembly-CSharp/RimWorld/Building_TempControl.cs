using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A91 RID: 6801
	public class Building_TempControl : Building
	{
		// Token: 0x0600AB9F RID: 43935 RVA: 0x003C9CB7 File Offset: 0x003C7EB7
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compTempControl = base.GetComp<CompTempControl>();
			this.compPowerTrader = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x040063EB RID: 25579
		public CompTempControl compTempControl;

		// Token: 0x040063EC RID: 25580
		public CompPowerTrader compPowerTrader;
	}
}
