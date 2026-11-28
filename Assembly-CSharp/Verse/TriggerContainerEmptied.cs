using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000553 RID: 1363
	public class TriggerContainerEmptied : Thing
	{
		// Token: 0x06002883 RID: 10371 RVA: 0x000FA294 File Offset: 0x000F8494
		protected override void Tick()
		{
			if (!base.Spawned || !this.IsHashIntervalTick(60))
			{
				return;
			}
			Thing thing = this.container;
			CompThingContainer compThingContainer = (thing != null) ? thing.TryGetComp<CompThingContainer>() : null;
			if (compThingContainer == null)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!compThingContainer.innerContainer.Any)
			{
				Find.SignalManager.SendSignal(new Signal(this.signalTag, false));
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000FA304 File Offset: 0x000F8504
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_References.Look<Thing>(ref this.container, "container", false);
		}

		// Token: 0x04001C9A RID: 7322
		public string signalTag;

		// Token: 0x04001C9B RID: 7323
		public Thing container;
	}
}
