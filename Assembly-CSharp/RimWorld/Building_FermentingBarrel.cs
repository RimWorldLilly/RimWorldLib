using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAD RID: 6829
	[StaticConstructorOnStartup]
	public class Building_FermentingBarrel : Building
	{
		// Token: 0x17001ED3 RID: 7891
		// (get) Token: 0x0600ACC4 RID: 44228 RVA: 0x003CE6E2 File Offset: 0x003CC8E2
		// (set) Token: 0x0600ACC5 RID: 44229 RVA: 0x003CE6EA File Offset: 0x003CC8EA
		public float Progress
		{
			get
			{
				return this.progressInt;
			}
			set
			{
				if (value == this.progressInt)
				{
					return;
				}
				this.progressInt = value;
				this.barFilledCachedMat = null;
			}
		}

		// Token: 0x17001ED4 RID: 7892
		// (get) Token: 0x0600ACC6 RID: 44230 RVA: 0x003CE704 File Offset: 0x003CC904
		private Material BarFilledMat
		{
			get
			{
				if (this.barFilledCachedMat == null)
				{
					this.barFilledCachedMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.Lerp(Building_FermentingBarrel.BarZeroProgressColor, Building_FermentingBarrel.BarFermentedColor, this.Progress), false);
				}
				return this.barFilledCachedMat;
			}
		}

		// Token: 0x17001ED5 RID: 7893
		// (get) Token: 0x0600ACC7 RID: 44231 RVA: 0x003CE73B File Offset: 0x003CC93B
		public int SpaceLeftForWort
		{
			get
			{
				if (!this.Fermented)
				{
					return 25 - this.wortCount;
				}
				return 0;
			}
		}

		// Token: 0x17001ED6 RID: 7894
		// (get) Token: 0x0600ACC8 RID: 44232 RVA: 0x003CE750 File Offset: 0x003CC950
		private bool Empty
		{
			get
			{
				return this.wortCount <= 0;
			}
		}

		// Token: 0x17001ED7 RID: 7895
		// (get) Token: 0x0600ACC9 RID: 44233 RVA: 0x003CE75E File Offset: 0x003CC95E
		public bool Fermented
		{
			get
			{
				return !this.Empty && this.Progress >= 1f;
			}
		}

		// Token: 0x17001ED8 RID: 7896
		// (get) Token: 0x0600ACCA RID: 44234 RVA: 0x003CE77C File Offset: 0x003CC97C
		private float CurrentTempProgressSpeedFactor
		{
			get
			{
				CompProperties_TemperatureRuinable compProperties = this.def.GetCompProperties<CompProperties_TemperatureRuinable>();
				float ambientTemperature = base.AmbientTemperature;
				if (ambientTemperature < compProperties.minSafeTemperature)
				{
					return 0.1f;
				}
				if (ambientTemperature < 7f)
				{
					return GenMath.LerpDouble(compProperties.minSafeTemperature, 7f, 0.1f, 1f, ambientTemperature);
				}
				return 1f;
			}
		}

		// Token: 0x17001ED9 RID: 7897
		// (get) Token: 0x0600ACCB RID: 44235 RVA: 0x003CE7D4 File Offset: 0x003CC9D4
		private float ProgressPerTickAtCurrentTemp
		{
			get
			{
				return 2.7777778E-06f * this.CurrentTempProgressSpeedFactor;
			}
		}

		// Token: 0x17001EDA RID: 7898
		// (get) Token: 0x0600ACCC RID: 44236 RVA: 0x003CE7E2 File Offset: 0x003CC9E2
		private int EstimatedTicksLeft
		{
			get
			{
				return Mathf.Max(Mathf.RoundToInt((1f - this.Progress) / this.ProgressPerTickAtCurrentTemp), 0);
			}
		}

		// Token: 0x0600ACCD RID: 44237 RVA: 0x003CE802 File Offset: 0x003CCA02
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.wortCount, "wortCount", 0, false);
			Scribe_Values.Look<float>(ref this.progressInt, "progress", 0f, false);
		}

		// Token: 0x0600ACCE RID: 44238 RVA: 0x003CE832 File Offset: 0x003CCA32
		public override void TickRare()
		{
			base.TickRare();
			if (!this.Empty)
			{
				this.Progress = Mathf.Min(this.Progress + 250f * this.ProgressPerTickAtCurrentTemp, 1f);
			}
		}

		// Token: 0x0600ACCF RID: 44239 RVA: 0x003CE868 File Offset: 0x003CCA68
		public void AddWort(int count)
		{
			base.GetComp<CompTemperatureRuinable>().Reset();
			if (this.Fermented)
			{
				Log.Warning("Tried to add wort to a barrel full of beer. Colonists should take the beer first.");
				return;
			}
			int num = Mathf.Min(count, 25 - this.wortCount);
			if (num <= 0)
			{
				return;
			}
			this.Progress = GenMath.WeightedAverage(0f, (float)num, this.Progress, (float)this.wortCount);
			this.wortCount += num;
		}

		// Token: 0x0600ACD0 RID: 44240 RVA: 0x003CE8D5 File Offset: 0x003CCAD5
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "RuinedByTemperature")
			{
				this.Reset();
			}
		}

		// Token: 0x0600ACD1 RID: 44241 RVA: 0x003CE8EA File Offset: 0x003CCAEA
		private void Reset()
		{
			this.wortCount = 0;
			this.Progress = 0f;
		}

		// Token: 0x0600ACD2 RID: 44242 RVA: 0x003CE900 File Offset: 0x003CCB00
		public void AddWort(Thing wort)
		{
			int num = Mathf.Min(wort.stackCount, 25 - this.wortCount);
			if (num > 0)
			{
				this.AddWort(num);
				wort.SplitOff(num).Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600ACD3 RID: 44243 RVA: 0x003CE93C File Offset: 0x003CCB3C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			CompTemperatureRuinable comp = base.GetComp<CompTemperatureRuinable>();
			if (!this.Empty && !comp.Ruined)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("ContainsBeer".Translate(this.wortCount, 25));
				}
				else
				{
					stringBuilder.AppendLine("ContainsWort".Translate(this.wortCount, 25));
				}
			}
			if (!this.Empty)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("Fermented".Translate());
				}
				else
				{
					stringBuilder.AppendLine("FermentationProgress".Translate(this.Progress.ToStringPercent(), this.EstimatedTicksLeft.ToStringTicksToPeriod(true, false, true, true, false)));
					if (this.CurrentTempProgressSpeedFactor != 1f)
					{
						stringBuilder.AppendLine("FermentationBarrelOutOfIdealTemperature".Translate(this.CurrentTempProgressSpeedFactor.ToStringPercent()));
					}
				}
			}
			stringBuilder.AppendLine("Temperature".Translate() + ": " + base.AmbientTemperature.ToStringTemperature("F0"));
			stringBuilder.AppendLine("IdealFermentingTemperature".Translate() + ": " + 7f.ToStringTemperature("F0") + " ~ " + comp.Props.maxSafeTemperature.ToStringTemperature("F0"));
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600ACD4 RID: 44244 RVA: 0x003CEB0D File Offset: 0x003CCD0D
		public Thing TakeOutBeer()
		{
			if (!this.Fermented)
			{
				Log.Warning("Tried to get beer but it's not yet fermented.");
				return null;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Beer, null);
			thing.stackCount = this.wortCount;
			this.Reset();
			return thing;
		}

		// Token: 0x0600ACD5 RID: 44245 RVA: 0x003CEB40 File Offset: 0x003CCD40
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.Empty)
			{
				return;
			}
			Vector3 center = drawLoc;
			center.y += 0.03658537f;
			center.z += 0.25f;
			GenDraw.DrawFillableBar(new GenDraw.FillableBarRequest
			{
				center = center,
				size = Building_FermentingBarrel.BarSize,
				fillPercent = (float)this.wortCount / 25f,
				filledMat = this.BarFilledMat,
				unfilledMat = Building_FermentingBarrel.BarUnfilledMat,
				margin = 0.1f,
				rotation = Rot4.North
			});
		}

		// Token: 0x0600ACD6 RID: 44246 RVA: 0x003CEBE7 File Offset: 0x003CCDE7
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_FermentingBarrel.<GetGizmos>d__36 <GetGizmos>d__ = new Building_FermentingBarrel.<GetGizmos>d__36(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0400643E RID: 25662
		private int wortCount;

		// Token: 0x0400643F RID: 25663
		private float progressInt;

		// Token: 0x04006440 RID: 25664
		private Material barFilledCachedMat;

		// Token: 0x04006441 RID: 25665
		public const int MaxCapacity = 25;

		// Token: 0x04006442 RID: 25666
		private const int BaseFermentationDuration = 360000;

		// Token: 0x04006443 RID: 25667
		public const float MinIdealTemperature = 7f;

		// Token: 0x04006444 RID: 25668
		private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

		// Token: 0x04006445 RID: 25669
		private static readonly Color BarZeroProgressColor = new Color(0.4f, 0.27f, 0.22f);

		// Token: 0x04006446 RID: 25670
		private static readonly Color BarFermentedColor = new Color(0.9f, 0.85f, 0.2f);

		// Token: 0x04006447 RID: 25671
		private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
