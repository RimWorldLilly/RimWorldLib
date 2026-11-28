using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001887 RID: 6279
	public class CompFleckEmitter : ThingComp
	{
		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x06009D2A RID: 40234 RVA: 0x0038837E File Offset: 0x0038657E
		private CompProperties_FleckEmitter Props
		{
			get
			{
				return (CompProperties_FleckEmitter)this.props;
			}
		}

		// Token: 0x06009D2B RID: 40235 RVA: 0x0038838C File Offset: 0x0038658C
		public override void CompTick()
		{
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return;
			}
			CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
			if (comp2 != null && comp2.ticksLeft <= 0)
			{
				return;
			}
			CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
			if (comp3 != null && !comp3.Initiated)
			{
				return;
			}
			if (this.Props.emissionInterval == -1)
			{
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

		// Token: 0x06009D2C RID: 40236 RVA: 0x00388420 File Offset: 0x00386620
		protected void Emit()
		{
			FleckMaker.Static(this.parent.DrawPos + this.Props.offset, this.parent.MapHeld, this.Props.fleck, 1f);
			if (!this.Props.soundOnEmission.NullOrUndefined())
			{
				this.Props.soundOnEmission.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
		}

		// Token: 0x06009D2D RID: 40237 RVA: 0x0038849C File Offset: 0x0038669C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		// Token: 0x04005D28 RID: 23848
		public int ticksSinceLastEmitted;
	}
}
