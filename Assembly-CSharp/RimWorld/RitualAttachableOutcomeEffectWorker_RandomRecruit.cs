using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200165D RID: 5725
	public class RitualAttachableOutcomeEffectWorker_RandomRecruit : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x06008FF4 RID: 36852 RVA: 0x0034E7F8 File Offset: 0x0034C9F8
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			if (Rand.Chance(0.5f))
			{
				Slate slate = new Slate();
				slate.Set<Map>("map", jobRitual.Map, false);
				Slate slate2 = slate;
				string name = "overridePawnGenParams";
				PawnKindDef villager = PawnKindDefOf.Villager;
				Faction faction = null;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				PlanetTile? tile = new PlanetTile?(jobRitual.Map.Tile);
				bool forceGenerateNewPawn = true;
				bool allowDead = false;
				bool allowDowned = false;
				bool canGeneratePawnRelations = true;
				bool mustBeCapableOfViolence = false;
				float colonistRelationChanceFactor = 20f;
				bool forceAddFreeWarmLayerIfNeeded = false;
				bool allowGay = true;
				bool allowPregnant = false;
				bool allowFood = true;
				bool allowAddictions = true;
				bool inhabitant = false;
				bool certainlyBeenInCryptosleep = false;
				bool forceRedressWorldPawnIfFormerColonist = false;
				bool worldPawnFactionDoesntMatter = false;
				float biocodeWeaponChance = 0f;
				float biocodeApparelChance = 0f;
				Pawn extraPawnForExtraRelationChance = null;
				float relationWithExtraPawnChanceFactor = 1f;
				Predicate<Pawn> validatorPreGear = null;
				Predicate<Pawn> validatorPostGear = null;
				IEnumerable<TraitDef> forcedTraits = null;
				IEnumerable<TraitDef> prohibitedTraits = null;
				Ideo ideo = jobRitual.Ritual.ideo;
				slate2.Set<PawnGenerationRequest>(name, new PawnGenerationRequest(villager, faction, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, null, null, null, null, ideo, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false), false);
				QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.WandererJoins, slate);
				extraOutcomeDesc = this.def.letterInfoText;
				return;
			}
			extraOutcomeDesc = null;
		}

		// Token: 0x040057EB RID: 22507
		public const float RecruitChance = 0.5f;
	}
}
