using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020011AC RID: 4524
	public class IncidentWorker_WildManWandersIn : IncidentWorker
	{
		// Token: 0x0600710B RID: 28939 RVA: 0x00278BC4 File Offset: 0x00276DC4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return !map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && (!ModsConfig.BiotechActive || !map.GameConditionManager.ConditionIsActive(GameConditionDefOf.NoxiousHaze)) && map.mapTemperature.SeasonAcceptableFor(ThingDefOf.Human, 0f) && this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x0600710C RID: 28940 RVA: 0x00278C48 File Offset: 0x00276E48
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!this.TryFindEntryCell(map, out loc))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			DevelopmentalStage developmentalStage = Find.Storyteller.difficulty.ChildrenAllowed ? (DevelopmentalStage.Child | DevelopmentalStage.Adult) : DevelopmentalStage.Adult;
			PawnKindDef wildMan = PawnKindDefOf.WildMan;
			Faction faction2 = faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			PlanetTile? tile = new PlanetTile?(map.Tile);
			bool forceGenerateNewPawn = false;
			bool allowDead = false;
			bool allowDowned = false;
			bool canGeneratePawnRelations = true;
			bool mustBeCapableOfViolence = false;
			float colonistRelationChanceFactor = 1f;
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
			DevelopmentalStage developmentalStages = developmentalStage;
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(wildMan, faction2, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStages, null, null, null, false, false, false, -1, 0, false));
			pawn.SetFaction(null, null);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			string value = pawn.DevelopmentalStage.Child() ? "FeralChild".Translate().ToString() : pawn.KindLabel;
			TaggedString value2 = pawn.DevelopmentalStage.Child() ? "Child".Translate() : "Person".Translate();
			TaggedString baseLetterLabel = this.def.letterLabel.Formatted(value, pawn.Named("PAWN")).CapitalizeFirst();
			TaggedString baseLetterText = this.def.letterText.Formatted(pawn.NameShortColored, value2, pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true).CapitalizeFirst();
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref baseLetterText, ref baseLetterLabel, pawn);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.def.letterDef, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600710D RID: 28941 RVA: 0x00278E44 File Offset: 0x00277044
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
		}

		// Token: 0x0600710E RID: 28942 RVA: 0x00278E7B File Offset: 0x0027707B
		private bool TryFindFormerFaction(out Faction formerFaction)
		{
			return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out formerFaction, false, true, TechLevel.Undefined, TechLevel.Undefined, false, false);
		}
	}
}
