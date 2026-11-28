using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000FD2 RID: 4050
	public abstract class RaidStrategyWorker
	{
		// Token: 0x060066B5 RID: 26293 RVA: 0x002471F4 File Offset: 0x002453F4
		public float SelectionWeightForFaction(Map map, Faction faction, float basePoints)
		{
			if (faction != null && this.def.selectionWeightCurvesPerFaction != null)
			{
				List<FactionCurve> selectionWeightCurvesPerFaction = this.def.selectionWeightCurvesPerFaction;
				for (int i = 0; i < selectionWeightCurvesPerFaction.Count; i++)
				{
					if (selectionWeightCurvesPerFaction[i].faction == faction.def)
					{
						return selectionWeightCurvesPerFaction[i].Evaluate(basePoints);
					}
				}
			}
			return this.SelectionWeight(map, basePoints);
		}

		// Token: 0x060066B6 RID: 26294 RVA: 0x00247258 File Offset: 0x00245458
		public virtual float SelectionWeight(Map map, float basePoints)
		{
			return this.def.selectionWeightPerPointsCurve.Evaluate(basePoints);
		}

		// Token: 0x060066B7 RID: 26295
		protected abstract LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed);

		// Token: 0x060066B8 RID: 26296 RVA: 0x0024726C File Offset: 0x0024546C
		public virtual void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			List<List<Pawn>> list = IncidentParmsUtility.SplitIntoGroups(pawns, parms.pawnGroups);
			int @int = Rand.Int;
			for (int i = 0; i < list.Count; i++)
			{
				List<Pawn> list2 = list[i];
				Lord lord = LordMaker.MakeNewLord(parms.faction, this.MakeLordJob(parms, map, list2, @int), map, list2);
				lord.inSignalLeave = parms.inSignalEnd;
				QuestUtility.AddQuestTag(lord, parms.questTag);
				if (DebugViewSettings.drawStealDebug && parms.faction.HostileTo(Faction.OfPlayer))
				{
					Log.Message(string.Concat(new string[]
					{
						"Market value threshold to start stealing (raiders=",
						lord.ownedPawns.Count.ToString(),
						"): ",
						StealAIUtility.StartStealingMarketValueThreshold(lord).ToString(),
						" (colony wealth=",
						map.wealthWatcher.WealthTotal.ToString(),
						")"
					}));
				}
			}
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x0024737C File Offset: 0x0024557C
		public virtual bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.faction != null && parms.faction.def.disallowedRaidStrategies.NotNullAndContains(this.def))
			{
				return false;
			}
			if (this.SelectionWeightForFaction(parms.target as Map, parms.faction, parms.points) <= 0f)
			{
				return false;
			}
			if (groupKind != null && parms.points < this.MinimumPoints(parms.faction, groupKind))
			{
				return false;
			}
			Map map = parms.target as Map;
			if (map != null)
			{
				using (IEnumerator<TileMutatorDef> enumerator = map.TileInfo.Mutators.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.blacklistedRaidStrategies.NotNullAndContains(this.def))
						{
							return false;
						}
					}
				}
				if (!map.Tile.Valid)
				{
					return true;
				}
				if (!this.def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.def.layerWhitelist.Contains(map.Tile.LayerDef))
				{
					return false;
				}
				if (!this.def.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && this.def.layerBlacklist.Contains(map.Tile.LayerDef))
				{
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x060066BA RID: 26298 RVA: 0x002474D4 File Offset: 0x002456D4
		public virtual float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return faction.def.MinPointsToGeneratePawnGroup(groupKind, null);
		}

		// Token: 0x060066BB RID: 26299 RVA: 0x002474E3 File Offset: 0x002456E3
		public virtual float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return 0f;
		}

		// Token: 0x060066BC RID: 26300 RVA: 0x002474EC File Offset: 0x002456EC
		public virtual bool CanUsePawnGenOption(float pointsTotal, PawnGenOption g, List<PawnGenOptionWithXenotype> chosenGroups, Faction faction = null)
		{
			if (faction != null && faction.def.humanlikeFaction && g.kind.RaceProps.Animal && chosenGroups != null)
			{
				if (!chosenGroups.Any((PawnGenOptionWithXenotype x) => x.Option.kind.RaceProps.Humanlike))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x0024754A File Offset: 0x0024574A
		public virtual bool CanUsePawn(float pointsTotal, Pawn p, List<Pawn> otherPawns)
		{
			return true;
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x0024754D File Offset: 0x0024574D
		public virtual void TryGenerateThreats(IncidentParms parms)
		{
		}

		// Token: 0x060066BF RID: 26303 RVA: 0x00247550 File Offset: 0x00245750
		public virtual List<Pawn> SpawnThreats(IncidentParms parms)
		{
			if (parms.pawnKind != null)
			{
				List<Pawn> list = new List<Pawn>();
				for (int i = 0; i < parms.pawnCount; i++)
				{
					PawnKindDef pawnKind = parms.pawnKind;
					Faction faction = parms.faction;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					PlanetTile? tile = new PlanetTile?(parms.target.Tile);
					bool forceGenerateNewPawn = false;
					bool allowDead = false;
					bool allowDowned = false;
					bool canGeneratePawnRelations = true;
					bool mustBeCapableOfViolence = true;
					float colonistRelationChanceFactor = 1f;
					bool forceAddFreeWarmLayerIfNeeded = false;
					bool allowGay = true;
					bool allowPregnant = false;
					float biocodeWeaponsChance = parms.biocodeWeaponsChance;
					float biocodeApparelChance = parms.biocodeApparelChance;
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, faction, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, this.def.pawnsCanBringFood, true, false, false, false, false, biocodeWeaponsChance, biocodeApparelChance, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)
					{
						BiocodeApparelChance = 1f
					});
					if (pawn != null)
					{
						list.Add(pawn);
					}
				}
				if (list.Any<Pawn>())
				{
					parms.raidArrivalMode.Worker.Arrive(list, parms);
					return list;
				}
			}
			return null;
		}

		// Token: 0x04003FD9 RID: 16345
		public RaidStrategyDef def;
	}
}
