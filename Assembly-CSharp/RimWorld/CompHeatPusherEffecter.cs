using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002096 RID: 8342
	public class CompHeatPusherEffecter : ThingComp
	{
		// Token: 0x170025C2 RID: 9666
		// (get) Token: 0x0600D248 RID: 53832 RVA: 0x004D3ECD File Offset: 0x004D20CD
		public CompProperties_HeatPusherEffecter Props
		{
			get
			{
				return (CompProperties_HeatPusherEffecter)this.props;
			}
		}

		// Token: 0x170025C3 RID: 9667
		// (get) Token: 0x0600D249 RID: 53833 RVA: 0x004D3EDA File Offset: 0x004D20DA
		public bool ShouldEmit
		{
			get
			{
				return this.pusherComp.ShouldPushHeatNow;
			}
		}

		// Token: 0x0600D24A RID: 53834 RVA: 0x004D3EE7 File Offset: 0x004D20E7
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.pusherComp = this.parent.GetComp<CompHeatPusher>();
		}

		// Token: 0x0600D24B RID: 53835 RVA: 0x004D3F04 File Offset: 0x004D2104
		public override void CompTick()
		{
			if (this.ShouldEmit && this.effecter == null)
			{
				this.effecter = this.Props.effecterDef.SpawnAttached(this.parent, this.parent.Map, 1f);
			}
			else if (!this.ShouldEmit && this.effecter != null)
			{
				this.effecter.Cleanup();
				this.effecter = null;
			}
			Effecter effecter = this.effecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this.parent, this.parent);
		}

		// Token: 0x04008536 RID: 34102
		private Effecter effecter;

		// Token: 0x04008537 RID: 34103
		private CompHeatPusher pusherComp;
	}
}
