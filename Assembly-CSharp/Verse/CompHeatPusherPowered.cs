using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000542 RID: 1346
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002790 RID: 10128 RVA: 0x000F6AE0 File Offset: 0x000F4CE0
		public override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000F6B44 File Offset: 0x000F4D44
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}

		// Token: 0x04001C4F RID: 7247
		protected CompPowerTrader powerComp;

		// Token: 0x04001C50 RID: 7248
		protected CompRefuelable refuelableComp;

		// Token: 0x04001C51 RID: 7249
		protected CompBreakdownable breakdownableComp;
	}
}
