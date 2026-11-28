using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D3 RID: 4307
	public static class MoveColonyUtility
	{
		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06006BAF RID: 27567 RVA: 0x0025C52F File Offset: 0x0025A72F
		public static bool TitleAndRoleRequirementsGracePeriodActive
		{
			get
			{
				return MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft > 0;
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06006BB0 RID: 27568 RVA: 0x0025C539 File Offset: 0x0025A739
		public static int TitleAndRoleRequirementGracePeriodTicksLeft
		{
			get
			{
				if (!Find.TickManager.HasSettledNewColony)
				{
					return 0;
				}
				return Mathf.Max(0, 600000 - Find.TickManager.TicksSinceSettle);
			}
		}

		// Token: 0x06006BB1 RID: 27569 RVA: 0x0025C560 File Offset: 0x0025A760
		public static void PickNewColonyTile(Action<PlanetTile> targetChosen, Action noTileChosen = null)
		{
			Find.TilePicker.StartTargeting_NewTemp(delegate(PlanetTile tile)
			{
				MoveColonyUtility.cannotPlaceTileReason.Clear();
				if (!TileFinder.IsValidTileForNewSettlement(tile, MoveColonyUtility.cannotPlaceTileReason, false))
				{
					Messages.Message(MoveColonyUtility.cannotPlaceTileReason.ToString(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				return true;
			}, delegate(PlanetTile tile)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				targetChosen(tile);
			}, null, null, false, noTileChosen, "ChooseNextColonySite".Translate(), true, false, false, true, false, null);
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x0025C5C8 File Offset: 0x0025A7C8
		public static Settlement MoveColonyAndReset(PlanetTile tile, IEnumerable<Thing> colonyThings, Faction takeoverFaction = null, WorldObjectDef worldObjectDef = null)
		{
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (quest.IsEndOnNewArchonexusSettlement())
				{
					quest.hidden = true;
					quest.End(QuestEndOutcome.Unknown, false, true);
				}
			}
			List<Pawn> list = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction.ToList<Pawn>();
			foreach (Pawn pawn in list)
			{
				if (!colonyThings.Contains(pawn))
				{
					pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
					if (pawn == pawn.Faction.leader)
					{
						pawn.Faction.Notify_LeaderLost();
					}
					if (pawn.IsCaravanMember())
					{
						pawn.GetCaravan().RemovePawn(pawn);
					}
					if (pawn.holdingOwner != null)
					{
						pawn.holdingOwner.Remove(pawn);
					}
					if (!pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
					if (ModsConfig.BiotechActive && pawn.RaceProps.IsMechanoid)
					{
						Pawn overseer = pawn.GetOverseer();
						if (overseer != null && colonyThings.Contains(overseer))
						{
							overseer.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, pawn);
						}
					}
				}
			}
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = caravans.Count - 1; i >= 0; i--)
			{
				if (caravans[i].IsPlayerControlled)
				{
					caravans[i].RemoveAllPawns();
					caravans[i].Destroy();
				}
			}
			List<TravellingTransporters> travellingTransporters = Find.WorldObjects.TravellingTransporters;
			for (int j = travellingTransporters.Count - 1; j >= 0; j--)
			{
				travellingTransporters[j].Destroy();
			}
			foreach (Thing thing4 in colonyThings)
			{
				thing4.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (thing4.holdingOwner != null)
				{
					thing4.holdingOwner.Remove(thing4);
				}
			}
			List<MapParent> mapParents = Find.World.worldObjects.MapParents;
			for (int k = mapParents.Count - 1; k >= 0; k--)
			{
				mapParents[k].CheckRemoveMapNow();
			}
			MoveColonyUtility.playerSettlementsRemoved.Clear();
			List<Map> maps = Find.Maps;
			for (int l = maps.Count - 1; l >= 0; l--)
			{
				Map map = maps[l];
				if (map.IsPlayerHome)
				{
					MoveColonyUtility.playerSettlementsRemoved.Add(map.Tile);
					map.Parent.SetFaction(null);
					Current.Game.DeinitAndRemoveMap(map, false);
					map.Parent.Destroy();
					foreach (Pawn pawn2 in list)
					{
						if (pawn2.playerSettings != null)
						{
							pawn2.playerSettings.Notify_MapRemoved(map);
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				List<Site> sites = Find.WorldObjects.Sites;
				for (int m = sites.Count - 1; m >= 0; m--)
				{
					if (sites[m].parts.Any((SitePart p) => p.def == SitePartDefOf.AncientComplex))
					{
						Find.WorldObjects.Remove(sites[m]);
					}
				}
			}
			if (ModsConfig.AnomalyActive)
			{
				Find.Anomaly.ResetMonolith();
				Find.Anomaly.monolith = null;
			}
			if (ModsConfig.OdysseyActive)
			{
				Find.QuestManager.QuestsListForReading.RemoveAll((Quest q) => q.root == QuestScriptDefOf.MechanoidSignal);
			}
			Find.GameInfo.startingTile = tile;
			WorldObjectDef worldObjectDef2 = worldObjectDef ?? tile.LayerDef.SettlementWorldObjectDef;
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(worldObjectDef2);
			settlement.SetFaction(Faction.OfPlayer);
			settlement.Tile = tile;
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, Faction.OfPlayer.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(settlement);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(settlement.Tile, worldObjectDef2, null);
			IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
			List<List<Thing>> list2 = new List<List<Thing>>();
			List<Pawn> list3 = new List<Pawn>();
			foreach (Thing thing2 in colonyThings)
			{
				if (thing2 is Pawn)
				{
					list2.Add(new List<Thing>
					{
						thing2
					});
					list3.Add((Pawn)thing2);
				}
			}
			int num = 0;
			using (IEnumerator<Thing> enumerator3 = colonyThings.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					Thing thing = enumerator3.Current;
					if (thing.def.CanHaveFaction && thing.Faction != Faction.OfPlayer)
					{
						Pawn pawn3 = thing as Pawn;
						if (pawn3 != null && pawn3.IsSlaveOfColony)
						{
							continue;
						}
						thing.SetFaction(Faction.OfPlayer, null);
					}
					if (!list2.Any((List<Thing> g) => g.Contains(thing)))
					{
						list2[num].Add(thing);
						num = (num + 1) % list2.Count;
					}
				}
			}
			foreach (Pawn pawn4 in list3)
			{
				pawn4.inventory.DestroyAll(DestroyMode.Vanish);
				pawn4.ownership.UnclaimThrone();
				pawn4.ownership.UnclaimDeathrestCasket();
				Pawn_GeneTracker genes = pawn4.genes;
				if (genes != null)
				{
					genes.Notify_NewColony();
				}
			}
			MoveColonyUtility.RemoveWeaponsAndUtilityItems(list3, colonyThings);
			foreach (Thing thing3 in MoveColonyUtility.GetAbandonedRelicsCarriedByPawns(colonyThings))
			{
				if (!thing3.DestroyedOrNull())
				{
					thing3.Destroy(DestroyMode.Vanish);
				}
			}
			DropPodUtility.DropThingGroupsNear(playerStartSpot, orGenerateMap, list2, 110, false, false, true, true, true, false, null);
			if (takeoverFaction != null)
			{
				foreach (PlanetTile tile2 in MoveColonyUtility.playerSettlementsRemoved)
				{
					SettleUtility.AddNewHome(tile2, takeoverFaction);
				}
			}
			MoveColonyUtility.playerSettlementsRemoved.Clear();
			List<ResearchProjectDef> list4 = (from proj in DefDatabase<ResearchProjectDef>.AllDefs
			where proj.IsFinished && proj.HasTag(ResearchProjectTagDefOf.ClassicStart)
			select proj).ToList<ResearchProjectDef>();
			Find.ResearchManager.ResetAllProgress();
			EntityCodex entityCodex = Find.EntityCodex;
			if (entityCodex != null)
			{
				entityCodex.Reset();
			}
			ResearchUtility.ApplyPlayerStartingResearch();
			foreach (ResearchProjectDef proj2 in list4)
			{
				Find.ResearchManager.FinishProject(proj2, false, null, false);
			}
			FactionUtility.ResetAllFactionRelations();
			if (ModsConfig.BiotechActive)
			{
				GameComponent_Bossgroup component = Current.Game.GetComponent<GameComponent_Bossgroup>();
				if (component != null)
				{
					component.ResetProgress();
				}
			}
			MoveColonyUtility.SetPawnThoughts(list3);
			MoveColonyUtility.ResetNeedLevels(list3);
			MoveColonyUtility.RemoveHediffs(list3);
			MoveColonyUtility.ResetStartingGracePeriods(list3);
			Find.FactionManager.OfPlayer.ideos.RecalculateIdeosBasedOnPlayerPawns();
			IdeoUtility.Notify_NewColonyStarted();
			return settlement;
		}

		// Token: 0x06006BB3 RID: 27571 RVA: 0x0025CD9C File Offset: 0x0025AF9C
		public static IEnumerable<Thing> GetStartingThingsForNewColony()
		{
			return new MoveColonyUtility.<GetStartingThingsForNewColony>d__9(-2);
		}

		// Token: 0x06006BB4 RID: 27572 RVA: 0x0025CDA5 File Offset: 0x0025AFA5
		public static bool IsBringableItem(Thing t)
		{
			return !t.def.destroyOnDrop && !t.IsRelic() && (t.Map == null || !t.Position.Fogged(t.Map));
		}

		// Token: 0x06006BB5 RID: 27573 RVA: 0x0025CDE0 File Offset: 0x0025AFE0
		public static bool IsDistinctArchonexusItem(ThingDef td)
		{
			if ((!td.IsWeapon && !td.IsApparel) || !td.HasComp(typeof(CompQuality)))
			{
				if (td != null)
				{
					List<string> weaponTags = td.weaponTags;
					if (((weaponTags != null) ? new bool?(weaponTags.Contains("Gun")) : null).GetValueOrDefault())
					{
						return true;
					}
				}
				if (td == null)
				{
					return false;
				}
				ApparelProperties apparel = td.apparel;
				bool? flag;
				if (apparel == null)
				{
					flag = null;
				}
				else
				{
					ApparelLayerDef lastLayer = apparel.LastLayer;
					flag = ((lastLayer != null) ? new bool?(lastLayer.IsUtilityLayer) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
			return true;
		}

		// Token: 0x06006BB6 RID: 27574 RVA: 0x0025CE84 File Offset: 0x0025B084
		public static void RemoveWeaponsAndUtilityItems(List<Pawn> pawns, IEnumerable<Thing> selectedThings)
		{
			List<Thing> list = new List<Thing>();
			foreach (Pawn pawn in pawns)
			{
				if (pawn.equipment != null)
				{
					foreach (ThingWithComps thingWithComps in pawn.equipment.AllEquipmentListForReading)
					{
						if (!thingWithComps.DestroyedOrNull() && !selectedThings.Contains(thingWithComps))
						{
							list.Add(thingWithComps);
						}
					}
				}
				if (pawn.apparel != null)
				{
					foreach (Apparel apparel in pawn.apparel.WornApparel)
					{
						ThingDef def = apparel.def;
						bool flag;
						if (def == null)
						{
							flag = false;
						}
						else
						{
							ApparelProperties apparel2 = def.apparel;
							bool? flag2;
							if (apparel2 == null)
							{
								flag2 = null;
							}
							else
							{
								ApparelLayerDef lastLayer = apparel2.LastLayer;
								flag2 = ((lastLayer != null) ? new bool?(lastLayer.IsUtilityLayer) : null);
							}
							bool? flag3 = flag2;
							flag = flag3.GetValueOrDefault();
						}
						if (flag && !selectedThings.Contains(apparel))
						{
							list.Add(apparel);
						}
					}
				}
			}
			foreach (Thing thing in list)
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006BB7 RID: 27575 RVA: 0x0025D054 File Offset: 0x0025B254
		public static List<Thing> GetAbandonedRelicsCarriedByPawns(IEnumerable<Thing> selectedThings)
		{
			List<Thing> list = new List<Thing>();
			List<Pawn> list2 = new List<Pawn>();
			foreach (Thing thing in selectedThings)
			{
				if (thing.IsRelic())
				{
					list.Add(thing);
				}
				if (thing is Pawn)
				{
					list2.Add((Pawn)thing);
				}
			}
			List<Thing> list3 = new List<Thing>();
			foreach (Pawn pawn in list2)
			{
				foreach (Thing thing2 in pawn.EquippedWornOrInventoryThings)
				{
					if (thing2.IsRelic() && !list.Contains(thing2))
					{
						list3.Add(thing2);
					}
				}
			}
			return list3;
		}

		// Token: 0x06006BB8 RID: 27576 RVA: 0x0025D158 File Offset: 0x0025B358
		private static void ResetStartingGracePeriods(List<Pawn> pawns)
		{
			Find.TickManager.ResetSettlementTicks();
			Find.StoryWatcher.watcherAdaptation.ResetAdaptDays();
			Find.StoryWatcher.watcherPopAdaptation.ResetAdaptDays();
			foreach (Pawn pawn in pawns)
			{
				if (pawn.IsColonist)
				{
					pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Initial, false);
				}
			}
		}

		// Token: 0x06006BB9 RID: 27577 RVA: 0x0025D1DC File Offset: 0x0025B3DC
		private static void SetPawnThoughts(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				MemoryThoughtHandler memoryThoughtHandler;
				if (needs == null)
				{
					memoryThoughtHandler = null;
				}
				else
				{
					Need_Mood mood = needs.mood;
					if (mood == null)
					{
						memoryThoughtHandler = null;
					}
					else
					{
						ThoughtHandler thoughts = mood.thoughts;
						memoryThoughtHandler = ((thoughts != null) ? thoughts.memories : null);
					}
				}
				MemoryThoughtHandler memoryThoughtHandler2 = memoryThoughtHandler;
				if (memoryThoughtHandler2 != null)
				{
					memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyOptimism);
					memoryThoughtHandler2.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyHope);
					if (pawn.IsFreeNonSlaveColonist)
					{
						memoryThoughtHandler2.TryGainMemory(ThoughtDefOf.NewColonyOptimism, null, null);
					}
				}
			}
		}

		// Token: 0x06006BBA RID: 27578 RVA: 0x0025D27C File Offset: 0x0025B47C
		private static void ResetNeedLevels(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Food food = needs.food;
					if (food != null)
					{
						food.SetInitialLevel();
					}
				}
				Pawn_NeedsTracker needs2 = pawn.needs;
				if (needs2 != null)
				{
					Need_Rest rest = needs2.rest;
					if (rest != null)
					{
						rest.SetInitialLevel();
					}
				}
			}
		}

		// Token: 0x06006BBB RID: 27579 RVA: 0x0025D2FC File Offset: 0x0025B4FC
		private static void RemoveHediffs(List<Pawn> pawns)
		{
			List<Hediff> list = new List<Hediff>();
			foreach (Pawn pawn in pawns)
			{
				list.Clear();
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					if (hediff.def == HediffDefOf.Hypothermia || hediff.def == HediffDefOf.Heatstroke || hediff.def == HediffDefOf.Malnutrition)
					{
						list.Add(hediff);
					}
				}
				foreach (Hediff hediff2 in list)
				{
					pawn.health.RemoveHediff(hediff2);
				}
			}
		}

		// Token: 0x04004479 RID: 17529
		private static StringBuilder cannotPlaceTileReason = new StringBuilder();

		// Token: 0x0400447A RID: 17530
		public const int TitleAndRoleRequirementGracePeriodTicks = 600000;

		// Token: 0x0400447B RID: 17531
		private static List<PlanetTile> playerSettlementsRemoved = new List<PlanetTile>();
	}
}
