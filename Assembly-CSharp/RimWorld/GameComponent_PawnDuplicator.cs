using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002027 RID: 8231
	public class GameComponent_PawnDuplicator : GameComponent
	{
		// Token: 0x0600D063 RID: 53347 RVA: 0x004C9838 File Offset: 0x004C7A38
		public GameComponent_PawnDuplicator(Game game)
		{
		}

		// Token: 0x0600D064 RID: 53348 RVA: 0x004C984C File Offset: 0x004C7A4C
		public Pawn Duplicate(Pawn pawn)
		{
			float ageBiologicalYearsFloat = pawn.ageTracker.AgeBiologicalYearsFloat;
			float num = pawn.ageTracker.AgeChronologicalYearsFloat;
			if (num > ageBiologicalYearsFloat)
			{
				num = ageBiologicalYearsFloat;
			}
			PawnKindDef kindDef = pawn.kindDef;
			Faction faction = pawn.Faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			Gender? fixedGender = new Gender?(pawn.gender);
			Ideo ideo = pawn.Ideo;
			float? fixedBiologicalAge = new float?(ageBiologicalYearsFloat);
			float? fixedChronologicalAge = new float?(num);
			XenotypeDef xenotype = pawn.genes.Xenotype;
			CustomXenotype customXenotype = pawn.genes.CustomXenotype;
			Pawn pawn2 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, faction, context, null, true, false, false, false, false, 0f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 0f, null, null, null, null, null, fixedBiologicalAge, fixedChronologicalAge, fixedGender, null, null, null, ideo, false, false, true, false, null, null, xenotype, customXenotype, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, true)
			{
				IsCreepJoiner = pawn.IsCreepJoiner,
				ForceNoIdeoGear = true,
				CanGeneratePawnRelations = false,
				DontGivePreArrivalPathway = true
			});
			if (ModsConfig.AnomalyActive)
			{
				int duplicateOf = (pawn.duplicate.duplicateOf == int.MinValue) ? pawn.thingIDNumber : pawn.duplicate.duplicateOf;
				pawn.duplicate.duplicateOf = duplicateOf;
				pawn2.duplicate.duplicateOf = duplicateOf;
			}
			pawn2.Name = NameTriple.FromString(pawn.Name.ToString(), false);
			if (ModsConfig.BiotechActive)
			{
				pawn2.ageTracker.growthPoints = pawn.ageTracker.growthPoints;
				pawn2.ageTracker.vatGrowTicks = pawn.ageTracker.vatGrowTicks;
				pawn2.genes.xenotypeName = pawn.genes.xenotypeName;
				pawn2.genes.iconDef = pawn.genes.iconDef;
			}
			GameComponent_PawnDuplicator.CopyStoryAndTraits(pawn, pawn2);
			GameComponent_PawnDuplicator.CopyGenes(pawn, pawn2);
			GameComponent_PawnDuplicator.CopyApperance(pawn, pawn2);
			GameComponent_PawnDuplicator.CopyStyle(pawn, pawn2);
			GameComponent_PawnDuplicator.CopySkills(pawn, pawn2);
			GameComponent_PawnDuplicator.CopyHediffs(pawn, pawn2);
			GameComponent_PawnDuplicator.CopyNeeds(pawn, pawn2);
			if (pawn.mutant != null)
			{
				MutantUtility.SetPawnAsMutantInstantly(pawn2, pawn.mutant.Def, pawn.GetRotStage());
			}
			GameComponent_PawnDuplicator.CopyAbilities(pawn, pawn2);
			if (pawn.guest != null)
			{
				pawn2.guest.Recruitable = pawn.guest.Recruitable;
			}
			pawn2.Notify_DuplicatedFrom(pawn);
			pawn2.Drawer.renderer.SetAllGraphicsDirty();
			pawn2.Notify_DisabledWorkTypesChanged();
			return pawn2;
		}

		// Token: 0x0600D065 RID: 53349 RVA: 0x004C9AB8 File Offset: 0x004C7CB8
		private static void CopyAbilities(Pawn pawn, Pawn newPawn)
		{
			foreach (Ability ability in pawn.abilities.abilities)
			{
				if (newPawn.abilities.GetAbility(ability.def, false) == null)
				{
					newPawn.abilities.GainAbility(ability.def);
				}
			}
			List<Ability> abilities = newPawn.abilities.abilities;
			for (int i = abilities.Count - 1; i >= 0; i--)
			{
				Ability ability2 = abilities[i];
				if (pawn.abilities.GetAbility(ability2.def, false) == null)
				{
					newPawn.abilities.RemoveAbility(ability2.def);
				}
			}
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
				{
					foreach (AbilityDef def in royalTitle.def.grantedAbilities)
					{
						if (newPawn.abilities.GetAbility(def, false) != null)
						{
							newPawn.abilities.RemoveAbility(def);
						}
					}
				}
			}
		}

		// Token: 0x0600D066 RID: 53350 RVA: 0x004C9C28 File Offset: 0x004C7E28
		private static void CopyStoryAndTraits(Pawn pawn, Pawn newPawn)
		{
			newPawn.story.favoriteColor = pawn.story.favoriteColor;
			newPawn.story.Childhood = pawn.story.Childhood;
			newPawn.story.Adulthood = pawn.story.Adulthood;
			newPawn.story.traits.allTraits.Clear();
			foreach (Trait trait in pawn.story.traits.allTraits)
			{
				if (!ModsConfig.BiotechActive || trait.sourceGene == null)
				{
					newPawn.story.traits.GainTrait(new Trait(trait.def, trait.Degree, trait.ScenForced), false);
				}
			}
		}

		// Token: 0x0600D067 RID: 53351 RVA: 0x004C9D0C File Offset: 0x004C7F0C
		private static void CopyGenes(Pawn pawn, Pawn newPawn)
		{
			int i;
			int j;
			if (ModsConfig.BiotechActive)
			{
				newPawn.genes.Xenogenes.Clear();
				List<Gene> sourceXenogenes = pawn.genes.Xenogenes;
				foreach (Gene gene in sourceXenogenes)
				{
					newPawn.genes.AddGene(gene.def, true);
				}
				int i;
				Func<Gene, bool> <>9__0;
				for (i = 0; i < sourceXenogenes.Count; i = j + 1)
				{
					Gene gene2 = newPawn.genes.Xenogenes[i];
					if (sourceXenogenes[i].Overridden)
					{
						Gene gene3 = gene2;
						IEnumerable<Gene> genesListForReading = newPawn.genes.GenesListForReading;
						Func<Gene, bool> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((Gene e) => e.def == sourceXenogenes[i].overriddenByGene.def));
						}
						gene3.overriddenByGene = genesListForReading.First(predicate);
					}
					else
					{
						gene2.overriddenByGene = null;
					}
					j = i;
				}
			}
			newPawn.genes.Endogenes.Clear();
			List<Gene> sourceEndogenes = pawn.genes.Endogenes;
			foreach (Gene gene4 in sourceEndogenes)
			{
				newPawn.genes.AddGene(gene4.def, false);
			}
			Func<Gene, bool> <>9__1;
			for (i = 0; i < sourceEndogenes.Count; i = j + 1)
			{
				Gene gene5 = newPawn.genes.Endogenes[i];
				if (sourceEndogenes[i].Overridden)
				{
					Gene gene6 = gene5;
					IEnumerable<Gene> genesListForReading2 = newPawn.genes.GenesListForReading;
					Func<Gene, bool> predicate2;
					if ((predicate2 = <>9__1) == null)
					{
						predicate2 = (<>9__1 = ((Gene e) => e.def == sourceEndogenes[i].overriddenByGene.def));
					}
					gene6.overriddenByGene = genesListForReading2.First(predicate2);
				}
				else
				{
					gene5.overriddenByGene = null;
				}
				j = i;
			}
		}

		// Token: 0x0600D068 RID: 53352 RVA: 0x004C9F60 File Offset: 0x004C8160
		private static void CopyApperance(Pawn pawn, Pawn newPawn)
		{
			newPawn.story.headType = pawn.story.headType;
			newPawn.story.bodyType = pawn.story.bodyType;
			newPawn.story.hairDef = pawn.story.hairDef;
			newPawn.story.HairColor = pawn.story.HairColor;
			newPawn.story.SkinColorBase = pawn.story.SkinColorBase;
			newPawn.story.skinColorOverride = pawn.story.skinColorOverride;
			newPawn.story.furDef = pawn.story.furDef;
		}

		// Token: 0x0600D069 RID: 53353 RVA: 0x004CA008 File Offset: 0x004C8208
		private static void CopyStyle(Pawn pawn, Pawn newPawn)
		{
			newPawn.style.beardDef = pawn.style.beardDef;
			if (ModsConfig.IdeologyActive)
			{
				newPawn.style.BodyTattoo = pawn.style.BodyTattoo;
				newPawn.style.FaceTattoo = pawn.style.FaceTattoo;
			}
		}

		// Token: 0x0600D06A RID: 53354 RVA: 0x004CA060 File Offset: 0x004C8260
		private static void CopySkills(Pawn pawn, Pawn newPawn)
		{
			newPawn.skills.skills.Clear();
			foreach (SkillRecord skillRecord in pawn.skills.skills)
			{
				SkillRecord item = new SkillRecord(newPawn, skillRecord.def)
				{
					levelInt = skillRecord.levelInt,
					passion = skillRecord.passion,
					xpSinceLastLevel = skillRecord.xpSinceLastLevel,
					xpSinceMidnight = skillRecord.xpSinceMidnight
				};
				newPawn.skills.skills.Add(item);
			}
		}

		// Token: 0x0600D06B RID: 53355 RVA: 0x004CA110 File Offset: 0x004C8310
		private static void CopyHediffs(Pawn pawn, Pawn newPawn)
		{
			newPawn.health.hediffSet.hediffs.Clear();
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			foreach (Hediff hediff in hediffs)
			{
				if (hediff.def.duplicationAllowed && (hediff.Part == null || newPawn.health.hediffSet.HasBodyPart(hediff.Part)) && (!(hediff is Hediff_AddedPart) || hediff.def.organicAddedBodypart) && (!(hediff is Hediff_Implant) || hediff.def.organicAddedBodypart))
				{
					Hediff hediff2 = HediffMaker.MakeHediff(hediff.def, newPawn, hediff.Part);
					hediff2.CopyFrom(hediff);
					newPawn.health.hediffSet.AddDirect(hediff2, null, null);
				}
			}
			foreach (Hediff hediff3 in hediffs)
			{
				if (hediff3 is Hediff_AddedPart && !hediff3.def.organicAddedBodypart)
				{
					newPawn.health.RestorePart(hediff3.Part, null, false);
				}
			}
		}

		// Token: 0x0600D06C RID: 53356 RVA: 0x004CA278 File Offset: 0x004C8478
		private static void CopyNeeds(Pawn pawn, Pawn newPawn)
		{
			newPawn.needs.AllNeeds.Clear();
			foreach (Need need in pawn.needs.AllNeeds)
			{
				Need need2 = (Need)Activator.CreateInstance(need.def.needClass, new object[]
				{
					newPawn
				});
				need2.def = need.def;
				newPawn.needs.AllNeeds.Add(need2);
				need2.SetInitialLevel();
				need2.CurLevel = need.CurLevel;
				newPawn.needs.BindDirectNeedFields();
			}
			if (pawn.needs.mood != null)
			{
				List<Thought_Memory> memories = newPawn.needs.mood.thoughts.memories.Memories;
				memories.Clear();
				foreach (Thought_Memory thought_Memory in pawn.needs.mood.thoughts.memories.Memories)
				{
					Thought_Memory thought_Memory2 = (Thought_Memory)ThoughtMaker.MakeThought(thought_Memory.def);
					thought_Memory2.CopyFrom(thought_Memory);
					thought_Memory2.pawn = newPawn;
					memories.Add(thought_Memory2);
				}
			}
		}

		// Token: 0x0600D06D RID: 53357 RVA: 0x004CA3E4 File Offset: 0x004C85E4
		public void AddDuplicate(int duplicateOf, Pawn pawn)
		{
			if (this.duplicates.ContainsKey(duplicateOf))
			{
				this.duplicates[duplicateOf].Add(pawn);
				return;
			}
			this.duplicates.Add(duplicateOf, new DuplicateSet());
			this.duplicates[duplicateOf].Add(pawn);
		}

		// Token: 0x0600D06E RID: 53358 RVA: 0x004CA438 File Offset: 0x004C8638
		public void RemoveDuplicate(int duplicateOf, Pawn pawn)
		{
			if (!this.duplicates.ContainsKey(duplicateOf))
			{
				return;
			}
			DuplicateSet duplicateSet = this.duplicates[duplicateOf];
			duplicateSet.Remove(pawn);
			if (duplicateSet.Contains(null))
			{
				duplicateSet.Remove(null);
			}
			Pawn pawn2 = null;
			foreach (Pawn pawn3 in duplicateSet)
			{
				if (pawn3.health.hediffSet.GetFirstHediff<Hediff_DuplicateSickness>() != null)
				{
					if (pawn2 != null)
					{
						pawn2 = null;
						break;
					}
					pawn2 = pawn3;
				}
			}
			if (pawn2 != null)
			{
				Hediff_DuplicateSickness firstHediff = pawn2.health.hediffSet.GetFirstHediff<Hediff_DuplicateSickness>();
				if (firstHediff != null)
				{
					firstHediff.GetComp<HediffComp_SeverityPerDay>().severityPerDay = -0.5f;
				}
			}
			if (duplicateSet.Count == 1)
			{
				duplicateSet.First().duplicate.duplicateOf = int.MinValue;
				duplicateSet.Clear();
				this.duplicates.Remove(duplicateOf);
				return;
			}
			if (duplicateSet.Count == 0)
			{
				this.duplicates.Remove(duplicateOf);
			}
		}

		// Token: 0x0600D06F RID: 53359 RVA: 0x004CA544 File Offset: 0x004C8744
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<int, DuplicateSet>(ref this.duplicates, "duplicates", LookMode.Undefined, LookMode.Undefined);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.duplicates == null)
			{
				this.duplicates = new Dictionary<int, DuplicateSet>();
			}
		}

		// Token: 0x040084A3 RID: 33955
		private Dictionary<int, DuplicateSet> duplicates = new Dictionary<int, DuplicateSet>();

		// Token: 0x040084A4 RID: 33956
		private static readonly HashSet<int> toRemove = new HashSet<int>();
	}
}
