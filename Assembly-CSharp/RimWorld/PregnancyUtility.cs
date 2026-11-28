using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014F4 RID: 5364
	public static class PregnancyUtility
	{
		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x06008389 RID: 33673 RVA: 0x002F3C52 File Offset: 0x002F1E52
		public static FloatRange GeneratedPawnPregnancyProgressRange
		{
			get
			{
				return new FloatRange(0.05f, 0.5f);
			}
		}

		// Token: 0x0600838A RID: 33674 RVA: 0x002F3C64 File Offset: 0x002F1E64
		public static AcceptanceReport CanEverProduceChild(Pawn first, Pawn second)
		{
			if (first.Dead)
			{
				return "PawnIsDead".Translate(first.Named("PAWN"));
			}
			if (second.Dead)
			{
				return "PawnIsDead".Translate(second.Named("PAWN"));
			}
			if (first.gender == second.gender)
			{
				return "PawnsHaveSameGender".Translate(first.Named("PAWN1"), second.Named("PAWN2")).Resolve();
			}
			Pawn pawn = (first.gender == Gender.Male) ? first : second;
			Pawn pawn2 = (first.gender == Gender.Female) ? first : second;
			bool flag = pawn.GetStatValue(StatDefOf.Fertility, true, -1) <= 0f;
			bool flag2 = pawn2.GetStatValue(StatDefOf.Fertility, true, -1) <= 0f;
			if (flag && flag2)
			{
				return "PawnsAreInfertile".Translate(pawn.Named("PAWN1"), pawn2.Named("PAWN2")).Resolve();
			}
			if (flag != flag2)
			{
				return "PawnIsInfertile".Translate((flag ? pawn : pawn2).Named("PAWN")).Resolve();
			}
			bool flag3 = !pawn.ageTracker.CurLifeStage.reproductive;
			bool flag4 = !pawn2.ageTracker.CurLifeStage.reproductive;
			if (flag3 && flag4)
			{
				return "PawnsAreTooYoung".Translate(pawn.Named("PAWN1"), pawn2.Named("PAWN2")).Resolve();
			}
			if (flag3 != flag4)
			{
				return "PawnIsTooYoung".Translate((flag3 ? pawn : pawn2).Named("PAWN")).Resolve();
			}
			bool flag5 = pawn2.Sterile() && PregnancyUtility.GetPregnancyHediff(pawn2) == null;
			bool flag6 = pawn.Sterile();
			if (flag6 && flag5)
			{
				return "PawnsAreSterile".Translate(pawn.Named("PAWN1"), pawn2.Named("PAWN2")).Resolve();
			}
			if (flag6 != flag5)
			{
				return "PawnIsSterile".Translate((flag6 ? pawn : pawn2).Named("PAWN")).Resolve();
			}
			return true;
		}

		// Token: 0x0600838B RID: 33675 RVA: 0x002F3EBE File Offset: 0x002F20BE
		public static float PregnancyChanceForPawn(Pawn pawn)
		{
			if (!Find.Storyteller.difficulty.ChildrenAllowed)
			{
				return 0f;
			}
			if (pawn.Sterile())
			{
				return 0f;
			}
			return pawn.GetStatValue(StatDefOf.Fertility, true, -1);
		}

		// Token: 0x0600838C RID: 33676 RVA: 0x002F3EF2 File Offset: 0x002F20F2
		public static float PregnancyChanceImplantEmbryo(Pawn surrogate)
		{
			return Mathf.Clamp01(PregnancyUtility.PregnancyChanceForPawn(surrogate) * 2f);
		}

		// Token: 0x0600838D RID: 33677 RVA: 0x002F3F08 File Offset: 0x002F2108
		private static float PregnancyChanceForPartnersWithoutPregnancyApproach(Pawn woman, Pawn man)
		{
			float num;
			if (man != null)
			{
				num = PregnancyUtility.PregnancyChanceForPawn(man);
			}
			else
			{
				num = 1f;
			}
			return num * PregnancyUtility.PregnancyChanceForWoman(woman);
		}

		// Token: 0x0600838E RID: 33678 RVA: 0x002F3F30 File Offset: 0x002F2130
		public static float PregnancyChanceForPartners(Pawn woman, Pawn man)
		{
			float num = PregnancyUtility.PregnancyChanceForPartnersWithoutPregnancyApproach(woman, man);
			float pregnancyChanceFactor = woman.relations.GetPregnancyApproachForPartner(man).GetPregnancyChanceFactor();
			return num * pregnancyChanceFactor;
		}

		// Token: 0x0600838F RID: 33679 RVA: 0x002F3F58 File Offset: 0x002F2158
		public static float PregnancyChanceForWoman(Pawn woman)
		{
			return PregnancyUtility.PregnancyChanceForPawn(woman);
		}

		// Token: 0x06008390 RID: 33680 RVA: 0x002F3F60 File Offset: 0x002F2160
		public static RitualRoleAssignments RitualAssignmentsForBirth(Precept_Ritual ritual, Pawn mother)
		{
			TargetInfo ritualTarget = new TargetInfo(mother.PositionHeld, mother.MapHeld, true);
			RitualRoleAssignments ritualRoleAssignments = new RitualRoleAssignments(ritual, ritualTarget);
			List<Pawn> list = new List<Pawn>
			{
				mother
			};
			Dictionary<string, Pawn> forcedRoles = new Dictionary<string, Pawn>
			{
				{
					"mother",
					mother
				}
			};
			ritualRoleAssignments.Setup(list, new List<Pawn>(), forcedRoles, list, mother);
			ritualRoleAssignments.FillPawns(null, TargetInfo.Invalid);
			return ritualRoleAssignments;
		}

		// Token: 0x06008391 RID: 33681 RVA: 0x002F3FC4 File Offset: 0x002F21C4
		public static float GetBirthQualityFor(Pawn mother)
		{
			Precept_Ritual ritual = (Precept_Ritual)mother.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
			TargetInfo ritualTarget = new TargetInfo(mother.PositionHeld, mother.MapHeld, true);
			return RitualUtility.CalculateQualityAbstract(ritual, ritualTarget, PregnancyUtility.RitualAssignmentsForBirth(ritual, mother), null);
		}

		// Token: 0x06008392 RID: 33682 RVA: 0x002F400A File Offset: 0x002F220A
		public static IEnumerable<Building_Bed> BedsForBirth(Pawn p)
		{
			PregnancyUtility.<BedsForBirth>d__25 <BedsForBirth>d__ = new PregnancyUtility.<BedsForBirth>d__25(-2);
			<BedsForBirth>d__.<>3__p = p;
			return <BedsForBirth>d__;
		}

		// Token: 0x06008393 RID: 33683 RVA: 0x002F401A File Offset: 0x002F221A
		public static bool IsUsableBedFor(Pawn mother, Pawn doctor, Building_Bed bed)
		{
			return RestUtility.IsValidBedFor(bed, mother, doctor, true, false, false, mother.GuestStatus);
		}

		// Token: 0x06008394 RID: 33684 RVA: 0x002F402D File Offset: 0x002F222D
		public static float ChanceMomDiesDuringBirth(float quality)
		{
			if (Find.Storyteller.difficulty.babiesAreHealthy)
			{
				return 0f;
			}
			return PregnancyUtility.DeathChanceFromBirthQuality.Evaluate(quality);
		}

		// Token: 0x06008395 RID: 33685 RVA: 0x002F4054 File Offset: 0x002F2254
		private static string RandomLastName(Pawn geneticMother, Pawn birthingMother, Pawn father)
		{
			PregnancyUtility.tmpLastNames.Clear();
			if (geneticMother != null)
			{
				PregnancyUtility.tmpLastNames.Add(PawnNamingUtility.GetLastName(geneticMother));
			}
			if (father != null)
			{
				PregnancyUtility.tmpLastNames.Add(PawnNamingUtility.GetLastName(father));
			}
			if (birthingMother != null && birthingMother != geneticMother && birthingMother != father)
			{
				PregnancyUtility.tmpLastNames.Add(PawnNamingUtility.GetLastName(birthingMother));
			}
			if (PregnancyUtility.tmpLastNames.Count == 0)
			{
				return null;
			}
			return PregnancyUtility.tmpLastNames.RandomElement<string>();
		}

		// Token: 0x06008396 RID: 33686 RVA: 0x002F40C4 File Offset: 0x002F22C4
		public static Thing ApplyBirthOutcome(RitualOutcomePossibility outcome, float quality, Precept_Ritual ritual, List<GeneDef> genes, Pawn geneticMother, Thing birtherThing, Pawn father = null, Pawn doctor = null, LordJob_Ritual lordJobRitual = null, RitualRoleAssignments assignments = null, bool preventLetter = false)
		{
			Pawn birtherPawn = birtherThing as Pawn;
			Building_GrowthVat building_GrowthVat = birtherThing as Building_GrowthVat;
			if (birtherThing.Spawned)
			{
				EffecterDefOf.Birth.Spawn(birtherThing, birtherThing.Map, 1f);
			}
			bool babiesAreHealthy = Find.Storyteller.difficulty.babiesAreHealthy;
			int positivityIndex = outcome.positivityIndex;
			bool flag = birtherPawn != null && Rand.Chance(PregnancyUtility.ChanceMomDiesDuringBirth(quality)) && (birtherPawn.genes == null || !birtherPawn.genes.HasActiveGene(GeneDefOf.Deathless));
			PawnKindDef pawnKindDef = ((geneticMother != null) ? geneticMother.kindDef : null) ?? PawnKindDefOf.Colonist;
			if (pawnKindDef is CreepJoinerFormKindDef)
			{
				pawnKindDef = PawnKindDefOf.Colonist;
			}
			PawnKindDef kind = pawnKindDef;
			Faction faction = birtherThing.Faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			string fixedLastName = PregnancyUtility.RandomLastName(geneticMother, birtherThing as Pawn, father);
			bool forceDead = positivityIndex == -1;
			XenotypeDef baseliner = XenotypeDefOf.Baseliner;
			List<GeneDef> forcedEndogenes = (genes != null) ? genes : PregnancyUtility.GetInheritedGenes(father, geneticMother);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, null, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, fixedLastName, null, null, null, true, false, false, forceDead, null, forcedEndogenes, baseliner, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false)
			{
				DontGivePreArrivalPathway = true
			});
			if (GeneUtility.SameHeritableXenotype(geneticMother, father) && geneticMother.genes.UniqueXenotype)
			{
				pawn.genes.xenotypeName = geneticMother.genes.xenotypeName;
				pawn.genes.iconDef = geneticMother.genes.iconDef;
			}
			XenotypeDef xenotypeDirect;
			if (PregnancyUtility.TryGetInheritedXenotype(geneticMother, father, out xenotypeDirect))
			{
				Pawn_GeneTracker genes2 = pawn.genes;
				if (genes2 != null)
				{
					genes2.SetXenotypeDirect(xenotypeDirect);
				}
			}
			else if (PregnancyUtility.ShouldByHybrid(geneticMother, father))
			{
				pawn.genes.hybrid = true;
				pawn.genes.xenotypeName = "Hybrid".Translate();
			}
			IntVec3? intVec = null;
			Pawn birtherPawn4 = birtherPawn;
			if (birtherPawn4 != null && birtherPawn4.Spawned)
			{
				int? num;
				Building_Bed building_Bed = birtherPawn.CurrentBed(out num);
				IntVec3 intVec2 = ((building_Bed != null) ? new IntVec3?(building_Bed.GetFootSlotPos(num.Value)) : null) ?? birtherPawn.PositionHeld;
				intVec = new IntVec3?(CellFinder.RandomClosewalkCellNear(intVec2, birtherPawn.Map, 1, delegate(IntVec3 cell)
				{
					if (!(cell != birtherPawn.PositionHeld))
					{
						return false;
					}
					Building building = birtherPawn.Map.edificeGrid[cell];
					if (building == null)
					{
						return true;
					}
					ThingDef def = building.def;
					return !((def != null) ? new bool?(def.IsBed) : null).GetValueOrDefault();
				}));
				PregnancyUtility.SpawnBirthFilth(birtherPawn, intVec2, ThingDefOf.Filth_AmnioticFluid, 1);
				if (flag)
				{
					PregnancyUtility.SpawnBirthFilth(birtherPawn, intVec2, ThingDefOf.Filth_Blood, 2);
				}
			}
			if (building_GrowthVat != null)
			{
				intVec = new IntVec3?(building_GrowthVat.InteractionCell);
				FilthMaker.TryMakeFilth(intVec.Value, building_GrowthVat.Map, ThingDefOf.Filth_AmnioticFluid, PregnancyUtility.BirthFilthCount.RandomInRange, FilthSourceFlags.None, true);
			}
			if (birtherPawn != null)
			{
				birtherPawn.health.AddHediff(HediffDefOf.PostpartumExhaustion, null, null, null);
				birtherPawn.health.AddHediff(HediffDefOf.Lactating, null, null, null);
			}
			if (pawn.RaceProps.IsFlesh)
			{
				if (geneticMother != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, geneticMother);
				}
				if (father != null)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
				}
				if (birtherPawn != null && birtherPawn != geneticMother)
				{
					pawn.relations.AddDirectRelation(PawnRelationDefOf.ParentBirth, birtherPawn);
				}
			}
			bool flag2 = false;
			bool flag3 = positivityIndex == 0;
			if (positivityIndex >= 0)
			{
				if (pawn.playerSettings != null && ((geneticMother != null) ? geneticMother.playerSettings : null) != null)
				{
					pawn.playerSettings.AreaRestrictionInPawnCurrentMap = geneticMother.playerSettings.AreaRestrictionInPawnCurrentMap;
				}
				if (flag3)
				{
					pawn.health.AddHediff(HediffDefOf.InfantIllness, null, null, null);
				}
				if (birtherPawn != null)
				{
					TaleRecorder.RecordTale(TaleDefOf.GaveBirth, new object[]
					{
						birtherPawn,
						pawn
					});
					pawn.mindState.SetAutofeeder(birtherPawn, AutofeedMode.Urgent);
				}
				if (doctor != null && doctor.Spawned && doctor != null)
				{
					Pawn_CarryTracker carryTracker = doctor.carryTracker;
					bool? keepCarryingThingOverride = (carryTracker != null) ? new bool?(carryTracker.TryStartCarry(pawn)) : null;
					if (keepCarryingThingOverride.GetValueOrDefault() && birtherPawn != null && (building_GrowthVat != null || doctor.CanReach(birtherPawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
					{
						Job job = JobMaker.MakeJob(JobDefOf.CarryToMomAfterBirth, pawn, birtherPawn);
						job.count = 1;
						Pawn_JobTracker jobs = doctor.jobs;
						Job newJob = job;
						JobCondition lastJobEndCondition = JobCondition.Succeeded;
						ThinkNode jobGiver = null;
						bool resumeCurJobAfterwards = false;
						bool cancelBusyStances = true;
						ThinkTreeDef thinkTree = null;
						keepCarryingThingOverride = new bool?(true);
						jobs.StartJob(newJob, lastJobEndCondition, jobGiver, resumeCurJobAfterwards, cancelBusyStances, thinkTree, null, false, false, keepCarryingThingOverride, false, true, false);
						goto IL_559;
					}
				}
				if (!PawnUtility.TrySpawnHatchedOrBornPawn(pawn, birtherThing, intVec))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				IL_559:
				if (!flag3)
				{
					if (father != null)
					{
						Pawn_NeedsTracker needs = father.needs;
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
										memories.TryGainMemory(ThoughtDefOf.BabyBorn, pawn, null);
									}
								}
							}
						}
					}
					if (geneticMother != null)
					{
						Pawn_NeedsTracker needs2 = geneticMother.needs;
						if (needs2 != null)
						{
							Need_Mood mood2 = needs2.mood;
							if (mood2 != null)
							{
								ThoughtHandler thoughts2 = mood2.thoughts;
								if (thoughts2 != null)
								{
									MemoryThoughtHandler memories2 = thoughts2.memories;
									if (memories2 != null)
									{
										memories2.TryGainMemory(ThoughtDefOf.BabyBorn, pawn, null);
									}
								}
							}
						}
					}
					Pawn birtherPawn2 = birtherPawn;
					if (birtherPawn2 != null)
					{
						Pawn_NeedsTracker needs3 = birtherPawn2.needs;
						if (needs3 != null)
						{
							Need_Mood mood3 = needs3.mood;
							if (mood3 != null)
							{
								ThoughtHandler thoughts3 = mood3.thoughts;
								if (thoughts3 != null)
								{
									MemoryThoughtHandler memories3 = thoughts3.memories;
									if (memories3 != null)
									{
										memories3.TryGainMemory(ThoughtDefOf.BabyBorn, pawn, null);
									}
								}
							}
						}
					}
				}
			}
			else
			{
				Hediff culpritHediff = pawn.health.AddHediff(HediffDefOf.Stillborn, null, null, null);
				flag2 = true;
				Pawn birtherPawn3 = birtherPawn;
				if (birtherPawn3 != null)
				{
					Ideo ideo = birtherPawn3.Ideo;
					if (ideo != null)
					{
						ideo.Notify_MemberDied(pawn);
					}
				}
				pawn.babyNamingDeadline = Find.TickManager.TicksGame + 1;
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(pawn, pawn.RaceProps.DeathActionWorker.DeathRules, null, culpritHediff, null));
				if (birtherThing.Spawned)
				{
					GenSpawn.Spawn(pawn.Corpse, intVec ?? birtherThing.PositionHeld, birtherThing.MapHeld, WipeMode.Vanish);
				}
			}
			if (flag2)
			{
				if (geneticMother != null)
				{
					Pawn_NeedsTracker needs4 = geneticMother.needs;
					if (needs4 != null)
					{
						Need_Mood mood4 = needs4.mood;
						if (mood4 != null)
						{
							ThoughtHandler thoughts4 = mood4.thoughts;
							if (thoughts4 != null)
							{
								MemoryThoughtHandler memories4 = thoughts4.memories;
								if (memories4 != null)
								{
									memories4.TryGainMemory(ThoughtDefOf.Stillbirth, null, null);
								}
							}
						}
					}
				}
				if (father != null)
				{
					Pawn_NeedsTracker needs5 = father.needs;
					if (needs5 != null)
					{
						Need_Mood mood5 = needs5.mood;
						if (mood5 != null)
						{
							ThoughtHandler thoughts5 = mood5.thoughts;
							if (thoughts5 != null)
							{
								MemoryThoughtHandler memories5 = thoughts5.memories;
								if (memories5 != null)
								{
									memories5.TryGainMemory(ThoughtDefOf.Stillbirth, null, null);
								}
							}
						}
					}
				}
			}
			Map mapHeld = birtherThing.MapHeld;
			IntVec3 positionHeld = birtherThing.PositionHeld;
			if (birtherPawn != null && birtherPawn.IsWorldPawn())
			{
				preventLetter = true;
			}
			if (flag)
			{
				birtherPawn.Kill(null, null);
			}
			Thing thing = (positivityIndex >= 0) ? pawn : pawn.Corpse;
			Find.QuestManager.Notify_PawnBorn(thing, birtherThing, geneticMother, father);
			if (ritual != null && !preventLetter)
			{
				TaggedString taggedString;
				if (birtherPawn != null)
				{
					taggedString = outcome.description.Formatted(birtherPawn.Named("MOTHER"));
				}
				else if (flag2)
				{
					if (geneticMother != null && father != null)
					{
						taggedString = "LetterVatStillborn".Translate(geneticMother, father);
					}
					else
					{
						taggedString = "LetterVatSillbornNoParents".Translate();
					}
				}
				else if (flag3)
				{
					if (geneticMother != null && father != null)
					{
						taggedString = "LetterVatInfantIllness".Translate(geneticMother, father);
					}
					else
					{
						taggedString = "LetterVatInfantIllnessNoParents".Translate();
					}
				}
				else if (geneticMother != null && father != null)
				{
					taggedString = "LetterVatHealthyBaby".Translate(geneticMother, father);
				}
				else
				{
					taggedString = "LetterVatHealthyBabyNoParents".Translate();
				}
				if (!flag2 && birtherPawn != null && geneticMother != null && birtherPawn != geneticMother && father != null)
				{
					taggedString += "\n\n" + "LetterPartSurrogacy".Translate(geneticMother, father);
				}
				if (birtherPawn != null && !babiesAreHealthy)
				{
					RitualOutcomeEffectWorker_ChildBirth ritualOutcomeEffectWorker_ChildBirth = (RitualOutcomeEffectWorker_ChildBirth)RitualOutcomeEffectDefOf.ChildBirth.GetInstance();
					if (lordJobRitual != null)
					{
						taggedString += "\n\n" + ritualOutcomeEffectWorker_ChildBirth.OutcomeQualityBreakdownDesc(quality, 1f, lordJobRitual);
					}
					else
					{
						taggedString += "\n\n" + RitualUtility.QualityBreakdownAbstract(ritual, new TargetInfo(positionHeld, mapHeld, true), assignments, null);
					}
					if (!babiesAreHealthy)
					{
						taggedString += "\n\n" + "BirthRitualHealthyBabyChance".Translate(ritualOutcomeEffectWorker_ChildBirth.GetOutcomeChanceAtQuality(lordJobRitual, ritualOutcomeEffectWorker_ChildBirth.def.BestOutcome, quality));
					}
					if (flag)
					{
						taggedString += "\n\n" + "LetterPartColonistDiedAfterChildbirth".Translate(birtherPawn);
					}
				}
				if (pawn.genes.HasActiveGene(GeneDefOf.Inbred))
				{
					taggedString += "\n\n" + "InbredBabyBorn".Translate();
				}
				taggedString += "\n\n" + "LetterPartTempBabyName".Translate(pawn) + " ";
				if (flag2)
				{
					taggedString += "LetterPartStillbirthNameDeadline".Translate();
				}
				else
				{
					taggedString += "LetterPartLiveBirthNameDeadline".Translate(60000.ToStringTicksToPeriod(true, false, true, true, false));
				}
				ChoiceLetter_BabyBirth choiceLetter_BabyBirth = (ChoiceLetter_BabyBirth)LetterMaker.MakeLetter((birtherPawn != null) ? "OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), ritual.Label.Named("RITUALLABEL")) : "LetterVatBirth".Translate(outcome.label), taggedString, LetterDefOf.BabyBirth, pawn, null, null, null);
				choiceLetter_BabyBirth.Start();
				Find.LetterStack.ReceiveLetter(choiceLetter_BabyBirth, null, 0, true);
			}
			return thing;
		}

		// Token: 0x06008397 RID: 33687 RVA: 0x002F4BB4 File Offset: 0x002F2DB4
		public static Gizmo BirthQualityGizmo(Pawn pawn)
		{
			return new Command_Action
			{
				defaultLabel = "DEV: Output Birth Quality Now",
				action = delegate()
				{
					Messages.Message("Birth ritual quality for " + pawn.LabelShort + " is " + PregnancyUtility.GetBirthQualityFor(pawn).ToStringPercent(), pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			};
		}

		// Token: 0x06008398 RID: 33688 RVA: 0x002F4BF0 File Offset: 0x002F2DF0
		private static void SpawnBirthFilth(Pawn mother, IntVec3 center, ThingDef filth, int radius)
		{
			int randomInRange = PregnancyUtility.BirthFilthCount.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				FilthMaker.TryMakeFilth(CellFinder.RandomClosewalkCellNear(center, mother.Map, radius, null), mother.Map, filth, mother.LabelIndefinite(), 1, FilthSourceFlags.None);
			}
		}

		// Token: 0x06008399 RID: 33689 RVA: 0x002F4C3C File Offset: 0x002F2E3C
		public static GeneSet GetInheritedGeneSet(Pawn father, Pawn mother, out bool success)
		{
			GeneSet geneSet = new GeneSet();
			foreach (GeneDef gene in PregnancyUtility.GetInheritedGenes(father, mother, out success))
			{
				geneSet.AddGene(gene);
			}
			if (GeneUtility.SameHeritableXenotype(father, mother))
			{
				geneSet.SetNameDirect(father.genes.xenotypeName);
			}
			return geneSet;
		}

		// Token: 0x0600839A RID: 33690 RVA: 0x002F4CB4 File Offset: 0x002F2EB4
		public static GeneSet GetInheritedGeneSet(Pawn father, Pawn mother)
		{
			bool flag;
			return PregnancyUtility.GetInheritedGeneSet(father, mother, out flag);
		}

		// Token: 0x0600839B RID: 33691 RVA: 0x002F4CCC File Offset: 0x002F2ECC
		public static List<GeneDef> GetInheritedGenes(Pawn father, Pawn mother, out bool success)
		{
			PregnancyUtility.tmpGenes.Clear();
			PregnancyUtility.tmpGenesShuffled.Clear();
			if (((father != null) ? father.genes : null) != null)
			{
				foreach (Gene gene in father.genes.Endogenes)
				{
					if (gene.def.endogeneCategory != EndogeneCategory.Melanin && gene.def.biostatArc <= 0)
					{
						PregnancyUtility.tmpGeneChances.SetOrAdd(gene.def, 0.5f);
						if (!PregnancyUtility.tmpGenesShuffled.Contains(gene.def))
						{
							PregnancyUtility.tmpGenesShuffled.Add(gene.def);
						}
					}
				}
			}
			if (((mother != null) ? mother.genes : null) != null)
			{
				foreach (Gene gene2 in mother.genes.Endogenes)
				{
					if (gene2.def.endogeneCategory != EndogeneCategory.Melanin && gene2.def.biostatArc <= 0)
					{
						if (!PregnancyUtility.tmpGenesShuffled.Contains(gene2.def))
						{
							PregnancyUtility.tmpGenesShuffled.Add(gene2.def);
						}
						if (PregnancyUtility.tmpGeneChances.ContainsKey(gene2.def))
						{
							PregnancyUtility.tmpGeneChances[gene2.def] = 1f;
						}
						else
						{
							PregnancyUtility.tmpGeneChances.Add(gene2.def, 0.5f);
						}
					}
				}
			}
			int num = 0;
			do
			{
				PregnancyUtility.tmpGenes.Clear();
				PregnancyUtility.tmpGenesShuffled.Shuffle<GeneDef>();
				foreach (GeneDef geneDef in PregnancyUtility.tmpGenesShuffled)
				{
					if (!PregnancyUtility.tmpGenes.Contains(geneDef) && Rand.Chance(PregnancyUtility.tmpGeneChances[geneDef]))
					{
						PregnancyUtility.tmpGenes.Add(geneDef);
					}
				}
				PregnancyUtility.tmpGenes.RemoveAll((GeneDef x) => x.prerequisite != null && !PregnancyUtility.tmpGenes.Contains(x.prerequisite));
				int val = PregnancyUtility.tmpGenes.NonOverriddenGenes(false).Sum((GeneDef x) => x.biostatMet);
				if (GeneTuning.BiostatRange.Includes(val))
				{
					break;
				}
				num++;
			}
			while (num < 50);
			success = (num < 50);
			GeneDef item;
			if (PawnSkinColors.SkinColorsFromParents(father, mother).TryRandomElement(out item))
			{
				PregnancyUtility.tmpGenes.Add(item);
			}
			if (!PregnancyUtility.tmpGenes.Any((GeneDef x) => x.endogeneCategory == EndogeneCategory.HairColor))
			{
				GeneDef geneDef2;
				if (father == null)
				{
					geneDef2 = null;
				}
				else
				{
					Pawn_GeneTracker genes = father.genes;
					geneDef2 = ((genes != null) ? genes.GetFirstEndogeneByCategory(EndogeneCategory.HairColor) : null);
				}
				GeneDef geneDef3 = geneDef2;
				GeneDef geneDef4;
				if (mother == null)
				{
					geneDef4 = null;
				}
				else
				{
					Pawn_GeneTracker genes2 = mother.genes;
					geneDef4 = ((genes2 != null) ? genes2.GetFirstEndogeneByCategory(EndogeneCategory.HairColor) : null);
				}
				GeneDef geneDef5 = geneDef4;
				GeneDef item2;
				if (geneDef3 != null && geneDef5 == null)
				{
					PregnancyUtility.tmpGenes.Add(geneDef3);
				}
				else if (geneDef5 != null && geneDef3 == null)
				{
					PregnancyUtility.tmpGenes.Add(geneDef5);
				}
				else if (geneDef3 != null && geneDef5 != null)
				{
					PregnancyUtility.tmpGenes.Add(Rand.Bool ? geneDef5 : geneDef3);
				}
				else if ((from x in DefDatabase<GeneDef>.AllDefs
				where x.endogeneCategory == EndogeneCategory.HairColor
				select x).TryRandomElementByWeight((GeneDef x) => x.selectionWeight, out item2))
				{
					PregnancyUtility.tmpGenes.Add(item2);
				}
			}
			PawnRelationDef pawnRelationDef;
			if (!PregnancyUtility.tmpGenes.Contains(GeneDefOf.Inbred) && Rand.Value < PregnancyUtility.InbredChanceFromParents(mother, father, out pawnRelationDef))
			{
				PregnancyUtility.tmpGenes.Add(GeneDefOf.Inbred);
			}
			PregnancyUtility.tmpGeneChances.Clear();
			PregnancyUtility.tmpGenesShuffled.Clear();
			return PregnancyUtility.tmpGenes;
		}

		// Token: 0x0600839C RID: 33692 RVA: 0x002F50E4 File Offset: 0x002F32E4
		public static List<GeneDef> GetInheritedGenes(Pawn father, Pawn mother)
		{
			bool flag;
			return PregnancyUtility.GetInheritedGenes(father, mother, out flag);
		}

		// Token: 0x0600839D RID: 33693 RVA: 0x002F50FC File Offset: 0x002F32FC
		public static float InbredChanceFromParents(Pawn mother, Pawn father, out PawnRelationDef relation)
		{
			relation = null;
			if (mother != null && father != null)
			{
				float num = 0f;
				foreach (PawnRelationDef pawnRelationDef in father.GetRelations(mother))
				{
					if (pawnRelationDef.inbredChanceOnChild > num)
					{
						num = pawnRelationDef.inbredChanceOnChild;
						relation = pawnRelationDef;
					}
					num = Mathf.Max(num, pawnRelationDef.inbredChanceOnChild);
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x0600839E RID: 33694 RVA: 0x002F517C File Offset: 0x002F337C
		private static bool TryGetInheritedXenotype(Pawn mother, Pawn father, out XenotypeDef xenotype)
		{
			bool flag = ((mother != null) ? mother.genes : null) != null;
			bool flag2 = ((father != null) ? father.genes : null) != null;
			if (flag && flag2 && mother.genes.Xenotype.inheritable && father.genes.Xenotype.inheritable && mother.genes.Xenotype == father.genes.Xenotype)
			{
				xenotype = mother.genes.Xenotype;
				return true;
			}
			if (flag && !flag2 && mother.genes.Xenotype.inheritable)
			{
				xenotype = mother.genes.Xenotype;
				return true;
			}
			if (flag2 && !flag && father.genes.Xenotype.inheritable)
			{
				xenotype = father.genes.Xenotype;
				return true;
			}
			xenotype = null;
			return false;
		}

		// Token: 0x0600839F RID: 33695 RVA: 0x002F524C File Offset: 0x002F344C
		private static bool ShouldByHybrid(Pawn mother, Pawn father)
		{
			bool flag = ((mother != null) ? mother.genes : null) != null;
			bool flag2 = ((father != null) ? father.genes : null) != null;
			if (flag && flag2)
			{
				if (mother.genes.hybrid && father.genes.hybrid)
				{
					return true;
				}
				if (mother.genes.Xenotype.inheritable && father.genes.Xenotype.inheritable)
				{
					return true;
				}
				bool flag3 = flag && (mother.genes.Xenotype.inheritable || mother.genes.hybrid);
				bool flag4 = flag2 && (father.genes.Xenotype.inheritable || father.genes.hybrid);
				if (flag3 || flag4)
				{
					return true;
				}
			}
			return (flag && !flag2 && mother.genes.hybrid) || (flag2 && !flag && father.genes.hybrid);
		}

		// Token: 0x060083A0 RID: 33696 RVA: 0x002F5340 File Offset: 0x002F3540
		public static bool TryTerminatePregnancy(Pawn pawn)
		{
			Hediff pregnancyHediff = PregnancyUtility.GetPregnancyHediff(pawn);
			if (pregnancyHediff == null)
			{
				return false;
			}
			int curStageIndex = pregnancyHediff.CurStageIndex;
			PregnancyAttitude? attitude = ((Hediff_Pregnant)pregnancyHediff).Attitude;
			pawn.health.RemoveHediff(pregnancyHediff);
			Pawn_NeedsTracker needs = pawn.needs;
			bool flag;
			if (needs == null)
			{
				flag = (null != null);
			}
			else
			{
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					flag = (null != null);
				}
				else
				{
					ThoughtHandler thoughts = mood.thoughts;
					flag = (((thoughts != null) ? thoughts.memories : null) != null);
				}
			}
			if (flag && attitude != null)
			{
				Thought_Memory thought_Memory = null;
				PregnancyAttitude value = attitude.Value;
				if (value != PregnancyAttitude.Positive)
				{
					if (value == PregnancyAttitude.Negative)
					{
						thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.PregnancyEnded);
					}
				}
				else
				{
					thought_Memory = ThoughtMaker.MakeThought(ThoughtDefOf.PregnancyTerminated, curStageIndex);
				}
				if (thought_Memory != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
				}
			}
			return true;
		}

		// Token: 0x060083A1 RID: 33697 RVA: 0x002F5404 File Offset: 0x002F3604
		public static Hediff GetPregnancyHediff(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike)
			{
				return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnantHuman, true);
			}
			if (pawn.RaceProps.Animal)
			{
				return pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, true);
			}
			return null;
		}

		// Token: 0x060083A2 RID: 33698 RVA: 0x002F545C File Offset: 0x002F365C
		public static void ForceEndPregnancy(Pawn pawn, bool preventLetter = false)
		{
			Hediff pregnancyHediff = PregnancyUtility.GetPregnancyHediff(pawn);
			if (pregnancyHediff != null)
			{
				pawn.health.RemoveHediff(pregnancyHediff);
			}
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			Hediff hediff;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.PregnancyLabor, out hediff))
			{
				pawn.health.RemoveHediff(hediff);
			}
			Hediff hediff2;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.PregnancyLaborPushing, out hediff2))
			{
				Hediff_LaborPushing hediff_LaborPushing = hediff2 as Hediff_LaborPushing;
				if (hediff_LaborPushing == null)
				{
					return;
				}
				hediff_LaborPushing.ForceBirth(-1, preventLetter);
			}
		}

		// Token: 0x060083A3 RID: 33699 RVA: 0x002F54D8 File Offset: 0x002F36D8
		public static string GetLabel(this PregnancyApproach mode)
		{
			switch (mode)
			{
			case PregnancyApproach.Normal:
				return "PregnancyApproach_Normal".Translate();
			case PregnancyApproach.AvoidPregnancy:
				return "PregnancyApproach_AvoidPregnancy".Translate();
			case PregnancyApproach.TryForBaby:
				return "PregnancyApproach_TryForBaby".Translate();
			default:
				Log.Error("Undefined rhythm method.");
				return string.Empty;
			}
		}

		// Token: 0x060083A4 RID: 33700 RVA: 0x002F5538 File Offset: 0x002F3738
		public static float GetPregnancyChanceFactor(this PregnancyApproach mode)
		{
			if (mode == PregnancyApproach.AvoidPregnancy)
			{
				return PregnancyUtility.PregnancyChanceFactor_PregnancyApproach_AvoidPregnancy;
			}
			if (mode != PregnancyApproach.TryForBaby)
			{
				return PregnancyUtility.PregnancyChanceFactor_PregnancyApproach_Normal;
			}
			return PregnancyUtility.PregnancyChanceFactor_PregnancyApproach_TryForBaby;
		}

		// Token: 0x060083A5 RID: 33701 RVA: 0x002F5558 File Offset: 0x002F3758
		public static string GetDescription(this PregnancyApproach mode)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
			defaultInterpolatedStringHandler.AppendFormatted(mode.GetLabel().CapitalizeFirst());
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("PregnancyChance".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(" x");
			defaultInterpolatedStringHandler.AppendFormatted(mode.GetPregnancyChanceFactor().ToStringPercent());
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060083A6 RID: 33702 RVA: 0x002F55CE File Offset: 0x002F37CE
		public static Texture2D GetIcon(this PregnancyApproach mode)
		{
			if (mode == PregnancyApproach.AvoidPregnancy)
			{
				return PregnancyUtility.PregnancyApproachIcon_AvoidPregnancy.Texture;
			}
			if (mode != PregnancyApproach.TryForBaby)
			{
				return PregnancyUtility.PregnancyApproachIcon_Normal.Texture;
			}
			return PregnancyUtility.PregnancyApproachIcon_TryForBaby.Texture;
		}

		// Token: 0x04005146 RID: 20806
		public static readonly SimpleCurve DeathChanceFromBirthQuality = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.2f),
				true
			},
			{
				new CurvePoint(0.2f, 0.05f),
				true
			},
			{
				new CurvePoint(0.9f, 0f),
				true
			}
		};

		// Token: 0x04005147 RID: 20807
		public static readonly Dictionary<PawnRelationDef, float> BeingFatherWeightPerRelation = new Dictionary<PawnRelationDef, float>
		{
			{
				PawnRelationDefOf.Spouse,
				1f
			},
			{
				PawnRelationDefOf.Lover,
				0.8f
			},
			{
				PawnRelationDefOf.ExSpouse,
				0.5f
			},
			{
				PawnRelationDefOf.ExLover,
				0.4f
			}
		};

		// Token: 0x04005148 RID: 20808
		public const int MaxGeneAssignmentTries = 50;

		// Token: 0x04005149 RID: 20809
		public const float MinFertileMaleAge = 14f;

		// Token: 0x0400514A RID: 20810
		public const float ChanceOfNoFatherWithCandidates = 0.2f;

		// Token: 0x0400514B RID: 20811
		public const float ChanceToInheritOneParent = 0.5f;

		// Token: 0x0400514C RID: 20812
		public const float ChanceToInheritTwoParents = 1f;

		// Token: 0x0400514D RID: 20813
		public const float IvfPregnancyFactor = 2f;

		// Token: 0x0400514E RID: 20814
		private static readonly IntRange BirthFilthCount = new IntRange(4, 7);

		// Token: 0x0400514F RID: 20815
		private static float PregnancyChanceFactor_PregnancyApproach_Normal = 1f;

		// Token: 0x04005150 RID: 20816
		private static float PregnancyChanceFactor_PregnancyApproach_AvoidPregnancy = 0.25f;

		// Token: 0x04005151 RID: 20817
		private static float PregnancyChanceFactor_PregnancyApproach_TryForBaby = 4f;

		// Token: 0x04005152 RID: 20818
		private static CachedTexture PregnancyApproachIcon_Normal = new CachedTexture("UI/Icons/PregnancyApproaches/Normal");

		// Token: 0x04005153 RID: 20819
		private static CachedTexture PregnancyApproachIcon_AvoidPregnancy = new CachedTexture("UI/Icons/PregnancyApproaches/AvoidPregnancy");

		// Token: 0x04005154 RID: 20820
		private static CachedTexture PregnancyApproachIcon_TryForBaby = new CachedTexture("UI/Icons/PregnancyApproaches/TryForBaby");

		// Token: 0x04005155 RID: 20821
		private static List<string> tmpLastNames = new List<string>(3);

		// Token: 0x04005156 RID: 20822
		private static Dictionary<GeneDef, float> tmpGeneChances = new Dictionary<GeneDef, float>();

		// Token: 0x04005157 RID: 20823
		private static List<GeneDef> tmpGenesShuffled = new List<GeneDef>();

		// Token: 0x04005158 RID: 20824
		private static List<GeneDef> tmpGenes = new List<GeneDef>();
	}
}
