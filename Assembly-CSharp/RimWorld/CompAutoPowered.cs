using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A20 RID: 6688
	public abstract class CompAutoPowered : ThingComp
	{
		// Token: 0x17001D71 RID: 7537
		// (get) Token: 0x0600A7FF RID: 43007 RVA: 0x003BBFC8 File Offset: 0x003BA1C8
		public bool AppearsPowered
		{
			get
			{
				return this.compPowerCached != null && this.compPowerCached.PowerNet != null && this.compPowerCached.PowerNet.HasActivePowerSource && this.compPowerCached.PowerNet.CanPowerNow(this.compPowerCached) && !this.parent.MapHeld.gameConditionManager.ElectricityDisabled(this.parent.MapHeld);
			}
		}

		// Token: 0x17001D72 RID: 7538
		// (get) Token: 0x0600A800 RID: 43008 RVA: 0x003BC03C File Offset: 0x003BA23C
		public CompFlickable Flickable
		{
			get
			{
				CompFlickable result;
				if ((result = this.compFlickableCached) == null)
				{
					result = (this.compFlickableCached = this.parent.TryGetComp<CompFlickable>());
				}
				return result;
			}
		}

		// Token: 0x17001D73 RID: 7539
		// (get) Token: 0x0600A801 RID: 43009
		public abstract bool WantsToBeOn { get; }

		// Token: 0x0600A802 RID: 43010 RVA: 0x003BC068 File Offset: 0x003BA268
		protected void UpdateOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayNeedsPower);
			if (!this.parent.IsBrokenDown() && this.Flickable.SwitchIsOn && !this.AppearsPowered && this.overlayNeedsPower == null)
			{
				this.overlayNeedsPower = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.NeedsPower));
			}
		}

		// Token: 0x040062C2 RID: 25282
		public const string AutoPoweredWantsOffSignal = "AutoPoweredWantsOff";

		// Token: 0x040062C3 RID: 25283
		protected CompPowerTrader compPowerCached;

		// Token: 0x040062C4 RID: 25284
		protected CompFlickable compFlickableCached;

		// Token: 0x040062C5 RID: 25285
		private OverlayHandle? overlayNeedsPower;
	}
}
