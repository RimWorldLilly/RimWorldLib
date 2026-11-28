using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001950 RID: 6480
	[StaticConstructorOnStartup]
	public class CompRefuelable : ThingComp_VacuumAware, IThingGlower
	{
		// Token: 0x17001C22 RID: 7202
		// (get) Token: 0x0600A28E RID: 41614 RVA: 0x003A1417 File Offset: 0x0039F617
		protected override bool FunctionsInVacuum
		{
			get
			{
				return this.Props.functionsInVacuum;
			}
		}

		// Token: 0x17001C23 RID: 7203
		// (get) Token: 0x0600A28F RID: 41615 RVA: 0x003A1424 File Offset: 0x0039F624
		// (set) Token: 0x0600A290 RID: 41616 RVA: 0x003A145E File Offset: 0x0039F65E
		public float TargetFuelLevel
		{
			get
			{
				if (this.configuredTargetFuelLevel >= 0f)
				{
					return this.configuredTargetFuelLevel;
				}
				if (this.Props.targetFuelLevelConfigurable)
				{
					return this.Props.initialConfigurableTargetFuelLevel;
				}
				return this.Props.fuelCapacity;
			}
			set
			{
				this.configuredTargetFuelLevel = Mathf.Clamp(value, 0f, this.Props.fuelCapacity);
			}
		}

		// Token: 0x17001C24 RID: 7204
		// (get) Token: 0x0600A291 RID: 41617 RVA: 0x003A147C File Offset: 0x0039F67C
		public CompProperties_Refuelable Props
		{
			get
			{
				return (CompProperties_Refuelable)this.props;
			}
		}

		// Token: 0x17001C25 RID: 7205
		// (get) Token: 0x0600A292 RID: 41618 RVA: 0x003A1489 File Offset: 0x0039F689
		public float Fuel
		{
			get
			{
				return this.fuel;
			}
		}

		// Token: 0x17001C26 RID: 7206
		// (get) Token: 0x0600A293 RID: 41619 RVA: 0x003A1491 File Offset: 0x0039F691
		public float FuelPercentOfTarget
		{
			get
			{
				return this.fuel / this.TargetFuelLevel;
			}
		}

		// Token: 0x17001C27 RID: 7207
		// (get) Token: 0x0600A294 RID: 41620 RVA: 0x003A14A0 File Offset: 0x0039F6A0
		public float FuelPercentOfMax
		{
			get
			{
				return this.fuel / this.Props.fuelCapacity;
			}
		}

		// Token: 0x17001C28 RID: 7208
		// (get) Token: 0x0600A295 RID: 41621 RVA: 0x003A14B4 File Offset: 0x0039F6B4
		public bool IsFull
		{
			get
			{
				return this.TargetFuelLevel - this.fuel < 1f * this.Props.FuelMultiplierCurrentDifficulty;
			}
		}

		// Token: 0x17001C29 RID: 7209
		// (get) Token: 0x0600A296 RID: 41622 RVA: 0x003A14D6 File Offset: 0x0039F6D6
		public bool HasFuel
		{
			get
			{
				return this.fuel > 0f && this.fuel >= this.Props.minimumFueledThreshold && (this.FunctionsInVacuum || !base.InVacuum);
			}
		}

		// Token: 0x17001C2A RID: 7210
		// (get) Token: 0x0600A297 RID: 41623 RVA: 0x003A150D File Offset: 0x0039F70D
		private float ConsumptionRatePerTick
		{
			get
			{
				return this.Props.fuelConsumptionRate / 60000f;
			}
		}

		// Token: 0x17001C2B RID: 7211
		// (get) Token: 0x0600A298 RID: 41624 RVA: 0x003A1520 File Offset: 0x0039F720
		public bool ShouldAutoRefuelNow
		{
			get
			{
				return this.FuelPercentOfTarget <= this.Props.autoRefuelPercent && !this.IsFull && this.TargetFuelLevel > 0f && this.ShouldAutoRefuelNowIgnoringFuelPct;
			}
		}

		// Token: 0x17001C2C RID: 7212
		// (get) Token: 0x0600A299 RID: 41625 RVA: 0x003A1554 File Offset: 0x0039F754
		public bool ShouldAutoRefuelNowIgnoringFuelPct
		{
			get
			{
				return !this.parent.IsBurning() && (this.flickComp == null || this.flickComp.SwitchIsOn) && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Flick) == null && this.parent.Map.designationManager.DesignationOn(this.parent, DesignationDefOf.Deconstruct) == null;
			}
		}

		// Token: 0x0600A29A RID: 41626 RVA: 0x003A15CA File Offset: 0x0039F7CA
		public bool ShouldBeLitNow()
		{
			return this.HasFuel;
		}

		// Token: 0x0600A29B RID: 41627 RVA: 0x003A15D2 File Offset: 0x0039F7D2
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.allowAutoRefuel = this.Props.initialAllowAutoRefuel;
			this.fuel = this.Props.fuelCapacity * this.Props.initialFuelPercent;
		}

		// Token: 0x0600A29C RID: 41628 RVA: 0x003A1609 File Offset: 0x0039F809
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.flickComp = this.parent.GetComp<CompFlickable>();
			this.explosiveComp = this.parent.GetComp<CompExplosive>();
		}

		// Token: 0x0600A29D RID: 41629 RVA: 0x003A1630 File Offset: 0x0039F830
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fuel, "fuel", 0f, false);
			Scribe_Values.Look<float>(ref this.configuredTargetFuelLevel, "configuredTargetFuelLevel", -1f, false);
			Scribe_Values.Look<bool>(ref this.allowAutoRefuel, "allowAutoRefuel", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.Props.showAllowAutoRefuelToggle)
			{
				this.allowAutoRefuel = this.Props.initialAllowAutoRefuel;
			}
		}

		// Token: 0x0600A29E RID: 41630 RVA: 0x003A16A8 File Offset: 0x0039F8A8
		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.allowAutoRefuel)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenRefuel);
			}
			else if (!this.HasFuel && this.Props.drawOutOfFuelOverlay)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.OutOfFuel);
			}
			if (this.Props.drawFuelGaugeInMap)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = this.parent.DrawPos + Vector3.up * 0.1f;
				r.size = CompRefuelable.FuelBarSize;
				r.fillPercent = this.FuelPercentOfMax;
				r.filledMat = CompRefuelable.FuelBarFilledMat;
				r.unfilledMat = CompRefuelable.FuelBarUnfilledMat;
				r.margin = 0.15f;
				Rot4 rotation = this.parent.Rotation;
				rotation.Rotate(RotationDirection.Clockwise);
				r.rotation = rotation;
				GenDraw.DrawFillableBar(r);
			}
		}

		// Token: 0x0600A29F RID: 41631 RVA: 0x003A17B8 File Offset: 0x0039F9B8
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
			{
				return;
			}
			if (mode != DestroyMode.Vanish && previousMap != null && this.Props.fuelFilter.AllowedDefCount == 1 && this.Props.initialFuelPercent == 0f)
			{
				ThingDef thingDef = this.Props.fuelFilter.AllowedThingDefs.First<ThingDef>();
				int i = Mathf.FloorToInt(1f * this.fuel);
				while (i > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = Mathf.Min(i, thingDef.stackLimit);
					i -= thing.stackCount;
					GenPlace.TryPlaceThing(thing, this.parent.Position, previousMap, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
		}

		// Token: 0x0600A2A0 RID: 41632 RVA: 0x003A1894 File Offset: 0x0039FA94
		public override string CompInspectStringExtra()
		{
			if (this.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
			{
				return string.Empty;
			}
			string text = base.CompInspectStringExtra();
			if (text == null)
			{
				text = string.Empty;
			}
			else
			{
				text += "\n";
			}
			text = string.Concat(new string[]
			{
				text,
				this.Props.FuelLabel,
				": ",
				this.fuel.ToStringDecimalIfSmall(),
				" / ",
				this.Props.fuelCapacity.ToStringDecimalIfSmall()
			});
			if (!this.Props.consumeFuelOnlyWhenUsed && this.HasFuel)
			{
				int numTicks = (int)(this.fuel / this.Props.fuelConsumptionRate * 60000f);
				text = text + " (" + numTicks.ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			if (!this.HasFuel && !this.Props.outOfFuelMessage.NullOrEmpty())
			{
				string value = (this.parent.def.building != null && this.parent.def.building.IsTurret) ? ("CannotShoot".Translate() + ": " + this.Props.outOfFuelMessage).Resolve() : this.Props.outOfFuelMessage;
				string str = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				defaultInterpolatedStringHandler.AppendFormatted(value);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.GetFuelCountToFullyRefuel());
				defaultInterpolatedStringHandler.AppendLiteral("x ");
				defaultInterpolatedStringHandler.AppendFormatted(this.Props.fuelFilter.AnyAllowedDef.label);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				text = str + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (this.Props.targetFuelLevelConfigurable)
			{
				text += "\n" + "ConfiguredTargetFuelLevel".Translate(this.TargetFuelLevel.ToStringDecimalIfSmall());
			}
			return text;
		}

		// Token: 0x0600A2A1 RID: 41633 RVA: 0x003A1AB6 File Offset: 0x0039FCB6
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompRefuelable.<SpecialDisplayStats>d__41 <SpecialDisplayStats>d__ = new CompRefuelable.<SpecialDisplayStats>d__41(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A2A2 RID: 41634 RVA: 0x003A1AC8 File Offset: 0x0039FCC8
		public override void CompTick()
		{
			base.CompTick();
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (!this.Props.consumeFuelOnlyWhenUsed && (this.flickComp == null || this.flickComp.SwitchIsOn) && (!this.Props.consumeFuelOnlyWhenPowered || (comp != null && comp.PowerOn)) && !this.Props.externalTicking)
			{
				this.ConsumeFuel(this.ConsumptionRatePerTick);
			}
			if (this.Props.fuelConsumptionPerTickInRain > 0f && this.parent.Spawned && this.parent.Map.weatherManager.RainRate > 0.4f && !this.parent.Map.roofGrid.Roofed(this.parent.Position) && !this.Props.externalTicking)
			{
				this.ConsumeFuel(this.Props.fuelConsumptionPerTickInRain);
			}
		}

		// Token: 0x0600A2A3 RID: 41635 RVA: 0x003A1BB8 File Offset: 0x0039FDB8
		public void ConsumeFuel(float amount)
		{
			if (this.Props.fuelIsMortarBarrel && Find.Storyteller.difficulty.classicMortars)
			{
				return;
			}
			if (this.fuel <= 0f)
			{
				return;
			}
			this.fuel -= amount;
			if (this.fuel <= 0f)
			{
				this.fuel = 0f;
				this.Notify_RanOutOfFuel();
			}
		}

		// Token: 0x0600A2A4 RID: 41636 RVA: 0x003A1C1E File Offset: 0x0039FE1E
		private void Notify_RanOutOfFuel()
		{
			if (this.Props.destroyOnNoFuel)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
			this.parent.BroadcastCompSignal("RanOutOfFuel");
		}

		// Token: 0x0600A2A5 RID: 41637 RVA: 0x003A1C4C File Offset: 0x0039FE4C
		public void Refuel(List<Thing> fuelThings)
		{
			if (this.Props.atomicFueling)
			{
				if (fuelThings.Sum((Thing t) => t.stackCount) < this.GetFuelCountToFullyRefuel())
				{
					Log.ErrorOnce("Error refueling; not enough fuel available for proper atomic refuel", 19586442);
					return;
				}
			}
			int num = this.GetFuelCountToFullyRefuel();
			while (num > 0 && fuelThings.Count > 0)
			{
				Thing thing = fuelThings.Pop<Thing>();
				int num2 = Mathf.Min(num, thing.stackCount);
				this.Refuel((float)num2);
				thing.SplitOff(num2).Destroy(DestroyMode.Vanish);
				num -= num2;
			}
		}

		// Token: 0x0600A2A6 RID: 41638 RVA: 0x003A1CE8 File Offset: 0x0039FEE8
		public void Refuel(float amount)
		{
			this.fuel += amount * this.Props.FuelMultiplierCurrentDifficulty;
			if (this.fuel > this.Props.fuelCapacity)
			{
				this.fuel = this.Props.fuelCapacity;
			}
			this.parent.BroadcastCompSignal("Refueled");
		}

		// Token: 0x0600A2A7 RID: 41639 RVA: 0x003A1D44 File Offset: 0x0039FF44
		public AcceptanceReport CanEjectFuel()
		{
			if (!this.Props.canEjectFuel)
			{
				return false;
			}
			CompExplosive compExplosive = this.explosiveComp;
			if (compExplosive != null && compExplosive.wickStarted)
			{
				return "AboutToExplode".Translate();
			}
			if (this.Fuel == 0f)
			{
				return "RefuelableNoFuelToEject".Translate();
			}
			return true;
		}

		// Token: 0x0600A2A8 RID: 41640 RVA: 0x003A1DAC File Offset: 0x0039FFAC
		public void EjectFuel()
		{
			ThingDef thingDef = this.Props.fuelFilter.AllowedThingDefs.First<ThingDef>();
			int i = Mathf.FloorToInt(this.fuel);
			while (i > 0)
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = Mathf.Min(i, thingDef.stackLimit);
				i -= thing.stackCount;
				GenPlace.TryPlaceThing(thing, this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, null, null, 1);
				thing.SetForbidden(true, true);
			}
			this.fuel = 0f;
			this.Notify_RanOutOfFuel();
		}

		// Token: 0x0600A2A9 RID: 41641 RVA: 0x003A1E47 File Offset: 0x003A0047
		public void Notify_UsedThisTick()
		{
			this.ConsumeFuel(this.ConsumptionRatePerTick);
		}

		// Token: 0x0600A2AA RID: 41642 RVA: 0x003A1E58 File Offset: 0x003A0058
		public int GetFuelCountToFullyRefuel()
		{
			if (this.Props.atomicFueling)
			{
				return Mathf.CeilToInt(this.Props.fuelCapacity / this.Props.FuelMultiplierCurrentDifficulty);
			}
			return Mathf.Max(Mathf.CeilToInt((this.TargetFuelLevel - this.fuel) / this.Props.FuelMultiplierCurrentDifficulty), 1);
		}

		// Token: 0x0600A2AB RID: 41643 RVA: 0x003A1EB3 File Offset: 0x003A00B3
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompRefuelable.<CompGetGizmosExtra>d__51 <CompGetGizmosExtra>d__ = new CompRefuelable.<CompGetGizmosExtra>d__51(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04006021 RID: 24609
		private float fuel;

		// Token: 0x04006022 RID: 24610
		private float configuredTargetFuelLevel = -1f;

		// Token: 0x04006023 RID: 24611
		public bool allowAutoRefuel = true;

		// Token: 0x04006024 RID: 24612
		private CompFlickable flickComp;

		// Token: 0x04006025 RID: 24613
		private CompExplosive explosiveComp;

		// Token: 0x04006026 RID: 24614
		public const string RefueledSignal = "Refueled";

		// Token: 0x04006027 RID: 24615
		public const string RanOutOfFuelSignal = "RanOutOfFuel";

		// Token: 0x04006028 RID: 24616
		private static readonly Texture2D SetTargetFuelLevelCommand = ContentFinder<Texture2D>.Get("UI/Commands/SetTargetFuelLevel", true);

		// Token: 0x04006029 RID: 24617
		private static readonly Vector2 FuelBarSize = new Vector2(1f, 0.2f);

		// Token: 0x0400602A RID: 24618
		private static readonly Material FuelBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.56f, 0.13f), false);

		// Token: 0x0400602B RID: 24619
		private static readonly Material FuelBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
