using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001364 RID: 4964
	public class CompPowerTrader : CompPower, IThingGlower
	{
		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x06007AFE RID: 31486 RVA: 0x002BC9A5 File Offset: 0x002BABA5
		// (set) Token: 0x06007AFF RID: 31487 RVA: 0x002BC9BB File Offset: 0x002BABBB
		public float PowerOutput
		{
			get
			{
				if (!this.StunnedByEMP)
				{
					return this.powerOutputInt;
				}
				return 0f;
			}
			set
			{
				this.powerOutputInt = value;
				if (this.powerOutputInt > 0f)
				{
					this.powerLastOutputted = true;
				}
				if (this.powerOutputInt < 0f)
				{
					this.powerLastOutputted = false;
				}
			}
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x06007B00 RID: 31488 RVA: 0x002BC9EC File Offset: 0x002BABEC
		public bool Off
		{
			get
			{
				return !this.parent.Spawned || !this.PowerOn || !FlickUtility.WantsToBeOn(this.parent);
			}
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x06007B01 RID: 31489 RVA: 0x002BCA13 File Offset: 0x002BAC13
		public float EnergyOutputPerTick
		{
			get
			{
				return this.PowerOutput * CompPower.WattsToWattDaysPerTick;
			}
		}

		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x06007B02 RID: 31490 RVA: 0x002BCA21 File Offset: 0x002BAC21
		// (set) Token: 0x06007B03 RID: 31491 RVA: 0x002BCA2C File Offset: 0x002BAC2C
		public bool PowerOn
		{
			get
			{
				return this.powerOnInt;
			}
			set
			{
				if (this.powerOnInt == value)
				{
					return;
				}
				this.powerOnInt = value;
				if (this.powerOnInt)
				{
					if (!FlickUtility.WantsToBeOn(this.parent))
					{
						string str = "Tried to power on ";
						ThingWithComps parent = this.parent;
						Log.Warning(str + ((parent != null) ? parent.ToString() : null) + " which did not desire it.");
						return;
					}
					if (this.parent.IsBrokenDown())
					{
						string str2 = "Tried to power on ";
						ThingWithComps parent2 = this.parent;
						Log.Warning(str2 + ((parent2 != null) ? parent2.ToString() : null) + " which is broken down.");
						return;
					}
					if (this.powerStartedAction != null)
					{
						this.powerStartedAction();
					}
					this.parent.BroadcastCompSignal("PowerTurnedOn");
					SoundDef soundDef = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOn;
					if (soundDef.NullOrUndefined())
					{
						soundDef = SoundDefOf.Power_OnSmall;
					}
					if (this.parent.Spawned)
					{
						soundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					}
					this.StartSustainerPoweredIfInactive();
				}
				else
				{
					if (this.powerStoppedAction != null)
					{
						this.powerStoppedAction();
					}
					this.parent.BroadcastCompSignal("PowerTurnedOff");
					SoundDef soundDef2 = ((CompProperties_Power)this.parent.def.CompDefForAssignableFrom<CompPowerTrader>()).soundPowerOff;
					if (soundDef2.NullOrUndefined())
					{
						soundDef2 = SoundDefOf.Power_OffSmall;
					}
					if (this.parent.Spawned)
					{
						soundDef2.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					}
					this.EndSustainerPoweredIfActive();
				}
				this.UpdateOverlays();
			}
		}

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x06007B04 RID: 31492 RVA: 0x002BCBD4 File Offset: 0x002BADD4
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.parent.LabelCap + " CompPower:");
				stringBuilder.AppendLine("   PowerOn: " + this.PowerOn.ToString());
				stringBuilder.AppendLine("   energyProduction: " + this.PowerOutput.ToString());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x06007B05 RID: 31493 RVA: 0x002BCC45 File Offset: 0x002BAE45
		private bool StunnedByEMP
		{
			get
			{
				return this.stunnableComp != null && this.stunnableComp.StunHandler.Stunned && this.stunnableComp.StunHandler.StunFromEMP;
			}
		}

		// Token: 0x06007B06 RID: 31494 RVA: 0x002BCC75 File Offset: 0x002BAE75
		public bool ShouldBeLitNow()
		{
			return this.PowerOn;
		}

		// Token: 0x06007B07 RID: 31495 RVA: 0x002BCC80 File Offset: 0x002BAE80
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "ScheduledOff" || signal == "Breakdown" || signal == "AutoPoweredWantsOff")
			{
				this.PowerOn = false;
			}
			if (signal == "RanOutOfFuel" && this.powerLastOutputted)
			{
				this.PowerOn = false;
			}
			this.UpdateOverlays();
		}

		// Token: 0x06007B08 RID: 31496 RVA: 0x002BCCEC File Offset: 0x002BAEEC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.flickableComp = this.parent.GetComp<CompFlickable>();
			this.stunnableComp = this.parent.GetComp<CompStunnable>();
			if (this.PowerOn)
			{
				LongEventHandler.ExecuteWhenFinished(new Action(this.StartSustainerPoweredIfInactive));
			}
			this.UpdateOverlays();
		}

		// Token: 0x06007B09 RID: 31497 RVA: 0x002BCD41 File Offset: 0x002BAF41
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.EndSustainerPoweredIfActive();
			if (mode != DestroyMode.WillReplace)
			{
				this.powerOutputInt = 0f;
			}
		}

		// Token: 0x06007B0A RID: 31498 RVA: 0x002BCD60 File Offset: 0x002BAF60
		public override void PostSwapMap()
		{
			if (base.PowerNet == null)
			{
				this.PowerOn = false;
			}
		}

		// Token: 0x06007B0B RID: 31499 RVA: 0x002BCD71 File Offset: 0x002BAF71
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.powerOnInt, "powerOn", true, false);
		}

		// Token: 0x06007B0C RID: 31500 RVA: 0x002BCD8C File Offset: 0x002BAF8C
		private void UpdateOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayPowerOff);
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayNeedsPower);
			if (!this.parent.IsBrokenDown())
			{
				if (this.flickableComp != null && !this.flickableComp.SwitchIsOn && this.overlayPowerOff == null)
				{
					this.overlayPowerOff = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.PowerOff));
					return;
				}
				if (FlickUtility.WantsToBeOn(this.parent) && !this.PowerOn && this.overlayNeedsPower == null && base.Props.showPowerNeededIfOff)
				{
					this.overlayNeedsPower = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.NeedsPower));
				}
			}
		}

		// Token: 0x06007B0D RID: 31501 RVA: 0x002BCE9C File Offset: 0x002BB09C
		public override void SetUpPowerVars()
		{
			base.SetUpPowerVars();
			CompProperties_Power props = base.Props;
			if (!this.PowerOn && !Mathf.Approximately(props.idlePowerDraw, -1f))
			{
				this.PowerOutput = -props.idlePowerDraw;
			}
			else
			{
				this.PowerOutput = -props.PowerConsumption;
			}
			this.powerLastOutputted = (props.PowerConsumption <= 0f);
		}

		// Token: 0x06007B0E RID: 31502 RVA: 0x002BCF04 File Offset: 0x002BB104
		public override void ResetPowerVars()
		{
			base.ResetPowerVars();
			this.powerOnInt = false;
			this.powerOutputInt = 0f;
			this.powerLastOutputted = false;
			this.sustainerPowered = null;
			if (this.flickableComp != null && !this.parent.BeingTransportedOnGravship)
			{
				this.flickableComp.ResetToOn();
			}
		}

		// Token: 0x06007B0F RID: 31503 RVA: 0x002BCF57 File Offset: 0x002BB157
		public override void LostConnectParent()
		{
			base.LostConnectParent();
			this.PowerOn = false;
		}

		// Token: 0x06007B10 RID: 31504 RVA: 0x002BCF68 File Offset: 0x002BB168
		public override string CompInspectStringExtra()
		{
			if (this.Off && (base.Props.idlePowerDraw > 0f || base.Props.alwaysDisplayAsUsingPower))
			{
				return null;
			}
			string text;
			if (this.powerLastOutputted && !base.Props.alwaysDisplayAsUsingPower)
			{
				text = "PowerOutput".Translate() + ": " + this.PowerOutput.ToString("#####0") + " W";
			}
			else
			{
				text = "PowerNeeded".Translate() + ": " + (-this.PowerOutput).ToString("#####0") + " W";
			}
			if (base.Props.idlePowerDraw > 0f || base.Props.alwaysDisplayAsUsingPower)
			{
				text += " (" + "PowerActiveNeeded".Translate(base.Props.PowerConsumption.ToString("#####0")) + ")";
			}
			return text + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x06007B11 RID: 31505 RVA: 0x002BD0A8 File Offset: 0x002BB2A8
		private void StartSustainerPoweredIfInactive()
		{
			if (!base.Props.soundAmbientPowered.NullOrUndefined() && this.sustainerPowered == null)
			{
				SoundInfo info = SoundInfo.InMap(this.parent, MaintenanceType.None);
				this.sustainerPowered = base.Props.soundAmbientPowered.TrySpawnSustainer(info);
			}
		}

		// Token: 0x06007B12 RID: 31506 RVA: 0x002BD0F8 File Offset: 0x002BB2F8
		private void EndSustainerPoweredIfActive()
		{
			if (this.sustainerPowered != null)
			{
				this.sustainerPowered.End();
				this.sustainerPowered = null;
			}
		}

		// Token: 0x06007B13 RID: 31507 RVA: 0x002BD114 File Offset: 0x002BB314
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPowerTrader.<CompGetGizmosExtra>d__39 <CompGetGizmosExtra>d__ = new CompPowerTrader.<CompGetGizmosExtra>d__39(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04004DAB RID: 19883
		public Action powerStartedAction;

		// Token: 0x04004DAC RID: 19884
		public Action powerStoppedAction;

		// Token: 0x04004DAD RID: 19885
		private bool powerOnInt;

		// Token: 0x04004DAE RID: 19886
		public float powerOutputInt;

		// Token: 0x04004DAF RID: 19887
		private bool powerLastOutputted;

		// Token: 0x04004DB0 RID: 19888
		private Sustainer sustainerPowered;

		// Token: 0x04004DB1 RID: 19889
		protected CompFlickable flickableComp;

		// Token: 0x04004DB2 RID: 19890
		private CompStunnable stunnableComp;

		// Token: 0x04004DB3 RID: 19891
		public const string PowerTurnedOnSignal = "PowerTurnedOn";

		// Token: 0x04004DB4 RID: 19892
		public const string PowerTurnedOffSignal = "PowerTurnedOff";

		// Token: 0x04004DB5 RID: 19893
		private OverlayHandle? overlayPowerOff;

		// Token: 0x04004DB6 RID: 19894
		private OverlayHandle? overlayNeedsPower;
	}
}
