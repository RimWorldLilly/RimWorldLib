using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019BE RID: 6590
	public class CompThrownFleckEmitter : ThingComp
	{
		// Token: 0x17001CCF RID: 7375
		// (get) Token: 0x0600A56D RID: 42349 RVA: 0x003AFE99 File Offset: 0x003AE099
		private CompProperties_ThrownFleckEmitter Props
		{
			get
			{
				return (CompProperties_ThrownFleckEmitter)this.props;
			}
		}

		// Token: 0x17001CD0 RID: 7376
		// (get) Token: 0x0600A56E RID: 42350 RVA: 0x003AFEA8 File Offset: 0x003AE0A8
		private Vector3 EmissionOffset
		{
			get
			{
				return new Vector3(Rand.Range(this.Props.offsetMin.x, this.Props.offsetMax.x), Rand.Range(this.Props.offsetMin.y, this.Props.offsetMax.y), Rand.Range(this.Props.offsetMin.z, this.Props.offsetMax.z));
			}
		}

		// Token: 0x17001CD1 RID: 7377
		// (get) Token: 0x0600A56F RID: 42351 RVA: 0x003AFF29 File Offset: 0x003AE129
		private Color EmissionColor
		{
			get
			{
				return Color.Lerp(this.Props.colorA, this.Props.colorB, Rand.Value);
			}
		}

		// Token: 0x17001CD2 RID: 7378
		// (get) Token: 0x0600A570 RID: 42352 RVA: 0x003AFF4C File Offset: 0x003AE14C
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
				CompLoudspeaker comp4 = this.parent.GetComp<CompLoudspeaker>();
				if (comp4 != null && !comp4.Active)
				{
					return false;
				}
				CompHackable comp5 = this.parent.GetComp<CompHackable>();
				if (comp5 != null && comp5.IsHacked)
				{
					return false;
				}
				Building_Crate building_Crate = this.parent as Building_Crate;
				return building_Crate == null || building_Crate.HasAnyContents;
			}
		}

		// Token: 0x0600A571 RID: 42353 RVA: 0x003B0024 File Offset: 0x003AE224
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

		// Token: 0x0600A572 RID: 42354 RVA: 0x003B0090 File Offset: 0x003AE290
		private void Emit()
		{
			for (int i = 0; i < this.Props.burstCount; i++)
			{
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(this.parent.DrawPos + this.EmissionOffset, this.parent.Map, this.Props.fleck, this.Props.scale.RandomInRange);
				dataStatic.rotationRate = this.Props.rotationRate.RandomInRange;
				dataStatic.instanceColor = new Color?(this.EmissionColor);
				dataStatic.velocityAngle = this.Props.velocityX.RandomInRange;
				dataStatic.velocitySpeed = this.Props.velocityY.RandomInRange;
				this.parent.Map.flecks.CreateFleck(dataStatic);
			}
		}

		// Token: 0x0600A573 RID: 42355 RVA: 0x003B0168 File Offset: 0x003AE368
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, "ticksSinceLastEmitted", 0, false);
			Scribe_Values.Look<bool>(ref this.emittedBefore, "emittedBefore", false, false);
		}

		// Token: 0x040061A5 RID: 24997
		public bool emittedBefore;

		// Token: 0x040061A6 RID: 24998
		public int ticksSinceLastEmitted;
	}
}
