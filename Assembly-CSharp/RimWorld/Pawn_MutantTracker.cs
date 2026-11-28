using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001562 RID: 5474
	public class Pawn_MutantTracker : IExposable, IVerbOwner
	{
		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x06008621 RID: 34337 RVA: 0x00307ABA File Offset: 0x00305CBA
		public MutantDef Def
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x06008622 RID: 34338 RVA: 0x00307AC2 File Offset: 0x00305CC2
		public bool HasTurned
		{
			get
			{
				return this.hasTurned;
			}
		}

		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x06008623 RID: 34339 RVA: 0x00307ACA File Offset: 0x00305CCA
		public bool IsPassive
		{
			get
			{
				return this.def.passive;
			}
		}

		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x06008624 RID: 34340 RVA: 0x00307AD7 File Offset: 0x00305CD7
		public Hediff Hediff
		{
			get
			{
				return this.mutantHediff;
			}
		}

		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x06008625 RID: 34341 RVA: 0x00307ADF File Offset: 0x00305CDF
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x06008626 RID: 34342 RVA: 0x00307AEC File Offset: 0x00305CEC
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x06008627 RID: 34343 RVA: 0x00307AF4 File Offset: 0x00305CF4
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Def.verbs;
			}
		}

		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x06008628 RID: 34344 RVA: 0x00307B01 File Offset: 0x00305D01
		public List<Tool> Tools
		{
			get
			{
				return this.Def.tools;
			}
		}

		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x06008629 RID: 34345 RVA: 0x00307B0E File Offset: 0x00305D0E
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x0600862A RID: 34346 RVA: 0x00307B15 File Offset: 0x00305D15
		public Thing ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x0600862B RID: 34347 RVA: 0x00307B1D File Offset: 0x00305D1D
		public string UniqueVerbOwnerID()
		{
			return this.pawn.GetUniqueLoadID() + "_mutant";
		}

		// Token: 0x0600862C RID: 34348 RVA: 0x00307B34 File Offset: 0x00305D34
		public bool VerbsStillUsableBy(Pawn p)
		{
			return true;
		}

		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x0600862D RID: 34349 RVA: 0x00307B38 File Offset: 0x00305D38
		public List<Ability> AllAbilitiesForReading
		{
			get
			{
				if (this.abilities == null)
				{
					this.abilities = new List<Ability>();
					foreach (AbilityDef abilityDef in this.def.abilities)
					{
						this.abilities.Add(AbilityUtility.MakeAbility(abilityDef, this.pawn));
					}
				}
				return this.abilities;
			}
		}

		// Token: 0x0600862E RID: 34350 RVA: 0x00307BBC File Offset: 0x00305DBC
		public Pawn_MutantTracker(Pawn pawn, MutantDef def, RotStage rotStage)
		{
			this.pawn = pawn;
			this.def = def;
			this.rotStage = rotStage;
			this.verbTracker = new VerbTracker(this);
			this.originalFaction = pawn.HomeFaction;
			if (pawn.RaceProps.Humanlike)
			{
				Ideo ideo;
				if ((ideo = pawn.Ideo) == null)
				{
					Faction faction = pawn.Faction;
					Ideo ideo2;
					if (faction == null)
					{
						ideo2 = null;
					}
					else
					{
						FactionIdeosTracker ideos = faction.ideos;
						ideo2 = ((ideos != null) ? ideos.PrimaryIdeo : null);
					}
					ideo = (ideo2 ?? Find.IdeoManager.IdeosListForReading.RandomElement<Ideo>());
				}
				this.originalIdeo = ideo;
				Faction faction2;
				if ((faction2 = this.originalFaction) == null)
				{
					if ((faction2 = Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.OutlanderCivil)) == null)
					{
						if ((faction2 = Find.FactionManager.AllFactionsVisible.FirstOrDefault((Faction f) => f.def == FactionDefOf.OutlanderRough)) == null)
						{
							faction2 = (from f in Find.FactionManager.AllFactionsListForReading
							where !f.IsPlayer && f.def.humanlikeFaction && !f.Hidden && !f.temporary
							select f).RandomElement<Faction>();
						}
					}
				}
				this.originalFaction = faction2;
			}
		}

		// Token: 0x0600862F RID: 34351 RVA: 0x00307CF7 File Offset: 0x00305EF7
		public Pawn_MutantTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008630 RID: 34352 RVA: 0x00307D08 File Offset: 0x00305F08
		public Pawn_MutantTracker.ThinkTrees GetThinkTrees()
		{
			return new Pawn_MutantTracker.ThinkTrees
			{
				main = this.def.thinkTree,
				constant = this.def.thinkTreeConstant
			};
		}

		// Token: 0x06008631 RID: 34353 RVA: 0x00307D42 File Offset: 0x00305F42
		public void MutantTrackerTick()
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06008632 RID: 34354 RVA: 0x00307D50 File Offset: 0x00305F50
		public void MutantTrackerTickInterval(int delta)
		{
			if (!this.pawn.Drawer.renderer.HasAnimation && this.Def.standingAnimation != null)
			{
				this.pawn.Drawer.renderer.SetAnimation(this.Def.standingAnimation);
			}
			if (this.pawn.Spawned && this.pawn.GetPosture() == PawnPosture.Standing)
			{
				if (this.rotStage == RotStage.Rotting && Rand.MTBEventOccurs(5f, 2500f, 1f))
				{
					FilthMaker.TryMakeFilth(this.pawn.PositionHeld, this.pawn.MapHeld, ThingDefOf.Filth_CorpseBile, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
				}
				float summaryHealthPercent = this.pawn.health.summaryHealth.SummaryHealthPercent;
				if (!this.Def.canBleed && this.Def.bloodDef != null && this.rotStage != RotStage.Dessicated && summaryHealthPercent < 1f && Rand.MTBEventOccurs(Pawn_MutantTracker.DropBloodMTBSecondsFromHealthPctCurve.Evaluate(summaryHealthPercent), 60f, 1f))
				{
					this.pawn.health.DropBloodFilth();
				}
			}
			if (this.Def.hediffGivers != null && this.pawn.IsHashIntervalTick(60, delta))
			{
				for (int i = 0; i < this.Def.hediffGivers.Count; i++)
				{
					this.Def.hediffGivers[i].OnIntervalPassed(this.pawn, this.mutantHediff);
				}
			}
		}

		// Token: 0x06008633 RID: 34355 RVA: 0x00307ED8 File Offset: 0x003060D8
		public void Turn(bool clearLord = false)
		{
			if (this.hasTurned)
			{
				string str = "Tried to turn mutant ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " again");
				return;
			}
			this.hasTurned = true;
			if (this.def.hediff != null)
			{
				this.mutantHediff = this.pawn.health.GetOrAddHediff(this.def.hediff, null, null, null);
			}
			if (!this.def.givesHediffs.NullOrEmpty<StartingHediff>())
			{
				HealthUtility.AddStartingHediffs(this.pawn, this.def.givesHediffs);
			}
			if (this.def.terminatePregnancy)
			{
				PregnancyUtility.ForceEndPregnancy(this.pawn, true);
			}
			if (this.pawn.caller == null)
			{
				this.pawn.caller = new Pawn_CallTracker(this.pawn);
			}
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				needs.AddOrRemoveNeedsAsAppropriate();
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (this.def.disablesIdeo)
			{
				Pawn_IdeoTracker ideo = this.pawn.ideo;
				if (ideo != null)
				{
					ideo.SetIdeo(null);
				}
			}
			Pawn_GuestTracker guest = this.pawn.guest;
			if (guest != null)
			{
				guest.SetGuestStatus(null, GuestStatus.Guest);
			}
			if (this.pawn.royalty != null)
			{
				this.pawn.royalty.Notify_PawnKilled();
				this.pawn.royalty.UpdateAvailableAbilities();
			}
			Pawn_PlayerSettings playerSettings = this.pawn.playerSettings;
			if (playerSettings != null)
			{
				playerSettings.ResetMedicalCare();
			}
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			if (clearLord)
			{
				Lord lord = this.pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.pawn, PawnLostCondition.Killed, null);
				}
			}
			if (!this.Def.passive && this.pawn.playerSettings != null)
			{
				this.pawn.playerSettings.hostilityResponse = HostilityResponseMode.Attack;
			}
			this.pawn.ownership.UnclaimAll();
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this.pawn);
			}
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "BecameMutant", this.pawn.Named("SUBJECT"));
			Find.QuestManager.Notify_PawnKilled(this.pawn, null);
			if (ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(this.pawn))
			{
				Find.History.Notify_MechanitorDied();
			}
			if (this.Def.relativeTurnedThought != null)
			{
				foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
				{
					if (pawn2.needs != null && pawn2.RaceProps.IsFlesh && pawn2.needs.mood != null && (pawn2.relations.OpinionOf(this.pawn) >= 20 || pawn2.relations.FamilyByBlood.Contains(this.pawn)))
					{
						pawn2.needs.mood.thoughts.memories.TryGainMemory(this.Def.relativeTurnedThought, this.pawn, null);
					}
				}
			}
			Pawn_GeneTracker genes = this.pawn.genes;
			if (genes != null)
			{
				genes.Reset();
			}
			Pawn_AbilityTracker pawn_AbilityTracker = this.pawn.abilities;
			if (pawn_AbilityTracker != null)
			{
				pawn_AbilityTracker.Notify_TemporaryAbilitiesChanged();
			}
			if (this.pawn.outfits != null)
			{
				this.pawn.outfits.CurrentApparelPolicy = null;
			}
			if (this.pawn.foodRestriction != null)
			{
				this.pawn.foodRestriction.CurrentFoodPolicy = null;
			}
			if (this.pawn.drugs != null)
			{
				this.pawn.drugs.CurrentPolicy = null;
			}
			if (this.pawn.reading != null)
			{
				this.pawn.reading.CurrentPolicy = null;
			}
			this.HandleEquipment();
			this.ResolveGraphics();
			this.pawn.Drawer.renderer.SetAnimation(this.Def.standingAnimation);
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			if (this.def.codexEntry != null)
			{
				Find.EntityCodex.SetDiscovered(this.def.codexEntry, this.pawn.def, this.pawn);
			}
			if (this.def.clearsEgo)
			{
				this.pawn.everLostEgo = true;
			}
		}

		// Token: 0x06008634 RID: 34356 RVA: 0x00308368 File Offset: 0x00306568
		public void Revert(bool beingKilled = false)
		{
			if (!this.hasTurned)
			{
				string str = "Tried to revert mutant ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " who hasn't turned");
				return;
			}
			this.hasTurned = false;
			this.pawn.mutant = null;
			if (this.mutantHediff != null)
			{
				HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = this.mutantHediff.TryGetComp<HediffComp_DisappearsAndKills>();
				if (hediffComp_DisappearsAndKills != null)
				{
					hediffComp_DisappearsAndKills.disabled = true;
				}
				this.pawn.health.RemoveHediff(this.mutantHediff);
			}
			foreach (StartingHediff startingHediff in this.def.givesHediffs)
			{
				Hediff hediff;
				if (this.pawn.health.hediffSet.TryGetHediff(startingHediff.def, out hediff))
				{
					this.pawn.health.RemoveHediff(hediff);
				}
			}
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				needs.AddOrRemoveNeedsAsAppropriate();
			}
			this.pawn.Notify_DisabledWorkTypesChanged();
			if (this.pawn.Faction != this.originalFaction)
			{
				this.pawn.SetFaction(this.originalFaction, null);
			}
			Pawn_IdeoTracker ideo = this.pawn.ideo;
			if (ideo != null)
			{
				ideo.SetIdeo(this.originalIdeo);
			}
			if (this.pawn.royalty != null)
			{
				if (!beingKilled)
				{
					this.pawn.royalty.Notify_Resurrected();
				}
				this.pawn.royalty.UpdateAvailableAbilities();
			}
			this.pawn.Drawer.renderer.SetAnimation(null);
			this.abilities = null;
			Pawn_AbilityTracker pawn_AbilityTracker = this.pawn.abilities;
			if (pawn_AbilityTracker != null)
			{
				pawn_AbilityTracker.Notify_TemporaryAbilitiesChanged();
			}
			Pawn_GeneTracker genes = this.pawn.genes;
			if (genes != null)
			{
				genes.Reset();
			}
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06008635 RID: 34357 RVA: 0x0030855C File Offset: 0x0030675C
		private void ResolveGraphics()
		{
			if (!this.Def.forcedHeadTypes.NullOrEmpty<HeadTypeDef>())
			{
				this.pawn.story.TryGetRandomHeadFromSet(this.Def.forcedHeadTypes);
			}
			if (this.Def.hairTagFilter != null)
			{
				this.pawn.story.hairDef = PawnStyleItemChooser.RandomHairFor(this.pawn);
			}
			if (this.Def.beardTagFilter != null)
			{
				this.pawn.style.beardDef = PawnStyleItemChooser.RandomBeardFor(this.pawn);
			}
			if (this.Def.hairColorOverride != null)
			{
				this.pawn.story.HairColor = this.Def.hairColorOverride.Value;
			}
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x06008636 RID: 34358 RVA: 0x00308630 File Offset: 0x00306830
		private void HandleEquipment()
		{
			Pawn_EquipmentTracker equipment = this.pawn.equipment;
			if (((equipment != null) ? equipment.Primary : null) != null)
			{
				ThingWithComps thingWithComps;
				this.pawn.equipment.TryDropEquipment(this.pawn.equipment.Primary, out thingWithComps, this.pawn.PositionHeld, true);
			}
			if (this.pawn.apparel != null)
			{
				if (this.def.disableApparel)
				{
					if (this.pawn.MapHeld != null)
					{
						this.pawn.apparel.DropAll(this.pawn.Position, true, true, null);
					}
					else
					{
						this.pawn.apparel.DestroyAll(DestroyMode.Vanish);
					}
				}
				if (this.def.isConsideredCorpse)
				{
					foreach (Apparel apparel in this.pawn.apparel.WornApparel)
					{
						if (apparel.def.apparel.careIfWornByCorpse)
						{
							apparel.WornByCorpse = true;
						}
					}
				}
			}
		}

		// Token: 0x06008637 RID: 34359 RVA: 0x00308750 File Offset: 0x00306950
		public bool HediffGiversCanGive(HediffDef hediffDef)
		{
			if (this.Def.preventIllnesses && hediffDef.chronic)
			{
				return false;
			}
			List<HediffDef> removesHediffs = this.Def.removesHediffs;
			return removesHediffs == null || !removesHediffs.Contains(hediffDef);
		}

		// Token: 0x06008638 RID: 34360 RVA: 0x00308786 File Offset: 0x00306986
		public void Notify_Downed()
		{
			this.pawn.Drawer.renderer.SetAnimation(null);
		}

		// Token: 0x06008639 RID: 34361 RVA: 0x003087A0 File Offset: 0x003069A0
		public void Notify_Died(Corpse corpse, DamageInfo? dinfo, Hediff hediff = null)
		{
			if (this.def.isConsideredCorpse)
			{
				if (this.rotStage == RotStage.Fresh)
				{
					this.rotStage = RotStage.Rotting;
				}
				CompRottable compRottable = (corpse != null) ? corpse.GetComp<CompRottable>() : null;
				if (compRottable != null)
				{
					compRottable.RotImmediately(this.rotStage);
				}
			}
			if (this.pawn.Faction == Faction.OfPlayer && this.def.deathLetter != null)
			{
				TaggedString label = "Death".Translate(this.pawn.LabelShortCap) + ": " + this.pawn.LabelShortCap;
				TaggedString text = this.def.deathLetter + " " + HealthUtility.GetDiedLetterText(this.pawn, dinfo, hediff) + "\n\n" + this.def.deathLetterExtra;
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.Death, corpse, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600863A RID: 34362 RVA: 0x003088A0 File Offset: 0x00306AA0
		public void Notify_Spawned(bool respawningAfterLoad)
		{
			this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			if (this.pawn.Faction == Faction.OfPlayer && this.Def == MutantDefOf.Ghoul)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.ColonyGhouls, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x0600863B RID: 34363 RVA: 0x003088F2 File Offset: 0x00306AF2
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_MutantTracker.<GetGizmos>d__49 <GetGizmos>d__ = new Pawn_MutantTracker.<GetGizmos>d__49(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600863C RID: 34364 RVA: 0x00308902 File Offset: 0x00306B02
		public IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Pawn_MutantTracker.<SpecialDisplayStats>d__50 <SpecialDisplayStats>d__ = new Pawn_MutantTracker.<SpecialDisplayStats>d__50(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600863D RID: 34365 RVA: 0x00308914 File Offset: 0x00306B14
		public string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.def.showJobReport && this.pawn.Faction == Faction.OfPlayer)
			{
				string jobReport = this.pawn.GetJobReport();
				if (!jobReport.NullOrEmpty())
				{
					stringBuilder.AppendLineIfNotEmpty();
					stringBuilder.Append(jobReport);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600863E RID: 34366 RVA: 0x00308970 File Offset: 0x00306B70
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.originalFaction != null && !Find.FactionManager.AllFactions.Contains(this.originalFaction))
				{
					this.originalFaction = null;
				}
				if (this.originalIdeo != null && !Find.IdeoManager.IdeosListForReading.Contains(this.originalIdeo))
				{
					this.originalIdeo = null;
				}
			}
			Scribe_Defs.Look<MutantDef>(ref this.def, "shamblerType");
			Scribe_Values.Look<RotStage>(ref this.rotStage, "rotStage", RotStage.Fresh, false);
			Scribe_Values.Look<bool>(ref this.hasTurned, "hasTurned", false, false);
			Scribe_References.Look<Hediff>(ref this.mutantHediff, "mutantHediff", false);
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_References.Look<Faction>(ref this.originalFaction, "originalFaction", false);
			Scribe_References.Look<Ideo>(ref this.originalIdeo, "originalIdeo", false);
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.abilities != null)
				{
					foreach (Ability ability in this.abilities)
					{
						ability.pawn = this.pawn;
						ability.verb.caster = this.pawn;
					}
				}
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
			}
		}

		// Token: 0x0400529D RID: 21149
		private const float RottenDropCorpseBileMTBHours = 5f;

		// Token: 0x0400529E RID: 21150
		private static readonly SimpleCurve DropBloodMTBSecondsFromHealthPctCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 5f),
				true
			},
			{
				new CurvePoint(1f, 120f),
				true
			}
		};

		// Token: 0x0400529F RID: 21151
		private Pawn pawn;

		// Token: 0x040052A0 RID: 21152
		private MutantDef def;

		// Token: 0x040052A1 RID: 21153
		public RotStage rotStage;

		// Token: 0x040052A2 RID: 21154
		private bool hasTurned;

		// Token: 0x040052A3 RID: 21155
		private Hediff mutantHediff;

		// Token: 0x040052A4 RID: 21156
		public VerbTracker verbTracker;

		// Token: 0x040052A5 RID: 21157
		private Faction originalFaction;

		// Token: 0x040052A6 RID: 21158
		private Ideo originalIdeo;

		// Token: 0x040052A7 RID: 21159
		private List<Ability> abilities;

		// Token: 0x02003382 RID: 13186
		public struct ThinkTrees
		{
			// Token: 0x0400CEB7 RID: 52919
			public ThinkTreeDef main;

			// Token: 0x0400CEB8 RID: 52920
			public ThinkTreeDef constant;
		}
	}
}
