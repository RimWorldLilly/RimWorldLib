using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001ABE RID: 6846
	[StaticConstructorOnStartup]
	public class Building_MechCharger : Building
	{
		// Token: 0x17001F39 RID: 7993
		// (get) Token: 0x0600AE0D RID: 44557 RVA: 0x003D5A38 File Offset: 0x003D3C38
		public CompPowerTrader Power
		{
			get
			{
				return this.TryGetComp<CompPowerTrader>();
			}
		}

		// Token: 0x17001F3A RID: 7994
		// (get) Token: 0x0600AE0E RID: 44558 RVA: 0x003D5A40 File Offset: 0x003D3C40
		public bool IsPowered
		{
			get
			{
				return this.Power.PowerOn;
			}
		}

		// Token: 0x17001F3B RID: 7995
		// (get) Token: 0x0600AE0F RID: 44559 RVA: 0x003D5A4D File Offset: 0x003D3C4D
		public bool IsFullOfWaste
		{
			get
			{
				return this.wasteProduced >= (float)this.WasteProducedPerChargingCycle && this.Container.innerContainer.Any;
			}
		}

		// Token: 0x17001F3C RID: 7996
		// (get) Token: 0x0600AE10 RID: 44560 RVA: 0x003D5A70 File Offset: 0x003D3C70
		private CompWasteProducer WasteProducer
		{
			get
			{
				if (this.wasteProducer == null)
				{
					this.wasteProducer = base.GetComp<CompWasteProducer>();
				}
				return this.wasteProducer;
			}
		}

		// Token: 0x17001F3D RID: 7997
		// (get) Token: 0x0600AE11 RID: 44561 RVA: 0x003D5A8C File Offset: 0x003D3C8C
		public CompThingContainer Container
		{
			get
			{
				if (this.container == null)
				{
					this.container = base.GetComp<CompThingContainer>();
				}
				return this.container;
			}
		}

		// Token: 0x17001F3E RID: 7998
		// (get) Token: 0x0600AE12 RID: 44562 RVA: 0x003D5AA8 File Offset: 0x003D3CA8
		public GenDraw.FillableBarRequest BarDrawData
		{
			get
			{
				return this.def.building.BarDrawDataFor(base.Rotation);
			}
		}

		// Token: 0x17001F3F RID: 7999
		// (get) Token: 0x0600AE13 RID: 44563 RVA: 0x003D5AC0 File Offset: 0x003D3CC0
		private Material WireMaterial
		{
			get
			{
				if (this.wireMaterial == null)
				{
					this.wireMaterial = MaterialPool.MatFrom("Other/BundledWires", ShaderDatabase.Transparent, Color.white);
				}
				return this.wireMaterial;
			}
		}

		// Token: 0x17001F40 RID: 8000
		// (get) Token: 0x0600AE14 RID: 44564 RVA: 0x003D5AF0 File Offset: 0x003D3CF0
		private bool IsAttachedToMech
		{
			get
			{
				return this.currentlyChargingMech != null && this.wireExtensionTicks >= 70;
			}
		}

		// Token: 0x17001F41 RID: 8001
		// (get) Token: 0x0600AE15 RID: 44565 RVA: 0x003D5B09 File Offset: 0x003D3D09
		private int WasteProducedPerChargingCycle
		{
			get
			{
				return this.Container.Props.stackLimit;
			}
		}

		// Token: 0x17001F42 RID: 8002
		// (get) Token: 0x0600AE16 RID: 44566 RVA: 0x003D5B1B File Offset: 0x003D3D1B
		private float WasteProducedPercentFull
		{
			get
			{
				return this.wasteProduced / (float)this.WasteProducedPerChargingCycle;
			}
		}

		// Token: 0x17001F43 RID: 8003
		// (get) Token: 0x0600AE17 RID: 44567 RVA: 0x003D5B2B File Offset: 0x003D3D2B
		private float WasteProducedPerTick
		{
			get
			{
				return this.currentlyChargingMech.GetStatValue(StatDefOf.WastepacksPerRecharge, true, -1) * (0.00083333335f / this.currentlyChargingMech.needs.energy.MaxLevel);
			}
		}

		// Token: 0x17001F44 RID: 8004
		// (get) Token: 0x0600AE18 RID: 44568 RVA: 0x003D5B5B File Offset: 0x003D3D5B
		public Pawn CurrentlyChargingMech
		{
			get
			{
				return this.currentlyChargingMech;
			}
		}

		// Token: 0x0600AE19 RID: 44569 RVA: 0x003D5B63 File Offset: 0x003D3D63
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Mech recharger"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
		}

		// Token: 0x0600AE1A RID: 44570 RVA: 0x003D5B80 File Offset: 0x003D3D80
		public bool CanPawnChargeCurrently(Pawn pawn)
		{
			if (this.Power.PowerNet == null)
			{
				return false;
			}
			if (this.IsFullOfWaste)
			{
				return false;
			}
			if (!this.IsCompatibleWithCharger(pawn.kindDef))
			{
				return false;
			}
			if (this.IsPowered)
			{
				if (this.currentlyChargingMech == null)
				{
					return true;
				}
				if (this.currentlyChargingMech == pawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600AE1B RID: 44571 RVA: 0x003D5BD4 File Offset: 0x003D3DD4
		public bool IsCompatibleWithCharger(PawnKindDef kindDef)
		{
			return Building_MechCharger.IsCompatibleWithCharger(this.def, kindDef);
		}

		// Token: 0x0600AE1C RID: 44572 RVA: 0x003D5BE2 File Offset: 0x003D3DE2
		public static bool IsCompatibleWithCharger(ThingDef chargerDef, PawnKindDef kindDef)
		{
			return Building_MechCharger.IsCompatibleWithCharger(chargerDef, kindDef.race);
		}

		// Token: 0x0600AE1D RID: 44573 RVA: 0x003D5BF0 File Offset: 0x003D3DF0
		public static bool IsCompatibleWithCharger(ThingDef chargerDef, ThingDef mechRace)
		{
			return mechRace.race.IsMechanoid && mechRace.GetCompProperties<CompProperties_OverseerSubject>() != null && mechRace.race.mechWeightClass != null && chargerDef.building.requiredMechWeightClasses.NotNullAndContains(mechRace.race.mechWeightClass);
		}

		// Token: 0x0600AE1E RID: 44574 RVA: 0x003D5C3C File Offset: 0x003D3E3C
		protected override void Tick()
		{
			base.Tick();
			if (this.currentlyChargingMech != null && (this.currentlyChargingMech.CurJobDef != JobDefOf.MechCharge || this.currentlyChargingMech.CurJob.targetA.Thing != this))
			{
				Log.Warning("Mech did not clean up his charging job properly");
				this.StopCharging();
			}
			if (this.currentlyChargingMech != null && this.Power.PowerOn)
			{
				this.currentlyChargingMech.needs.energy.CurLevel += 0.00083333335f;
				this.wasteProduced += this.WasteProducedPerTick;
				this.wasteProduced = Mathf.Clamp(this.wasteProduced, 0f, (float)this.WasteProducedPerChargingCycle);
				if (this.wasteProduced >= (float)this.WasteProducedPerChargingCycle && !this.Container.innerContainer.Any)
				{
					this.wasteProduced = 0f;
					this.GenerateWastePack();
				}
				if (this.moteCablePulse == null || this.moteCablePulse.Destroyed)
				{
					this.moteCablePulse = MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_ChargingCablesPulse, this, new TargetInfo(this.InteractionCell, base.Map, false));
				}
				Mote mote = this.moteCablePulse;
				if (mote != null)
				{
					mote.Maintain();
				}
			}
			if (this.currentlyChargingMech != null && this.Power.PowerOn && this.IsAttachedToMech)
			{
				if (this.sustainerCharging == null)
				{
					this.sustainerCharging = SoundDefOf.MechChargerCharging.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.None));
				}
				this.sustainerCharging.Maintain();
				if (this.moteCharging == null || this.moteCharging.Destroyed)
				{
					this.moteCharging = MoteMaker.MakeAttachedOverlay(this.currentlyChargingMech, ThingDefOf.Mote_MechCharging, Vector3.zero, 1f, -1f);
				}
				Mote mote2 = this.moteCharging;
				if (mote2 != null)
				{
					mote2.Maintain();
				}
			}
			else if (this.sustainerCharging != null && (this.currentlyChargingMech == null || !this.Power.PowerOn))
			{
				this.sustainerCharging.End();
				this.sustainerCharging = null;
			}
			if (this.wireExtensionTicks < 70)
			{
				this.wireExtensionTicks++;
			}
		}

		// Token: 0x0600AE1F RID: 44575 RVA: 0x003D5E63 File Offset: 0x003D4063
		public void GenerateWastePack()
		{
			this.WasteProducer.ProduceWaste(this.WasteProducedPerChargingCycle);
			EffecterDefOf.MechChargerWasteProduced.Spawn(base.Position, base.Map, 1f).Cleanup();
		}

		// Token: 0x0600AE20 RID: 44576 RVA: 0x003D5E96 File Offset: 0x003D4096
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_MechCharger.<GetGizmos>d__46 <GetGizmos>d__ = new Building_MechCharger.<GetGizmos>d__46(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AE21 RID: 44577 RVA: 0x003D5EA8 File Offset: 0x003D40A8
		public void StartCharging(Pawn mech)
		{
			if (!ModLister.CheckBiotech("Mech charging"))
			{
				return;
			}
			if (this.currentlyChargingMech != null)
			{
				Log.Error("Tried charging on already charging mech charger!");
				return;
			}
			if (!mech.IsColonyMech)
			{
				mech.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				return;
			}
			this.currentlyChargingMech = mech;
			mech.needs.energy.currentCharger = this;
			this.wireExtensionTicks = 0;
			SoundDefOf.MechChargerStart.PlayOneShot(this);
		}

		// Token: 0x0600AE22 RID: 44578 RVA: 0x003D5F1C File Offset: 0x003D411C
		public void StopCharging()
		{
			if (this.currentlyChargingMech == null)
			{
				Log.Error("Tried stopping charging on currently not charging mech charger!");
				return;
			}
			if (this.currentlyChargingMech.needs.energy != null)
			{
				this.currentlyChargingMech.needs.energy.currentCharger = null;
			}
			this.currentlyChargingMech = null;
			this.wireExtensionTicks = 0;
		}

		// Token: 0x0600AE23 RID: 44579 RVA: 0x003D5F74 File Offset: 0x003D4174
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (base.BeingTransportedOnGravship)
			{
				base.DeSpawn(mode);
				return;
			}
			if (this.currentlyChargingMech != null && mode != DestroyMode.WillReplace)
			{
				Messages.Message("MessageMechChargerDestroyedMechGoesBerserk".Translate(this.currentlyChargingMech.Named("PAWN")), new LookTargets(this.currentlyChargingMech), MessageTypeDefOf.NegativeEvent, true);
				this.currentlyChargingMech.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.BerserkMechanoid, null, false, false, false, null, false, false, false);
			}
			this.WasteProducer.ProduceWaste(Mathf.CeilToInt(this.wasteProduced));
			base.DeSpawn(mode);
		}

		// Token: 0x0600AE24 RID: 44580 RVA: 0x003D6014 File Offset: 0x003D4214
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			GenDraw.FillableBarRequest barDrawData = this.BarDrawData;
			barDrawData.center = this.DrawPos + Vector3.up * 0.1f;
			barDrawData.fillPercent = this.WasteProducedPercentFull;
			barDrawData.filledMat = Building_MechCharger.WasteBarFilledMat;
			barDrawData.unfilledMat = Building_MechCharger.WasteBarUnfilledMat;
			barDrawData.rotation = base.Rotation;
			GenDraw.DrawFillableBar(barDrawData);
			Vector3 a = drawLoc;
			float num = this.EaseInOutQuart((float)this.wireExtensionTicks / 70f);
			if (this.currentlyChargingMech == null)
			{
				num = 1f - num;
			}
			num = Mathf.Max(num, 0.32f);
			Vector3 b = Vector3.Lerp(drawLoc, this.InteractionCell.ToVector3Shifted(), num);
			b.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
			a.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
			GenDraw.DrawLineBetween(a, b, this.WireMaterial, 1f);
		}

		// Token: 0x0600AE25 RID: 44581 RVA: 0x003D6100 File Offset: 0x003D4300
		private float EaseInOutQuart(float val)
		{
			if ((double)val >= 0.5)
			{
				return 1f - Mathf.Pow(-2f * val + 2f, 4f) / 2f;
			}
			return 8f * val * val * val * val;
		}

		// Token: 0x0600AE26 RID: 44582 RVA: 0x003D6140 File Offset: 0x003D4340
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.Append("WasteLevel".Translate() + ": " + this.WasteProducedPercentFull.ToStringPercent());
			return stringBuilder.ToString();
		}

		// Token: 0x0600AE27 RID: 44583 RVA: 0x003D619B File Offset: 0x003D439B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Building_MechCharger.<SpecialDisplayStats>d__53 <SpecialDisplayStats>d__ = new Building_MechCharger.<SpecialDisplayStats>d__53(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600AE28 RID: 44584 RVA: 0x003D61AC File Offset: 0x003D43AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.currentlyChargingMech, "currentlyChargingMech", false);
			Scribe_Values.Look<float>(ref this.wasteProduced, "wasteProduced", 0f, false);
			Scribe_Values.Look<int>(ref this.wireExtensionTicks, "wireExtensionTicks", 0, false);
		}

		// Token: 0x040064C4 RID: 25796
		private Pawn currentlyChargingMech;

		// Token: 0x040064C5 RID: 25797
		private float wasteProduced;

		// Token: 0x040064C6 RID: 25798
		private int wireExtensionTicks = 70;

		// Token: 0x040064C7 RID: 25799
		private CompWasteProducer wasteProducer;

		// Token: 0x040064C8 RID: 25800
		private CompThingContainer container;

		// Token: 0x040064C9 RID: 25801
		private Sustainer sustainerCharging;

		// Token: 0x040064CA RID: 25802
		private Mote moteCharging;

		// Token: 0x040064CB RID: 25803
		private Mote moteCablePulse;

		// Token: 0x040064CC RID: 25804
		public const float ChargePerDay = 50f;

		// Token: 0x040064CD RID: 25805
		private const float ChargePerTick = 0.00083333335f;

		// Token: 0x040064CE RID: 25806
		private static readonly Material WasteBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f), false);

		// Token: 0x040064CF RID: 25807
		private static readonly Material WasteBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f, 1f), false);

		// Token: 0x040064D0 RID: 25808
		private const int TicksToExtendWire = 70;

		// Token: 0x040064D1 RID: 25809
		private const float MinWireExtension = 0.32f;

		// Token: 0x040064D2 RID: 25810
		private Material wireMaterial;
	}
}
