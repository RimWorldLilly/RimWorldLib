using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002161 RID: 8545
	public static class QuestGen_Pawns
	{
		// Token: 0x0600D5CF RID: 54735 RVA: 0x004F5E54 File Offset: 0x004F4054
		public static Pawn GeneratePawn(this Quest quest, PawnKindDef kindDef, Faction faction, bool allowAddictions = true, IEnumerable<TraitDef> forcedTraits = null, float biocodeWeaponChance = 0f, bool mustBeCapableOfViolence = true, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 0f, float biocodeApparelChance = 0f, bool ensureNonNumericName = false, bool forceGenerateNewPawn = false, DevelopmentalStage developmentalStages = DevelopmentalStage.Adult, bool allowPregnant = false)
		{
			return quest.GeneratePawn_NewTemp(kindDef, faction, null, allowAddictions, forcedTraits, biocodeWeaponChance, mustBeCapableOfViolence, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, biocodeApparelChance, ensureNonNumericName, forceGenerateNewPawn, developmentalStages, allowPregnant);
		}

		// Token: 0x0600D5D0 RID: 54736 RVA: 0x004F5E88 File Offset: 0x004F4088
		public static Pawn GeneratePawn_NewTemp(this Quest quest, PawnKindDef kindDef, Faction faction, PlanetTile? tile = null, bool allowAddictions = true, IEnumerable<TraitDef> forcedTraits = null, float biocodeWeaponChance = 0f, bool mustBeCapableOfViolence = true, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 0f, float biocodeApparelChance = 0f, bool ensureNonNumericName = false, bool forceGenerateNewPawn = false, DevelopmentalStage developmentalStages = DevelopmentalStage.Adult, bool allowPregnant = false)
		{
			return quest.GeneratePawn(new PawnGenerationRequest(kindDef, faction, PawnGenerationContext.NonPlayer, tile, forceGenerateNewPawn, false, false, true, mustBeCapableOfViolence, 1f, false, true, allowPregnant, true, allowAddictions, false, false, false, false, biocodeWeaponChance, 0f, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, null, null, forcedTraits, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStages, null, null, null, false, false, false, -1, 0, false)
			{
				BiocodeApparelChance = biocodeApparelChance
			}, ensureNonNumericName);
		}

		// Token: 0x0600D5D1 RID: 54737 RVA: 0x004F5F48 File Offset: 0x004F4148
		public static Pawn GeneratePawn(this Quest quest, PawnGenerationRequest request, bool ensureNonNumericName = false)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			if (ensureNonNumericName && (pawn.Name == null || pawn.Name.Numerical))
			{
				Pawn pawn2 = pawn;
				Pawn pawn3 = pawn;
				NameStyle style = NameStyle.Full;
				string forcedLastName = null;
				bool forceNoNick = false;
				Pawn_GeneTracker genes = pawn.genes;
				pawn2.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn3, style, forcedLastName, forceNoNick, (genes != null) ? genes.Xenotype : null);
			}
			QuestGen.AddToGeneratedPawns(pawn);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x0600D5D2 RID: 54738 RVA: 0x004F5FB0 File Offset: 0x004F41B0
		public static bool GetPawnTest(QuestGen_Pawns.GetPawnParms parms, out Pawn pawn)
		{
			pawn = null;
			if (parms.mustHaveNoFaction && parms.mustHaveRoyalTitleInCurrentFaction)
			{
				return false;
			}
			if (parms.canGeneratePawn && (parms.mustBeFactionLeader || parms.mustBePlayerPrisoner || parms.mustBeFreeColonist))
			{
				Log.Warning("QuestGen_GetPawn has incompatible flags set, when canGeneratePawn is true these flags cannot be set: mustBeFactionLeader, mustBePlayerPrisoner, mustBeFreeColonist");
				return false;
			}
			IEnumerable<Pawn> source = QuestGen_Pawns.ExistingUsablePawns(parms);
			if (source.Count<Pawn>() > 0)
			{
				pawn = source.RandomElement<Pawn>();
				return true;
			}
			if (!parms.canGeneratePawn)
			{
				return false;
			}
			Faction faction;
			if (!parms.mustHaveNoFaction && !QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction))
			{
				return false;
			}
			FloatRange senRange = parms.seniorityRange;
			return !parms.mustHaveRoyalTitleInCurrentFaction || !parms.requireResearchedBedroomFurnitureIfRoyal || DefDatabase<RoyalTitleDef>.AllDefsListForReading.Any((RoyalTitleDef x) => (senRange.max <= 0f || senRange.IncludesEpsilon((float)x.seniority)) && QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(x));
		}

		// Token: 0x0600D5D3 RID: 54739 RVA: 0x004F6074 File Offset: 0x004F4274
		public static Pawn GetPawn(this Quest quest, QuestGen_Pawns.GetPawnParms parms)
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> source = QuestGen_Pawns.ExistingUsablePawns(parms);
			int num = source.Count<Pawn>();
			Faction faction;
			Pawn pawn;
			if (Rand.Chance(parms.canGeneratePawn ? Mathf.Clamp01(1f - (float)num / 10f) : 0f) && (parms.mustHaveNoFaction || QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction)))
			{
				pawn = QuestGen_Pawns.GeneratePawn(parms, null);
			}
			else
			{
				pawn = source.RandomElement<Pawn>();
				if (parms.redressPawn)
				{
					PawnGenerator.RedressPawn(pawn, parms.GenerationRequest_NewTemp(pawn.kindDef, pawn.Faction, null, parms.tile));
				}
			}
			if (pawn.Faction != null && !pawn.Faction.Hidden)
			{
				quest.AddPart(new QuestPart_InvolvedFactions
				{
					factions = 
					{
						pawn.Faction
					}
				});
			}
			QuestGen.AddToGeneratedPawns(pawn);
			return pawn;
		}

		// Token: 0x0600D5D4 RID: 54740 RVA: 0x004F6148 File Offset: 0x004F4348
		public static IEnumerable<Pawn> ExistingUsablePawns(QuestGen_Pawns.GetPawnParms parms)
		{
			return from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where QuestGen_Pawns.IsGoodPawn(x, parms)
			select x;
		}

		// Token: 0x0600D5D5 RID: 54741 RVA: 0x004F6178 File Offset: 0x004F4378
		private static bool TryFindFactionForPawnGeneration(QuestGen_Pawns.GetPawnParms parms, out Faction faction)
		{
			FactionManager factionManager = Find.FactionManager;
			bool allowTemporaryFactions = parms.allowTemporaryFactions;
			return (from x in factionManager.GetFactions(parms.allowHidden, false, false, TechLevel.Undefined, allowTemporaryFactions)
			where (parms.mustBeOfFaction == null || x == parms.mustBeOfFaction) && (parms.excludeFactionDefs == null || !parms.excludeFactionDefs.Contains(x.def)) && (!parms.mustHaveRoyalTitleInCurrentFaction || x.def.HasRoyalTitles) && (!parms.mustBeNonHostileToPlayer || !x.HostileTo(Faction.OfPlayer)) && (parms.allowPermanentEnemyFaction.GetValueOrDefault() || !x.def.permanentEnemy) && x.def.techLevel >= parms.minTechLevel
			select x).TryRandomElement(out faction);
		}

		// Token: 0x0600D5D6 RID: 54742 RVA: 0x004F61D0 File Offset: 0x004F43D0
		private static Pawn GeneratePawn(QuestGen_Pawns.GetPawnParms parms, Faction faction = null)
		{
			PawnKindDef pawnKindDef = parms.mustBeOfKind;
			if (faction == null && !parms.mustHaveNoFaction)
			{
				if (!QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction))
				{
					Log.Error("QuestNode_GetPawn tried generating pawn but couldn't find a proper faction for new pawn.");
				}
				else if (pawnKindDef == null)
				{
					pawnKindDef = faction.RandomPawnKind();
				}
			}
			RoyalTitleDef fixedTitle;
			if (parms.mustHaveRoyalTitleInCurrentFaction)
			{
				FloatRange senRange = parms.seniorityRange;
				IEnumerable<RoyalTitleDef> source = from t in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where faction.def.RoyalTitlesAllInSeniorityOrderForReading.Contains(t) && (senRange.max <= 0f || senRange.IncludesEpsilon((float)t.seniority))
				select t;
				if (parms.requireResearchedBedroomFurnitureIfRoyal)
				{
					if (source.Any((RoyalTitleDef x) => QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(x)))
					{
						source = from x in source
						where QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(x)
						select x;
					}
				}
				fixedTitle = source.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
				if (parms.mustBeOfKind == null && !(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
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
			Pawn pawn = PawnGenerator.GeneratePawn(parms.GenerationRequest_NewTemp(pawnKindDef, faction, fixedTitle, parms.tile));
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Any<RoyalTitle>())
			{
				QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
				questPart_Hyperlinks.pawns.Add(pawn);
				QuestGen.quest.AddPart(questPart_Hyperlinks);
			}
			return pawn;
		}

		// Token: 0x0600D5D7 RID: 54743 RVA: 0x004F63E0 File Offset: 0x004F45E0
		private static bool IsGoodPawn(Pawn pawn, QuestGen_Pawns.GetPawnParms parms)
		{
			if (parms.mustBeFactionLeader)
			{
				Faction faction = pawn.Faction;
				if (faction == null || faction.leader != pawn || !faction.def.humanlikeFaction || faction.defeated || faction.Hidden || faction.IsPlayer || pawn.IsPrisoner)
				{
					return false;
				}
			}
			if (parms.mustBeOfFaction != null && pawn.Faction != parms.mustBeOfFaction)
			{
				return false;
			}
			if (pawn.Faction != null && parms.excludeFactionDefs != null && parms.excludeFactionDefs.Contains(pawn.Faction.def))
			{
				return false;
			}
			if (pawn.Faction != null && pawn.Faction.def.techLevel < parms.minTechLevel)
			{
				return false;
			}
			if (parms.mustBeOfKind != null && pawn.kindDef != parms.mustBeOfKind)
			{
				return false;
			}
			if (parms.mustHaveRoyalTitleInCurrentFaction && (pawn.Faction == null || pawn.royalty == null || !pawn.royalty.HasAnyTitleIn(pawn.Faction)))
			{
				return false;
			}
			if (parms.seniorityRange != default(FloatRange) && (pawn.royalty == null || pawn.royalty.MostSeniorTitle == null || !parms.seniorityRange.IncludesEpsilon((float)pawn.royalty.MostSeniorTitle.def.seniority)))
			{
				return false;
			}
			if (parms.mustBeWorldPawn && !pawn.IsWorldPawn())
			{
				return false;
			}
			if (parms.ifWorldPawnThenMustBeFree && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free)
			{
				return false;
			}
			if (parms.ifWorldPawnThenMustBeFreeOrLeader && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.FactionLeader)
			{
				return false;
			}
			if (pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.ReservedByQuest)
			{
				return false;
			}
			if (parms.mustHaveNoFaction && pawn.Faction != null)
			{
				return false;
			}
			if (parms.mustBeFreeColonist && !pawn.IsFreeColonist)
			{
				return false;
			}
			if (parms.mustBePlayerPrisoner && !pawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (parms.mustBeNotSuspended && pawn.Suspended)
			{
				return false;
			}
			if (parms.mustBeNonHostileToPlayer && (pawn.HostileTo(Faction.OfPlayer) || (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.Faction.HostileTo(Faction.OfPlayer))))
			{
				return false;
			}
			if (!parms.allowPermanentEnemyFaction.GetValueOrDefault(true) && pawn.Faction != null && pawn.Faction.def.permanentEnemy)
			{
				return false;
			}
			if (parms.requireResearchedBedroomFurnitureIfRoyal)
			{
				RoyalTitle royalTitle = pawn.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null && !QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(royalTitle.def))
				{
					return false;
				}
			}
			return !parms.mustBeCapableOfViolence || !pawn.WorkTagIsDisabled(WorkTags.Violent);
		}

		// Token: 0x0600D5D8 RID: 54744 RVA: 0x004F6698 File Offset: 0x004F4898
		private static bool PlayerHasResearchedBedroomRequirementsFor(RoyalTitleDef title)
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

		// Token: 0x0600D5D9 RID: 54745 RVA: 0x004F66DC File Offset: 0x004F48DC
		public static QuestPart_ReservePawns ReservePawns(this Quest quest, IEnumerable<Pawn> pawns)
		{
			QuestPart_ReservePawns questPart_ReservePawns = new QuestPart_ReservePawns();
			questPart_ReservePawns.pawns.AddRange(pawns);
			quest.AddPart(questPart_ReservePawns);
			return questPart_ReservePawns;
		}

		// Token: 0x0600D5DA RID: 54746 RVA: 0x004F6704 File Offset: 0x004F4904
		public static QuestPart_TransporterPawns_Feed FeedPawns(this Quest quest, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_TransporterPawns_Feed questPart_TransporterPawns_Feed = new QuestPart_TransporterPawns_Feed();
			questPart_TransporterPawns_Feed.pawnsInTransporter = pawnsInTransporter;
			questPart_TransporterPawns_Feed.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_TransporterPawns_Feed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_TransporterPawns_Feed);
			return questPart_TransporterPawns_Feed;
		}

		// Token: 0x0600D5DB RID: 54747 RVA: 0x004F675C File Offset: 0x004F495C
		public static QuestPart_TransporterPawns_Tend TendPawns(this Quest quest, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_TransporterPawns_Tend questPart_TransporterPawns_Tend = new QuestPart_TransporterPawns_Tend();
			questPart_TransporterPawns_Tend.pawnsInTransporter = pawnsInTransporter;
			questPart_TransporterPawns_Tend.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_TransporterPawns_Tend.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_TransporterPawns_Tend);
			return questPart_TransporterPawns_Tend;
		}

		// Token: 0x0600D5DC RID: 54748 RVA: 0x004F67B4 File Offset: 0x004F49B4
		public static QuestPart_TransporterPawns_TendWithMedicine TendPawnsWithMedicine(this Quest quest, ThingDef medicineDef, bool allowSelfTend = true, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_TransporterPawns_TendWithMedicine questPart_TransporterPawns_TendWithMedicine = new QuestPart_TransporterPawns_TendWithMedicine();
			questPart_TransporterPawns_TendWithMedicine.pawnsInTransporter = pawnsInTransporter;
			questPart_TransporterPawns_TendWithMedicine.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_TransporterPawns_TendWithMedicine.medicineDef = medicineDef;
			questPart_TransporterPawns_TendWithMedicine.allowSelfTend = allowSelfTend;
			questPart_TransporterPawns_TendWithMedicine.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_TransporterPawns_TendWithMedicine);
			return questPart_TransporterPawns_TendWithMedicine;
		}

		// Token: 0x0600D5DD RID: 54749 RVA: 0x004F681C File Offset: 0x004F4A1C
		public static QuestPart_EnsureNotDowned EnsureNotDowned(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null)
		{
			QuestPart_EnsureNotDowned questPart_EnsureNotDowned = new QuestPart_EnsureNotDowned();
			questPart_EnsureNotDowned.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_EnsureNotDowned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_EnsureNotDowned);
			return questPart_EnsureNotDowned;
		}

		// Token: 0x0600D5DE RID: 54750 RVA: 0x004F686C File Offset: 0x004F4A6C
		public static QuestPart_Bestowing_TargetChangedTitle Bestowing_TargetChangedTitle(this Quest quest, Pawn target, Pawn bestower, RoyalTitleDef currentTitle, string inSignal)
		{
			QuestPart_Bestowing_TargetChangedTitle questPart_Bestowing_TargetChangedTitle = new QuestPart_Bestowing_TargetChangedTitle();
			questPart_Bestowing_TargetChangedTitle.pawn = target;
			questPart_Bestowing_TargetChangedTitle.bestower = bestower;
			questPart_Bestowing_TargetChangedTitle.currentTitle = currentTitle;
			questPart_Bestowing_TargetChangedTitle.inSignal = inSignal;
			questPart_Bestowing_TargetChangedTitle.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
			quest.AddPart(questPart_Bestowing_TargetChangedTitle);
			return questPart_Bestowing_TargetChangedTitle;
		}

		// Token: 0x0600D5DF RID: 54751 RVA: 0x004F68AC File Offset: 0x004F4AAC
		public static QuestPart_SetAllApparelLocked SetAllApparelLocked(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null)
		{
			QuestPart_SetAllApparelLocked questPart_SetAllApparelLocked = new QuestPart_SetAllApparelLocked();
			questPart_SetAllApparelLocked.pawns.AddRange(pawns);
			questPart_SetAllApparelLocked.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_SetAllApparelLocked);
			return questPart_SetAllApparelLocked;
		}

		// Token: 0x04008740 RID: 34624
		public const int MaxUsablePawnsToGenerate = 10;

		// Token: 0x02003CFC RID: 15612
		public struct GetPawnParms
		{
			// Token: 0x06015BF7 RID: 89079 RVA: 0x006D2950 File Offset: 0x006D0B50
			public PawnGenerationRequest GenerationRequest(PawnKindDef pawnKind, Faction faction, RoyalTitleDef fixedTitle)
			{
				return this.GenerationRequest_NewTemp(pawnKind, faction, fixedTitle, null);
			}

			// Token: 0x06015BF8 RID: 89080 RVA: 0x006D2970 File Offset: 0x006D0B70
			public PawnGenerationRequest GenerationRequest_NewTemp(PawnKindDef pawnKind, Faction faction, RoyalTitleDef fixedTitle, PlanetTile? tile = null)
			{
				return new PawnGenerationRequest(pawnKind, faction, PawnGenerationContext.NonPlayer, tile, true, false, false, true, this.mustBeCapableOfViolence, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, fixedTitle, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			}

			// Token: 0x0400F0F4 RID: 61684
			public PlanetTile? tile;

			// Token: 0x0400F0F5 RID: 61685
			public bool mustBeFactionLeader;

			// Token: 0x0400F0F6 RID: 61686
			public bool mustBeWorldPawn;

			// Token: 0x0400F0F7 RID: 61687
			public bool ifWorldPawnThenMustBeFree;

			// Token: 0x0400F0F8 RID: 61688
			public bool ifWorldPawnThenMustBeFreeOrLeader;

			// Token: 0x0400F0F9 RID: 61689
			public bool mustHaveNoFaction;

			// Token: 0x0400F0FA RID: 61690
			public bool mustBeFreeColonist;

			// Token: 0x0400F0FB RID: 61691
			public bool mustBePlayerPrisoner;

			// Token: 0x0400F0FC RID: 61692
			public bool mustBeNotSuspended;

			// Token: 0x0400F0FD RID: 61693
			public bool mustHaveRoyalTitleInCurrentFaction;

			// Token: 0x0400F0FE RID: 61694
			public bool mustBeNonHostileToPlayer;

			// Token: 0x0400F0FF RID: 61695
			public bool? allowPermanentEnemyFaction;

			// Token: 0x0400F100 RID: 61696
			public bool canGeneratePawn;

			// Token: 0x0400F101 RID: 61697
			public bool requireResearchedBedroomFurnitureIfRoyal;

			// Token: 0x0400F102 RID: 61698
			public PawnKindDef mustBeOfKind;

			// Token: 0x0400F103 RID: 61699
			public Faction mustBeOfFaction;

			// Token: 0x0400F104 RID: 61700
			public FloatRange seniorityRange;

			// Token: 0x0400F105 RID: 61701
			public TechLevel minTechLevel;

			// Token: 0x0400F106 RID: 61702
			public List<FactionDef> excludeFactionDefs;

			// Token: 0x0400F107 RID: 61703
			public bool allowTemporaryFactions;

			// Token: 0x0400F108 RID: 61704
			public bool allowHidden;

			// Token: 0x0400F109 RID: 61705
			public bool mustBeCapableOfViolence;

			// Token: 0x0400F10A RID: 61706
			public bool redressPawn;
		}
	}
}
