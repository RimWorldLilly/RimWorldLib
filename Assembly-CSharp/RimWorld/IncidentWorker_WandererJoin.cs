using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020011A5 RID: 4517
	public class IncidentWorker_WandererJoin : IncidentWorker
	{
		// Token: 0x060070EC RID: 28908 RVA: 0x002782F0 File Offset: 0x002764F0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return this.CanSpawnJoiner(map);
		}

		// Token: 0x060070ED RID: 28909 RVA: 0x0027831C File Offset: 0x0027651C
		public virtual Pawn GeneratePawn(Map map)
		{
			Gender? gender = null;
			if (this.def.pawnFixedGender != Gender.None)
			{
				gender = new Gender?(this.def.pawnFixedGender);
			}
			Ideo ideo = null;
			if (ModsConfig.IdeologyActive)
			{
				if (!(from i in Find.IdeoManager.IdeosListForReading
				where !Faction.OfPlayer.ideos.Has(i)
				select i).TryRandomElementByWeight((Ideo x) => IdeoUtility.IdeoChangeToWeight(null, x), out ideo))
				{
					(from i in Find.IdeoManager.IdeosListForReading
					where !Faction.OfPlayer.ideos.IsPrimary(i)
					select i).TryRandomElementByWeight((Ideo x) => IdeoUtility.IdeoChangeToWeight(null, x), out ideo);
				}
			}
			PawnKindDef pawnKind = this.def.pawnKind;
			Faction ofPlayer = Faction.OfPlayer;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			PlanetTile? tile = (map != null) ? new PlanetTile?(map.Tile) : null;
			bool forceGenerateNewPawn = true;
			bool allowDead = false;
			bool allowDowned = false;
			bool canGeneratePawnRelations = true;
			bool pawnMustBeCapableOfViolence = this.def.pawnMustBeCapableOfViolence;
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
			Gender? fixedGender = gender;
			Ideo fixedIdeo = ideo;
			return PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, ofPlayer, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, pawnMustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, fixedGender, null, null, null, fixedIdeo, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
		}

		// Token: 0x060070EE RID: 28910 RVA: 0x002784C0 File Offset: 0x002766C0
		public virtual bool CanSpawnJoiner(Map map)
		{
			IntVec3 intVec;
			return this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x060070EF RID: 28911 RVA: 0x002784D8 File Offset: 0x002766D8
		public virtual void SpawnJoiner(Map map, Pawn pawn)
		{
			IntVec3 loc;
			this.TryFindEntryCell(map, out loc);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
		}

		// Token: 0x060070F0 RID: 28912 RVA: 0x002784FC File Offset: 0x002766FC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!this.CanSpawnJoiner(map))
			{
				return false;
			}
			Pawn pawn = this.GeneratePawn(map);
			this.SpawnJoiner(map, pawn);
			if (this.def.pawnHediff != null)
			{
				pawn.health.AddHediff(this.def.pawnHediff, null, null, null);
			}
			TaggedString baseLetterText = (this.def.pawnHediff != null) ? this.def.letterText.Formatted(pawn.Named("PAWN"), this.def.pawnHediff.Named("HEDIFF")).AdjustedFor(pawn, "PAWN", true) : this.def.letterText.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			TaggedString baseLetterLabel = this.def.letterLabel.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref baseLetterText, ref baseLetterLabel, pawn);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, LetterDefOf.PositiveEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x060070F1 RID: 28913 RVA: 0x0027862C File Offset: 0x0027682C
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}

		// Token: 0x040047C0 RID: 18368
		private const float RelationWithColonistWeight = 20f;
	}
}
