using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D8 RID: 5336
	public class PawnGroupKindWorker_Shamblers : PawnGroupKindWorker_Normal
	{
		// Token: 0x0600827B RID: 33403 RVA: 0x002ECDAC File Offset: 0x002EAFAC
		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					string[] array = new string[5];
					array[0] = "Cannot generate pawns for ";
					int num = 1;
					Faction faction = parms.faction;
					array[num] = ((faction != null) ? faction.ToString() : null);
					array[2] = " with ";
					array[3] = parms.points.ToString();
					array[4] = ". Defaulting to a single random cheap group.";
					Log.Error(string.Concat(array));
				}
				return;
			}
			float num2 = parms.points;
			float num3 = groupMaker.options.Min((PawnGenOption opt) => opt.Cost);
			int num4 = 0;
			while (num2 > num3 && num4 < 200)
			{
				num4++;
				PawnGenOption pawnGenOption;
				groupMaker.options.TryRandomElementByWeight((PawnGenOption gr) => gr.selectionWeight, out pawnGenOption);
				if (pawnGenOption.Cost <= num2)
				{
					num2 -= pawnGenOption.Cost;
					DevelopmentalStage developmentalStage = DevelopmentalStage.Adult;
					if (Find.Storyteller.difficulty.ChildrenAllowed && Find.Storyteller.difficulty.childShamblersAllowed)
					{
						developmentalStage |= DevelopmentalStage.Child;
					}
					PawnKindDef kind = pawnGenOption.kind;
					Faction faction2 = parms.faction;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					PlanetTile? tile = new PlanetTile?(parms.tile);
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
					FloatRange? excludeBiologicalAgeRange = new FloatRange?(PawnGroupKindWorker_Shamblers.ChildrenDisabledExcludedAgeRange);
					Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction2, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStages, null, excludeBiologicalAgeRange, null, false, false, false, -1, 0, false));
					outPawns.Add(item);
				}
			}
		}

		// Token: 0x040050D2 RID: 20690
		private static readonly FloatRange ChildrenDisabledExcludedAgeRange = new FloatRange(0f, 8f);
	}
}
