using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020001EF RID: 495
	public class UnnaturalCorpseTracker : IExposable, ILoadReferenceable
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x00048593 File Offset: 0x00046793
		public UnnaturalCorpse Corpse
		{
			get
			{
				return this.corpse;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0004859B File Offset: 0x0004679B
		public bool CorpseSpawned
		{
			get
			{
				return this.corpse != null;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x000485A6 File Offset: 0x000467A6
		public Pawn AwokenPawn
		{
			get
			{
				return this.awokenPawn;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x000485AE File Offset: 0x000467AE
		public int SpawnedTicks
		{
			get
			{
				return GenTicks.TicksGame - this.spawnedTick;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000DFF RID: 3583 RVA: 0x000485BC File Offset: 0x000467BC
		public int TicksToKill
		{
			get
			{
				return this.ticksToKill;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x000485C4 File Offset: 0x000467C4
		public bool ShouldDisappear
		{
			get
			{
				return this.awokenFinished || (this.disappearTick != 0 && GenTicks.TicksGame >= this.disappearTick);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000E01 RID: 3585 RVA: 0x000485EA File Offset: 0x000467EA
		public Pawn Haunted
		{
			get
			{
				return this.haunted;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x000485F2 File Offset: 0x000467F2
		public bool CanDestroyViaResearch
		{
			get
			{
				return this.research >= this.researchToDestroy;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x00048605 File Offset: 0x00046805
		public IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0004860D File Offset: 0x0004680D
		public UnnaturalCorpseTracker()
		{
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00048620 File Offset: 0x00046820
		public UnnaturalCorpseTracker(Pawn haunted, UnnaturalCorpse corpse)
		{
			this.haunted = haunted;
			this.corpse = corpse;
			this.loadID = haunted.thingIDNumber;
			this.researchToDestroy = UnnaturalCorpseTracker.DestroyResearchRequiredRange.RandomInRange;
			this.awakenTick = GenTicks.TicksGame + Mathf.CeilToInt(UnnaturalCorpseTracker.GrowingLetterDelayDays.RandomInRange * 60000f);
			this.spawnedTick = GenTicks.TicksGame;
			corpse.LinkToTracker(this);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x000486A4 File Offset: 0x000468A4
		public void Notify_Finished()
		{
			this.removing = true;
			if (this.awokenPawn != null && this.awokenPawn.Spawned)
			{
				Messages.Message("MessageAwokenDisappeared".Translate(), MessageTypeDefOf.NeutralEvent, true);
				SkipUtility.SkipDeSpawn(this.awokenPawn);
				this.awokenPawn.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.Corpse != null)
			{
				this.Corpse.DoDisappear();
			}
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00048712 File Offset: 0x00046912
		public void Notify_CorpseDestroyed()
		{
			if (!this.removing && !this.Haunted.DestroyedOrNull())
			{
				this.applyThoughtNextTick = true;
			}
			this.lastSpawnInCheck = GenTicks.TicksGame;
			this.corpse = null;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00048744 File Offset: 0x00046944
		public void Notify_AwokenAttackStarting()
		{
			if (this.awokenPawn == null)
			{
				return;
			}
			Hediff hd;
			if (!this.awokenPawn.health.hediffSet.TryGetHediff(HediffDefOf.AwokenCorpse, out hd))
			{
				return;
			}
			hd.TryGetComp<HediffComp_MessageStageIncreased>().sendMessages = false;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00048788 File Offset: 0x00046988
		private void ApplyCorpseDestroyedThoughts()
		{
			this.applyThoughtNextTick = false;
			Pawn_NeedsTracker needs = this.haunted.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.UnnaturalCorpseDestroyed, null, null);
						}
					}
				}
			}
			Hediff hd;
			if (this.haunted.health.hediffSet.TryGetHediff(HediffDefOf.CorpseTorment, out hd))
			{
				hd.TryGetComp<HediffComp_Disappears>().ResetElapsedTicks();
				return;
			}
			this.haunted.health.AddHediff(HediffDefOf.CorpseTorment, null, null, null);
			Messages.Message("MessagePawnFainted".Translate(this.Haunted.Named("PAWN")), this.Haunted, MessageTypeDefOf.NegativeHealthEvent, true);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0004885A File Offset: 0x00046A5A
		public void Notify_PawnDied()
		{
			if (this.disappearTick != 0)
			{
				return;
			}
			if (this.awoken)
			{
				this.disappearTick = GenTicks.TicksGame + 600;
				return;
			}
			this.disappearTick = GenTicks.TicksGame + 7500;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00048890 File Offset: 0x00046A90
		public void Notify_PawnKilledViaAwoken()
		{
			this.disappearTick = GenTicks.TicksGame + 600;
			Messages.Message("MessageAwokenKilledVictim".Translate(this.Haunted.Named("PAWN")), this.Haunted, MessageTypeDefOf.NegativeEvent, true);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000488E4 File Offset: 0x00046AE4
		public void CorpseTick()
		{
			if (this.disappearTick == 0 && this.Haunted.SpawnedOrAnyParentSpawned && this.Haunted.IsMutant)
			{
				this.disappearTick = GenTicks.TicksGame + 7500;
			}
			if (this.awoken)
			{
				this.AwokenTick();
				return;
			}
			if (this.applyThoughtNextTick)
			{
				this.ApplyCorpseDestroyedThoughts();
			}
			this.UpdateCorpseState();
			this.UpdateMentalBreak();
			this.AwakeningTick();
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00048954 File Offset: 0x00046B54
		private void UpdateMentalBreak()
		{
			if (!this.Haunted.SpawnedOrAnyParentSpawned || !this.CorpseSpawned)
			{
				return;
			}
			if (!this.mentalBreak && !this.Haunted.InMentalState && GenTicks.TicksGame >= this.minMentalBreakRefireTick && Rand.MTBEventOccurs(5f, 60000f, 1f))
			{
				this.mentalBreak = true;
			}
			if (this.mentalBreak)
			{
				this.TryTriggerMentalBreak();
			}
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x000489C4 File Offset: 0x00046BC4
		public void TryTriggerMentalBreak()
		{
			if (MentalBreakDefOf.CorpseObsession.Worker.BreakCanOccur(this.Haunted))
			{
				this.mentalBreak = false;
				this.minMentalBreakRefireTick = GenTicks.TicksGame + 150000;
				this.Haunted.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_MysteriousCorpse".Translate(), MentalBreakDefOf.CorpseObsession);
				return;
			}
			MentalBreakDef breakDef;
			if (this.Haunted.mindState.mentalBreaker.TryGetRandomMentalBreak(MentalBreakIntensity.Major, out breakDef))
			{
				this.mentalBreak = false;
				this.minMentalBreakRefireTick = GenTicks.TicksGame + 150000;
				this.Haunted.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_MysteriousCorpse".Translate(), breakDef);
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00048A84 File Offset: 0x00046C84
		private void AwokenTick()
		{
			if (this.awokenFinished)
			{
				return;
			}
			if (this.awokenPawn != null)
			{
				if (this.awokenPawn.Downed && !this.awokenPawn.Dead && !this.awokenPawn.health.hediffSet.HasHediff(HediffDefOf.RapidRegeneration, false))
				{
					this.awokenPawn.Kill(null, null);
				}
				if (this.awokenPawn.Dead)
				{
					TaggedString label = "UnnaturalCorpseAwokenDefeatedLetter".Translate();
					TaggedString text = "UnnaturalCorpseAwokenDefeatedLetterDesc".Translate(this.Haunted.Named("PAWN"));
					Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, this.awokenPawn, null, null, null, null, 0, true);
					if (this.awokenPawn.SpawnedOrAnyParentSpawned)
					{
						Thing thing;
						GenDrop.TryDropSpawn(ThingMaker.MakeThing(ThingDefOf.Shard, null), this.awokenPawn.PositionHeld, this.awokenPawn.MapHeld, ThingPlaceMode.Near, out thing, null, null, true);
						FleshbeastUtility.MeatSplatter(3, this.awokenPawn.PositionHeld, this.awokenPawn.MapHeld, FleshbeastUtility.MeatExplosionSize.Normal);
						this.awokenPawn.Corpse.Destroy(DestroyMode.Vanish);
					}
					this.awokenFinished = true;
					return;
				}
			}
			if ((this.awokenPawn.DestroyedOrNull() || !this.awokenPawn.SpawnedOrAnyParentSpawned) && this.Haunted.SpawnedOrAnyParentSpawned)
			{
				if (!this.teleportIn)
				{
					int num = Mathf.Max(GenTicks.TicksGame - this.lastSpawnInCheck, 1);
					this.lastSpawnInCheck = GenTicks.TicksGame;
					if (Rand.MTBEventOccurs(6f, 2500f, (float)num))
					{
						this.teleportIn = true;
					}
				}
				IntVec3 cell;
				if (this.teleportIn && CellFinder.TryFindRandomReachableNearbyCell(this.Haunted.PositionHeld, this.Haunted.MapHeld, 70f, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false), new Predicate<IntVec3>(this.IsValidAwokenSpawnCell), null, out cell, 999999))
				{
					this.teleportIn = false;
					this.awokenPawn = this.MakeNewAwoken();
					SkipUtility.SkipTo(this.awokenPawn, cell, this.Haunted.MapHeld);
					this.TurnAwoken(this.awokenPawn);
					Messages.Message("MessageAwokenReappeared".Translate(this.Haunted.Named("PAWN")), this.awokenPawn, MessageTypeDefOf.NegativeEvent, true);
					Find.MusicManagerPlay.CheckTransitions();
					return;
				}
			}
			else if (!this.awokenPawn.DestroyedOrNull() && this.awokenPawn.Spawned && !this.Haunted.DestroyedOrNull() && this.Haunted.MapHeld != this.awokenPawn.Map)
			{
				SkipUtility.SkipDeSpawn(this.awokenPawn);
				this.awokenPawn.Destroy(DestroyMode.Vanish);
				this.awokenPawn = null;
				Messages.Message("MessageAwokenVanished".Translate(this.Haunted.Named("PAWN")), this.awokenPawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00048D88 File Offset: 0x00046F88
		private bool IsValidAwokenSpawnCell(IntVec3 cell)
		{
			Map mapHeld = this.Haunted.MapHeld;
			return cell.Standable(mapHeld) && !cell.Fogged(mapHeld);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00048DB8 File Offset: 0x00046FB8
		private void UpdateCorpseState()
		{
			if (this.Haunted.Dead)
			{
				return;
			}
			if (this.corpse == null && this.Haunted.SpawnedOrAnyParentSpawned)
			{
				if (!this.teleportIn)
				{
					int num = Mathf.Max(GenTicks.TicksGame - this.lastSpawnInCheck, 1);
					this.lastSpawnInCheck = GenTicks.TicksGame;
					if (Rand.MTBEventOccurs(24f, 2500f, (float)num))
					{
						this.teleportIn = true;
					}
				}
				IntVec3 cell;
				if (this.teleportIn && AnomalyUtility.TryGetNearbyUnseenCell(this.Haunted, out cell))
				{
					this.SpawnNewCorpse(cell);
				}
			}
			if (this.corpse != null && this.InEscapableContainer())
			{
				if (!this.escape && Rand.MTBEventOccurs(24f, 2500f, 1f))
				{
					this.escape = true;
				}
				if (this.escape && AnomalyUtility.IsValidUnseenCell(Find.CameraDriver.CurrentViewRect.ExpandedBy(1), this.corpse.PositionHeld, this.corpse.MapHeld))
				{
					this.BreakoutOfContainer();
				}
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00048EBC File Offset: 0x000470BC
		private void AwakeningTick()
		{
			if (this.CorpseSpawned && this.rising && this.Corpse.Spawned)
			{
				if (this.riseSustainer == null || this.riseSustainer.Ended)
				{
					SoundInfo info = SoundInfo.InMap(this.Corpse, MaintenanceType.PerTick);
					this.riseSustainer = SoundDefOf.Pawn_Shambler_Rise.TrySpawnSustainer(info);
				}
				if (this.riseEffecter == null)
				{
					this.riseEffecter = EffecterDefOf.ShamblerRaise.Spawn(this.Corpse, this.Corpse.Map, 1f);
				}
				this.riseSustainer.Maintain();
				this.riseEffecter.EffectTick(this.Corpse, TargetInfo.Invalid);
			}
			if (this.awoken || GenTicks.TicksGame <= this.awakenTick)
			{
				return;
			}
			if (this.CorpseSpawned && this.Haunted.SpawnedOrAnyParentSpawned && !this.awoken && !this.sentAwakeningLetter)
			{
				this.sentAwakeningLetter = true;
				this.awakenTick = GenTicks.TicksGame + Mathf.CeilToInt(UnnaturalCorpseTracker.AwakenDelayDays.RandomInRange * 60000f);
				TaggedString label = "UnnaturalCorpseAwakeningLetter".Translate();
				TaggedString taggedString = "UnnaturalCorpseAwakeningLetterDesc".Translate(this.Haunted.Named("PAWN"));
				if (this.CanDestroyViaResearch)
				{
					taggedString += "\n\n" + "UnnaturalCorpseAwakeningLetterExtra_StudyComplete".Translate();
				}
				else
				{
					taggedString += "\n\n" + "UnnaturalCorpseAwakeningLetterExtra_StudyIncomplete".Translate();
				}
				Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, this.corpse, null, null, null, null, 0, true);
				return;
			}
			if (this.CorpseSpawned && this.Haunted.SpawnedOrAnyParentSpawned && this.Corpse.MapHeld == this.Haunted.MapHeld && !this.Haunted.Downed && this.Haunted.Awake() && !this.rising)
			{
				this.rising = true;
				this.awakenTick = GenTicks.TicksGame + 420;
				this.Corpse.InnerPawn.Drawer.renderer.SetAnimation(AnimationDefOf.ShamblerRise);
				TaggedString label2 = "UnnaturalCorpseAwokenLetter".Translate();
				TaggedString text = "UnnaturalCorpseAwokenLetterDesc".Translate(this.Haunted.Named("PAWN"));
				Find.LetterStack.ReceiveLetter(label2, text, LetterDefOf.ThreatBig, this.awokenPawn, null, null, null, null, 0, true);
				Find.TickManager.slower.SignalForceNormalSpeed();
				return;
			}
			if (this.CorpseSpawned)
			{
				this.awoken = true;
				IntVec3 positionHeld = this.Corpse.PositionHeld;
				this.Corpse.Destroy(DestroyMode.Vanish);
				this.awokenPawn = this.MakeNewAwoken();
				GenSpawn.Spawn(this.awokenPawn, positionHeld, this.Haunted.MapHeld, WipeMode.Vanish);
				this.TurnAwoken(this.awokenPawn);
				this.awokenPawn.Drawer.renderer.SetAnimation(null);
				float numSeconds = UnnaturalCorpseTracker.CombatPointsSecondsToKillCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.Haunted.MapHeld));
				this.ticksToKill = numSeconds.SecondsToTicks();
				Find.MusicManagerPlay.CheckTransitions();
			}
			this.awakenTick = GenTicks.TicksGame + 2500;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00049224 File Offset: 0x00047424
		private void SpawnNewCorpse(IntVec3 cell)
		{
			this.teleportIn = false;
			UnnaturalCorpse unnaturalCorpse = AnomalyUtility.MakeUnnaturalCorpse(this.Haunted);
			unnaturalCorpse.LinkToTracker(this);
			GenSpawn.Spawn(unnaturalCorpse, cell, this.Haunted.MapHeld, WipeMode.Vanish);
			this.spawnedTick = GenTicks.TicksGame;
			this.corpse = unnaturalCorpse;
			this.corpse.Forbiddable.Forbidden = true;
			TaggedString label = "LetterLabelCorpseReappeared".Translate();
			TaggedString text = "LetterCorpseReappeared".Translate(this.Haunted.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, this.corpse, null, null, null, null, 0, true);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000492CA File Offset: 0x000474CA
		private Pawn MakeNewAwoken()
		{
			Pawn pawn = Find.PawnDuplicator.Duplicate(this.Haunted);
			pawn.apparel.DestroyAll(DestroyMode.Vanish);
			return pawn;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000492E8 File Offset: 0x000474E8
		private void TurnAwoken(Pawn pawn)
		{
			MutantUtility.SetPawnAsMutantInstantly(pawn, MutantDefOf.AwokenCorpse, pawn.GetRotStage());
			if (pawn.Faction != Faction.OfEntities)
			{
				pawn.SetFaction(Faction.OfEntities, null);
			}
			pawn.health.hediffSet.GetFirstHediff<Hediff_RapidRegeneration>().SetHpCapacity(UnnaturalCorpseTracker.CombatPointsPerHpRegenCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.Haunted.MapHeld)));
			Pawn_AbilityTracker abilities = pawn.abilities;
			Ability ability = (abilities != null) ? abilities.GetAbility(AbilityDefOf.UnnaturalCorpseSkip, true) : null;
			if (ability != null)
			{
				ability.StartCooldown(ability.def.cooldownTicksRange.RandomInRange);
			}
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00049380 File Offset: 0x00047580
		private void BreakoutOfContainer()
		{
			if (!this.InEscapableContainer())
			{
				return;
			}
			this.escape = false;
			Thing thing = ThingOwnerUtility.SpawnedParentOrMe(this.corpse);
			Messages.Message("MessageCorpseEscaped".Translate(this.Haunted.Named("PAWN"), thing.Named("CONTAINER")), this.corpse, MessageTypeDefOf.NeutralEvent, true);
			Building_Casket building_Casket = thing as Building_Casket;
			if (building_Casket != null)
			{
				building_Casket.EjectContents();
			}
			else
			{
				Thing thing2;
				this.corpse.ParentHolder.GetDirectlyHeldThings().TryDrop(this.corpse, ThingPlaceMode.Near, 1, out thing2, null, null);
			}
			this.corpse.Forbiddable.Forbidden = true;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00049430 File Offset: 0x00047630
		public void Notify_Studied(Pawn studier, float amount)
		{
			this.research += amount;
			if (this.research >= 3f && !this.sentLetterOne)
			{
				this.sentLetterOne = true;
				this.SendLetter(studier, "UnnaturalCorpseStudyLetter", "UnnaturalCorpseStudyLetterDesc");
				return;
			}
			if (this.CanDestroyViaResearch && !this.sentLetterTwo)
			{
				this.sentLetterTwo = true;
				this.SendLetter(studier, "UnnaturalCorpseStudyCompletedLetter", "UnnaturalCorpseStudyCompletedLetterDesc");
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x000494A4 File Offset: 0x000476A4
		private void SendLetter(Pawn studier, string labelKey, string descKey)
		{
			TaggedString label = labelKey.Translate();
			TaggedString text = descKey.Translate(this.haunted.Named("VICTIM"), studier.Named("RESEARCHER"));
			Thing t = this.corpse.Spawned ? this.corpse : this.haunted;
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, t, null, null, null, null, 0, true);
			ChoiceLetter choiceLetter = LetterMaker.MakeLetter(label, text, LetterDefOf.NeutralEvent, t, null, null, null);
			choiceLetter.arrivalTick = Find.TickManager.TicksGame;
			this.letters.Add(choiceLetter);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00049545 File Offset: 0x00047745
		public void DevAwaken()
		{
			this.sentAwakeningLetter = true;
			this.awakenTick = GenTicks.TicksGame;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00049559 File Offset: 0x00047759
		public void DevUnlockDeactivation()
		{
			this.sentLetterOne = true;
			this.sentLetterTwo = true;
			this.research += this.researchToDestroy;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0004957C File Offset: 0x0004777C
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.haunted, "haunted", false);
			Scribe_References.Look<Pawn>(ref this.awokenPawn, "awokenPawn", false);
			Scribe_References.Look<UnnaturalCorpse>(ref this.corpse, "corpse", true);
			Scribe_Values.Look<int>(ref this.disappearTick, "disappearTick", 0, false);
			Scribe_Values.Look<int>(ref this.awakenTick, "awakenTick", 0, false);
			Scribe_Values.Look<bool>(ref this.sentAwakeningLetter, "sentAwakeningLetter", false, false);
			Scribe_Values.Look<int>(ref this.minMentalBreakRefireTick, "minMentalBreakRefireTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastSpawnInCheck, "lastSpawnInCheck", 0, false);
			Scribe_Values.Look<bool>(ref this.teleportIn, "teleportIn", false, false);
			Scribe_Values.Look<bool>(ref this.escape, "escape", false, false);
			Scribe_Values.Look<bool>(ref this.mentalBreak, "mentalBreak", false, false);
			Scribe_Values.Look<bool>(ref this.removing, "removing", false, false);
			Scribe_Values.Look<float>(ref this.research, "research", 0f, false);
			Scribe_Values.Look<float>(ref this.researchToDestroy, "researchToDestroy", 0f, false);
			Scribe_Values.Look<bool>(ref this.sentLetterOne, "sentLetterOne", false, false);
			Scribe_Values.Look<bool>(ref this.sentLetterTwo, "sentLetterTwo", false, false);
			Scribe_Values.Look<bool>(ref this.awoken, "awoken", false, false);
			Scribe_Values.Look<bool>(ref this.awokenFinished, "awokenFinished", false, false);
			Scribe_Values.Look<bool>(ref this.rising, "rising", false, false);
			Scribe_Values.Look<int>(ref this.ticksToKill, "ticksToKill", 0, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Collections.Look<ChoiceLetter>(ref this.letters, "letters", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00049720 File Offset: 0x00047920
		public string GetUniqueLoadID()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
			defaultInterpolatedStringHandler.AppendLiteral("MysteriousCorpseTracker_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.loadID);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00049758 File Offset: 0x00047958
		private bool InEscapableContainer()
		{
			if (this.corpse == null)
			{
				return false;
			}
			if (this.corpse.ParentHolder == null || this.corpse.ParentHolder is Map)
			{
				return false;
			}
			Building building = this.corpse.ParentHolder as Building;
			return (building != null && building.def.building.isEscapableContainer) || this.corpse.ParentHolder is CompTransporter;
		}

		// Token: 0x04000DE6 RID: 3558
		private static readonly FloatRange GrowingLetterDelayDays = new FloatRange(11f, 13f);

		// Token: 0x04000DE7 RID: 3559
		private static readonly FloatRange AwakenDelayDays = new FloatRange(1f, 3f);

		// Token: 0x04000DE8 RID: 3560
		private static readonly FloatRange DestroyResearchRequiredRange = new FloatRange(8f, 12f);

		// Token: 0x04000DE9 RID: 3561
		private const float FirstLetterResearchRequired = 3f;

		// Token: 0x04000DEA RID: 3562
		private const int PawnDiedDisappearHours = 3;

		// Token: 0x04000DEB RID: 3563
		private const int PawnKilledDisappearTicks = 600;

		// Token: 0x04000DEC RID: 3564
		private const int MTBReappearHoursCorpse = 24;

		// Token: 0x04000DED RID: 3565
		private const int MTBReappearHoursAwoken = 6;

		// Token: 0x04000DEE RID: 3566
		private const int MTBMentalBreakDays = 5;

		// Token: 0x04000DEF RID: 3567
		private const int MTBEscapeContainerHours = 24;

		// Token: 0x04000DF0 RID: 3568
		private const int MinMentalBreakRefireTicks = 150000;

		// Token: 0x04000DF1 RID: 3569
		private const int RisingTime = 420;

		// Token: 0x04000DF2 RID: 3570
		private static readonly SimpleCurve CombatPointsPerHpRegenCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 150f),
				true
			},
			{
				new CurvePoint(3000f, 800f),
				true
			}
		};

		// Token: 0x04000DF3 RID: 3571
		private static readonly SimpleCurve CombatPointsSecondsToKillCurve = new SimpleCurve
		{
			{
				new CurvePoint(300f, 25f),
				true
			},
			{
				new CurvePoint(3000f, 10f),
				true
			}
		};

		// Token: 0x04000DF4 RID: 3572
		private UnnaturalCorpse corpse;

		// Token: 0x04000DF5 RID: 3573
		private Pawn awokenPawn;

		// Token: 0x04000DF6 RID: 3574
		private Pawn haunted;

		// Token: 0x04000DF7 RID: 3575
		private int disappearTick;

		// Token: 0x04000DF8 RID: 3576
		private int lastSpawnInCheck;

		// Token: 0x04000DF9 RID: 3577
		private int loadID;

		// Token: 0x04000DFA RID: 3578
		private int awakenTick;

		// Token: 0x04000DFB RID: 3579
		private int spawnedTick;

		// Token: 0x04000DFC RID: 3580
		private int minMentalBreakRefireTick;

		// Token: 0x04000DFD RID: 3581
		private int ticksToKill;

		// Token: 0x04000DFE RID: 3582
		private float research;

		// Token: 0x04000DFF RID: 3583
		private float researchToDestroy;

		// Token: 0x04000E00 RID: 3584
		private bool teleportIn;

		// Token: 0x04000E01 RID: 3585
		private bool escape;

		// Token: 0x04000E02 RID: 3586
		private bool mentalBreak;

		// Token: 0x04000E03 RID: 3587
		private bool removing;

		// Token: 0x04000E04 RID: 3588
		private bool sentLetterOne;

		// Token: 0x04000E05 RID: 3589
		private bool sentLetterTwo;

		// Token: 0x04000E06 RID: 3590
		private bool sentAwakeningLetter;

		// Token: 0x04000E07 RID: 3591
		private bool awoken;

		// Token: 0x04000E08 RID: 3592
		private bool rising;

		// Token: 0x04000E09 RID: 3593
		private bool awokenFinished;

		// Token: 0x04000E0A RID: 3594
		private List<ChoiceLetter> letters = new List<ChoiceLetter>();

		// Token: 0x04000E0B RID: 3595
		private bool applyThoughtNextTick;

		// Token: 0x04000E0C RID: 3596
		private Effecter riseEffecter;

		// Token: 0x04000E0D RID: 3597
		private Sustainer riseSustainer;
	}
}
