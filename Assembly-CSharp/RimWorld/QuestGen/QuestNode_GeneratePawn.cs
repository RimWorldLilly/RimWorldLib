using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021AE RID: 8622
	public class QuestNode_GeneratePawn : QuestNode
	{
		// Token: 0x0600D6E4 RID: 55012 RVA: 0x004FC38B File Offset: 0x004FA58B
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D6E5 RID: 55013 RVA: 0x004FC38E File Offset: 0x004FA58E
		protected virtual DevelopmentalStage GetDevelopmentalStage(Slate slate)
		{
			if (!Find.Storyteller.difficulty.ChildrenAllowed || !this.isChild.GetValue(slate))
			{
				return DevelopmentalStage.Adult;
			}
			return DevelopmentalStage.Child;
		}

		// Token: 0x0600D6E6 RID: 55014 RVA: 0x004FC3B4 File Offset: 0x004FA5B4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			PawnKindDef value = this.kindDef.GetValue(slate);
			Faction value2 = this.faction.GetValue(slate);
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			PlanetTile? tile = (map != null) ? new PlanetTile?(map.Tile) : null;
			bool forceGenerateNewPawn = false;
			bool allowDead = false;
			bool allowDowned = false;
			bool canGeneratePawnRelations = true;
			bool valueOrDefault = this.allowAddictions.GetValue(slate).GetValueOrDefault(true);
			bool value3 = this.allowPregnant.GetValue(slate);
			IEnumerable<TraitDef> value4 = this.forcedTraits.GetValue(slate);
			IEnumerable<TraitDef> value5 = this.prohibitedTraits.GetValue(slate);
			float value6 = this.biocodeWeaponChance.GetValue(slate);
			bool value7 = this.mustBeCapableOfViolence.GetValue(slate);
			float colonistRelationChanceFactor = 1f;
			bool forceAddFreeWarmLayerIfNeeded = false;
			bool allowGay = true;
			bool flag = value3;
			bool allowFood = true;
			bool flag2 = valueOrDefault;
			bool inhabitant = false;
			bool certainlyBeenInCryptosleep = false;
			bool forceRedressWorldPawnIfFormerColonist = false;
			bool worldPawnFactionDoesntMatter = false;
			float num = value6;
			Pawn value8 = this.extraPawnForExtraRelationChance.GetValue(slate);
			float value9 = this.relationWithExtraPawnChanceFactor.GetValue(slate);
			Gender? value10 = this.fixedGender.GetValue(slate);
			float value11 = this.biocodeApparelChance.GetValue(slate);
			Pawn pawn = value8;
			float num2 = value9;
			Predicate<Pawn> validatorPreGear = null;
			Predicate<Pawn> validatorPostGear = null;
			IEnumerable<TraitDef> enumerable = value4;
			IEnumerable<TraitDef> enumerable2 = value5;
			DevelopmentalStage developmentalStage = this.GetDevelopmentalStage(slate);
			Pawn pawn2 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(value, value2, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, value7, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, flag, allowFood, flag2, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, num, value11, pawn, num2, validatorPreGear, validatorPostGear, enumerable, enumerable2, null, null, null, value10, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStage, null, null, null, false, false, false, -1, 0, false)
			{
				BiocodeApparelChance = this.biocodeApparelChance.GetValue(slate),
				ForbidAnyTitle = this.forbidAnyTitle.GetValue(slate)
			});
			if (this.ensureNonNumericName.GetValue(slate) && (pawn2.Name == null || pawn2.Name.Numerical))
			{
				pawn2.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn2, NameStyle.Full, null, false, null);
			}
			if (this.giveDependentDrugs.GetValue(slate) && ModsConfig.BiotechActive && pawn2.genes != null)
			{
				foreach (Gene gene in pawn2.genes.GenesListForReading)
				{
					if (gene.Active)
					{
						Gene_ChemicalDependency dep = gene as Gene_ChemicalDependency;
						if (dep != null)
						{
							ThingDef def;
							if ((from x in DefDatabase<ThingDef>.AllDefs
							where x.IsDrug && x.GetCompProperties<CompProperties_Drug>().chemical == dep.def.chemical
							select x).TryRandomElementByWeight((ThingDef x) => x.generateCommonality, out def))
							{
								Thing thing = ThingMaker.MakeThing(def, null);
								thing.stackCount = Rand.Range(1, 3);
								pawn2.inventory.innerContainer.TryAddOrTransfer(thing, true);
							}
						}
					}
				}
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Pawn>(this.storeAs.GetValue(slate), pawn2, false);
			}
			if (this.addToList.GetValue(slate) != null)
			{
				QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.addToList.GetValue(slate), pawn2);
			}
			if (this.addToLists.GetValue(slate) != null)
			{
				foreach (string name in this.addToLists.GetValue(slate))
				{
					QuestGenUtility.AddToOrMakeList(QuestGen.slate, name, pawn2);
				}
			}
			QuestGen.AddToGeneratedPawns(pawn2);
			if (!pawn2.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Decide);
			}
		}

		// Token: 0x04008822 RID: 34850
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008823 RID: 34851
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x04008824 RID: 34852
		[NoTranslate]
		public SlateRef<IEnumerable<string>> addToLists;

		// Token: 0x04008825 RID: 34853
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x04008826 RID: 34854
		public SlateRef<Faction> faction;

		// Token: 0x04008827 RID: 34855
		public SlateRef<bool> forbidAnyTitle;

		// Token: 0x04008828 RID: 34856
		public SlateRef<bool> ensureNonNumericName;

		// Token: 0x04008829 RID: 34857
		public SlateRef<IEnumerable<TraitDef>> forcedTraits;

		// Token: 0x0400882A RID: 34858
		public SlateRef<IEnumerable<TraitDef>> prohibitedTraits;

		// Token: 0x0400882B RID: 34859
		public SlateRef<Pawn> extraPawnForExtraRelationChance;

		// Token: 0x0400882C RID: 34860
		public SlateRef<float> relationWithExtraPawnChanceFactor;

		// Token: 0x0400882D RID: 34861
		public SlateRef<bool?> allowAddictions;

		// Token: 0x0400882E RID: 34862
		public SlateRef<float> biocodeWeaponChance;

		// Token: 0x0400882F RID: 34863
		public SlateRef<float> biocodeApparelChance;

		// Token: 0x04008830 RID: 34864
		public SlateRef<bool> mustBeCapableOfViolence;

		// Token: 0x04008831 RID: 34865
		public SlateRef<bool> isChild;

		// Token: 0x04008832 RID: 34866
		public SlateRef<bool> allowPregnant;

		// Token: 0x04008833 RID: 34867
		public SlateRef<Gender?> fixedGender;

		// Token: 0x04008834 RID: 34868
		public SlateRef<bool> giveDependentDrugs;

		// Token: 0x04008835 RID: 34869
		private const int MinExpertSkill = 11;
	}
}
