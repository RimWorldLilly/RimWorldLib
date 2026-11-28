using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001B3A RID: 6970
	[StaticConstructorOnStartup]
	public class Plant : ThingWithComps
	{
		// Token: 0x1700204B RID: 8267
		// (get) Token: 0x0600B2DB RID: 45787 RVA: 0x003EF79F File Offset: 0x003ED99F
		// (set) Token: 0x0600B2DC RID: 45788 RVA: 0x003EF7A7 File Offset: 0x003ED9A7
		public virtual float Growth
		{
			get
			{
				return this.growthInt;
			}
			set
			{
				this.growthInt = Mathf.Clamp01(value);
				this.cachedLabelMouseover = null;
			}
		}

		// Token: 0x1700204C RID: 8268
		// (get) Token: 0x0600B2DD RID: 45789 RVA: 0x003EF7BC File Offset: 0x003ED9BC
		// (set) Token: 0x0600B2DE RID: 45790 RVA: 0x003EF7C4 File Offset: 0x003ED9C4
		public virtual int Age
		{
			get
			{
				return this.ageInt;
			}
			set
			{
				this.ageInt = value;
				this.cachedLabelMouseover = null;
			}
		}

		// Token: 0x1700204D RID: 8269
		// (get) Token: 0x0600B2DF RID: 45791 RVA: 0x003EF7D4 File Offset: 0x003ED9D4
		public virtual bool HarvestableNow
		{
			get
			{
				return this.def.plant.Harvestable && this.growthInt > this.def.plant.harvestMinGrowth;
			}
		}

		// Token: 0x1700204E RID: 8270
		// (get) Token: 0x0600B2E0 RID: 45792 RVA: 0x003EF804 File Offset: 0x003EDA04
		public bool HarvestableSoon
		{
			get
			{
				if (this.HarvestableNow)
				{
					return true;
				}
				if (!this.def.plant.Harvestable)
				{
					return false;
				}
				float num = Mathf.Max(1f - this.Growth, 0f) * this.def.plant.growDays;
				float num2 = Mathf.Max(1f - this.def.plant.harvestMinGrowth, 0f) * this.def.plant.growDays;
				return (num <= 10f || num2 <= 1f) && this.GrowthRateFactor_Fertility > 0f && this.GrowthRateFactor_Temperature > 0f;
			}
		}

		// Token: 0x1700204F RID: 8271
		// (get) Token: 0x0600B2E1 RID: 45793 RVA: 0x003EF8B4 File Offset: 0x003EDAB4
		public virtual bool BlightableNow
		{
			get
			{
				return !this.Blighted && this.def.plant.Blightable && this.sown && this.LifeStage != PlantLifeStage.Sowing && !base.Map.wildPlantSpawner.AllWildPlants.Contains(this.def);
			}
		}

		// Token: 0x17002050 RID: 8272
		// (get) Token: 0x0600B2E2 RID: 45794 RVA: 0x003EF90B File Offset: 0x003EDB0B
		public Blight Blight
		{
			get
			{
				if (!base.Spawned || !this.def.plant.Blightable)
				{
					return null;
				}
				return base.Position.GetFirstBlight(base.Map);
			}
		}

		// Token: 0x17002051 RID: 8273
		// (get) Token: 0x0600B2E3 RID: 45795 RVA: 0x003EF93A File Offset: 0x003EDB3A
		public bool Blighted
		{
			get
			{
				return this.Blight != null;
			}
		}

		// Token: 0x17002052 RID: 8274
		// (get) Token: 0x0600B2E4 RID: 45796 RVA: 0x003EF948 File Offset: 0x003EDB48
		public override bool IngestibleNow
		{
			get
			{
				return base.IngestibleNow && (this.def.plant.IsTree || (this.growthInt >= this.def.plant.harvestMinGrowth && this.growthInt >= 0.1f && !this.LeaflessNow && (!base.Spawned || Math.Max(base.Position.GetSnowDepth(base.Map), base.Position.GetSandDepth(base.Map)) <= this.def.hideAtSnowOrSandDepth)));
			}
		}

		// Token: 0x17002053 RID: 8275
		// (get) Token: 0x0600B2E5 RID: 45797 RVA: 0x003EF9E8 File Offset: 0x003EDBE8
		public virtual float CurrentDyingDamagePerTick
		{
			get
			{
				if (!base.Spawned)
				{
					return 0f;
				}
				float num = 0f;
				if (this.def.plant.LimitedLifespan && this.ageInt > this.def.plant.LifespanTicks)
				{
					num = Mathf.Max(num, 0.005f);
				}
				if (!this.def.plant.diesToLight && this.def.plant.dieIfNoSunlight && this.unlitTicks > 450000)
				{
					num = Mathf.Max(num, 0.005f);
				}
				if (this.DyingBecauseExposedToLight)
				{
					float lerpPct = base.Map.glowGrid.GroundGlowAt(base.Position, true, false);
					num = Mathf.Max(num, Plant.DyingDamagePerTickBecauseExposedToLight.LerpThroughRange(lerpPct));
				}
				if (this.DyingBecauseExposedToVacuum)
				{
					num = Mathf.Max(num, 1f * base.Position.GetVacuum(base.Map));
				}
				if (this.DyingFromPollution || this.DyingFromNoPollution)
				{
					num = Mathf.Max(num, Plant.PollutionDamagePerTickRange.RandomInRangeSeeded(base.Position.GetHashCode()));
				}
				if (this.DyingBecauseOfTerrainTags)
				{
					num = Mathf.Max(num, 0.005f);
				}
				return num;
			}
		}

		// Token: 0x17002054 RID: 8276
		// (get) Token: 0x0600B2E6 RID: 45798 RVA: 0x003EFB25 File Offset: 0x003EDD25
		public virtual bool DyingBecauseExposedToLight
		{
			get
			{
				return this.def.plant.diesToLight && base.Spawned && base.Map.glowGrid.GroundGlowAt(base.Position, true, false) > 0f;
			}
		}

		// Token: 0x17002055 RID: 8277
		// (get) Token: 0x0600B2E7 RID: 45799 RVA: 0x003EFB62 File Offset: 0x003EDD62
		public virtual bool DyingBecauseExposedToVacuum
		{
			get
			{
				return !this.def.plant.vacuumResistant && base.Spawned && base.Position.GetVacuum(base.Map) >= 0.5f;
			}
		}

		// Token: 0x17002056 RID: 8278
		// (get) Token: 0x0600B2E8 RID: 45800 RVA: 0x003EFB9C File Offset: 0x003EDD9C
		public virtual bool DyingBecauseOfTerrainTags
		{
			get
			{
				return this.def.plant.WildTerrainTags.Count > 0 && !this.def.plant.WildTerrainTags.Overlaps(base.Position.GetTerrain(base.Map).tags.OrElseEmptyEnumerable<string>());
			}
		}

		// Token: 0x17002057 RID: 8279
		// (get) Token: 0x0600B2E9 RID: 45801 RVA: 0x003EFBF6 File Offset: 0x003EDDF6
		public bool Dying
		{
			get
			{
				return this.CurrentDyingDamagePerTick > 0f;
			}
		}

		// Token: 0x17002058 RID: 8280
		// (get) Token: 0x0600B2EA RID: 45802 RVA: 0x003EFC05 File Offset: 0x003EDE05
		protected virtual bool Resting
		{
			get
			{
				return GenLocalDate.DayPercent(this) < 0.25f || GenLocalDate.DayPercent(this) > 0.8f;
			}
		}

		// Token: 0x17002059 RID: 8281
		// (get) Token: 0x0600B2EB RID: 45803 RVA: 0x003EFC24 File Offset: 0x003EDE24
		public virtual float GrowthRate
		{
			get
			{
				if (this.Blighted)
				{
					return 0f;
				}
				if (base.Spawned && !PlantUtility.GrowthSeasonNow(base.Position, base.Map, this.def))
				{
					return 0f;
				}
				return this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Temperature * this.GrowthRateFactor_Light * this.GrowthRateFactor_NoxiousHaze * this.GrowthRateFactor_Drought;
			}
		}

		// Token: 0x1700205A RID: 8282
		// (get) Token: 0x0600B2EC RID: 45804 RVA: 0x003EFC88 File Offset: 0x003EDE88
		public virtual string GrowthRateCalcDesc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.GrowthRateFactor_Fertility != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("FertilityLower".Translate()) + ": " + this.GrowthRateFactor_Fertility.ToStringPercent());
				}
				if (this.GrowthRateFactor_Temperature != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("TemperatureLower".Translate()) + ": " + this.GrowthRateFactor_Temperature.ToStringPercent());
				}
				if (this.GrowthRateFactor_Light != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("LightLower".Translate()) + ": " + this.GrowthRateFactor_Light.ToStringPercent());
				}
				if (ModsConfig.BiotechActive && base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.NoxiousHaze) && this.GrowthRateFactor_NoxiousHaze != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate(GameConditionDefOf.NoxiousHaze.label) + ": " + this.GrowthRateFactor_NoxiousHaze.ToStringPercent());
				}
				if (ModsConfig.OdysseyActive && base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Drought) && this.GrowthRateFactor_Drought != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate(GameConditionDefOf.Drought.label) + ": " + this.GrowthRateFactor_Drought.ToStringPercent());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700205B RID: 8283
		// (get) Token: 0x0600B2ED RID: 45805 RVA: 0x003EFE5C File Offset: 0x003EE05C
		protected float GrowthPerTick
		{
			get
			{
				if (this.LifeStage != PlantLifeStage.Growing || this.Resting)
				{
					return 0f;
				}
				if (!this.def.plant.vacuumResistant && base.Position.GetVacuum(base.Map) >= 0.5f)
				{
					return 0f;
				}
				return 1f / (60000f * this.def.plant.growDays) * this.GrowthRate;
			}
		}

		// Token: 0x1700205C RID: 8284
		// (get) Token: 0x0600B2EE RID: 45806 RVA: 0x003EFED3 File Offset: 0x003EE0D3
		public float GrowthRateFactor_Fertility
		{
			get
			{
				return PlantUtility.GrowthRateFactorFor_Fertility(this.def, base.Map.fertilityGrid.FertilityAt(base.Position));
			}
		}

		// Token: 0x1700205D RID: 8285
		// (get) Token: 0x0600B2EF RID: 45807 RVA: 0x003EFEF8 File Offset: 0x003EE0F8
		public float GrowthRateFactor_Light
		{
			get
			{
				float glow = base.Map.glowGrid.GroundGlowAt(base.Position, false, false);
				return PlantUtility.GrowthRateFactorFor_Light(this.def, glow);
			}
		}

		// Token: 0x1700205E RID: 8286
		// (get) Token: 0x0600B2F0 RID: 45808 RVA: 0x003EFF2C File Offset: 0x003EE12C
		public float GrowthRateFactor_Temperature
		{
			get
			{
				float cellTemp;
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out cellTemp))
				{
					return 1f;
				}
				return PlantUtility.GrowthRateFactorFor_Temperature(this.def, cellTemp);
			}
		}

		// Token: 0x1700205F RID: 8287
		// (get) Token: 0x0600B2F1 RID: 45809 RVA: 0x003EFF60 File Offset: 0x003EE160
		public float GrowthRateFactor_NoxiousHaze
		{
			get
			{
				if (NoxiousHazeUtility.IsExposedToNoxiousHaze(this))
				{
					return 0.5f;
				}
				return 1f;
			}
		}

		// Token: 0x17002060 RID: 8288
		// (get) Token: 0x0600B2F2 RID: 45810 RVA: 0x003EFF78 File Offset: 0x003EE178
		public float GrowthRateFactor_Drought
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return 1f;
				}
				if (this.CurrentlyCultivated())
				{
					Building edifice = base.Position.GetEdifice(base.Map);
					if (edifice != null && edifice.def.building.SupportsPlants)
					{
						return 1f;
					}
				}
				if (this.def.plant.cavePlant)
				{
					return 1f;
				}
				if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Drought))
				{
					return 0.3f;
				}
				return 1f;
			}
		}

		// Token: 0x17002061 RID: 8289
		// (get) Token: 0x0600B2F3 RID: 45811 RVA: 0x003F0004 File Offset: 0x003EE204
		protected int TicksUntilFullyGrown
		{
			get
			{
				if (this.growthInt > 0.9999f)
				{
					return 0;
				}
				float growthPerTick = this.GrowthPerTick;
				if (growthPerTick == 0f)
				{
					return int.MaxValue;
				}
				return (int)((1f - this.growthInt) / growthPerTick);
			}
		}

		// Token: 0x17002062 RID: 8290
		// (get) Token: 0x0600B2F4 RID: 45812 RVA: 0x003F0044 File Offset: 0x003EE244
		protected string GrowthPercentString
		{
			get
			{
				return (Mathf.Floor((this.growthInt + 0.0001f) * 100f) / 100f).ToStringPercent();
			}
		}

		// Token: 0x17002063 RID: 8291
		// (get) Token: 0x0600B2F5 RID: 45813 RVA: 0x003F0068 File Offset: 0x003EE268
		public override string LabelMouseover
		{
			get
			{
				if (this.cachedLabelMouseover == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.def.LabelCap);
					stringBuilder.Append(" (" + "PercentGrowth".Translate(this.GrowthPercentString));
					if (this.Dying)
					{
						stringBuilder.Append(", " + "DyingLower".Translate());
					}
					stringBuilder.Append(")");
					this.cachedLabelMouseover = stringBuilder.ToString();
				}
				return this.cachedLabelMouseover;
			}
		}

		// Token: 0x17002064 RID: 8292
		// (get) Token: 0x0600B2F6 RID: 45814 RVA: 0x003F010E File Offset: 0x003EE30E
		protected virtual bool HasEnoughLightToGrow
		{
			get
			{
				return this.GrowthRateFactor_Light > 0.001f;
			}
		}

		// Token: 0x17002065 RID: 8293
		// (get) Token: 0x0600B2F7 RID: 45815 RVA: 0x003F011D File Offset: 0x003EE31D
		public virtual PlantLifeStage LifeStage
		{
			get
			{
				if (this.growthInt < 0.0001f)
				{
					return PlantLifeStage.Sowing;
				}
				if (this.growthInt > 0.999f)
				{
					return PlantLifeStage.Mature;
				}
				return PlantLifeStage.Growing;
			}
		}

		// Token: 0x17002066 RID: 8294
		// (get) Token: 0x0600B2F8 RID: 45816 RVA: 0x003F0140 File Offset: 0x003EE340
		public override Graphic Graphic
		{
			get
			{
				if (this.LifeStage == PlantLifeStage.Sowing)
				{
					return Plant.GraphicSowing;
				}
				if (this.def.plant.pollutedGraphic != null && base.PositionHeld.IsPolluted(base.MapHeld))
				{
					return this.def.plant.pollutedGraphic;
				}
				if (this.def.plant.leaflessImmatureGraphic != null && this.LeaflessNow && !this.HarvestableNow)
				{
					return this.def.plant.leaflessImmatureGraphic;
				}
				if (this.def.plant.leaflessGraphic != null && this.LeaflessNow && (!this.sown || !this.HarvestableNow))
				{
					return this.def.plant.leaflessGraphic;
				}
				if (this.def.plant.immatureGraphic != null && !this.HarvestableNow)
				{
					return this.def.plant.immatureGraphic;
				}
				return base.Graphic;
			}
		}

		// Token: 0x17002067 RID: 8295
		// (get) Token: 0x0600B2F9 RID: 45817 RVA: 0x003F0230 File Offset: 0x003EE430
		public Graphic SnowOverlayGraphic
		{
			get
			{
				if (this.def.plant.immatureSnowOverlayGraphic != null && !this.HarvestableNow)
				{
					return this.def.plant.immatureSnowOverlayGraphic;
				}
				if (this.def.plant.leaflessSnowOverlayGraphic != null && this.LeaflessNow && (!this.sown || !this.HarvestableNow))
				{
					return this.def.plant.leaflessSnowOverlayGraphic;
				}
				return this.def.plant.snowOverlayGraphic;
			}
		}

		// Token: 0x17002068 RID: 8296
		// (get) Token: 0x0600B2FA RID: 45818 RVA: 0x003F02B3 File Offset: 0x003EE4B3
		public bool LeaflessNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.madeLeaflessTick < 60000;
			}
		}

		// Token: 0x17002069 RID: 8297
		// (get) Token: 0x0600B2FB RID: 45819 RVA: 0x003F02D0 File Offset: 0x003EE4D0
		protected virtual float LeaflessTemperatureThresh
		{
			get
			{
				return this.def.plant.minGrowthTemperature + Rand.RangeSeeded(-18f, -10f, this.thingIDNumber ^ 838051265);
			}
		}

		// Token: 0x1700206A RID: 8298
		// (get) Token: 0x0600B2FC RID: 45820 RVA: 0x003F0300 File Offset: 0x003EE500
		public bool IsCrop
		{
			get
			{
				if (!this.def.plant.Sowable)
				{
					return false;
				}
				if (!base.Spawned)
				{
					Log.Warning("Can't determine if crop when unspawned.");
					return false;
				}
				return this.def == WorkGiver_Grower.CalculateWantedPlantDef(base.Position, base.Map);
			}
		}

		// Token: 0x1700206B RID: 8299
		// (get) Token: 0x0600B2FD RID: 45821 RVA: 0x003F034E File Offset: 0x003EE54E
		public bool DyingFromPollution
		{
			get
			{
				return this.def.plant.RequiresNoPollution && base.Position.IsPolluted(base.Map);
			}
		}

		// Token: 0x1700206C RID: 8300
		// (get) Token: 0x0600B2FE RID: 45822 RVA: 0x003F0375 File Offset: 0x003EE575
		public bool DyingFromNoPollution
		{
			get
			{
				return this.def.plant.RequiresPollution && !base.Position.IsPolluted(base.Map);
			}
		}

		// Token: 0x0600B2FF RID: 45823 RVA: 0x003F039F File Offset: 0x003EE59F
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing && !respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.CheckMakeLeafless();
			}
		}

		// Token: 0x0600B300 RID: 45824 RVA: 0x003F03C4 File Offset: 0x003EE5C4
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Blight firstBlight = base.Position.GetFirstBlight(base.Map);
			base.DeSpawn(mode);
			if (mode != DestroyMode.WillReplace && firstBlight != null)
			{
				firstBlight.Notify_PlantDeSpawned();
			}
		}

		// Token: 0x0600B301 RID: 45825 RVA: 0x003F03F8 File Offset: 0x003EE5F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.growthInt, "growth", 0f, false);
			Scribe_Values.Look<int>(ref this.ageInt, "age", 0, false);
			Scribe_Values.Look<int>(ref this.unlitTicks, "unlitTicks", 0, false);
			Scribe_Values.Look<int>(ref this.madeLeaflessTick, "madeLeaflessTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.sown, "sown", false, false);
		}

		// Token: 0x0600B302 RID: 45826 RVA: 0x003F046D File Offset: 0x003EE66D
		public override void PostMapInit()
		{
			this.CheckMakeLeafless();
		}

		// Token: 0x0600B303 RID: 45827 RVA: 0x003F0478 File Offset: 0x003EE678
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			float statValue = this.GetStatValue(StatDefOf.Nutrition, true, -1);
			float num = this.growthInt * statValue;
			nutritionIngested = Mathf.Min(nutritionWanted, num);
			if (nutritionIngested >= num)
			{
				numTaken = 1;
				return;
			}
			numTaken = 0;
			this.growthInt -= nutritionIngested / statValue;
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600B304 RID: 45828 RVA: 0x003F04F0 File Offset: 0x003EE6F0
		public virtual void PlantCollected(Pawn by, PlantDestructionMode plantDestructionMode)
		{
			if (this.def.plant.HarvestDestroys)
			{
				if (this.def.plant.IsTree && this.def.plant.treeLoversCareIfChopped)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.CutTree, by.Named(HistoryEventArgsNames.Doer)), true);
					base.Map.treeDestructionTracker.Notify_TreeCut(by);
				}
				if (this.Blighted && plantDestructionMode == PlantDestructionMode.Cut)
				{
					base.Map.floodFiller.FloodFill(base.Position, delegate(IntVec3 cell)
					{
						if (this.Position == cell)
						{
							return true;
						}
						Plant plant = cell.GetPlant(this.Map);
						return plant != null && !this.Map.designationManager.HasMapDesignationOn(plant) && plant.Blighted;
					}, delegate(IntVec3 cell)
					{
						if (this.Position == cell)
						{
							return;
						}
						Plant plant = cell.GetPlant(this.Map);
						this.Map.designationManager.AddDesignation(new Designation(plant, DesignationDefOf.CutPlant, null));
						Pawn_JobTracker jobs2 = by.jobs;
						if (jobs2 == null)
						{
							return;
						}
						Job curJob2 = jobs2.curJob;
						if (curJob2 == null)
						{
							return;
						}
						List<LocalTargetInfo> targetQueueA2 = curJob2.targetQueueA;
						if (targetQueueA2 == null)
						{
							return;
						}
						targetQueueA2.Add(plant);
					}, 100, false, null);
				}
				Thing thing = this.TrySpawnStump(plantDestructionMode);
				Map map = base.Map;
				this.Destroy(DestroyMode.KillFinalizeLeavingsOnly);
				if (thing != null && plantDestructionMode == PlantDestructionMode.Cut && by.Faction == Faction.OfPlayer)
				{
					map.designationManager.AddDesignation(new Designation(thing, DesignationDefOf.CutPlant, null));
					Pawn_JobTracker jobs = by.jobs;
					if (jobs == null)
					{
						return;
					}
					Job curJob = jobs.curJob;
					if (curJob == null)
					{
						return;
					}
					List<LocalTargetInfo> targetQueueA = curJob.targetQueueA;
					if (targetQueueA == null)
					{
						return;
					}
					targetQueueA.Add(thing);
					return;
				}
			}
			else
			{
				this.growthInt = this.def.plant.harvestAfterGrowth;
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600B305 RID: 45829 RVA: 0x003F0680 File Offset: 0x003EE880
		public Thing TrySpawnStump(PlantDestructionMode treeDestructionMode)
		{
			if (!base.Spawned || this.LifeStage == PlantLifeStage.Sowing)
			{
				return null;
			}
			if (!this.HarvestableNow)
			{
				return null;
			}
			ThingDef thingDef = null;
			switch (treeDestructionMode)
			{
			case PlantDestructionMode.Smash:
				thingDef = this.def.plant.smashedThingDef;
				break;
			case PlantDestructionMode.Flame:
				thingDef = this.def.plant.burnedThingDef;
				break;
			case PlantDestructionMode.Chop:
			case PlantDestructionMode.Cut:
				thingDef = this.def.plant.choppedThingDef;
				break;
			}
			if (thingDef != null)
			{
				Thing thing = GenSpawn.Spawn(thingDef, base.Position, base.Map, WipeMode.Vanish);
				DeadPlant deadPlant = thing as DeadPlant;
				if (deadPlant != null)
				{
					deadPlant.Growth = this.Growth;
				}
				if (Find.Selector.IsSelected(this))
				{
					Find.Selector.Select(thing, false, false);
				}
				return thing;
			}
			return null;
		}

		// Token: 0x0600B306 RID: 45830 RVA: 0x003F0744 File Offset: 0x003EE944
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.Spawned && dinfo != null)
			{
				if (dinfo.Value.Def == DamageDefOf.Flame)
				{
					this.TrySpawnStump(PlantDestructionMode.Flame);
				}
				else
				{
					this.TrySpawnStump(PlantDestructionMode.Smash);
				}
				if (this.def.plant.IsTree && this.def.plant.treeLoversCareIfChopped)
				{
					base.Map.treeDestructionTracker.Notify_TreeDestroyed(dinfo.Value);
				}
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x0600B307 RID: 45831 RVA: 0x003F07D0 File Offset: 0x003EE9D0
		protected virtual void CheckMakeLeafless()
		{
			if (this.DyingFromPollution)
			{
				this.MakeLeafless(Plant.LeaflessCause.Pollution, true);
				return;
			}
			if (this.DyingFromNoPollution)
			{
				this.MakeLeafless(Plant.LeaflessCause.NoPollution, true);
				return;
			}
			Room room = this.GetRoom(RegionType.Set_All);
			if (room != null && room.UsesOutdoorTemperature && base.AmbientTemperature < this.LeaflessTemperatureThresh)
			{
				this.MakeLeafless(Plant.LeaflessCause.Cold, true);
			}
		}

		// Token: 0x0600B308 RID: 45832 RVA: 0x003F082C File Offset: 0x003EEA2C
		public virtual void MakeLeafless(Plant.LeaflessCause cause, bool sendMessage = true)
		{
			bool flag = !this.LeaflessNow;
			Map map = base.Map;
			if (cause == Plant.LeaflessCause.Poison && this.def.plant.leaflessGraphic == null)
			{
				if (this.IsCrop && sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
				{
					Messages.Message("MessagePlantDiedOfPoison".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			else if (this.def.plant.dieIfLeafless)
			{
				if (this.IsCrop)
				{
					if (cause == Plant.LeaflessCause.Cold)
					{
						if (sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfCold-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfCold".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
					else if (cause == Plant.LeaflessCause.Poison)
					{
						if (sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfPoison".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
					else if (cause == Plant.LeaflessCause.Pollution)
					{
						if (sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPollution-" + this.def.defName, 240f))
						{
							Messages.Message("MessagePlantDiedOfPollution".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
						}
					}
					else if (cause == Plant.LeaflessCause.NoPollution && sendMessage && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfNoPollution-" + this.def.defName, 240f))
					{
						Messages.Message("MessagePlantDiedOfNoPollution".Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
					}
				}
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 99999f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			else
			{
				this.madeLeaflessTick = Find.TickManager.TicksGame;
			}
			if (flag)
			{
				map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600B309 RID: 45833 RVA: 0x003F0B0D File Offset: 0x003EED0D
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.IsHashIntervalTick(2000, delta))
			{
				this.TickLong();
			}
		}

		// Token: 0x0600B30A RID: 45834 RVA: 0x003F0B2C File Offset: 0x003EED2C
		public override void TickLong()
		{
			this.CheckMakeLeafless();
			if (base.Destroyed)
			{
				return;
			}
			base.TickLong();
			if (PlantUtility.GrowthSeasonNow(base.Position, base.Map, this.def))
			{
				float num = this.growthInt;
				bool flag = this.LifeStage == PlantLifeStage.Mature;
				this.growthInt += this.GrowthPerTick * 2000f;
				if (this.growthInt > 1f)
				{
					this.growthInt = 1f;
				}
				if (((!flag && this.LifeStage == PlantLifeStage.Mature) || (int)(num * 10f) != (int)(this.growthInt * 10f)) && this.CurrentlyCultivated())
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
				}
			}
			if (!this.HasEnoughLightToGrow)
			{
				this.unlitTicks += 2000;
			}
			else
			{
				this.unlitTicks = 0;
			}
			this.ageInt += 2000;
			if (this.Dying)
			{
				Map map = base.Map;
				bool isCrop = this.IsCrop;
				bool harvestableNow = this.HarvestableNow;
				bool dyingBecauseExposedToLight = this.DyingBecauseExposedToLight;
				bool dyingBecauseExposedToVacuum = this.DyingBecauseExposedToVacuum;
				int num2 = Mathf.CeilToInt(this.CurrentDyingDamagePerTick * 2000f);
				base.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num2, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				if (base.Destroyed && !this.def.plant.skipDeteriorationMessage)
				{
					if (isCrop && this.def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + this.def.defName, 240f))
					{
						string key;
						if (harvestableNow)
						{
							if (ModsConfig.BiotechActive && base.Position.IsPolluted(map))
							{
								key = "MessagePlantDiedOfRot_PollutedTerrain";
							}
							else
							{
								key = "MessagePlantDiedOfRot_LeftUnharvested";
							}
						}
						else if (dyingBecauseExposedToLight)
						{
							key = "MessagePlantDiedOfRot_ExposedToLight";
						}
						else if (dyingBecauseExposedToVacuum)
						{
							key = "MessagePlantDiedOfRot_ExposedToVacuum";
						}
						else
						{
							key = "MessagePlantDiedOfRot";
						}
						Messages.Message(key.Translate(this.GetCustomLabelNoCount(false)), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
					}
					return;
				}
			}
			this.cachedLabelMouseover = null;
			if (this.def.plant.dropLeaves)
			{
				MoteLeaf moteLeaf = MoteMaker.MakeStaticMote(Vector3.zero, base.Map, ThingDefOf.Mote_Leaf, 1f, false, 0f) as MoteLeaf;
				if (moteLeaf != null)
				{
					float num3 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
					float treeHeight = this.def.graphicData.drawSize.x * num3;
					Vector3 vector = Rand.InsideUnitCircleVec3 * Plant.LeafSpawnRadius;
					moteLeaf.Initialize(base.Position.ToVector3Shifted() + Vector3.up * Rand.Range(Plant.LeafSpawnYMin, Plant.LeafSpawnYMax) + vector + Vector3.forward * this.def.graphicData.shadowData.offset.z, Rand.Value * 2000.TicksToSeconds(), vector.z > 0f, treeHeight);
				}
			}
		}

		// Token: 0x0600B30B RID: 45835 RVA: 0x003F0E8C File Offset: 0x003EF08C
		protected virtual bool CurrentlyCultivated()
		{
			if (!this.def.plant.Sowable)
			{
				return false;
			}
			if (!base.Spawned)
			{
				return false;
			}
			Zone zone = base.Map.zoneManager.ZoneAt(base.Position);
			if (zone != null && zone is Zone_Growing)
			{
				return true;
			}
			Building edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.building.SupportsPlants;
		}

		// Token: 0x0600B30C RID: 45836 RVA: 0x003F0F08 File Offset: 0x003EF108
		public bool DeliberatelyCultivated()
		{
			if (!this.def.plant.Sowable)
			{
				return false;
			}
			if (!base.Spawned)
			{
				return false;
			}
			Zone_Growing zone_Growing = base.Map.zoneManager.ZoneAt(base.Position) as Zone_Growing;
			if (zone_Growing != null && zone_Growing.GetPlantDefToGrow() == this.def)
			{
				return true;
			}
			Building edifice = base.Position.GetEdifice(base.Map);
			return edifice != null && edifice.def.building.SupportsPlants;
		}

		// Token: 0x0600B30D RID: 45837 RVA: 0x003F0F8C File Offset: 0x003EF18C
		public virtual bool CanYieldNow()
		{
			return this.HarvestableNow && this.def.plant.harvestYield > 0f && !this.Blighted;
		}

		// Token: 0x0600B30E RID: 45838 RVA: 0x003F0FBC File Offset: 0x003EF1BC
		public virtual int YieldNow()
		{
			if (!this.CanYieldNow())
			{
				return 0;
			}
			float num = this.def.plant.harvestYield;
			float num2 = Mathf.InverseLerp(this.def.plant.harvestMinGrowth, 1f, this.growthInt);
			num2 = 0.5f + num2 * 0.5f;
			num *= num2;
			num *= Mathf.Lerp(0.5f, 1f, (float)this.HitPoints / (float)base.MaxHitPoints);
			if (this.def.plant.harvestYieldAffectedByDifficulty)
			{
				num *= Find.Storyteller.difficulty.cropYieldFactor;
			}
			return GenMath.RoundRandom(num);
		}

		// Token: 0x0600B30F RID: 45839 RVA: 0x003F1064 File Offset: 0x003EF264
		public override void Print(SectionLayer layer)
		{
			Vector3 a = this.TrueCenter();
			bool flag = base.Position.GetSnowDepth(base.Map) > 0.8f;
			Rand.PushState();
			Rand.Seed = base.Position.GetHashCode();
			int num = Mathf.CeilToInt(this.growthInt * (float)this.def.plant.maxMeshCount);
			if (num < 1)
			{
				num = 1;
			}
			float num2 = this.def.plant.visualSizeRange.LerpThroughRange(this.growthInt);
			float num3 = this.def.graphicData.drawSize.x * num2;
			int num4 = 0;
			int[] positionIndices = PlantPosIndices.GetPositionIndices(this);
			bool flag2 = false;
			int i = 0;
			while (i < positionIndices.Length)
			{
				int num5 = positionIndices[i];
				Vector3 vector;
				if (this.def.plant.maxMeshCount != 1)
				{
					int num6 = 1;
					int maxMeshCount = this.def.plant.maxMeshCount;
					if (maxMeshCount <= 4)
					{
						if (maxMeshCount != 1)
						{
							if (maxMeshCount != 4)
							{
								goto IL_16A;
							}
							num6 = 2;
						}
						else
						{
							num6 = 1;
						}
					}
					else if (maxMeshCount != 9)
					{
						if (maxMeshCount != 16)
						{
							if (maxMeshCount != 25)
							{
								goto IL_16A;
							}
							num6 = 5;
						}
						else
						{
							num6 = 4;
						}
					}
					else
					{
						num6 = 3;
					}
					IL_18B:
					float num7 = 1f / (float)num6;
					vector = base.Position.ToVector3();
					vector.y = this.def.Altitude;
					vector.x += 0.5f * num7;
					vector.z += 0.5f * num7;
					int num8 = num5 / num6;
					int num9 = num5 % num6;
					vector.x += (float)num8 * num7;
					vector.z += (float)num9 * num7;
					float max = num7 * 0.3f;
					vector += Gen.RandomHorizontalVector(max);
					goto IL_229;
					IL_16A:
					ThingDef def = this.def;
					Log.Error(((def != null) ? def.ToString() : null) + " must have plant.MaxMeshCount that is a perfect square.");
					goto IL_18B;
				}
				vector = a + Gen.RandomHorizontalVector(0.05f);
				float num10 = (float)base.Position.z;
				if (vector.z - num2 / 2f < num10)
				{
					vector.z = num10 + num2 / 2f;
					flag2 = true;
				}
				IL_229:
				bool @bool = Rand.Bool;
				if (!flag)
				{
					goto IL_23F;
				}
				Graphic snowOverlayGraphic = this.SnowOverlayGraphic;
				if (snowOverlayGraphic == null)
				{
					goto IL_23F;
				}
				Material material = snowOverlayGraphic.MatSingleFor(this);
				IL_24A:
				Material material2 = material;
				Material mat = this.Graphic.MatSingleFor(this);
				Graphic_Random graphic_Random = this.Graphic as Graphic_Random;
				if (graphic_Random != null)
				{
					int subGraphicsCount = graphic_Random.SubGraphicsCount;
					int index = Rand.Range(0, subGraphicsCount);
					mat = graphic_Random.SubGraphicAtIndex(index).MatSingle;
					if (flag)
					{
						Graphic_Random graphic_Random2 = this.SnowOverlayGraphic as Graphic_Random;
						if (graphic_Random2 != null)
						{
							material2 = graphic_Random2.SubGraphicAtIndex(index).MatSingle;
						}
					}
				}
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, this.def.category.ToAtlasGroup(), @bool, false, out mat, out uvs, out color);
				PlantUtility.SetWindExposureColors(Plant.workingColors, this);
				Vector2 size = new Vector2(num3, num3);
				Printer_Plane.PrintPlane(layer, vector, size, mat, 0f, @bool, uvs, Plant.workingColors, 0.1f, (float)(this.HashOffset() % 1024));
				if (flag && material2 != null)
				{
					Graphic.TryGetTextureAtlasReplacementInfo(material2, this.def.category.ToAtlasGroup(), @bool, false, out material2, out uvs, out color);
					Printer_Plane.PrintPlane(layer, vector.WithYOffset(0.0003658537f), size, material2, 0f, @bool, uvs, Plant.workingColors, 0.1f, (float)(this.HashOffset() % 1024));
				}
				num4++;
				if (num4 < num)
				{
					i++;
					continue;
				}
				break;
				IL_23F:
				material = null;
				goto IL_24A;
			}
			if (this.def.graphicData.shadowData != null)
			{
				Vector3 center = a + this.def.graphicData.shadowData.offset * num2;
				if (flag2)
				{
					center.z = base.Position.ToVector3Shifted().z + this.def.graphicData.shadowData.offset.z;
				}
				center.y -= 0.03658537f;
				Vector3 volume = this.def.graphicData.shadowData.volume * num2;
				Printer_Shadow.PrintShadow(layer, center, volume, Rot4.North);
			}
			Rand.PopState();
		}

		// Token: 0x0600B310 RID: 45840 RVA: 0x003F14BC File Offset: 0x003EF6BC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.def.plant.showGrowthInInspectPane)
			{
				if (this.LifeStage == PlantLifeStage.Growing)
				{
					stringBuilder.AppendLine("PercentGrowth".Translate(this.GrowthPercentString));
					stringBuilder.Append("GrowthRate".Translate() + ": " + this.GrowthRate.ToStringPercent());
					if (!this.Blighted)
					{
						string[] array = ArrayPool<string>.Shared.Rent(4);
						int count = 0;
						if (this.Resting)
						{
							Plant.<GetInspectString>g__AddCondition|117_0(array, ref count, "PlantResting".Translate());
						}
						if (!this.HasEnoughLightToGrow)
						{
							Plant.<GetInspectString>g__AddCondition|117_0(array, ref count, "PlantNeedsLightLevel".Translate() + " " + this.def.plant.growMinGlow.ToStringPercent());
						}
						float growthRateFactor_Temperature = this.GrowthRateFactor_Temperature;
						if (growthRateFactor_Temperature < 0.99f)
						{
							if (Mathf.Approximately(growthRateFactor_Temperature, 0f) || !PlantUtility.GrowthSeasonNow(base.Position, base.Map, this.def))
							{
								Plant.<GetInspectString>g__AddCondition|117_0(array, ref count, "OutOfIdealTemperatureRangeNotGrowing".Translate());
							}
							else
							{
								Plant.<GetInspectString>g__AddCondition|117_0(array, ref count, "OutOfIdealTemperatureRange".Translate(Mathf.Max(1, Mathf.RoundToInt(growthRateFactor_Temperature * 100f)).ToString()));
							}
						}
						if (this.GrowthRateFactor_Drought < 0.99f)
						{
							Plant.<GetInspectString>g__AddCondition|117_0(array, ref count, GameConditionDefOf.Drought.label);
						}
						string text = string.Join(", ", array, 0, count);
						ArrayPool<string>.Shared.Return(array, false);
						if (!text.NullOrEmpty())
						{
							stringBuilder.Append(" (").Append(text).Append(')');
						}
					}
					stringBuilder.AppendLine();
				}
				else if (this.LifeStage == PlantLifeStage.Mature)
				{
					stringBuilder.AppendLine(this.HarvestableNow ? "ReadyToHarvest".Translate() : "Mature".Translate());
				}
				if (this.DyingBecauseExposedToLight)
				{
					stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
				}
				if (this.DyingBecauseExposedToVacuum)
				{
					stringBuilder.AppendLine("DyingBecauseExposedToVacuum".Translate());
				}
				if (this.DyingBecauseOfTerrainTags)
				{
					stringBuilder.AppendLine("DyingBecauseOfTerrain".Translate());
				}
				if (this.Blighted)
				{
					stringBuilder.AppendLine(string.Format("{0} ({1})", "Blighted".Translate(), this.Blight.Severity.ToStringPercent()));
				}
			}
			string text2 = base.InspectStringPartsFromComps();
			if (!text2.NullOrEmpty())
			{
				stringBuilder.Append(text2);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600B311 RID: 45841 RVA: 0x003F1792 File Offset: 0x003EF992
		public virtual void CropBlighted()
		{
			if (!this.Blighted)
			{
				GenSpawn.Spawn(ThingDefOf.Blight, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600B312 RID: 45842 RVA: 0x003F17B4 File Offset: 0x003EF9B4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Plant.<GetGizmos>d__119 <GetGizmos>d__ = new Plant.<GetGizmos>d__119(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B313 RID: 45843 RVA: 0x003F17C4 File Offset: 0x003EF9C4
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Plant.<SpecialDisplayStats>d__120 <SpecialDisplayStats>d__ = new Plant.<SpecialDisplayStats>d__120(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600B316 RID: 45846 RVA: 0x003F1880 File Offset: 0x003EFA80
		[CompilerGenerated]
		internal static void <GetInspectString>g__AddCondition|117_0(string[] conditions, ref int count, string condition)
		{
			if (count < conditions.Length)
			{
				int num = count;
				count = num + 1;
				conditions[num] = condition;
				return;
			}
			Log.Error("Too many conditions for plant growth inspect string");
		}

		// Token: 0x04006735 RID: 26421
		protected float growthInt = 0.15f;

		// Token: 0x04006736 RID: 26422
		protected int ageInt;

		// Token: 0x04006737 RID: 26423
		protected int unlitTicks;

		// Token: 0x04006738 RID: 26424
		protected int madeLeaflessTick = -99999;

		// Token: 0x04006739 RID: 26425
		public bool sown;

		// Token: 0x0400673A RID: 26426
		private string cachedLabelMouseover;

		// Token: 0x0400673B RID: 26427
		private static Color32[] workingColors = new Color32[4];

		// Token: 0x0400673C RID: 26428
		public const float BaseGrowthPercent = 0.15f;

		// Token: 0x0400673D RID: 26429
		public const float BaseSownGrowthPercent = 0.0001f;

		// Token: 0x0400673E RID: 26430
		public const float MinGrowthForAnimalIngestion = 0.1f;

		// Token: 0x0400673F RID: 26431
		private const float BaseDyingDamagePerTick = 0.005f;

		// Token: 0x04006740 RID: 26432
		private const float GridPosRandomnessFactor = 0.3f;

		// Token: 0x04006741 RID: 26433
		private const int TicksWithoutLightBeforeStartDying = 450000;

		// Token: 0x04006742 RID: 26434
		private const int LeaflessMinRecoveryTicks = 60000;

		// Token: 0x04006743 RID: 26435
		public const float DefaultMinGrowthTemperature = 0f;

		// Token: 0x04006744 RID: 26436
		public const float DefaultMinOptimalGrowthTemperature = 6f;

		// Token: 0x04006745 RID: 26437
		public const float DefaultMaxOptimalGrowthTemperature = 42f;

		// Token: 0x04006746 RID: 26438
		public const float DefaultMaxGrowthTemperature = 58f;

		// Token: 0x04006747 RID: 26439
		private const float MinLeaflessTemperatureOffset = -18f;

		// Token: 0x04006748 RID: 26440
		public const float MaxLeaflessTemperatureOffset = -10f;

		// Token: 0x04006749 RID: 26441
		public const float TopVerticesAltitudeBias = 0.1f;

		// Token: 0x0400674A RID: 26442
		private static Graphic GraphicSowing = GraphicDatabase.Get<Graphic_Single>("Things/Plant/Plant_Sowing", ShaderDatabase.Cutout, Vector2.one, Color.white);

		// Token: 0x0400674B RID: 26443
		private static readonly FloatRange DyingDamagePerTickBecauseExposedToLight = new FloatRange(0.0001f, 0.001f);

		// Token: 0x0400674C RID: 26444
		private static readonly FloatRange PollutionDamagePerTickRange = new FloatRange(1.6666667E-05f, 0.00016666666f);

		// Token: 0x0400674D RID: 26445
		private const float VacuumDamagePerTick = 1f;

		// Token: 0x0400674E RID: 26446
		private static readonly Texture2D CutAllBlightTex = ContentFinder<Texture2D>.Get("UI/Commands/CutAllBlightedPlants", true);

		// Token: 0x0400674F RID: 26447
		[TweakValue("Graphics", -1f, 1f)]
		private static float LeafSpawnRadius = 0.4f;

		// Token: 0x04006750 RID: 26448
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMin = 0.3f;

		// Token: 0x04006751 RID: 26449
		[TweakValue("Graphics", 0f, 2f)]
		private static float LeafSpawnYMax = 1f;

		// Token: 0x0200384D RID: 14413
		public enum LeaflessCause
		{
			// Token: 0x0400E0C7 RID: 57543
			Cold,
			// Token: 0x0400E0C8 RID: 57544
			Poison,
			// Token: 0x0400E0C9 RID: 57545
			Pollution,
			// Token: 0x0400E0CA RID: 57546
			NoPollution
		}
	}
}
