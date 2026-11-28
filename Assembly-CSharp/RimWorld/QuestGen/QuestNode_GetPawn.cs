using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021CD RID: 8653
	public class QuestNode_GetPawn : QuestNode
	{
		// Token: 0x0600D761 RID: 55137 RVA: 0x004FECF0 File Offset: 0x004FCEF0
		private IEnumerable<Pawn> ExistingUsablePawns(Slate slate)
		{
			return from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where this.IsGoodPawn(x, slate)
			select x;
		}

		// Token: 0x0600D762 RID: 55138 RVA: 0x004FED28 File Offset: 0x004FCF28
		protected override bool TestRunInt(Slate slate)
		{
			if (this.mustHaveNoFaction.GetValue(slate) && this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate))
			{
				return false;
			}
			if (this.canGeneratePawn.GetValue(slate) && (this.mustBeFactionLeader.GetValue(slate) || this.mustBeWorldPawn.GetValue(slate) || this.mustBePlayerPrisoner.GetValue(slate) || this.mustBeFreeColonist.GetValue(slate)))
			{
				Log.Warning("QuestNode_GetPawn has incompatible flags set, when canGeneratePawn is true these flags cannot be set: mustBeFactionLeader, mustBeWorldPawn, mustBePlayerPrisoner, mustBeFreeColonist");
				return false;
			}
			Pawn pawn;
			if (slate.TryGet<Pawn>(this.storeAs.GetValue(slate), out pawn, false) && this.IsGoodPawn(pawn, slate))
			{
				return true;
			}
			IEnumerable<Pawn> source = this.ExistingUsablePawns(slate);
			if (source.Count<Pawn>() > 0)
			{
				slate.Set<Pawn>(this.storeAs.GetValue(slate), source.RandomElement<Pawn>(), false);
				return true;
			}
			if (!this.canGeneratePawn.GetValue(slate))
			{
				return false;
			}
			Faction faction;
			if (!this.mustHaveNoFaction.GetValue(slate) && !this.TryFindFactionForPawnGeneration(slate, out faction))
			{
				return false;
			}
			FloatRange senRange = this.seniorityRange.GetValue(slate);
			return !this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) || !this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate) || DefDatabase<RoyalTitleDef>.AllDefsListForReading.Any((RoyalTitleDef x) => (senRange.max <= 0f || senRange.IncludesEpsilon((float)x.seniority)) && this.PlayerHasResearchedBedroomRequirementsFor(x));
		}

		// Token: 0x0600D763 RID: 55139 RVA: 0x004FEE78 File Offset: 0x004FD078
		private bool TryFindFactionForPawnGeneration(Slate slate, out Faction faction)
		{
			return Find.FactionManager.GetFactions(false, false, false, TechLevel.Undefined, false).Where(delegate(Faction x)
			{
				Map map;
				return (this.excludeFactionDefs.GetValue(slate) == null || !this.excludeFactionDefs.GetValue(slate).Contains(x.def)) && (!this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) || x.def.HasRoyalTitles) && (!this.mustBeNonHostileToPlayer.GetValue(slate) || !x.HostileTo(Faction.OfPlayer)) && (!slate.TryGet<Map>("map", out map, false) || !this.mustHaveSettlementOnLayer.GetValue(slate) || !map.Tile.Valid || Find.WorldObjects.AnyFactionSettlementOnLayer(x, map.Tile.Layer)) && (this.allowPermanentEnemyFaction.GetValue(slate).GetValueOrDefault() || !x.def.permanentEnemy) && x.def.techLevel >= this.minTechLevel.GetValue(slate) && (!this.factionMustBePermanent.GetValue(slate) || !x.temporary);
			}).TryRandomElementByWeight(delegate(Faction x)
			{
				if (x.HostileTo(Faction.OfPlayer))
				{
					return this.hostileWeight.GetValue(slate).GetValueOrDefault(1f);
				}
				return this.nonHostileWeight.GetValue(slate).GetValueOrDefault(1f);
			}, out faction);
		}

		// Token: 0x0600D764 RID: 55140 RVA: 0x004FEECC File Offset: 0x004FD0CC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn pawn;
			if (QuestGen.slate.TryGet<Pawn>(this.storeAs.GetValue(slate), out pawn, false) && this.IsGoodPawn(pawn, slate))
			{
				return;
			}
			IEnumerable<Pawn> source = this.ExistingUsablePawns(slate);
			int num = source.Count<Pawn>();
			Faction faction;
			if (Rand.Chance(this.canGeneratePawn.GetValue(slate) ? Mathf.Clamp01(1f - (float)num / (float)this.maxUsablePawnsToGenerate.GetValue(slate)) : 0f) && (this.mustHaveNoFaction.GetValue(slate) || this.TryFindFactionForPawnGeneration(slate, out faction)))
			{
				pawn = this.GeneratePawn(slate, null);
			}
			else
			{
				pawn = source.RandomElementByWeight(delegate(Pawn x)
				{
					if (x.Faction != null && x.Faction.HostileTo(Faction.OfPlayer))
					{
						return this.hostileWeight.GetValue(slate).GetValueOrDefault(1f);
					}
					return this.nonHostileWeight.GetValue(slate).GetValueOrDefault(1f);
				});
			}
			if (pawn.Faction != null && !pawn.Faction.Hidden)
			{
				QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
				questPart_InvolvedFactions.factions.Add(pawn.Faction);
				QuestGen.quest.AddPart(questPart_InvolvedFactions);
			}
			QuestGen.slate.Set<Pawn>(this.storeAs.GetValue(slate), pawn, false);
		}

		// Token: 0x0600D765 RID: 55141 RVA: 0x004FF014 File Offset: 0x004FD214
		private Pawn GeneratePawn(Slate slate, Faction faction = null)
		{
			PawnKindDef pawnKindDef = this.mustBeOfKind.GetValue(slate);
			if (faction == null && !this.mustHaveNoFaction.GetValue(slate))
			{
				if (!this.TryFindFactionForPawnGeneration(slate, out faction))
				{
					Log.Error("QuestNode_GetPawn tried generating pawn but couldn't find a proper faction for new pawn.");
				}
				else if (pawnKindDef == null)
				{
					pawnKindDef = faction.RandomPawnKind();
				}
			}
			RoyalTitleDef fixedTitle;
			if (this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate))
			{
				FloatRange senRange;
				if (!this.seniorityRange.TryGetValue(slate, out senRange))
				{
					senRange = FloatRange.Zero;
				}
				IEnumerable<RoyalTitleDef> source = from t in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where faction.def.RoyalTitlesAllInSeniorityOrderForReading.Contains(t) && (senRange.max <= 0f || senRange.IncludesEpsilon((float)t.seniority))
				select t;
				if (this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate) && source.Any(new Func<RoyalTitleDef, bool>(this.PlayerHasResearchedBedroomRequirementsFor)))
				{
					source = source.Where(new Func<RoyalTitleDef, bool>(this.PlayerHasResearchedBedroomRequirementsFor));
				}
				fixedTitle = source.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
				if (this.mustBeOfKind.GetValue(slate) == null && !(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
				where k.titleRequired != null && k.titleRequired == fixedTitle
				select k).TryRandomElement(out pawnKindDef))
				{
					(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
					where k.titleSelectOne != null && k.titleSelectOne.Contains(fixedTitle)
					select k).TryRandomElement(out pawnKindDef);
				}
			}
			else
			{
				fixedTitle = null;
			}
			if (pawnKindDef == null)
			{
				pawnKindDef = (from kind in DefDatabase<PawnKindDef>.AllDefsListForReading
				where kind.race.race.Humanlike
				select kind).RandomElement<PawnKindDef>();
			}
			Map map;
			slate.TryGet<Map>("map", out map, false);
			PawnKindDef kind2 = pawnKindDef;
			Faction faction2 = faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			PlanetTile? tile = (map != null) ? new PlanetTile?(map.Tile) : null;
			bool forceGenerateNewPawn = true;
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
			RoyalTitleDef fixedTitle2 = fixedTitle;
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind2, faction2, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, null, null, null, fixedTitle2, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Any<RoyalTitle>())
			{
				QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
				questPart_Hyperlinks.pawns.Add(pawn);
				QuestGen.quest.AddPart(questPart_Hyperlinks);
			}
			return pawn;
		}

		// Token: 0x0600D766 RID: 55142 RVA: 0x004FF2DC File Offset: 0x004FD4DC
		private bool IsGoodPawn(Pawn pawn, Slate slate)
		{
			if (this.mustBeFactionLeader.GetValue(slate))
			{
				Faction faction = pawn.Faction;
				if (faction == null || faction.leader != pawn || !faction.def.humanlikeFaction || faction.defeated || faction.Hidden || faction.IsPlayer || pawn.IsPrisoner)
				{
					return false;
				}
			}
			if (pawn.Faction != null && this.excludeFactionDefs.GetValue(slate) != null && this.excludeFactionDefs.GetValue(slate).Contains(pawn.Faction.def))
			{
				return false;
			}
			if (pawn.Faction != null && pawn.Faction.def.techLevel < this.minTechLevel.GetValue(slate))
			{
				return false;
			}
			if (this.mustBeOfKind.GetValue(slate) != null && pawn.kindDef != this.mustBeOfKind.GetValue(slate))
			{
				return false;
			}
			if (this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) && (pawn.Faction == null || pawn.royalty == null || !pawn.royalty.HasAnyTitleIn(pawn.Faction)))
			{
				return false;
			}
			if (this.seniorityRange.GetValue(slate) != default(FloatRange))
			{
				Pawn_RoyaltyTracker royalty = pawn.royalty;
				if (((royalty != null) ? royalty.MostSeniorTitle : null) == null || !this.seniorityRange.GetValue(slate).IncludesEpsilon((float)pawn.royalty.MostSeniorTitle.def.seniority))
				{
					return false;
				}
			}
			if (this.factionMustBePermanent.GetValue(slate) && pawn.Faction != null && pawn.Faction.temporary)
			{
				return false;
			}
			if (this.mustBeWorldPawn.GetValue(slate) && !pawn.IsWorldPawn())
			{
				return false;
			}
			if (this.ifWorldPawnThenMustBeFree.GetValue(slate) && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free)
			{
				return false;
			}
			if (this.ifWorldPawnThenMustBeFreeOrLeader.GetValue(slate) && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.FactionLeader)
			{
				return false;
			}
			if (pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.ReservedByQuest)
			{
				return false;
			}
			if (this.mustHaveNoFaction.GetValue(slate) && pawn.Faction != null)
			{
				return false;
			}
			if (this.mustBeFreeColonist.GetValue(slate) && !pawn.IsFreeColonist)
			{
				return false;
			}
			if (this.mustBePlayerPrisoner.GetValue(slate) && !pawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (this.mustBeNotSuspended.GetValue(slate) && pawn.Suspended)
			{
				return false;
			}
			if (this.mustBeNonHostileToPlayer.GetValue(slate) && (pawn.HostileTo(Faction.OfPlayer) || (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.Faction.HostileTo(Faction.OfPlayer))))
			{
				return false;
			}
			if (!this.allowPermanentEnemyFaction.GetValue(slate).GetValueOrDefault(true) && pawn.Faction != null && pawn.Faction.def.permanentEnemy)
			{
				return false;
			}
			if (this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate))
			{
				RoyalTitle royalTitle = pawn.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null && !this.PlayerHasResearchedBedroomRequirementsFor(royalTitle.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D767 RID: 55143 RVA: 0x004FF604 File Offset: 0x004FD804
		private bool PlayerHasResearchedBedroomRequirementsFor(RoyalTitleDef title)
		{
			if (title.bedroomRequirements == null)
			{
				return true;
			}
			for (int i = 0; i < title.bedroomRequirements.Count; i++)
			{
				if (!title.bedroomRequirements[i].PlayerCanBuildNow())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040088B7 RID: 34999
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088B8 RID: 35000
		public SlateRef<bool> mustBeFactionLeader;

		// Token: 0x040088B9 RID: 35001
		public SlateRef<bool> mustBeWorldPawn;

		// Token: 0x040088BA RID: 35002
		public SlateRef<bool> ifWorldPawnThenMustBeFree;

		// Token: 0x040088BB RID: 35003
		public SlateRef<bool> ifWorldPawnThenMustBeFreeOrLeader;

		// Token: 0x040088BC RID: 35004
		public SlateRef<bool> mustHaveNoFaction;

		// Token: 0x040088BD RID: 35005
		public SlateRef<bool> mustBeFreeColonist;

		// Token: 0x040088BE RID: 35006
		public SlateRef<bool> mustBePlayerPrisoner;

		// Token: 0x040088BF RID: 35007
		public SlateRef<bool> mustBeNotSuspended;

		// Token: 0x040088C0 RID: 35008
		public SlateRef<bool> mustHaveRoyalTitleInCurrentFaction;

		// Token: 0x040088C1 RID: 35009
		public SlateRef<bool> mustBeNonHostileToPlayer;

		// Token: 0x040088C2 RID: 35010
		public SlateRef<bool?> allowPermanentEnemyFaction;

		// Token: 0x040088C3 RID: 35011
		public SlateRef<bool> canGeneratePawn;

		// Token: 0x040088C4 RID: 35012
		public SlateRef<bool> mustHaveSettlementOnLayer;

		// Token: 0x040088C5 RID: 35013
		public SlateRef<bool> requireResearchedBedroomFurnitureIfRoyal;

		// Token: 0x040088C6 RID: 35014
		public SlateRef<PawnKindDef> mustBeOfKind;

		// Token: 0x040088C7 RID: 35015
		public SlateRef<FloatRange> seniorityRange;

		// Token: 0x040088C8 RID: 35016
		public SlateRef<TechLevel> minTechLevel;

		// Token: 0x040088C9 RID: 35017
		public SlateRef<List<FactionDef>> excludeFactionDefs;

		// Token: 0x040088CA RID: 35018
		public SlateRef<float?> hostileWeight;

		// Token: 0x040088CB RID: 35019
		public SlateRef<float?> nonHostileWeight;

		// Token: 0x040088CC RID: 35020
		public SlateRef<bool> factionMustBePermanent = true;

		// Token: 0x040088CD RID: 35021
		public SlateRef<int> maxUsablePawnsToGenerate = 10;
	}
}
