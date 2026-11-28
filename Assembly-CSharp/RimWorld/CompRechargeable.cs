using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200194E RID: 6478
	public class CompRechargeable : ThingComp
	{
		// Token: 0x17001C1C RID: 7196
		// (get) Token: 0x0600A279 RID: 41593 RVA: 0x003A0F76 File Offset: 0x0039F176
		private CompProperties_Rechargeable Props
		{
			get
			{
				return (CompProperties_Rechargeable)this.props;
			}
		}

		// Token: 0x17001C1D RID: 7197
		// (get) Token: 0x0600A27A RID: 41594 RVA: 0x003A0F83 File Offset: 0x0039F183
		public bool Charged
		{
			get
			{
				return this.ticksUntilCharged == 0;
			}
		}

		// Token: 0x0600A27B RID: 41595 RVA: 0x003A0F8E File Offset: 0x0039F18E
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.ticksUntilCharged = this.Props.ticksToRecharge;
		}

		// Token: 0x0600A27C RID: 41596 RVA: 0x003A0FA8 File Offset: 0x0039F1A8
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.WillReplace)
		{
			if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
		}

		// Token: 0x0600A27D RID: 41597 RVA: 0x003A0FC4 File Offset: 0x0039F1C4
		public override void CompTick()
		{
			base.CompTick();
			if (this.compPowerCached == null)
			{
				this.compPowerCached = this.parent.TryGetComp<CompPowerTrader>();
			}
			if (this.ticksUntilCharged > 0 && this.compPowerCached.PowerOn)
			{
				if (this.ticksUntilCharged == 1 && this.Props.chargedSoundDef != null)
				{
					this.Props.chargedSoundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				this.ticksUntilCharged--;
			}
			if (!this.Charged && this.parent.Spawned)
			{
				if (this.progressBar == null)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
				}
				this.progressBar.EffectTick(this.parent, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
				if (mote != null)
				{
					mote.progress = 1f - Mathf.Clamp01((float)this.ticksUntilCharged / (float)this.Props.ticksToRecharge);
					mote.offsetZ = -0.5f;
					return;
				}
			}
			else if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
		}

		// Token: 0x0600A27E RID: 41598 RVA: 0x003A1114 File Offset: 0x0039F314
		public void Discharge()
		{
			this.ticksUntilCharged = this.Props.ticksToRecharge;
			if (this.Props.dischargeSoundDef != null)
			{
				this.Props.dischargeSoundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x0600A27F RID: 41599 RVA: 0x003A1170 File Offset: 0x0039F370
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompRechargeable.<CompGetGizmosExtra>d__11 <CompGetGizmosExtra>d__ = new CompRechargeable.<CompGetGizmosExtra>d__11(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A280 RID: 41600 RVA: 0x003A1180 File Offset: 0x0039F380
		public override string CompInspectStringExtra()
		{
			if (this.Charged)
			{
				return "RechargeableReady".Translate() + ".";
			}
			return "RechargeableCharging".Translate() + ": " + "DurationLeft".Translate(this.ticksUntilCharged.ToStringTicksToPeriod(true, true, true, true, false)) + ".";
		}

		// Token: 0x0600A281 RID: 41601 RVA: 0x003A11F6 File Offset: 0x0039F3F6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksUntilCharged, "ticksUntilCharged", 0, false);
		}

		// Token: 0x04005FFF RID: 24575
		private int ticksUntilCharged;

		// Token: 0x04006000 RID: 24576
		private Effecter progressBar;

		// Token: 0x04006001 RID: 24577
		private CompPowerTrader compPowerCached;
	}
}
