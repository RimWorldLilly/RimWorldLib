using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001882 RID: 6274
	public class CompFireBurst : ThingComp
	{
		// Token: 0x17001AE1 RID: 6881
		// (get) Token: 0x06009D16 RID: 40214 RVA: 0x00388004 File Offset: 0x00386204
		private CompProperties_FireBurst Props
		{
			get
			{
				return (CompProperties_FireBurst)this.props;
			}
		}

		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x06009D17 RID: 40215 RVA: 0x00388011 File Offset: 0x00386211
		private CompExplosive CompExplosive
		{
			get
			{
				if (this.compExplosive == null)
				{
					this.compExplosive = this.parent.TryGetComp<CompExplosive>();
				}
				return this.compExplosive;
			}
		}

		// Token: 0x06009D18 RID: 40216 RVA: 0x00388034 File Offset: 0x00386234
		public override void CompTick()
		{
			if (this.CompExplosive.wickStarted)
			{
				FireBurstUtility.ThrowFuelTick(this.parent.Position, this.Props.radius, this.parent.Map);
				if (this.CompExplosive.wickTicksLeft <= this.Props.ticksAwayFromDetonate && this.effecter == null)
				{
					this.effecter = EffecterDefOf.Fire_Burst.Spawn(this.parent.Position, this.parent.Map, 1f);
				}
			}
			Effecter effecter = this.effecter;
			if (effecter == null)
			{
				return;
			}
			effecter.EffectTick(this.parent, this.parent);
		}

		// Token: 0x04005D1D RID: 23837
		private CompExplosive compExplosive;

		// Token: 0x04005D1E RID: 23838
		private Effecter effecter;
	}
}
