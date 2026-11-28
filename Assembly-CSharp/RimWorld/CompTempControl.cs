using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019AF RID: 6575
	public class CompTempControl : ThingComp
	{
		// Token: 0x17001CB7 RID: 7351
		// (get) Token: 0x0600A505 RID: 42245 RVA: 0x003AEBEF File Offset: 0x003ACDEF
		// (set) Token: 0x0600A506 RID: 42246 RVA: 0x003AEBF7 File Offset: 0x003ACDF7
		public virtual float TargetTemperature
		{
			get
			{
				return this.targetTemperature;
			}
			set
			{
				this.targetTemperature = value;
			}
		}

		// Token: 0x17001CB8 RID: 7352
		// (get) Token: 0x0600A507 RID: 42247 RVA: 0x003AEC00 File Offset: 0x003ACE00
		public CompProperties_TempControl Props
		{
			get
			{
				return (CompProperties_TempControl)this.props;
			}
		}

		// Token: 0x17001CB9 RID: 7353
		// (get) Token: 0x0600A508 RID: 42248 RVA: 0x003AEC10 File Offset: 0x003ACE10
		public CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.intPowerTrader) == null)
				{
					result = (this.intPowerTrader = this.parent.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x0600A509 RID: 42249 RVA: 0x003AEC3B File Offset: 0x003ACE3B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.TargetTemperature < -2000f)
			{
				this.TargetTemperature = this.Props.defaultTargetTemperature;
			}
		}

		// Token: 0x0600A50A RID: 42250 RVA: 0x003AEC62 File Offset: 0x003ACE62
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.targetTemperature, "targetTemperature", 0f, false);
		}

		// Token: 0x0600A50B RID: 42251 RVA: 0x003AEC80 File Offset: 0x003ACE80
		protected float RoundedToCurrentTempModeOffset(float celsiusTemp)
		{
			return GenTemperature.ConvertTemperatureOffset((float)Mathf.RoundToInt(GenTemperature.CelsiusToOffset(celsiusTemp, Prefs.TemperatureMode)), Prefs.TemperatureMode, TemperatureDisplayMode.Celsius);
		}

		// Token: 0x0600A50C RID: 42252 RVA: 0x003AEC9E File Offset: 0x003ACE9E
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompTempControl.<CompGetGizmosExtra>d__14 <CompGetGizmosExtra>d__ = new CompTempControl.<CompGetGizmosExtra>d__14(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A50D RID: 42253 RVA: 0x003AECAE File Offset: 0x003ACEAE
		protected void InterfaceChangeTargetTemperature(float offset)
		{
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			this.TargetTemperature += offset;
			this.TargetTemperature = Mathf.Clamp(this.TargetTemperature, -273.15f, 1000f);
			this.ThrowCurrentTemperatureText();
		}

		// Token: 0x0600A50E RID: 42254 RVA: 0x003AECEC File Offset: 0x003ACEEC
		protected void ThrowCurrentTemperatureText()
		{
			MoteMaker.ThrowText(this.parent.TrueCenter() + new Vector3(0.5f, 0f, 0.5f), this.parent.Map, this.TargetTemperature.ToStringTemperature("F0"), Color.white, -1f);
		}

		// Token: 0x0600A50F RID: 42255 RVA: 0x003AED48 File Offset: 0x003ACF48
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append((this.Props.inspectString ?? "TargetTemperature".Translate()) + ": ");
			stringBuilder.AppendLine(this.TargetTemperature.ToStringTemperature("F0"));
			if (this.PowerTrader != null)
			{
				string str;
				if (this.PowerTrader.Off)
				{
					str = "PowerConsumptionOff".Translate();
				}
				else if (!this.operatingAtHighPower)
				{
					str = "PowerConsumptionLow".Translate();
				}
				else
				{
					str = "PowerConsumptionHigh".Translate();
				}
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PowerConsumptionMode".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(str.CapitalizeFirst());
				stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400617F RID: 24959
		protected const float DefaultTargetTemperature = 21f;

		// Token: 0x04006180 RID: 24960
		public bool operatingAtHighPower;

		// Token: 0x04006181 RID: 24961
		private CompPowerTrader intPowerTrader;

		// Token: 0x04006182 RID: 24962
		public float targetTemperature = -99999f;
	}
}
