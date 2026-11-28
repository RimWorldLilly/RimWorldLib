using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A05 RID: 2565
	public class JobDriver_WatchTelevision : JobDriver_WatchBuilding
	{
		// Token: 0x06004D91 RID: 19857 RVA: 0x001DAE80 File Offset: 0x001D9080
		protected override void WatchTickAction(int delta)
		{
			if (!((Building)base.TargetA.Thing).TryGetComp<CompPowerTrader>().PowerOn)
			{
				base.EndJobWith(JobCondition.Incompletable);
				return;
			}
			base.WatchTickAction(delta);
		}
	}
}
