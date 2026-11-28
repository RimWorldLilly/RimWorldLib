using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ECC RID: 3788
	public class CompEffecter : ThingComp
	{
		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x0600637E RID: 25470 RVA: 0x0023B995 File Offset: 0x00239B95
		public CompProperties_EffecterBase Props
		{
			get
			{
				return (CompProperties_EffecterBase)this.props;
			}
		}

		// Token: 0x0600637F RID: 25471 RVA: 0x0023B9A2 File Offset: 0x00239BA2
		protected virtual bool ShouldShowEffecter()
		{
			return this.parent.Spawned && this.parent.MapHeld == Find.CurrentMap;
		}

		// Token: 0x06006380 RID: 25472 RVA: 0x0023B9C8 File Offset: 0x00239BC8
		public override void CompTick()
		{
			base.CompTick();
			if (!this.ShouldShowEffecter())
			{
				Effecter effecter = this.effecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effecter = null;
				return;
			}
			if (this.effecter == null)
			{
				this.effecter = this.Props.effecterDef.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
			}
			Effecter effecter2 = this.effecter;
			if (effecter2 == null)
			{
				return;
			}
			effecter2.EffectTick(this.parent, this.parent);
		}

		// Token: 0x040037A6 RID: 14246
		private Effecter effecter;
	}
}
