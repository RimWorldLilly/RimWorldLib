using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020004E3 RID: 1251
	public class Pawn_HealthTracker : IExposable
	{
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x060024D8 RID: 9432 RVA: 0x000DE2BD File Offset: 0x000DC4BD
		public PawnHealthState State
		{
			get
			{
				return this.healthState;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060024D9 RID: 9433 RVA: 0x000DE2C5 File Offset: 0x000DC4C5
		public bool Downed
		{
			get
			{
				return this.healthState == PawnHealthState.Down;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x060024DA RID: 9434 RVA: 0x000DE2D0 File Offset: 0x000DC4D0
		public bool Dead
		{
			get
			{
				return this.healthState == PawnHealthState.Dead;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x060024DB RID: 9435 RVA: 0x000DE2DB File Offset: 0x000DC4DB
		public float LethalDamageThreshold
		{
			get
			{
				return 150f * this.pawn.HealthScale;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060024DC RID: 9436 RVA: 0x000DE2F0 File Offset: 0x000DC4F0
		public bool CanBleed
		{
			get
			{
				return (!this.pawn.IsMutant || this.pawn.mutant.Def.canBleed) && this.pawn.RaceProps.BloodDef != null && this.pawn.RaceProps.bleedRateFactor > 0f && this.pawn.RaceProps.IsFlesh;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060024DD RID: 9437 RVA: 0x000DE360 File Offset: 0x000DC560
		public bool InPainShock
		{
			get
			{
				return !this.pawn.kindDef.ignoresPainShock && this.hediffSet.PainTotal >= this.pawn.GetStatValue(StatDefOf.PainShockThreshold, true, -1);
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x000DE398 File Offset: 0x000DC598
		public bool CanCrawlOrMove
		{
			get
			{
				return this.CanCrawl || !this.Downed;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x000DE3B0 File Offset: 0x000DC5B0
		public bool CanCrawl
		{
			get
			{
				return this.pawn.RaceProps.Humanlike && this.pawn.Awake() && this.capacities.GetLevel(PawnCapacityDefOf.Manipulation) >= 0.15f && this.pawn.ageTracker.AgeBiologicalYears >= 8 && !this.hediffSet.AnyHediffPreventsCrawling;
			}
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000DE420 File Offset: 0x000DC620
		public Pawn_HealthTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.hediffSet = new HediffSet(pawn);
			this.capacities = new PawnCapacitiesHandler(pawn);
			this.summaryHealth = new SummaryHealthHandler(pawn);
			this.surgeryBills = new BillStack(pawn);
			this.immunity = new ImmunityHandler(pawn);
			this.beCarriedByCaravanIfSick = pawn.RaceProps.Humanlike;
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000DE4C4 File Offset: 0x000DC6C4
		public void Reset()
		{
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.Clear();
			this.capacities.Clear();
			this.summaryHealth.Notify_HealthChanged();
			this.surgeryBills.Clear();
			this.immunity = new ImmunityHandler(this.pawn);
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000DE518 File Offset: 0x000DC718
		public void ExposeData()
		{
			Scribe_Values.Look<PawnHealthState>(ref this.healthState, "healthState", PawnHealthState.Mobile, false);
			Scribe_Values.Look<bool>(ref this.forceDowned, "forceDowned", false, false);
			Scribe_Values.Look<bool>(ref this.beCarriedByCaravanIfSick, "beCarriedByCaravanIfSick", true, false);
			Scribe_Values.Look<bool>(ref this.killedByRitual, "killedByRitual", false, false);
			Scribe_Values.Look<int>(ref this.lastReceivedNeuralSuperchargeTick, "lastReceivedNeuralSuperchargeTick", -1, false);
			Scribe_Deep.Look<HediffSet>(ref this.hediffSet, "hediffSet", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<BillStack>(ref this.surgeryBills, "surgeryBills", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<ImmunityHandler>(ref this.immunity, "immunity", new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<Vector3?>(ref this.lastSmearDropPos, "lastSmearDropPos", null, false);
			Scribe_Values.Look<float>(ref this.overrideDeathOnDownedChance, "overrideDeathOnDownedChance", -1f, false);
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000DE60C File Offset: 0x000DC80C
		public Hediff AddHediff(HediffDef def, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			this.AddHediff(hediff, part, dinfo, result);
			return hediff;
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000DE634 File Offset: 0x000DC834
		public Hediff GetOrAddHediff(HediffDef def, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			Hediff result2;
			if (!this.hediffSet.TryGetHediff(def, out result2))
			{
				result2 = this.AddHediff(def, part, dinfo, result);
			}
			return result2;
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000DE660 File Offset: 0x000DC860
		public void AddHediff(Hediff hediff, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			if (part == null && hediff.def.defaultInstallPart != null)
			{
				part = (from x in this.pawn.RaceProps.body.AllParts
				where x.def == hediff.def.defaultInstallPart
				select x).RandomElement<BodyPartRecord>();
			}
			if (part != null)
			{
				hediff.Part = part;
			}
			this.hediffSet.AddDirect(hediff, dinfo, result);
			this.CheckForStateChange(dinfo, hediff);
			if (this.pawn.RaceProps.hediffGiverSets != null)
			{
				for (int i = 0; i < this.pawn.RaceProps.hediffGiverSets.Count; i++)
				{
					HediffGiverSetDef hediffGiverSetDef = this.pawn.RaceProps.hediffGiverSets[i];
					for (int j = 0; j < hediffGiverSetDef.hediffGivers.Count; j++)
					{
						hediffGiverSetDef.hediffGivers[j].OnHediffAdded(this.pawn, hediff);
					}
				}
			}
			if (hediff.def.hairColorOverride != null)
			{
				this.pawn.story.HairColor = hediff.def.hairColorOverride.Value;
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			if (hediff.def.HasDefinedGraphicProperties)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			if (hediff.def.givesInfectionPathways != null && this.pawn.RaceProps.Humanlike)
			{
				foreach (InfectionPathwayDef def in hediff.def.givesInfectionPathways)
				{
					this.pawn.infectionVectors.AddInfectionVector(def, null);
				}
			}
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000DE864 File Offset: 0x000DCA64
		public void RemoveHediff(Hediff hediff)
		{
			hediff.PreRemoved();
			this.hediffSet.hediffs.Remove(hediff);
			this.hediffSet.DirtyCache();
			hediff.PostRemoved();
			this.CheckForStateChange(null, hediff);
			if (hediff.def.HasDefinedGraphicProperties || hediff.def.forceRenderTreeRecache)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
			Pawn_HealthTracker.tmpRemovedHediffs.Add(hediff);
		}

		// Token: 0x060024E7 RID: 9447 RVA: 0x000DE8E8 File Offset: 0x000DCAE8
		public void RemoveAllHediffs()
		{
			for (int i = this.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				this.RemoveHediff(this.hediffSet.hediffs[i]);
			}
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000DE92C File Offset: 0x000DCB2C
		public void Notify_HediffChanged(Hediff hediff)
		{
			this.hediffSet.DirtyCache();
			this.CheckForStateChange(null, hediff);
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000DE954 File Offset: 0x000DCB54
		public void Notify_UsedVerb(Verb verb, LocalTargetInfo target)
		{
			foreach (Hediff hediff in this.hediffSet.hediffs)
			{
				hediff.Notify_PawnUsedVerb(verb, target);
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000DE9AC File Offset: 0x000DCBAC
		public void Notify_Spawned()
		{
			foreach (Hediff hediff in this.hediffSet.hediffs)
			{
				hediff.Notify_Spawned();
			}
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x000DEA04 File Offset: 0x000DCC04
		public void Notify_PawnCorpseDestroyed()
		{
			foreach (Hediff hediff in this.hediffSet.hediffs)
			{
				hediff.Notify_PawnCorpseDestroyed();
			}
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x000DEA5C File Offset: 0x000DCC5C
		public void PreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			Faction homeFaction = this.pawn.HomeFaction;
			if (dinfo.Instigator != null && homeFaction != null && homeFaction.IsPlayer && !this.pawn.InAggroMentalState && !dinfo.Def.consideredHelpful && !this.pawn.IsSubhuman)
			{
				Pawn pawn = dinfo.Instigator as Pawn;
				if (dinfo.InstigatorGuilty && pawn != null && pawn.guilt != null && pawn.mindState != null)
				{
					pawn.guilt.Notify_Guilty(60000);
				}
			}
			if (this.pawn.Spawned)
			{
				if (!this.pawn.Position.Fogged(this.pawn.Map))
				{
					this.pawn.mindState.Active = true;
				}
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnDamaged(this.pawn, dinfo);
				}
				if (dinfo.Def.ExternalViolenceFor(this.pawn))
				{
					GenClamor.DoClamor(this.pawn, 18f, ClamorDefOf.Harm);
				}
			}
			if (homeFaction != null)
			{
				homeFaction.Notify_MemberTookDamage(this.pawn, dinfo);
				if (Current.ProgramState == ProgramState.Playing && homeFaction == Faction.OfPlayer && dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.SpawnedOrAnyParentSpawned)
				{
					this.pawn.MapHeld.dangerWatcher.Notify_ColonistHarmedExternally();
				}
			}
			if (this.pawn.apparel != null && !dinfo.IgnoreArmor)
			{
				List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i].CheckPreAbsorbDamage(dinfo))
					{
						absorbed = true;
						if (this.pawn.Spawned && dinfo.CheckForJobOverride)
						{
							this.pawn.jobs.Notify_DamageTaken(dinfo);
						}
						return;
					}
				}
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.Notify_DamageTaken(dinfo);
				this.pawn.stances.stunner.Notify_DamageApplied(dinfo);
			}
			if (this.pawn.RaceProps.IsFlesh && dinfo.Def.ExternalViolenceFor(this.pawn))
			{
				Pawn pawn2 = dinfo.Instigator as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.HostileTo(this.pawn))
					{
						this.pawn.relations.canGetRescuedThought = true;
					}
					if (this.pawn.RaceProps.Humanlike && pawn2.RaceProps.Humanlike && this.pawn.needs.mood != null && (!pawn2.HostileTo(this.pawn) || (pawn2.Faction == homeFaction && pawn2.InMentalState)))
					{
						this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HarmedMe, pawn2, null);
					}
				}
				ThingDef thingDef = (pawn2 != null && dinfo.Weapon != pawn2.def) ? dinfo.Weapon : null;
				TaleRecorder.RecordTale(TaleDefOf.Wounded, new object[]
				{
					this.pawn,
					pawn2,
					thingDef
				});
			}
			absorbed = false;
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x000DED8C File Offset: 0x000DCF8C
		public void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.ShouldBeDead())
			{
				if (!this.ShouldBeDeathrestingOrInComa())
				{
					if (!this.pawn.Destroyed)
					{
						this.pawn.Kill(new DamageInfo?(dinfo), null);
					}
					return;
				}
				this.ForceDeathrestOrComa(new DamageInfo?(dinfo), null);
			}
			if (dinfo.Def.additionalHediffs != null)
			{
				if (!dinfo.Def.applyAdditionalHediffsIfHuntingForFood)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (pawn != null && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.PredatorHunt)
					{
						goto IL_18B;
					}
				}
				List<DamageDefAdditionalHediff> additionalHediffs = dinfo.Def.additionalHediffs;
				for (int i = 0; i < additionalHediffs.Count; i++)
				{
					DamageDefAdditionalHediff damageDefAdditionalHediff = additionalHediffs[i];
					if (damageDefAdditionalHediff.hediff != null)
					{
						float num = (damageDefAdditionalHediff.severityFixed <= 0f) ? (totalDamageDealt * damageDefAdditionalHediff.severityPerDamageDealt) : damageDefAdditionalHediff.severityFixed;
						if (damageDefAdditionalHediff.victimSeverityScalingByInvBodySize)
						{
							num *= 1f / this.pawn.BodySize;
						}
						if (damageDefAdditionalHediff.victimSeverityScaling != null)
						{
							num *= (damageDefAdditionalHediff.inverseStatScaling ? Mathf.Max(1f - this.pawn.GetStatValue(damageDefAdditionalHediff.victimSeverityScaling, true, -1), 0f) : this.pawn.GetStatValue(damageDefAdditionalHediff.victimSeverityScaling, true, -1));
						}
						if (num >= 0f)
						{
							Hediff hediff = HediffMaker.MakeHediff(damageDefAdditionalHediff.hediff, this.pawn, null);
							hediff.Severity = num;
							this.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
							if (this.Dead)
							{
								return;
							}
						}
					}
				}
			}
			IL_18B:
			for (int j = 0; j < this.hediffSet.hediffs.Count; j++)
			{
				this.hediffSet.hediffs[j].Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			}
			if (this.pawn.Spawned && dinfo.CheckForJobOverride)
			{
				this.pawn.jobs.Notify_DamageTaken(dinfo);
			}
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x000DEF84 File Offset: 0x000DD184
		public void RestorePart(BodyPartRecord part, Hediff diffException = null, bool checkStateChange = true)
		{
			if (part == null)
			{
				Log.Error("Tried to restore null body part.");
				return;
			}
			this.RestorePartRecursiveInt(part, diffException);
			this.hediffSet.DirtyCache();
			if (checkStateChange)
			{
				this.CheckForStateChange(null, null);
			}
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000DEFC8 File Offset: 0x000DD1C8
		private void RestorePartRecursiveInt(BodyPartRecord part, Hediff diffException = null)
		{
			List<Hediff> hediffs = this.hediffSet.hediffs;
			for (int i = hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff = hediffs[i];
				if (hediff.Part == part && hediff != diffException && !hediff.def.keepOnBodyPartRestoration)
				{
					hediffs.RemoveAt(i);
					hediff.PostRemoved();
				}
			}
			for (int j = 0; j < part.parts.Count; j++)
			{
				this.RestorePartRecursiveInt(part.parts[j], diffException);
			}
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x000DF04C File Offset: 0x000DD24C
		public float FactorForDamage(DamageInfo dinfo)
		{
			return this.hediffSet.FactorForDamage(dinfo);
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x000DF05C File Offset: 0x000DD25C
		public void CheckForStateChange(DamageInfo? dinfo, Hediff hediff)
		{
			if (!this.Dead && !this.isBeingKilled)
			{
				if (ModsConfig.BiotechActive && this.pawn.mechanitor != null)
				{
					this.pawn.mechanitor.Notify_HediffStateChange(hediff);
				}
				if (hediff != null && hediff.def.blocksSleeping && !this.pawn.Awake())
				{
					RestUtility.WakeUp(this.pawn, true);
				}
				if (((hediff != null) ? hediff.CurStage : null) != null && hediff.CurStage.disabledWorkTags != WorkTags.None)
				{
					this.pawn.Notify_DisabledWorkTypesChanged();
				}
				if (this.pawn.Crawling && !this.CanCrawl && this.pawn.CurJob != null)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
				}
				else if (this.ShouldBeDead())
				{
					if (this.ShouldBeDeathrestingOrInComa())
					{
						this.ForceDeathrestOrComa(dinfo, hediff);
					}
					else if (!this.pawn.Destroyed)
					{
						this.pawn.Kill(dinfo, hediff);
					}
				}
				else if (!this.Downed)
				{
					if (this.ShouldBeDowned())
					{
						if (this.pawn.kindDef.forceDeathOnDowned)
						{
							this.pawn.Kill(dinfo, null);
							return;
						}
						if (!this.forceDowned && ((dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this.pawn)) || (hediff != null && hediff.def.canApplyDodChanceForCapacityChanges)) && !this.pawn.IsWildMan() && !this.pawn.IsDeactivated() && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && (this.pawn.HostFaction == null || !this.pawn.HostFaction.IsPlayer))
						{
							bool flag = (ModsConfig.BiotechActive && this.pawn.genes != null && this.pawn.genes.HasActiveGene(GeneDefOf.Deathless)) || this.hediffSet.HasPreventsDeath;
							float num;
							if (this.overrideDeathOnDownedChance >= 0f)
							{
								num = this.overrideDeathOnDownedChance;
							}
							else if (this.pawn.IsMutant && this.pawn.mutant.Def.deathOnDownedChance >= 0f)
							{
								num = this.pawn.mutant.Def.deathOnDownedChance;
							}
							else if (flag && this.pawn.Faction == Faction.OfPlayer)
							{
								num = 0f;
							}
							else if (this.pawn.kindDef.overrideDeathOnDownedChance != null)
							{
								num = this.pawn.kindDef.overrideDeathOnDownedChance.Value;
							}
							else if (ModsConfig.AnomalyActive && this.pawn.Faction == Faction.OfEntities && this.pawn.MapHeld != null)
							{
								num = HealthTuning.DeathOnDownedChance_EntityFromThreatCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.pawn.MapHeld));
							}
							else if (this.pawn.RaceProps.Animal)
							{
								num = 0.5f;
							}
							else if (this.pawn.RaceProps.IsMechanoid)
							{
								num = 1f;
							}
							else
							{
								num = (Find.Storyteller.difficulty.unwaveringPrisoners ? HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve : HealthTuning.DeathOnDownedChance_NonColonyHumanlikeFromPopulationIntentCurve_WaveringPrisoners).Evaluate(StorytellerUtilityPopulation.PopulationIntent) * Find.Storyteller.difficulty.enemyDeathOnDownedChanceFactor;
							}
							if (Rand.Chance(num))
							{
								if (DebugViewSettings.logCauseOfDeath)
								{
									Log.Message("CauseOfDeath: chance on downed " + num.ToStringPercent());
								}
								if (flag && !this.pawn.Dead)
								{
									this.pawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.pawn.health.hediffSet.GetBrain(), dinfo, null);
									return;
								}
								this.pawn.Kill(dinfo, null);
								return;
							}
						}
						this.forceDowned = false;
						this.MakeDowned(dinfo, hediff);
						return;
					}
					else if (!this.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						if (this.pawn.carryTracker != null && this.pawn.carryTracker.CarriedThing != null && this.pawn.jobs != null && this.pawn.CurJob != null)
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
						if (this.pawn.equipment != null && this.pawn.equipment.Primary != null)
						{
							if (this.pawn.kindDef.destroyGearOnDrop)
							{
								this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
							}
							else if (this.pawn.InContainerEnclosed)
							{
								this.pawn.equipment.TryTransferEquipmentToContainer(this.pawn.equipment.Primary, this.pawn.holdingOwner);
							}
							else if (this.pawn.SpawnedOrAnyParentSpawned)
							{
								ThingWithComps thingWithComps;
								this.pawn.equipment.TryDropEquipment(this.pawn.equipment.Primary, out thingWithComps, this.pawn.PositionHeld, true);
							}
							else if (this.pawn.IsCaravanMember())
							{
								ThingWithComps primary = this.pawn.equipment.Primary;
								this.pawn.equipment.Remove(primary);
								if (!this.pawn.inventory.innerContainer.TryAdd(primary, true))
								{
									primary.Destroy(DestroyMode.Vanish);
								}
							}
							else
							{
								this.pawn.equipment.DestroyEquipment(this.pawn.equipment.Primary);
							}
						}
					}
				}
				else if (!this.ShouldBeDowned())
				{
					this.MakeUndowned(hediff);
				}
				if (this.Downed && this.couldCrawl && !this.CanCrawl && !this.pawn.InBed())
				{
					Pawn_PathFollower pather = this.pawn.pather;
					if (pather != null)
					{
						pather.StopDead();
					}
					Pawn_JobTracker jobs = this.pawn.jobs;
					if (jobs != null)
					{
						jobs.StopAll(false, true);
					}
					Lord lord = this.pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnDowned(this.pawn);
					}
				}
				this.couldCrawl = this.CanCrawl;
			}
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x000DF6A3 File Offset: 0x000DD8A3
		private bool ShouldBeDeathrestingOrInComa()
		{
			return ModsConfig.BiotechActive && SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(this.pawn);
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x000DF6BE File Offset: 0x000DD8BE
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				Pawn_HealthTracker.<get_DisabledWorkTypes>d__56 <get_DisabledWorkTypes>d__ = new Pawn_HealthTracker.<get_DisabledWorkTypes>d__56(-2);
				<get_DisabledWorkTypes>d__.<>4__this = this;
				return <get_DisabledWorkTypes>d__;
			}
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000DF6D0 File Offset: 0x000DD8D0
		public bool ShouldBeDowned()
		{
			return this.InPainShock || !this.capacities.CanBeAwake || (!this.capacities.CapableOf(PawnCapacityDefOf.Moving) && !this.pawn.RaceProps.doesntMove) || this.pawn.ageTracker.CurLifeStage.alwaysDowned;
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000DF730 File Offset: 0x000DD930
		public bool ShouldBeDead()
		{
			if (this.Dead)
			{
				return true;
			}
			if (this.hediffSet.HasPreventsDeath)
			{
				return false;
			}
			using (List<Hediff>.Enumerator enumerator = this.hediffSet.hediffs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CauseDeathNow())
					{
						return true;
					}
				}
			}
			if (this.ShouldBeDeadFromRequiredCapacity() != null)
			{
				return true;
			}
			if (PawnCapacityUtility.CalculatePartEfficiency(this.hediffSet, this.pawn.RaceProps.body.corePart, false, null) <= 0.0001f)
			{
				if (DebugViewSettings.logCauseOfDeath)
				{
					Log.Message("CauseOfDeath: zero efficiency of " + this.pawn.RaceProps.body.corePart.Label);
				}
				return true;
			}
			return this.ShouldBeDeadFromLethalDamageThreshold();
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000DF81C File Offset: 0x000DDA1C
		public PawnCapacityDef ShouldBeDeadFromRequiredCapacity()
		{
			List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				PawnCapacityDef pawnCapacityDef = allDefsListForReading[i];
				if ((this.pawn.RaceProps.IsFlesh ? pawnCapacityDef.lethalFlesh : pawnCapacityDef.lethalMechanoids) && !this.capacities.CapableOf(pawnCapacityDef))
				{
					if (DebugViewSettings.logCauseOfDeath)
					{
						Log.Message("CauseOfDeath: no longer capable of " + pawnCapacityDef.defName);
					}
					return pawnCapacityDef;
				}
			}
			return null;
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000DF898 File Offset: 0x000DDA98
		public bool ShouldBeDeadFromLethalDamageThreshold()
		{
			float num = 0f;
			for (int i = 0; i < this.hediffSet.hediffs.Count; i++)
			{
				if (this.hediffSet.hediffs[i] is Hediff_Injury)
				{
					num += this.hediffSet.hediffs[i].Severity;
				}
			}
			bool flag = num >= this.LethalDamageThreshold;
			if (flag && DebugViewSettings.logCauseOfDeath)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 2);
				defaultInterpolatedStringHandler.AppendLiteral("CauseOfDeath: lethal damage ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" >= ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(this.LethalDamageThreshold);
				Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return flag;
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000DF950 File Offset: 0x000DDB50
		public bool WouldLosePartAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.CheckPredicateAfterAddingHediff(hediff, () => this.hediffSet.PartIsMissing(part));
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000DF9A0 File Offset: 0x000DDBA0
		public bool WouldDieAfterAddingHediff(Hediff hediff)
		{
			if (this.Dead)
			{
				return true;
			}
			bool flag = this.CheckPredicateAfterAddingHediff(hediff, new Func<bool>(this.ShouldBeDead));
			if (flag && DebugViewSettings.logCauseOfDeath)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("CauseOfDeath: WouldDieAfterAddingHediff=true for ");
				defaultInterpolatedStringHandler.AppendFormatted<Name>(this.pawn.Name);
				Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return flag;
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x000DFA08 File Offset: 0x000DDC08
		public bool WouldDieAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldDieAfterAddingHediff(hediff);
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x000DFA31 File Offset: 0x000DDC31
		public bool WouldBeDownedAfterAddingHediff(Hediff hediff)
		{
			return !this.Dead && this.CheckPredicateAfterAddingHediff(hediff, new Func<bool>(this.ShouldBeDowned));
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x000DFA50 File Offset: 0x000DDC50
		public bool WouldBeDownedAfterAddingHediff(HediffDef def, BodyPartRecord part, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(def, this.pawn, part);
			hediff.Severity = severity;
			return this.WouldBeDownedAfterAddingHediff(hediff);
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000DFA7C File Offset: 0x000DDC7C
		public void SetDead()
		{
			if (this.Dead)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" set dead while already dead.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.healthState = PawnHealthState.Dead;
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000DFAC8 File Offset: 0x000DDCC8
		private bool CheckPredicateAfterAddingHediff(Hediff hediff, Func<bool> pred)
		{
			HashSet<Hediff> missing = this.CalculateMissingPartHediffsFromInjury(hediff);
			this.hediffSet.hediffs.Add(hediff);
			if (missing != null)
			{
				this.hediffSet.hediffs.AddRange(missing);
			}
			this.hediffSet.DirtyCache();
			bool result = pred();
			if (missing != null)
			{
				this.hediffSet.hediffs.RemoveAll((Hediff x) => missing.Contains(x));
			}
			this.hediffSet.hediffs.Remove(hediff);
			this.hediffSet.DirtyCache();
			return result;
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000DFB6C File Offset: 0x000DDD6C
		private HashSet<Hediff> CalculateMissingPartHediffsFromInjury(Hediff hediff)
		{
			Pawn_HealthTracker.<>c__DisplayClass68_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.hediff = hediff;
			CS$<>8__locals1.missing = null;
			if (CS$<>8__locals1.hediff.Part != null && CS$<>8__locals1.hediff.Part != this.pawn.RaceProps.body.corePart && CS$<>8__locals1.hediff.Severity >= this.hediffSet.GetPartHealth(CS$<>8__locals1.hediff.Part))
			{
				CS$<>8__locals1.missing = new HashSet<Hediff>();
				this.<CalculateMissingPartHediffsFromInjury>g__AddAllParts|68_0(CS$<>8__locals1.hediff.Part, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.missing;
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x000DFC08 File Offset: 0x000DDE08
		private void ForceDeathrestOrComa(DamageInfo? dinfo, Hediff hediff)
		{
			if (this.pawn.CanDeathrest())
			{
				if (SanguophageUtility.TryStartDeathrest(this.pawn, DeathrestStartReason.LethalDamage))
				{
					GeneUtility.OffsetHemogen(this.pawn, -9999f, true);
				}
			}
			else
			{
				SanguophageUtility.TryStartRegenComa(this.pawn, DeathrestStartReason.LethalDamage);
			}
			if (!this.Downed)
			{
				this.forceDowned = true;
				this.MakeDowned(dinfo, hediff);
			}
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000DFC68 File Offset: 0x000DDE68
		private void MakeDowned(DamageInfo? dinfo, Hediff hediff)
		{
			if (this.Downed)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" tried to do MakeDowned while already downed.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (this.pawn.guilt != null)
			{
				Lord lord = this.pawn.GetLord();
				if (((lord != null) ? lord.LordJob : null) != null && this.pawn.GetLord().LordJob.GuiltyOnDowned)
				{
					this.pawn.guilt.Notify_Guilty(60000);
				}
			}
			this.healthState = PawnHealthState.Down;
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this.pawn, dinfo, PawnDiedOrDownedThoughtsKind.Downed);
			if (this.pawn.InMentalState && this.pawn.MentalStateDef.recoverFromDowned)
			{
				this.pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
			this.pawn.mindState.droppedWeapon = null;
			this.pawn.mindState.nextMoveOrderIsCrawlBreak = true;
			if (this.pawn.Spawned)
			{
				this.pawn.DropAndForbidEverything(true, true);
				this.pawn.stances.CancelBusyStanceSoft();
			}
			if (!this.pawn.DutyActiveWhenDown(true) && (!this.pawn.IsMutant || !this.pawn.health.CanCrawl || !this.pawn.mutant.Def.canAttackWhileCrawling))
			{
				this.pawn.ClearMind_NewTemp(true, false, false, true);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Lord lord2 = this.pawn.GetLord();
				if (lord2 != null)
				{
					lord2.Notify_PawnLost(this.pawn, PawnLostCondition.Incapped, dinfo);
				}
			}
			if (this.pawn.Drafted)
			{
				this.pawn.drafter.Drafted = false;
			}
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			if (this.pawn.SpawnedOrAnyParentSpawned)
			{
				GenHostility.Notify_PawnLostForTutor(this.pawn, this.pawn.MapHeld);
			}
			if (this.pawn.RaceProps.Humanlike && !this.pawn.IsSubhuman && Current.ProgramState == ProgramState.Playing && this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.pawn.HostileTo(Faction.OfPlayer))
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Capturing, this.pawn, OpportunityType.Important);
				}
				if (this.pawn.Faction == Faction.OfPlayer)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.Rescuing, this.pawn, OpportunityType.Critical);
				}
			}
			Pawn pawn = ((dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null) as Pawn;
			if (pawn != null)
			{
				RecordsUtility.Notify_PawnDowned(this.pawn, pawn);
			}
			if (this.pawn.Spawned && (hediff == null || hediff.def.recordDownedTale))
			{
				TaleRecorder.RecordTale(TaleDefOf.Downed, new object[]
				{
					this.pawn,
					((dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null) as Pawn,
					(dinfo != null) ? dinfo.GetValueOrDefault().Weapon : null
				});
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this.pawn, RulePackDefOf.Transition_Downed, ((dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null) as Pawn, hediff, (dinfo != null) ? dinfo.GetValueOrDefault().HitPart : null));
			}
			Find.Storyteller.Notify_PawnEvent(this.pawn, AdaptationEvent.Downed, dinfo);
			Pawn_MechanitorTracker mechanitor = this.pawn.mechanitor;
			if (mechanitor != null)
			{
				mechanitor.Notify_Downed();
			}
			Pawn_MutantTracker mutant = this.pawn.mutant;
			if (mutant != null)
			{
				mutant.Notify_Downed();
			}
			foreach (Hediff hediff2 in this.hediffSet.hediffs)
			{
				hediff2.Notify_Downed();
			}
			this.pawn.Notify_Downed();
			Lord lord3 = this.pawn.GetLord();
			if (lord3 != null)
			{
				lord3.Notify_PawnDowned(this.pawn);
			}
			Pawn_FlightTracker flight = this.pawn.flight;
			if (flight == null)
			{
				return;
			}
			flight.ForceLand();
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000E00AC File Offset: 0x000DE2AC
		private void MakeUndowned(Hediff hediff)
		{
			if (!this.Downed)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" tried to do MakeUndowned when already undowned.");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			Pawn_PathFollower pather = this.pawn.pather;
			if (pather != null)
			{
				pather.StopDead();
			}
			Pawn_JobTracker jobs = this.pawn.jobs;
			if (jobs != null)
			{
				jobs.CheckForJobOverride(0f, true);
			}
			this.healthState = PawnHealthState.Mobile;
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn) && (hediff == null || hediff.def != HediffDefOf.Deathrest))
			{
				Messages.Message("MessageNoLongerDowned".Translate(this.pawn.LabelCap, this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			if (this.pawn.guest != null)
			{
				this.pawn.guest.Notify_PawnUndowned();
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_PawnUndowned(this.pawn);
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x000E01D8 File Offset: 0x000DE3D8
		public void NotifyPlayerOfKilled(DamageInfo? dinfo, Hediff hediff, Caravan caravan)
		{
			TaggedString taggedString = HealthUtility.GetDiedLetterText(this.pawn, dinfo, hediff);
			Quest quest = null;
			if (this.pawn.IsBorrowedByAnyFaction())
			{
				foreach (QuestPart_LendColonistsToFaction questPart_LendColonistsToFaction in QuestUtility.GetAllQuestPartsOfType<QuestPart_LendColonistsToFaction>(true))
				{
					if (questPart_LendColonistsToFaction.LentColonistsListForReading.Contains(this.pawn))
					{
						taggedString += "\n\n" + "LentColonistDied".Translate(this.pawn.Named("PAWN"), questPart_LendColonistsToFaction.lendColonistsToFaction.Named("FACTION"));
						quest = questPart_LendColonistsToFaction.quest;
						break;
					}
				}
			}
			taggedString = taggedString.AdjustedFor(this.pawn, "PAWN", true);
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				TaggedString taggedString2 = "Death".Translate() + ": " + this.pawn.LabelShortCap;
				if (caravan != null)
				{
					Messages.Message("MessageCaravanDeathCorpseAddedToInventory".Translate(this.pawn.Named("PAWN")), caravan, MessageTypeDefOf.PawnDeath, true);
				}
				Hediff_DeathRefusal firstHediff = this.pawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
				if (this.pawn.Ideo != null && firstHediff == null)
				{
					foreach (Precept precept in this.pawn.Ideo.PreceptsListForReading)
					{
						if (!string.IsNullOrWhiteSpace(precept.def.extraTextPawnDeathLetter))
						{
							taggedString += "\n\n" + precept.def.extraTextPawnDeathLetter.Formatted(this.pawn.Named("PAWN"));
						}
					}
				}
				if (firstHediff != null)
				{
					taggedString += "\n\n" + "SelfResurrectText".Translate(this.pawn.Named("PAWN"));
				}
				if (this.pawn.Name != null && !this.pawn.Name.Numerical && this.pawn.RaceProps.Animal)
				{
					taggedString2 += " (" + this.pawn.KindLabel + ")";
				}
				Pawn_RelationsTracker relations = this.pawn.relations;
				if (relations != null)
				{
					relations.CheckAppendBondedAnimalDiedInfo(ref taggedString, ref taggedString2);
				}
				Find.LetterStack.ReceiveLetter(taggedString2, taggedString, LetterDefOf.Death, this.pawn, null, quest, null, null, 0, true);
				return;
			}
			Messages.Message(taggedString, this.pawn, MessageTypeDefOf.PawnDeath, true);
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x000E04B0 File Offset: 0x000DE6B0
		public void Notify_Resurrected(bool restoreMissingParts = true, float gettingScarsChance = 0f)
		{
			if (gettingScarsChance > 0f)
			{
				for (int i = 0; i < this.hediffSet.hediffs.Count; i++)
				{
					Hediff_Injury hediff_Injury = this.hediffSet.hediffs[i] as Hediff_Injury;
					if (hediff_Injury != null && !this.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff_Injury.Part))
					{
						HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff_Injury.TryGetComp<HediffComp_GetsPermanent>();
						if (hediffComp_GetsPermanent != null && !hediffComp_GetsPermanent.IsPermanent && Rand.Chance(gettingScarsChance))
						{
							hediffComp_GetsPermanent.IsPermanent = true;
							hediff_Injury.Severity = Mathf.Min(hediff_Injury.Severity, (float)Rand.RangeInclusive(2, 6));
						}
					}
				}
			}
			this.healthState = PawnHealthState.Mobile;
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x.TryGetComp<HediffComp_Immunizable>() != null);
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && (x.IsLethal || x.IsAnyStageLifeThreatening()));
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.forceRemoveOnResurrection);
			if (!this.pawn.RaceProps.IsMechanoid)
			{
				this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x is Hediff_Injury && !x.IsPermanent());
			}
			else
			{
				this.tmpMechInjuries.Clear();
				this.hediffSet.GetHediffs<Hediff_Injury>(ref this.tmpMechInjuries, (Hediff_Injury x) => x != null && x.def.everCurableByItem && !x.IsPermanent());
				if (this.tmpMechInjuries.Count > 0)
				{
					float num = this.tmpMechInjuries.Sum((Hediff_Injury x) => x.Severity) * 0.5f / (float)this.tmpMechInjuries.Count;
					for (int j = 0; j < this.tmpMechInjuries.Count; j++)
					{
						this.tmpMechInjuries[j].Severity -= num;
					}
					this.tmpMechInjuries.Clear();
				}
			}
			this.hediffSet.hediffs.RemoveAll((Hediff x) => x.def.everCurableByItem && x is Hediff_Injury && x.IsPermanent() && this.hediffSet.GetPartHealth(x.Part) <= 0f);
			if (restoreMissingParts)
			{
				for (;;)
				{
					Hediff_MissingPart hediff_MissingPart = this.hediffSet.GetMissingPartsCommonAncestors().FirstOrDefault((Hediff_MissingPart x) => !this.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(x.Part));
					if (hediff_MissingPart == null)
					{
						break;
					}
					this.RestorePart(hediff_MissingPart.Part, null, false);
				}
			}
			for (int k = this.hediffSet.hediffs.Count - 1; k >= 0; k--)
			{
				this.hediffSet.hediffs[k].Notify_Resurrected();
			}
			this.hediffSet.DirtyCache();
			if (this.ShouldBeDead())
			{
				this.hediffSet.hediffs.RemoveAll((Hediff h) => !h.def.keepOnBodyPartRestoration);
			}
			this.Notify_HediffChanged(null);
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x000E07C4 File Offset: 0x000DE9C4
		public void HealthTick()
		{
			if (this.Dead)
			{
				return;
			}
			Pawn_HealthTracker.tmpRemovedHediffs.Clear();
			Pawn_HealthTracker.tmpHediffs.Clear();
			Pawn_HealthTracker.tmpHediffs.AddRange(this.hediffSet.hediffs);
			foreach (Hediff hediff in Pawn_HealthTracker.tmpHediffs)
			{
				if (!Pawn_HealthTracker.tmpRemovedHediffs.Contains(hediff))
				{
					try
					{
						hediff.Tick();
						hediff.PostTick();
					}
					catch (Exception value)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Exception ticking hediff ");
						defaultInterpolatedStringHandler.AppendFormatted(hediff.ToStringSafe<Hediff>());
						defaultInterpolatedStringHandler.AppendLiteral(" for pawn ");
						defaultInterpolatedStringHandler.AppendFormatted(this.pawn.ToStringSafe<Pawn>());
						defaultInterpolatedStringHandler.AppendLiteral(". Removing hediff... Exception: ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						try
						{
							this.RemoveHediff(hediff);
						}
						catch (Exception value2)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(29, 1);
							defaultInterpolatedStringHandler2.AppendLiteral("Error while removing hediff: ");
							defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value2);
							Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
						}
					}
					if (this.Dead)
					{
						return;
					}
				}
			}
			for (int i = this.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff2 = this.hediffSet.hediffs[i];
				if (hediff2.ShouldRemove)
				{
					this.RemoveHediff(hediff2);
				}
			}
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x000E096C File Offset: 0x000DEB6C
		public void HealthTickInterval(int delta)
		{
			if (this.Dead)
			{
				return;
			}
			Pawn_HealthTracker.tmpRemovedHediffs.Clear();
			Pawn_HealthTracker.tmpHediffs.Clear();
			Pawn_HealthTracker.tmpHediffs.AddRange(this.hediffSet.hediffs);
			foreach (Hediff hediff in Pawn_HealthTracker.tmpHediffs)
			{
				if (!Pawn_HealthTracker.tmpRemovedHediffs.Contains(hediff))
				{
					try
					{
						hediff.TickInterval(delta);
						hediff.PostTickInterval(delta);
					}
					catch (Exception value)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Exception interval ticking hediff ");
						defaultInterpolatedStringHandler.AppendFormatted(hediff.ToStringSafe<Hediff>());
						defaultInterpolatedStringHandler.AppendLiteral(" for pawn ");
						defaultInterpolatedStringHandler.AppendFormatted(this.pawn.ToStringSafe<Pawn>());
						defaultInterpolatedStringHandler.AppendLiteral(". Removing hediff... Exception: ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						try
						{
							this.RemoveHediff(hediff);
						}
						catch (Exception value2)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(29, 1);
							defaultInterpolatedStringHandler2.AppendLiteral("Error while removing hediff: ");
							defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value2);
							Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
						}
					}
					if (this.Dead)
					{
						return;
					}
				}
			}
			for (int i = this.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff2 = this.hediffSet.hediffs[i];
				if (hediff2.ShouldRemove)
				{
					this.RemoveHediff(hediff2);
				}
			}
			if (this.Dead)
			{
				return;
			}
			this.immunity.ImmunityHandlerTickInterval(delta);
			if (this.pawn.Spawned && this.pawn.Crawling && this.pawn.MapHeld.reservationManager.IsReservedAndRespected(this.pawn, this.pawn))
			{
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			if ((this.pawn.RaceProps.IsFlesh || this.pawn.RaceProps.IsAnomalyEntity) && this.pawn.IsHashIntervalTick(600, delta) && (this.pawn.needs.food == null || !this.pawn.needs.food.Starving))
			{
				bool flag = false;
				if (this.hediffSet.HasNaturallyHealingInjury())
				{
					float num = 8f;
					if (this.pawn.GetPosture() != PawnPosture.Standing)
					{
						num += 4f;
						Building_Bed building_Bed = this.pawn.CurrentBed();
						if (building_Bed != null)
						{
							num += building_Bed.def.building.bed_healPerDay;
						}
					}
					foreach (Hediff hediff3 in this.hediffSet.hediffs)
					{
						HediffStage curStage = hediff3.CurStage;
						if (curStage != null && curStage.naturalHealingFactor != -1f)
						{
							num *= curStage.naturalHealingFactor;
						}
					}
					this.hediffSet.GetHediffs<Hediff_Injury>(ref this.tmpHediffInjuries, (Hediff_Injury h) => h.CanHealNaturally());
					this.tmpHediffInjuries.RandomElement<Hediff_Injury>().Heal(num * this.pawn.HealthScale * 0.01f * this.pawn.GetStatValue(StatDefOf.InjuryHealingFactor, true, -1));
					flag = true;
				}
				if (this.hediffSet.HasTendedAndHealingInjury())
				{
					Need_Food food = this.pawn.needs.food;
					if (food == null || !food.Starving)
					{
						this.hediffSet.GetHediffs<Hediff_Injury>(ref this.tmpHediffInjuries, (Hediff_Injury h) => h.CanHealFromTending());
						Hediff_Injury hediff_Injury = this.tmpHediffInjuries.RandomElement<Hediff_Injury>();
						float tendQuality = hediff_Injury.TryGetComp<HediffComp_TendDuration>().tendQuality;
						float num2 = GenMath.LerpDouble(0f, 1f, 0.5f, 1.5f, Mathf.Clamp01(tendQuality));
						hediff_Injury.Heal(8f * num2 * this.pawn.HealthScale * 0.01f * this.pawn.GetStatValue(StatDefOf.InjuryHealingFactor, true, -1));
						flag = true;
					}
				}
				if (flag && !this.HasHediffsNeedingTendByPlayer(false) && !HealthAIUtility.ShouldSeekMedicalRest(this.pawn) && PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageFullyHealed".Translate(this.pawn.LabelCap, this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			if ((this.pawn.RaceProps.IsFlesh || this.pawn.RaceProps.IsAnomalyEntity) && this.pawn.IsHashIntervalTick(15, delta) && ModsConfig.AnomalyActive && this.hediffSet.HasRegeneration)
			{
				float num3 = 0f;
				foreach (Hediff hediff4 in this.hediffSet.hediffs)
				{
					if (hediff4.CurStage != null)
					{
						num3 += hediff4.CurStage.regeneration;
					}
				}
				num3 *= 0.00025f;
				if (num3 > 0f)
				{
					this.hediffSet.GetHediffs<Hediff_Injury>(ref this.tmpHediffInjuries, (Hediff_Injury h) => true);
					foreach (Hediff_Injury hediff_Injury2 in this.tmpHediffInjuries)
					{
						float num4 = Mathf.Min(num3, hediff_Injury2.Severity);
						num3 -= num4;
						hediff_Injury2.Heal(num4);
						this.hediffSet.Notify_Regenerated(num4);
						if (num3 <= 0f)
						{
							break;
						}
					}
					if (num3 > 0f)
					{
						this.hediffSet.GetHediffs<Hediff_MissingPart>(ref this.tmpHediffMissing, (Hediff_MissingPart h) => h.Part.parent != null && !this.tmpHediffInjuries.Any((Hediff_Injury x) => x.Part == h.Part.parent) && this.hediffSet.GetFirstHediffMatchingPart<Hediff_MissingPart>(h.Part.parent) == null && this.hediffSet.GetFirstHediffMatchingPart<Hediff_AddedPart>(h.Part.parent) == null);
						using (List<Hediff_MissingPart>.Enumerator enumerator3 = this.tmpHediffMissing.GetEnumerator())
						{
							if (enumerator3.MoveNext())
							{
								Hediff_MissingPart hediff_MissingPart = enumerator3.Current;
								BodyPartRecord part = hediff_MissingPart.Part;
								this.RemoveHediff(hediff_MissingPart);
								Hediff hediff5 = this.AddHediff(HediffDefOf.Misc, part, null, null);
								float partHealth = this.hediffSet.GetPartHealth(part);
								hediff5.Severity = Mathf.Max(partHealth - 1f, partHealth * 0.9f);
								this.hediffSet.Notify_Regenerated(partHealth - hediff5.Severity);
							}
						}
					}
				}
			}
			if (this.CanBleed && this.hediffSet.BleedRateTotal >= 0.1f && (this.pawn.Spawned || this.pawn.ParentHolder is Pawn_CarryTracker) && this.pawn.SpawnedOrAnyParentSpawned)
			{
				if (this.pawn.Crawling && this.pawn.Spawned)
				{
					if (this.lastSmearDropPos == null || Vector3.Distance(this.pawn.DrawPos, this.lastSmearDropPos.Value) > Pawn_HealthTracker.BloodFilthDropDistanceRangeFromBleedRate.LerpThroughRange(this.hediffSet.BleedRateTotal))
					{
						this.DropBloodSmear();
					}
				}
				else
				{
					this.lastSmearDropPos = null;
					float num5 = this.hediffSet.BleedRateTotal * this.pawn.BodySize;
					if (this.pawn.GetPosture() == PawnPosture.Standing)
					{
						num5 *= 0.004f;
					}
					else
					{
						num5 *= 0.0004f;
					}
					if (Rand.Chance(num5 * (float)delta))
					{
						this.DropBloodFilth();
					}
				}
			}
			if (this.pawn.IsHashIntervalTick(60, delta))
			{
				List<HediffGiverSetDef> hediffGiverSets = this.pawn.RaceProps.hediffGiverSets;
				if (hediffGiverSets != null)
				{
					for (int j = 0; j < hediffGiverSets.Count; j++)
					{
						List<HediffGiver> hediffGivers = hediffGiverSets[j].hediffGivers;
						for (int k = 0; k < hediffGivers.Count; k++)
						{
							hediffGivers[k].OnIntervalPassed(this.pawn, null);
							if (this.pawn.Dead)
							{
								return;
							}
						}
					}
				}
				if (this.pawn.story != null && !this.pawn.IsWorldPawn())
				{
					List<Trait> allTraits = this.pawn.story.traits.allTraits;
					for (int l = 0; l < allTraits.Count; l++)
					{
						if (!allTraits[l].Suppressed)
						{
							TraitDegreeData currentData = allTraits[l].CurrentData;
							if (currentData.randomDiseaseMtbDays > 0f && Rand.MTBEventOccurs(currentData.randomDiseaseMtbDays, 60000f, 60f))
							{
								BiomeDef biome = this.pawn.Tile.Valid ? Find.WorldGrid[this.pawn.Tile].PrimaryBiome : DefDatabase<BiomeDef>.GetRandom();
								IncidentDef incidentDef = (from d in DefDatabase<IncidentDef>.AllDefs
								where d.category == IncidentCategoryDefOf.DiseaseHuman
								select d).RandomElementByWeightWithFallback((IncidentDef d) => biome.CommonalityOfDisease(d), null);
								if (incidentDef != null)
								{
									string text;
									List<Pawn> list = ((IncidentWorker_Disease)incidentDef.Worker).ApplyToPawns(Gen.YieldSingle<Pawn>(this.pawn), out text);
									if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
									{
										if (list.Contains(this.pawn))
										{
											Find.LetterStack.ReceiveLetter("LetterLabelTraitDisease".Translate(incidentDef.diseaseIncident.label), "LetterTraitDisease".Translate(this.pawn.LabelCap, incidentDef.diseaseIncident.label, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true), LetterDefOf.NegativeEvent, this.pawn, null, null, null, null, 0, true);
										}
										else if (!text.NullOrEmpty())
										{
											Messages.Message(text, this.pawn, MessageTypeDefOf.NeutralEvent, true);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x000E1444 File Offset: 0x000DF644
		public bool HasHediffsNeedingTend(bool forAlert = false)
		{
			return this.hediffSet.HasTendableHediff(forAlert);
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x000E1454 File Offset: 0x000DF654
		public bool HasHediffsNeedingTendByPlayer(bool forAlert = false)
		{
			if (this.HasHediffsNeedingTend(forAlert))
			{
				if (this.pawn.NonHumanlikeOrWildMan())
				{
					if (this.pawn.Faction == Faction.OfPlayer)
					{
						return true;
					}
					Building_Bed building_Bed = this.pawn.CurrentBed();
					if (building_Bed != null && building_Bed.Faction == Faction.OfPlayer)
					{
						return true;
					}
					if (this.pawn.IsOnHoldingPlatform)
					{
						return true;
					}
				}
				else
				{
					if (this.pawn.IsOnHoldingPlatform)
					{
						return true;
					}
					if ((this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null) || this.pawn.HostFaction == Faction.OfPlayer)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x000E1500 File Offset: 0x000DF700
		public void DropBloodFilth()
		{
			if ((this.pawn.Spawned || this.pawn.ParentHolder is Pawn_CarryTracker) && this.pawn.SpawnedOrAnyParentSpawned)
			{
				ThingDef thingDef = this.pawn.IsMutant ? (this.pawn.mutant.Def.bloodDef ?? this.pawn.RaceProps.BloodDef) : this.pawn.RaceProps.BloodDef;
				if (thingDef != null)
				{
					FilthMaker.TryMakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, thingDef, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
				}
			}
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x000E15B4 File Offset: 0x000DF7B4
		public void DropBloodSmear()
		{
			ThingDef thingDef = this.pawn.IsMutant ? (this.pawn.mutant.Def.bloodSmearDef ?? this.pawn.RaceProps.BloodSmearDef) : this.pawn.RaceProps.BloodSmearDef;
			if (thingDef == null)
			{
				this.lastSmearDropPos = new Vector3?(this.pawn.DrawPos);
				return;
			}
			Filth filth;
			FilthMaker.TryMakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, thingDef, out filth, this.pawn.LabelIndefinite(), FilthSourceFlags.None, false);
			if (filth != null)
			{
				float rotation;
				if (this.lastSmearDropPos != null)
				{
					rotation = (this.lastSmearDropPos.Value - this.pawn.DrawPos).AngleFlat();
				}
				else
				{
					rotation = this.pawn.pather.lastMoveDirection;
				}
				filth.SetOverrideDrawPositionAndRotation(this.pawn.DrawPos.WithY(thingDef.Altitude), rotation);
				this.lastSmearDropPos = new Vector3?(this.pawn.DrawPos);
			}
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x000E16C7 File Offset: 0x000DF8C7
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_HealthTracker.<GetGizmos>d__85 <GetGizmos>d__ = new Pawn_HealthTracker.<GetGizmos>d__85(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x000E1704 File Offset: 0x000DF904
		[CompilerGenerated]
		private void <CalculateMissingPartHediffsFromInjury>g__AddAllParts|68_0(BodyPartRecord part, ref Pawn_HealthTracker.<>c__DisplayClass68_0 A_2)
		{
			Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
			hediff_MissingPart.lastInjury = A_2.hediff.def;
			hediff_MissingPart.Part = part;
			A_2.missing.Add(hediff_MissingPart);
			foreach (BodyPartRecord part2 in part.parts)
			{
				this.<CalculateMissingPartHediffsFromInjury>g__AddAllParts|68_0(part2, ref A_2);
			}
		}

		// Token: 0x04001AC4 RID: 6852
		private const float CrawlingManipulationRequirement = 0.15f;

		// Token: 0x04001AC5 RID: 6853
		private const int CrawlingAgeRequirement = 8;

		// Token: 0x04001AC6 RID: 6854
		private static FloatRange BloodFilthDropDistanceRangeFromBleedRate = new FloatRange(0.7f, 0.25f);

		// Token: 0x04001AC7 RID: 6855
		private readonly Pawn pawn;

		// Token: 0x04001AC8 RID: 6856
		private PawnHealthState healthState = PawnHealthState.Mobile;

		// Token: 0x04001AC9 RID: 6857
		[Unsaved(false)]
		public Effecter woundedEffecter;

		// Token: 0x04001ACA RID: 6858
		[Unsaved(false)]
		public Effecter deflectionEffecter;

		// Token: 0x04001ACB RID: 6859
		[LoadAlias("forceIncap")]
		public bool forceDowned;

		// Token: 0x04001ACC RID: 6860
		public bool beCarriedByCaravanIfSick;

		// Token: 0x04001ACD RID: 6861
		public bool killedByRitual;

		// Token: 0x04001ACE RID: 6862
		public int lastReceivedNeuralSuperchargeTick = -1;

		// Token: 0x04001ACF RID: 6863
		public float overrideDeathOnDownedChance = -1f;

		// Token: 0x04001AD0 RID: 6864
		private Vector3? lastSmearDropPos;

		// Token: 0x04001AD1 RID: 6865
		public bool isBeingKilled;

		// Token: 0x04001AD2 RID: 6866
		public bool couldCrawl;

		// Token: 0x04001AD3 RID: 6867
		public HediffSet hediffSet;

		// Token: 0x04001AD4 RID: 6868
		public PawnCapacitiesHandler capacities;

		// Token: 0x04001AD5 RID: 6869
		public BillStack surgeryBills;

		// Token: 0x04001AD6 RID: 6870
		public SummaryHealthHandler summaryHealth;

		// Token: 0x04001AD7 RID: 6871
		public ImmunityHandler immunity;

		// Token: 0x04001AD8 RID: 6872
		private List<Hediff_Injury> tmpMechInjuries = new List<Hediff_Injury>();

		// Token: 0x04001AD9 RID: 6873
		private List<Hediff_Injury> tmpHediffInjuries = new List<Hediff_Injury>();

		// Token: 0x04001ADA RID: 6874
		private List<Hediff_MissingPart> tmpHediffMissing = new List<Hediff_MissingPart>();

		// Token: 0x04001ADB RID: 6875
		private static readonly List<Hediff> tmpHediffs = new List<Hediff>(100);

		// Token: 0x04001ADC RID: 6876
		private static readonly HashSet<Hediff> tmpRemovedHediffs = new HashSet<Hediff>();
	}
}
