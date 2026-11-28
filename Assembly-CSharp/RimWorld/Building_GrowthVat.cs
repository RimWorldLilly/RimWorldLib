using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AB8 RID: 6840
	[StaticConstructorOnStartup]
	public class Building_GrowthVat : Building_Enterable, IStoreSettingsParent, IThingHolderWithDrawnPawn, IThingHolder
	{
		// Token: 0x17001F11 RID: 7953
		// (get) Token: 0x0600AD88 RID: 44424 RVA: 0x003D287A File Offset: 0x003D0A7A
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F12 RID: 7954
		// (get) Token: 0x0600AD89 RID: 44425 RVA: 0x003D287D File Offset: 0x003D0A7D
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.DrawPos.y + 0.03658537f;
			}
		}

		// Token: 0x17001F13 RID: 7955
		// (get) Token: 0x0600AD8A RID: 44426 RVA: 0x003D2890 File Offset: 0x003D0A90
		public float HeldPawnBodyAngle
		{
			get
			{
				return base.Rotation.AsAngle;
			}
		}

		// Token: 0x17001F14 RID: 7956
		// (get) Token: 0x0600AD8B RID: 44427 RVA: 0x003D28AB File Offset: 0x003D0AAB
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x17001F15 RID: 7957
		// (get) Token: 0x0600AD8C RID: 44428 RVA: 0x003D28AE File Offset: 0x003D0AAE
		public bool PowerOn
		{
			get
			{
				return this.PowerTraderComp.PowerOn;
			}
		}

		// Token: 0x17001F16 RID: 7958
		// (get) Token: 0x0600AD8D RID: 44429 RVA: 0x003D28BB File Offset: 0x003D0ABB
		public override Vector3 PawnDrawOffset
		{
			get
			{
				return CompBiosculpterPod.FloatingOffset((float)Find.TickManager.TicksGame);
			}
		}

		// Token: 0x17001F17 RID: 7959
		// (get) Token: 0x0600AD8E RID: 44430 RVA: 0x003D28CD File Offset: 0x003D0ACD
		private CompPowerTrader PowerTraderComp
		{
			get
			{
				if (this.cachedPowerComp == null)
				{
					this.cachedPowerComp = this.TryGetComp<CompPowerTrader>();
				}
				return this.cachedPowerComp;
			}
		}

		// Token: 0x17001F18 RID: 7960
		// (get) Token: 0x0600AD8F RID: 44431 RVA: 0x003D28E9 File Offset: 0x003D0AE9
		public float BiostarvationDailyOffset
		{
			get
			{
				if (!base.Working)
				{
					return 0f;
				}
				if (!this.PowerOn || this.containedNutrition <= 0f)
				{
					return 0.5f;
				}
				return -0.1f;
			}
		}

		// Token: 0x17001F19 RID: 7961
		// (get) Token: 0x0600AD90 RID: 44432 RVA: 0x003D291C File Offset: 0x003D0B1C
		private float BiostarvationSeverityPercent
		{
			get
			{
				if (this.selectedEmbryo != null)
				{
					return this.embryoStarvation;
				}
				if (this.selectedPawn != null)
				{
					Hediff firstHediffOfDef = this.selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BioStarvation, false);
					if (firstHediffOfDef != null)
					{
						return firstHediffOfDef.Severity / HediffDefOf.BioStarvation.maxSeverity;
					}
				}
				return 0f;
			}
		}

		// Token: 0x17001F1A RID: 7962
		// (get) Token: 0x0600AD91 RID: 44433 RVA: 0x003D2978 File Offset: 0x003D0B78
		public float NutritionConsumedPerDay
		{
			get
			{
				float num = (this.selectedEmbryo != null) ? 6f : 3f;
				if (this.BiostarvationSeverityPercent > 0f)
				{
					float num2 = 1.1f;
					num *= num2;
				}
				return num;
			}
		}

		// Token: 0x17001F1B RID: 7963
		// (get) Token: 0x0600AD92 RID: 44434 RVA: 0x003D29B4 File Offset: 0x003D0BB4
		public float NutritionStored
		{
			get
			{
				float num = this.containedNutrition;
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Thing thing = this.innerContainer[i];
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition, true, -1);
				}
				return num;
			}
		}

		// Token: 0x17001F1C RID: 7964
		// (get) Token: 0x0600AD93 RID: 44435 RVA: 0x003D2A04 File Offset: 0x003D0C04
		public float NutritionNeeded
		{
			get
			{
				if (this.selectedPawn == null && this.selectedEmbryo == null)
				{
					return 0f;
				}
				return 10f - this.NutritionStored;
			}
		}

		// Token: 0x17001F1D RID: 7965
		// (get) Token: 0x0600AD94 RID: 44436 RVA: 0x003D2A28 File Offset: 0x003D0C28
		public int EmbryoGestationTicksRemaining
		{
			get
			{
				return this.startTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001F1E RID: 7966
		// (get) Token: 0x0600AD95 RID: 44437 RVA: 0x003D2A3B File Offset: 0x003D0C3B
		public float EmbryoGestationPct
		{
			get
			{
				return 1f - Mathf.Clamp01((float)this.EmbryoGestationTicksRemaining / 540000f);
			}
		}

		// Token: 0x17001F1F RID: 7967
		// (get) Token: 0x0600AD96 RID: 44438 RVA: 0x003D2A55 File Offset: 0x003D0C55
		private Graphic TopGraphic
		{
			get
			{
				if (this.cachedTopGraphic == null)
				{
					this.cachedTopGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/Misc/GrowthVat/GrowthVatTop", ShaderDatabase.Transparent, this.def.graphicData.drawSize, Color.white);
				}
				return this.cachedTopGraphic;
			}
		}

		// Token: 0x17001F20 RID: 7968
		// (get) Token: 0x0600AD97 RID: 44439 RVA: 0x003D2A8F File Offset: 0x003D0C8F
		private Graphic FetusEarlyStage
		{
			get
			{
				if (this.fetusEarlyStageGraphic == null)
				{
					this.fetusEarlyStageGraphic = GraphicDatabase.Get<Graphic_Single>("Other/VatGrownFetus_EarlyStage", ShaderDatabase.Cutout, Vector2.one, Color.white);
				}
				return this.fetusEarlyStageGraphic;
			}
		}

		// Token: 0x17001F21 RID: 7969
		// (get) Token: 0x0600AD98 RID: 44440 RVA: 0x003D2ABE File Offset: 0x003D0CBE
		private Graphic FetusLateStage
		{
			get
			{
				if (this.fetusLateStageGraphic == null)
				{
					this.fetusLateStageGraphic = GraphicDatabase.Get<Graphic_Single>("Other/VatGrownFetus_LateStage", ShaderDatabase.Cutout, Vector2.one, Color.white);
				}
				return this.fetusLateStageGraphic;
			}
		}

		// Token: 0x17001F22 RID: 7970
		// (get) Token: 0x0600AD99 RID: 44441 RVA: 0x003D2AF0 File Offset: 0x003D0CF0
		private Hediff VatLearning
		{
			get
			{
				if (this.cachedVatLearning == null)
				{
					Hediff hediff;
					if ((hediff = this.selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.VatLearning, false)) == null)
					{
						hediff = this.selectedPawn.health.AddHediff(HediffDefOf.VatLearning, null, null, null);
					}
					this.cachedVatLearning = hediff;
				}
				return this.cachedVatLearning;
			}
		}

		// Token: 0x0600AD9A RID: 44442 RVA: 0x003D2B50 File Offset: 0x003D0D50
		public override void PostMake()
		{
			base.PostMake();
			this.allowedNutritionSettings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.allowedNutritionSettings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x0600AD9B RID: 44443 RVA: 0x003D2B9C File Offset: 0x003D0D9C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad && this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					Color color = this.EmbryoColor();
					this.fetusEarlyStageGraphic = this.FetusEarlyStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
					this.fetusLateStageGraphic = this.FetusLateStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
				});
			}
		}

		// Token: 0x0600AD9C RID: 44444 RVA: 0x003D2BD8 File Offset: 0x003D0DD8
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				if (this.selectedPawn != null && this.innerContainer.Contains(this.selectedPawn))
				{
					this.Notify_PawnRemoved();
				}
				this.DestroyEmbryo(false);
			}
			this.sustainerWorking = null;
			this.cachedVatLearning = null;
			base.DeSpawn(mode);
		}

		// Token: 0x0600AD9D RID: 44445 RVA: 0x003D2C28 File Offset: 0x003D0E28
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (base.Working && this.selectedPawn != null && this.innerContainer.Contains(this.selectedPawn))
			{
				Hediff vatLearning = this.VatLearning;
				if (vatLearning != null)
				{
					vatLearning.TickInterval(delta);
				}
				Hediff vatLearning2 = this.VatLearning;
				if (vatLearning2 == null)
				{
					return;
				}
				vatLearning2.PostTickInterval(delta);
			}
		}

		// Token: 0x0600AD9E RID: 44446 RVA: 0x003D2C84 File Offset: 0x003D0E84
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				this.PowerTraderComp.PowerOutput = (base.Working ? (-base.PowerComp.Props.PowerConsumption) : (-base.PowerComp.Props.idlePowerDraw));
			}
			Pawn selectedPawn = this.selectedPawn;
			if (selectedPawn == null || !selectedPawn.Destroyed)
			{
				HumanEmbryo humanEmbryo = this.selectedEmbryo;
				if (humanEmbryo == null || !humanEmbryo.Destroyed)
				{
					goto IL_78;
				}
			}
			this.OnStop();
			IL_78:
			foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
			{
				HumanEmbryo humanEmbryo2 = thing as HumanEmbryo;
				if (humanEmbryo2 != null && humanEmbryo2 != this.selectedEmbryo)
				{
					Thing thing2;
					this.innerContainer.TryDrop(thing, this.InteractionCell, base.Map, ThingPlaceMode.Near, 1, out thing2, null, null);
				}
			}
			if (!base.Working)
			{
				this.TryGrowEmbryo();
				Effecter effecter = this.bubbleEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.bubbleEffecter = null;
				return;
			}
			if (this.selectedPawn != null)
			{
				if (this.selectedPawn.ageTracker.AgeBiologicalYearsFloat >= 18f)
				{
					Messages.Message("OccupantEjectedFromGrowthVat".Translate(this.selectedPawn.Named("PAWN")) + ": " + "PawnIsTooOld".Translate(this.selectedPawn.Named("PAWN")), this.selectedPawn, MessageTypeDefOf.NeutralEvent, true);
					this.Finish();
					return;
				}
				if (this.innerContainer.Contains(this.selectedPawn))
				{
					int ticks = Mathf.RoundToInt(20f * this.selectedPawn.GetStatValue(StatDefOf.GrowthVatOccupantSpeed, true, -1));
					this.selectedPawn.ageTracker.Notify_TickedInGrowthVat(ticks);
					Hediff vatLearning = this.VatLearning;
					if (vatLearning != null)
					{
						vatLearning.Tick();
					}
					Hediff vatLearning2 = this.VatLearning;
					if (vatLearning2 != null)
					{
						vatLearning2.PostTick();
					}
				}
				float num = this.BiostarvationDailyOffset / 60000f * HediffDefOf.BioStarvation.maxSeverity;
				Hediff firstHediffOfDef = this.selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BioStarvation, false);
				if (firstHediffOfDef != null)
				{
					firstHediffOfDef.Severity += num;
					if (firstHediffOfDef.ShouldRemove)
					{
						this.selectedPawn.health.RemoveHediff(firstHediffOfDef);
					}
				}
				else if (num > 0f)
				{
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BioStarvation, this.selectedPawn, null);
					hediff.Severity = num;
					this.selectedPawn.health.AddHediff(hediff, null, null, null);
				}
			}
			else if (this.selectedEmbryo != null)
			{
				if (this.EmbryoGestationTicksRemaining <= 0)
				{
					this.Finish();
					return;
				}
				this.embryoStarvation = Mathf.Clamp01(this.embryoStarvation + this.BiostarvationDailyOffset / 60000f);
			}
			if (this.BiostarvationSeverityPercent >= 1f)
			{
				this.Fail();
				return;
			}
			if (this.sustainerWorking == null || this.sustainerWorking.Ended)
			{
				this.sustainerWorking = SoundDefOf.GrowthVat_Working.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			}
			else
			{
				this.sustainerWorking.Maintain();
			}
			this.containedNutrition = Mathf.Clamp(this.containedNutrition - this.NutritionConsumedPerDay / 60000f, 0f, 2.1474836E+09f);
			if (this.containedNutrition <= 0f)
			{
				this.TryAbsorbNutritiousThing();
			}
			if (Building_GrowthVat.GlowMotePerRotation == null)
			{
				Building_GrowthVat.GlowMotePerRotation = new Dictionary<Rot4, ThingDef>
				{
					{
						Rot4.South,
						ThingDefOf.Mote_VatGlowVertical
					},
					{
						Rot4.East,
						ThingDefOf.Mote_VatGlowHorizontal
					},
					{
						Rot4.West,
						ThingDefOf.Mote_VatGlowHorizontal
					},
					{
						Rot4.North,
						ThingDefOf.Mote_VatGlowVertical
					}
				};
				Building_GrowthVat.BubbleEffecterPerRotation = new Dictionary<Rot4, EffecterDef>
				{
					{
						Rot4.South,
						EffecterDefOf.Vat_Bubbles_South
					},
					{
						Rot4.East,
						EffecterDefOf.Vat_Bubbles_East
					},
					{
						Rot4.West,
						EffecterDefOf.Vat_Bubbles_West
					},
					{
						Rot4.North,
						EffecterDefOf.Vat_Bubbles_North
					}
				};
			}
			if (this.IsHashIntervalTick(132))
			{
				MoteMaker.MakeStaticMote(this.DrawPos, base.MapHeld, Building_GrowthVat.GlowMotePerRotation[base.Rotation], 1f, false, 0f);
			}
			if (this.bubbleEffecter == null)
			{
				this.bubbleEffecter = Building_GrowthVat.BubbleEffecterPerRotation[base.Rotation].SpawnAttached(this, base.MapHeld, 1f);
			}
			this.bubbleEffecter.EffectTick(this, this);
		}

		// Token: 0x0600AD9F RID: 44447 RVA: 0x003D311C File Offset: 0x003D131C
		public override AcceptanceReport CanAcceptPawn(Pawn pawn)
		{
			if (base.Working)
			{
				return "Occupied".Translate();
			}
			if (!this.PowerOn)
			{
				return "NoPower".Translate().CapitalizeFirst();
			}
			if (this.selectedEmbryo != null)
			{
				return "EmbryoSelected".Translate();
			}
			if (pawn.ageTracker.AgeBiologicalYearsFloat >= 18f)
			{
				return "TooOld".Translate(pawn.Named("PAWN"), 18f.Named("AGEYEARS"));
			}
			if (this.selectedPawn != null && this.selectedPawn != pawn)
			{
				return "WaitingForPawn".Translate(this.selectedPawn.Named("PAWN"));
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.BioStarvation, false))
			{
				return "PawnBiostarving".Translate(pawn.Named("PAWN"));
			}
			return pawn.IsColonist && !pawn.IsQuestLodger();
		}

		// Token: 0x0600ADA0 RID: 44448 RVA: 0x003D3238 File Offset: 0x003D1438
		public override void TryAcceptPawn(Pawn pawn)
		{
			if (this.selectedPawn != null && this.CanAcceptPawn(pawn))
			{
				this.selectedPawn = pawn;
				bool flag = pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (this.innerContainer.TryAddOrTransfer(pawn, true))
				{
					SoundDefOf.GrowthVat_Close.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
					this.startTick = Find.TickManager.TicksGame;
					if (!pawn.health.hediffSet.HasHediff(HediffDefOf.VatLearning, false))
					{
						pawn.health.AddHediff(HediffDefOf.VatLearning, null, null, null);
					}
					if (!pawn.health.hediffSet.HasHediff(HediffDefOf.VatGrowing, false))
					{
						pawn.health.AddHediff(HediffDefOf.VatGrowing, null, null, null);
					}
				}
				if (flag)
				{
					Find.Selector.Select(pawn, false, false);
				}
			}
		}

		// Token: 0x0600ADA1 RID: 44449 RVA: 0x003D3320 File Offset: 0x003D1520
		private void TryGrowEmbryo()
		{
			if (base.Working || !this.PowerOn)
			{
				return;
			}
			if (this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
			{
				SoundDefOf.GrowthVat_Close.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
				this.startTick = Find.TickManager.TicksGame + 540000;
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					Color color = this.EmbryoColor();
					this.fetusEarlyStageGraphic = this.FetusEarlyStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
					this.fetusLateStageGraphic = this.FetusLateStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
				});
				if (this.selectedPawn != null)
				{
					Log.Error("Growing embryo while pawn was somehow marked as selected");
					this.selectedPawn = null;
				}
			}
		}

		// Token: 0x0600ADA2 RID: 44450 RVA: 0x003D33B0 File Offset: 0x003D15B0
		private void TryAbsorbNutritiousThing()
		{
			for (int i = 0; i < this.innerContainer.Count; i++)
			{
				if (this.innerContainer[i] != this.selectedPawn && this.innerContainer[i].def != ThingDefOf.Xenogerm && this.innerContainer[i].def != ThingDefOf.HumanEmbryo)
				{
					float statValue = this.innerContainer[i].GetStatValue(StatDefOf.Nutrition, true, -1);
					if (statValue > 0f)
					{
						this.containedNutrition += statValue;
						this.innerContainer[i].SplitOff(1).Destroy(DestroyMode.Vanish);
						return;
					}
				}
			}
		}

		// Token: 0x0600ADA3 RID: 44451 RVA: 0x003D3465 File Offset: 0x003D1665
		private void Finish()
		{
			if (this.selectedPawn != null)
			{
				this.FinishPawn();
				return;
			}
			if (this.selectedEmbryo != null)
			{
				this.FinishEmbryo();
			}
		}

		// Token: 0x0600ADA4 RID: 44452 RVA: 0x003D3484 File Offset: 0x003D1684
		private void FinishEmbryo()
		{
			this.EmbryoBirth();
			this.DestroyEmbryo(false);
			this.OnStop();
		}

		// Token: 0x0600ADA5 RID: 44453 RVA: 0x003D349C File Offset: 0x003D169C
		private void FinishPawn()
		{
			if (this.selectedPawn == null || !this.innerContainer.Contains(this.selectedPawn))
			{
				return;
			}
			this.Notify_PawnRemoved();
			Thing thing;
			this.innerContainer.TryDrop(this.selectedPawn, this.InteractionCell, base.Map, ThingPlaceMode.Near, 1, out thing, null, null);
			this.OnStop();
		}

		// Token: 0x0600ADA6 RID: 44454 RVA: 0x003D34F8 File Offset: 0x003D16F8
		private void Fail()
		{
			if (this.innerContainer.Contains(this.selectedPawn))
			{
				this.Notify_PawnRemoved();
				Thing thing;
				this.innerContainer.TryDrop(this.selectedPawn, this.InteractionCell, base.Map, ThingPlaceMode.Near, 1, out thing, null, null);
				Hediff firstHediffOfDef = this.selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BioStarvation, false);
				this.selectedPawn.Kill(null, firstHediffOfDef);
			}
			this.DestroyEmbryo(true);
			this.OnStop();
		}

		// Token: 0x0600ADA7 RID: 44455 RVA: 0x003D3580 File Offset: 0x003D1780
		private void OnStop()
		{
			this.selectedPawn = null;
			this.selectedEmbryo = null;
			this.startTick = -1;
			this.embryoStarvation = 0f;
			this.sustainerWorking = null;
			this.cachedVatLearning = null;
		}

		// Token: 0x0600ADA8 RID: 44456 RVA: 0x003D35B0 File Offset: 0x003D17B0
		private void DestroyEmbryo(bool biostarvation = false)
		{
			if (this.startTick >= 0 && this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
			{
				if (this.startTick > Find.TickManager.TicksGame)
				{
					if (biostarvation)
					{
						Messages.Message("EmbryoEjectedFromGrowthVatBiostarvation".Translate(this.selectedEmbryo.Label), this, MessageTypeDefOf.NegativeEvent, true);
					}
					else
					{
						Messages.Message("EmbryoEjectedFromGrowthVat".Translate(this.selectedEmbryo.Label), this, MessageTypeDefOf.NegativeEvent, true);
					}
				}
				this.innerContainer.Remove(this.selectedEmbryo);
				this.selectedEmbryo.Destroy(DestroyMode.Vanish);
				this.selectedEmbryo = null;
			}
		}

		// Token: 0x0600ADA9 RID: 44457 RVA: 0x003D3688 File Offset: 0x003D1888
		private void EmbryoBirth()
		{
			if (this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo) && this.startTick <= Find.TickManager.TicksGame)
			{
				Precept_Ritual precept_Ritual = Faction.OfPlayer.ideos.PrimaryIdeo.GetPrecept(PreceptDefOf.ChildBirth) as Precept_Ritual;
				RitualOutcomePossibility outcome = ((RitualOutcomeEffectWorker_ChildBirth)RitualOutcomeEffectDefOf.ChildBirth.GetInstance()).GetOutcome(0.7f, null);
				float quality = 0.7f;
				Precept_Ritual ritual = precept_Ritual;
				HumanEmbryo humanEmbryo = this.selectedEmbryo;
				List<GeneDef> genes;
				if (humanEmbryo == null)
				{
					genes = null;
				}
				else
				{
					GeneSet geneSet = humanEmbryo.GeneSet;
					genes = ((geneSet != null) ? geneSet.GenesListForReading : null);
				}
				Thing thing = PregnancyUtility.ApplyBirthOutcome(outcome, quality, ritual, genes, this.selectedEmbryo.Mother, this, this.selectedEmbryo.Father, null, null, null, false);
				if (thing != null && this.embryoStarvation > 0f)
				{
					Corpse corpse = thing as Corpse;
					Pawn pawn = (corpse != null) ? corpse.InnerPawn : ((Pawn)thing);
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BioStarvation, pawn, null);
					hediff.Severity = Mathf.Lerp(0f, HediffDefOf.BioStarvation.maxSeverity, this.embryoStarvation);
					pawn.health.AddHediff(hediff, null, null, null);
				}
			}
		}

		// Token: 0x0600ADAA RID: 44458 RVA: 0x003D37BA File Offset: 0x003D19BA
		private void Notify_PawnRemoved()
		{
			SoundDefOf.GrowthVat_Open.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
		}

		// Token: 0x0600ADAB RID: 44459 RVA: 0x003D37D2 File Offset: 0x003D19D2
		public bool CanAcceptNutrition(Thing thing)
		{
			return this.allowedNutritionSettings.AllowedToAccept(thing);
		}

		// Token: 0x0600ADAC RID: 44460 RVA: 0x003D37E0 File Offset: 0x003D19E0
		public StorageSettings GetStoreSettings()
		{
			return this.allowedNutritionSettings;
		}

		// Token: 0x0600ADAD RID: 44461 RVA: 0x003D37E8 File Offset: 0x003D19E8
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x0600ADAE RID: 44462 RVA: 0x003D37FA File Offset: 0x003D19FA
		public void Notify_SettingsChanged()
		{
		}

		// Token: 0x0600ADAF RID: 44463 RVA: 0x003D37FC File Offset: 0x003D19FC
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_GrowthVat.<GetGizmos>d__86 <GetGizmos>d__ = new Building_GrowthVat.<GetGizmos>d__86(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600ADB0 RID: 44464 RVA: 0x003D380C File Offset: 0x003D1A0C
		public void SelectEmbryo(HumanEmbryo embryo)
		{
			this.selectedEmbryo = embryo;
			embryo.implantTarget = this;
		}

		// Token: 0x0600ADB1 RID: 44465 RVA: 0x003D381C File Offset: 0x003D1A1C
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			if (base.Working && this.selectedPawn != null && this.innerContainer.Contains(this.selectedPawn))
			{
				this.selectedPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc + this.PawnDrawOffset, null, true);
			}
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
		}

		// Token: 0x0600ADB2 RID: 44466 RVA: 0x003D3884 File Offset: 0x003D1A84
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (base.Working && this.selectedPawn == null && this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
			{
				Vector2 drawSize = Vector2.one * Mathf.Lerp(0.4f, 0.95f, this.EmbryoGestationPct);
				if (this.EmbryoGestationTicksRemaining > 540000)
				{
					this.FetusEarlyStage.drawSize = drawSize;
					this.FetusEarlyStage.DrawFromDef(this.DrawPos + this.PawnDrawOffset + Altitudes.AltIncVect * 0.25f, base.Rotation, null, 0f);
				}
				else
				{
					this.FetusLateStage.drawSize = drawSize;
					this.FetusLateStage.DrawFromDef(this.DrawPos + this.PawnDrawOffset + Altitudes.AltIncVect * 0.25f, base.Rotation, null, 0f);
				}
			}
			this.TopGraphic.Draw(this.DrawPos + Altitudes.AltIncVect * 2f, base.Rotation, this, 0f);
		}

		// Token: 0x0600ADB3 RID: 44467 RVA: 0x003D39C0 File Offset: 0x003D1BC0
		private Color EmbryoColor()
		{
			Color result = PawnSkinColors.GetSkinColor(0.5f);
			HumanEmbryo humanEmbryo = this.selectedEmbryo;
			if (((humanEmbryo != null) ? humanEmbryo.GeneSet : null) != null)
			{
				foreach (GeneDef geneDef in this.selectedEmbryo.GeneSet.GenesListForReading)
				{
					if (geneDef.skinColorOverride != null)
					{
						return geneDef.skinColorOverride.Value;
					}
					if (geneDef.skinColorBase != null)
					{
						result = geneDef.skinColorBase.Value;
					}
				}
				return result;
			}
			return result;
		}

		// Token: 0x0600ADB4 RID: 44468 RVA: 0x003D3A6C File Offset: 0x003D1C6C
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.selectedEmbryo != null && this.selectedEmbryo.Map == base.Map)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.selectedEmbryo.TrueCenter());
			}
		}

		// Token: 0x0600ADB5 RID: 44469 RVA: 0x003D3AA8 File Offset: 0x003D1CA8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (base.Working)
			{
				if (this.selectedPawn != null && this.innerContainer.Contains(this.selectedPawn))
				{
					StringBuilder stringBuilder2 = stringBuilder.AppendLineIfNotEmpty();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
					defaultInterpolatedStringHandler.AppendFormatted("CasketContains".Translate().ToString());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(this.selectedPawn.NameShortColored.Resolve());
					defaultInterpolatedStringHandler.AppendLiteral(", ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.selectedPawn.ageTracker.AgeBiologicalYears);
					stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (this.selectedEmbryo != null && this.innerContainer.Contains(this.selectedEmbryo))
				{
					stringBuilder.AppendLineIfNotEmpty().AppendLine("Gestating".Translate() + ": " + this.selectedEmbryo.Label.CapitalizeFirst());
					stringBuilder.AppendLineTagged("EmbryoTimeUntilBirth".Translate() + ": " + this.EmbryoGestationTicksRemaining.ToStringTicksToDays("F1").Colorize(ColoredText.DateTimeColor));
					stringBuilder.Append("EmbryoBirthQuality".Translate() + ": " + 0.7f.ToStringPercent());
				}
				float biostarvationSeverityPercent = this.BiostarvationSeverityPercent;
				if (biostarvationSeverityPercent > 0f)
				{
					string str = (this.BiostarvationDailyOffset >= 0f) ? "+" : string.Empty;
					StringBuilder stringBuilder3 = stringBuilder.AppendLineIfNotEmpty();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(5, 3);
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("Biostarvation".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted(biostarvationSeverityPercent.ToStringPercent());
					defaultInterpolatedStringHandler2.AppendLiteral(" (");
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("PerDay".Translate(str + this.BiostarvationDailyOffset.ToStringPercent()));
					defaultInterpolatedStringHandler2.AppendLiteral(")");
					stringBuilder3.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			else if (this.selectedPawn != null)
			{
				stringBuilder.AppendLineIfNotEmpty().Append("WaitingForPawn".Translate(this.selectedPawn.Named("PAWN")).Resolve());
			}
			stringBuilder.AppendLineIfNotEmpty().Append("Nutrition".Translate()).Append(": ").Append(this.NutritionStored.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Absolute));
			if (base.Working)
			{
				stringBuilder.Append(" (-").Append("PerDay".Translate(this.NutritionConsumedPerDay.ToString("F1"))).Append(")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600ADB6 RID: 44470 RVA: 0x003D3DAF File Offset: 0x003D1FAF
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Building_GrowthVat.<GetFloatMenuOptions>d__93 <GetFloatMenuOptions>d__ = new Building_GrowthVat.<GetFloatMenuOptions>d__93(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600ADB7 RID: 44471 RVA: 0x003D3DC8 File Offset: 0x003D1FC8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<HumanEmbryo>(ref this.selectedEmbryo, "selectedEmbryo", false);
			Scribe_Values.Look<float>(ref this.embryoStarvation, "embryoStarvation", 0f, false);
			Scribe_Values.Look<float>(ref this.containedNutrition, "containedNutrition", 0f, false);
			Scribe_Deep.Look<StorageSettings>(ref this.allowedNutritionSettings, "allowedNutritionSettings", new object[]
			{
				this
			});
			if (this.allowedNutritionSettings == null)
			{
				this.allowedNutritionSettings = new StorageSettings(this);
				if (this.def.building.defaultStorageSettings != null)
				{
					this.allowedNutritionSettings.CopyFrom(this.def.building.defaultStorageSettings);
				}
			}
		}

		// Token: 0x04006494 RID: 25748
		public HumanEmbryo selectedEmbryo;

		// Token: 0x04006495 RID: 25749
		private float embryoStarvation;

		// Token: 0x04006496 RID: 25750
		private float containedNutrition;

		// Token: 0x04006497 RID: 25751
		private StorageSettings allowedNutritionSettings;

		// Token: 0x04006498 RID: 25752
		[Unsaved(false)]
		private CompPowerTrader cachedPowerComp;

		// Token: 0x04006499 RID: 25753
		[Unsaved(false)]
		private Graphic cachedTopGraphic;

		// Token: 0x0400649A RID: 25754
		[Unsaved(false)]
		private Graphic fetusEarlyStageGraphic;

		// Token: 0x0400649B RID: 25755
		[Unsaved(false)]
		private Graphic fetusLateStageGraphic;

		// Token: 0x0400649C RID: 25756
		[Unsaved(false)]
		private Sustainer sustainerWorking;

		// Token: 0x0400649D RID: 25757
		[Unsaved(false)]
		private Hediff cachedVatLearning;

		// Token: 0x0400649E RID: 25758
		[Unsaved(false)]
		private Effecter bubbleEffecter;

		// Token: 0x0400649F RID: 25759
		private static readonly Texture2D CancelLoadingIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x040064A0 RID: 25760
		public static readonly CachedTexture InsertPawnIcon = new CachedTexture("UI/Gizmos/InsertPawn");

		// Token: 0x040064A1 RID: 25761
		public static readonly CachedTexture InsertEmbryoIcon = new CachedTexture("UI/Gizmos/InsertEmbryo");

		// Token: 0x040064A2 RID: 25762
		private const float BiostarvationGainPerDayNoFoodOrPower = 0.5f;

		// Token: 0x040064A3 RID: 25763
		private const float BiostarvationFallPerDayPoweredAndFed = 0.1f;

		// Token: 0x040064A4 RID: 25764
		private const float BasePawnConsumedNutritionPerDay = 3f;

		// Token: 0x040064A5 RID: 25765
		private const float BaseEmbryoConsumedNutritionPerDay = 6f;

		// Token: 0x040064A6 RID: 25766
		private const float AgeToEject = 18f;

		// Token: 0x040064A7 RID: 25767
		public const float NutritionBuffer = 10f;

		// Token: 0x040064A8 RID: 25768
		public const int AgeTicksPerTickInGrowthVat = 20;

		// Token: 0x040064A9 RID: 25769
		private const float EmbryoBirthQuality = 0.7f;

		// Token: 0x040064AA RID: 25770
		public const int EmbryoGestationTicks = 540000;

		// Token: 0x040064AB RID: 25771
		private const int EmbryoLateStageGraphicTicksRemaining = 540000;

		// Token: 0x040064AC RID: 25772
		private const float FetusMinSize = 0.4f;

		// Token: 0x040064AD RID: 25773
		private const float FetusMaxSize = 0.95f;

		// Token: 0x040064AE RID: 25774
		private const int GlowIntervalTicks = 132;

		// Token: 0x040064AF RID: 25775
		private static Dictionary<Rot4, ThingDef> GlowMotePerRotation;

		// Token: 0x040064B0 RID: 25776
		private static Dictionary<Rot4, EffecterDef> BubbleEffecterPerRotation;
	}
}
