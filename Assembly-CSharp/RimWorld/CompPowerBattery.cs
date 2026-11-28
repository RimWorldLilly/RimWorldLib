using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001355 RID: 4949
	public class CompPowerBattery : CompPower
	{
		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x06007A76 RID: 31350 RVA: 0x002BA37C File Offset: 0x002B857C
		public float AmountCanAccept
		{
			get
			{
				if (this.parent.IsBrokenDown() || this.StunnedByEMP)
				{
					return 0f;
				}
				CompProperties_Battery props = this.Props;
				return (props.storedEnergyMax - this.storedEnergy) / props.efficiency;
			}
		}

		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x06007A77 RID: 31351 RVA: 0x002BA3BF File Offset: 0x002B85BF
		public float StoredEnergy
		{
			get
			{
				return this.storedEnergy;
			}
		}

		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x06007A78 RID: 31352 RVA: 0x002BA3C7 File Offset: 0x002B85C7
		public float StoredEnergyPct
		{
			get
			{
				return this.storedEnergy / this.Props.storedEnergyMax;
			}
		}

		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x06007A79 RID: 31353 RVA: 0x002BA3DB File Offset: 0x002B85DB
		public new CompProperties_Battery Props
		{
			get
			{
				return (CompProperties_Battery)this.props;
			}
		}

		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x06007A7A RID: 31354 RVA: 0x002BA3E8 File Offset: 0x002B85E8
		public bool StunnedByEMP
		{
			get
			{
				return this.stunnableComp != null && this.stunnableComp.StunHandler.Stunned && this.stunnableComp.StunHandler.StunFromEMP;
			}
		}

		// Token: 0x06007A7B RID: 31355 RVA: 0x002BA418 File Offset: 0x002B8618
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.stunnableComp = this.parent.GetComp<CompStunnable>();
		}

		// Token: 0x06007A7C RID: 31356 RVA: 0x002BA434 File Offset: 0x002B8634
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.storedEnergy, "storedPower", 0f, false);
			CompProperties_Battery props = this.Props;
			if (this.storedEnergy > props.storedEnergyMax)
			{
				this.storedEnergy = props.storedEnergyMax;
			}
		}

		// Token: 0x06007A7D RID: 31357 RVA: 0x002BA47E File Offset: 0x002B867E
		public override void CompTick()
		{
			base.CompTick();
			this.DrawPower(Mathf.Min(5f * CompPower.WattsToWattDaysPerTick, this.storedEnergy));
		}

		// Token: 0x06007A7E RID: 31358 RVA: 0x002BA4A4 File Offset: 0x002B86A4
		public void AddEnergy(float amount)
		{
			if (amount < 0f)
			{
				Log.Error("Cannot add negative energy " + amount.ToString());
				return;
			}
			if (this.StunnedByEMP)
			{
				return;
			}
			if (amount > this.AmountCanAccept)
			{
				amount = this.AmountCanAccept;
			}
			amount *= this.Props.efficiency;
			this.storedEnergy += amount;
		}

		// Token: 0x06007A7F RID: 31359 RVA: 0x002BA508 File Offset: 0x002B8708
		public void DrawPower(float amount)
		{
			this.storedEnergy -= amount;
			if (this.storedEnergy < 0f)
			{
				string str = "Drawing power we don't have from ";
				ThingWithComps parent = this.parent;
				Log.Error(str + ((parent != null) ? parent.ToString() : null));
				this.storedEnergy = 0f;
			}
		}

		// Token: 0x06007A80 RID: 31360 RVA: 0x002BA55C File Offset: 0x002B875C
		public void SetStoredEnergyPct(float pct)
		{
			pct = Mathf.Clamp01(pct);
			this.storedEnergy = this.Props.storedEnergyMax * pct;
		}

		// Token: 0x06007A81 RID: 31361 RVA: 0x002BA579 File Offset: 0x002B8779
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "Breakdown")
			{
				this.DrawPower(this.StoredEnergy);
			}
		}

		// Token: 0x06007A82 RID: 31362 RVA: 0x002BA594 File Offset: 0x002B8794
		public override string CompInspectStringExtra()
		{
			CompProperties_Battery props = this.Props;
			string text = "PowerBatteryStored".Translate() + ": " + this.storedEnergy.ToString("F0") + " / " + props.storedEnergyMax.ToString("F0") + " Wd";
			text += "\n" + "PowerBatteryEfficiency".Translate() + ": " + (props.efficiency * 100f).ToString("F0") + "%";
			if (this.storedEnergy > 0f)
			{
				text += "\n" + "SelfDischarging".Translate() + ": " + 5f.ToString("F0") + " W";
			}
			return text + "\n" + base.CompInspectStringExtra();
		}

		// Token: 0x06007A83 RID: 31363 RVA: 0x002BA6BD File Offset: 0x002B88BD
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPowerBattery.<CompGetGizmosExtra>d__21 <CompGetGizmosExtra>d__ = new CompPowerBattery.<CompGetGizmosExtra>d__21(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04004D65 RID: 19813
		private float storedEnergy;

		// Token: 0x04004D66 RID: 19814
		private CompStunnable stunnableComp;

		// Token: 0x04004D67 RID: 19815
		private const float SelfDischargingWatts = 5f;
	}
}
