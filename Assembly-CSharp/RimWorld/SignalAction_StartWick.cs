using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B0C RID: 6924
	public class SignalAction_StartWick : SignalAction_Delay
	{
		// Token: 0x17001FE7 RID: 8167
		// (get) Token: 0x0600B0F8 RID: 45304 RVA: 0x003E578A File Offset: 0x003E398A
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null && this.thingWithWick.def == ThingDefOf.AncientFuelNode)
				{
					this.cachedAlert = new Alert_FuelNodeIgnition(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x17001FE8 RID: 8168
		// (get) Token: 0x0600B0F9 RID: 45305 RVA: 0x003E57B8 File Offset: 0x003E39B8
		public override bool ShouldRemoveNow
		{
			get
			{
				if (this.thingWithWick == null || this.thingWithWick.Destroyed)
				{
					return true;
				}
				CompExplosive compExplosive = this.thingWithWick.TryGetComp<CompExplosive>();
				return compExplosive == null || compExplosive.wickStarted;
			}
		}

		// Token: 0x0600B0FA RID: 45306 RVA: 0x003E57F8 File Offset: 0x003E39F8
		protected override void Complete()
		{
			base.Complete();
			if (this.ShouldRemoveNow)
			{
				return;
			}
			CompExplosive compExplosive = this.thingWithWick.TryGetComp<CompExplosive>();
			if (compExplosive != null)
			{
				compExplosive.StartWick(this.instigator);
			}
			if (this.delayTicks > 0)
			{
				Messages.Message("MessageFuelNodeDelayActivated".Translate(this.thingWithWick.LabelShort), this.thingWithWick, MessageTypeDefOf.NegativeEvent, true);
				return;
			}
			Messages.Message("MessageFuelNodeTriggered".Translate(this.thingWithWick.LabelShort), this.thingWithWick, MessageTypeDefOf.ThreatBig, true);
		}

		// Token: 0x0600B0FB RID: 45307 RVA: 0x003E58A3 File Offset: 0x003E3AA3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_References.Look<Thing>(ref this.thingWithWick, "thingWithWick", false);
		}

		// Token: 0x0400662E RID: 26158
		public Thing instigator;

		// Token: 0x0400662F RID: 26159
		public Thing thingWithWick;

		// Token: 0x04006630 RID: 26160
		private Alert_ActionDelay cachedAlert;
	}
}
