using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C0 RID: 6592
	public class CompThrownMoteEmitter : ThingComp
	{
		// Token: 0x17001CD3 RID: 7379
		// (get) Token: 0x0600A577 RID: 42359 RVA: 0x003B01FE File Offset: 0x003AE3FE
		private CompProperties_ThrownMoteEmitter Props
		{
			get
			{
				return (CompProperties_ThrownMoteEmitter)this.props;
			}
		}

		// Token: 0x17001CD4 RID: 7380
		// (get) Token: 0x0600A578 RID: 42360 RVA: 0x003B020C File Offset: 0x003AE40C
		private Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.Props.offsetMin.x, this.Props.offsetMax.x), Rand.Range(this.Props.offsetMin.y, this.Props.offsetMax.y), Rand.Range(this.Props.offsetMin.z, this.Props.offsetMax.z));
			}
		}

		// Token: 0x17001CD5 RID: 7381
		// (get) Token: 0x0600A579 RID: 42361 RVA: 0x003B028D File Offset: 0x003AE48D
		private Color EmissionColor
		{
			get
			{
				return Color.Lerp(this.Props.colorA, this.Props.colorB, Rand.Value);
			}
		}

		// Token: 0x17001CD6 RID: 7382
		// (get) Token: 0x0600A57A RID: 42362 RVA: 0x003B02B0 File Offset: 0x003AE4B0
		private bool IsOn
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
				CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
				if (comp2 != null && comp2.ticksLeft <= 0)
				{
					return false;
				}
				Building_MusicalInstrument building_MusicalInstrument = this.parent as Building_MusicalInstrument;
				if (building_MusicalInstrument != null && !building_MusicalInstrument.IsBeingPlayed)
				{
					return false;
				}
				CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
				if (comp3 != null && !comp3.Initiated)
				{
					return false;
				}
				Skyfaller skyfaller = this.parent as Skyfaller;
				return skyfaller == null || !skyfaller.FadingOut;
			}
		}

		// Token: 0x0600A57B RID: 42363 RVA: 0x003B0350 File Offset: 0x003AE550
		public override void CompTick()
		{
			if (!this.IsOn)
			{
				return;
			}
			if (this.Props.emissionInterval == -1)
			{
				if (!this.emittedBefore)
				{
					this.Emit();
					this.emittedBefore = true;
				}
				return;
			}
			if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
			{
				this.Emit();
				this.ticksSinceLastEmitted = 0;
				return;
			}
			this.ticksSinceLastEmitted++;
		}

		// Token: 0x0600A57C RID: 42364 RVA: 0x003B03BC File Offset: 0x003AE5BC
		private void Emit()
		{
			for (int i = 0; i < this.Props.burstCount; i++)
			{
				MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(this.Props.mote, null);
				moteThrown.Scale = this.Props.scale.RandomInRange;
				moteThrown.rotationRate = this.Props.rotationRate.RandomInRange;
				moteThrown.exactPosition = this.parent.DrawPos + this.EmissionOffset;
				moteThrown.instanceColor = this.EmissionColor;
				moteThrown.SetVelocity(this.Props.velocityX.RandomInRange, this.Props.velocityY.RandomInRange);
				if (moteThrown.exactPosition.ToIntVec3().InBounds(this.parent.Map))
				{
					GenSpawn.Spawn(moteThrown, moteThrown.exactPosition.ToIntVec3(), this.parent.Map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x0600A57D RID: 42365 RVA: 0x003B04B1 File Offset: 0x003AE6B1
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, "ticksSinceLastEmitted", 0, false);
			Scribe_Values.Look<bool>(ref this.emittedBefore, "emittedBefore", false, false);
		}

		// Token: 0x040061B2 RID: 25010
		public bool emittedBefore;

		// Token: 0x040061B3 RID: 25011
		public int ticksSinceLastEmitted;
	}
}
