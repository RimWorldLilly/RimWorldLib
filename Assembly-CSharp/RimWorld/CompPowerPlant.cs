using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200135B RID: 4955
	public class CompPowerPlant : CompPowerTrader
	{
		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x06007ABF RID: 31423 RVA: 0x002BB4B4 File Offset: 0x002B96B4
		protected virtual float DesiredPowerOutput
		{
			get
			{
				return -base.Props.PowerConsumption;
			}
		}

		// Token: 0x06007AC0 RID: 31424 RVA: 0x002BB4C4 File Offset: 0x002B96C4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
			this.autoPoweredComp = this.parent.GetComp<CompAutoPowered>();
			this.toxifier = this.parent.GetComp<CompToxifier>();
			if (base.Props.PowerConsumption < 0f && !this.parent.IsBrokenDown() && FlickUtility.WantsToBeOn(this.parent))
			{
				base.PowerOn = true;
			}
		}

		// Token: 0x06007AC1 RID: 31425 RVA: 0x002BB54F File Offset: 0x002B974F
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.sustainerProducingPower != null && !this.sustainerProducingPower.Ended)
			{
				this.sustainerProducingPower.End();
			}
		}

		// Token: 0x06007AC2 RID: 31426 RVA: 0x002BB57C File Offset: 0x002B977C
		public override void CompTick()
		{
			base.CompTick();
			this.UpdateDesiredPowerOutput();
			if (base.Props.soundAmbientProducingPower != null)
			{
				if (base.PowerOutput > 0.01f)
				{
					if (this.sustainerProducingPower == null || this.sustainerProducingPower.Ended)
					{
						this.sustainerProducingPower = base.Props.soundAmbientProducingPower.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
					}
					this.sustainerProducingPower.Maintain();
					return;
				}
				if (this.sustainerProducingPower != null)
				{
					this.sustainerProducingPower.End();
					this.sustainerProducingPower = null;
				}
			}
		}

		// Token: 0x06007AC3 RID: 31427 RVA: 0x002BB614 File Offset: 0x002B9814
		public virtual void UpdateDesiredPowerOutput()
		{
			if ((this.breakdownableComp != null && this.breakdownableComp.BrokenDown) || (this.refuelableComp != null && !this.refuelableComp.HasFuel) || (this.flickableComp != null && !this.flickableComp.SwitchIsOn) || (this.autoPoweredComp != null && !this.autoPoweredComp.WantsToBeOn) || (this.toxifier != null && !this.toxifier.CanPolluteNow) || !base.PowerOn)
			{
				base.PowerOutput = 0f;
				return;
			}
			base.PowerOutput = this.DesiredPowerOutput;
		}

		// Token: 0x04004D7C RID: 19836
		protected CompRefuelable refuelableComp;

		// Token: 0x04004D7D RID: 19837
		protected CompBreakdownable breakdownableComp;

		// Token: 0x04004D7E RID: 19838
		protected CompAutoPowered autoPoweredComp;

		// Token: 0x04004D7F RID: 19839
		protected CompToxifier toxifier;

		// Token: 0x04004D80 RID: 19840
		private Sustainer sustainerProducingPower;
	}
}
