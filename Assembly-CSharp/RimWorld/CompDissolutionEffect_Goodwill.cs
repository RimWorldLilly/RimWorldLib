using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E0 RID: 6624
	public class CompDissolutionEffect_Goodwill : CompDissolutionEffect
	{
		// Token: 0x0600A6D5 RID: 42709 RVA: 0x003B722C File Offset: 0x003B542C
		public override void DoDissolutionEffectMap(int amount)
		{
			if (!this.parent.Spawned || this.parent.Map.IsPlayerHome)
			{
				return;
			}
			CompDissolutionEffect_Goodwill.GoodwillPollutionEvent item = new CompDissolutionEffect_Goodwill.GoodwillPollutionEvent
			{
				tile = this.parent.Map.Tile,
				amount = amount
			};
			CompDissolutionEffect_Goodwill.pendingGoodwillEvents.Add(item);
		}

		// Token: 0x0600A6D6 RID: 42710 RVA: 0x003B728D File Offset: 0x003B548D
		public override void DoDissolutionEffectWorld(int amount, PlanetTile tileId)
		{
			CompDissolutionEffect_Goodwill.AddWorldDissolutionEvent(amount, tileId);
		}

		// Token: 0x0600A6D7 RID: 42711 RVA: 0x003B7298 File Offset: 0x003B5498
		public static void AddWorldDissolutionEvent(int amount, PlanetTile tile)
		{
			if (PollutionUtility.IsExecutingPollutionIgnoredQuest())
			{
				return;
			}
			CompDissolutionEffect_Goodwill.GoodwillPollutionEvent item = new CompDissolutionEffect_Goodwill.GoodwillPollutionEvent
			{
				tile = tile,
				amount = amount
			};
			CompDissolutionEffect_Goodwill.pendingGoodwillEvents.Add(item);
		}

		// Token: 0x0600A6D8 RID: 42712 RVA: 0x003B72D4 File Offset: 0x003B54D4
		public static void WorldUpdate()
		{
			if (CompDissolutionEffect_Goodwill.pendingGoodwillEvents.Count <= 0)
			{
				return;
			}
			foreach (IGrouping<PlanetTile, CompDissolutionEffect_Goodwill.GoodwillPollutionEvent> grouping in from g in CompDissolutionEffect_Goodwill.pendingGoodwillEvents
			group g by g.tile)
			{
				PlanetTile key = grouping.Key;
				Settlement settlement;
				int num;
				if (CompDissolutionEffect_Goodwill.TryGetAffectedSettlement(key, out settlement, out num))
				{
					int num2 = grouping.Sum((CompDissolutionEffect_Goodwill.GoodwillPollutionEvent p) => p.amount);
					int num3 = Mathf.Min(-Mathf.RoundToInt(CompDissolutionEffect_Goodwill.GoodwillFactorOverDistanceCurvePerWastepack.Evaluate((float)num) * (float)num2), -1);
					HistoryEventDef historyEventDef;
					if (ModsConfig.OdysseyActive && key.LayerDef.isSpace)
					{
						historyEventDef = HistoryEventDefOf.OrbitalPollution;
					}
					else if (settlement.Tile == key)
					{
						historyEventDef = HistoryEventDefOf.PollutedBase;
					}
					else if (num <= 8)
					{
						historyEventDef = HistoryEventDefOf.PollutedNearbySite;
					}
					else
					{
						historyEventDef = HistoryEventDefOf.ToxicWasteDumping;
					}
					if (settlement.Faction.IsPlayerGoodwillMinimum())
					{
						Messages.Message("MessageAngeredPollutedCell".Translate(settlement.Faction.Name, historyEventDef.label), settlement, MessageTypeDefOf.NegativeEvent, true);
					}
					Faction.OfPlayer.TryAffectGoodwillWith(settlement.Faction, num3, true, true, historyEventDef, new GlobalTargetInfo?(settlement));
					if (!Current.Game.IsPlayerTile(key) && settlement.Faction.HostileTo(Faction.OfPlayer) && Find.AnyPlayerHomeMap != null && Rand.Chance(Mathf.Clamp01((float)(-(float)num3) / 100f)))
					{
						CompDissolutionEffect_Goodwill.TriggerRetaliationEvent(settlement.Faction);
					}
				}
				CompDissolutionEffect_Goodwill.tmpAvailableSettlements.Clear();
			}
			CompDissolutionEffect_Goodwill.pendingGoodwillEvents.Clear();
		}

		// Token: 0x0600A6D9 RID: 42713 RVA: 0x003B74CC File Offset: 0x003B56CC
		private static bool TryGetAffectedSettlement(PlanetTile tile, out Settlement result, out int distance)
		{
			result = null;
			distance = 0;
			if (!tile.Valid)
			{
				return false;
			}
			Settlement settlement = null;
			float num = 0f;
			foreach (WorldObject worldObject in Find.WorldObjects.AllSettlementsOnLayer(tile.Layer))
			{
				if (worldObject.Faction != Faction.OfPlayer && !worldObject.Faction.Hidden)
				{
					if (tile == worldObject.Tile)
					{
						result = (Settlement)worldObject;
						distance = 0;
						return true;
					}
					float num2 = Find.WorldGrid.ApproxDistanceInTiles(worldObject.Tile, tile);
					if (settlement == null || num2 < num)
					{
						settlement = (Settlement)worldObject;
						num = num2;
					}
				}
			}
			if (settlement == null)
			{
				return false;
			}
			float maxDistance = Find.WorldGrid.ApproxDistanceInTiles(settlement.Tile, tile) * 1.25f;
			CompDissolutionEffect_Goodwill.tmpAvailableSettlements.Clear();
			foreach (WorldObject worldObject2 in Find.WorldObjects.AllSettlementsOnLayer(tile.Layer))
			{
				if (worldObject2.Faction != Faction.OfPlayer && !worldObject2.Faction.Hidden)
				{
					float num3 = Find.WorldGrid.ApproxDistanceInTiles(worldObject2.Tile, tile);
					if (num3 <= maxDistance)
					{
						CompDissolutionEffect_Goodwill.tmpAvailableSettlements.Add((Settlement)worldObject2, num3);
					}
				}
			}
			KeyValuePair<Settlement, float> keyValuePair;
			if (CompDissolutionEffect_Goodwill.tmpAvailableSettlements.TryRandomElementByWeight((KeyValuePair<Settlement, float> kvp) => maxDistance - kvp.Value, out keyValuePair))
			{
				result = keyValuePair.Key;
				distance = Mathf.RoundToInt(keyValuePair.Value);
			}
			CompDissolutionEffect_Goodwill.tmpAvailableSettlements.Clear();
			return result != null;
		}

		// Token: 0x0600A6DA RID: 42714 RVA: 0x003B76B0 File Offset: 0x003B58B0
		private static void TriggerRetaliationEvent(Faction faction)
		{
			Slate slate = new Slate();
			Map map = QuestGen_Get.GetMap(false, null, true);
			slate.Set<Map>("map", map, false);
			slate.Set<Faction>("enemyFaction", faction, false);
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(map), false);
			CompDissolutionEffect_Goodwill.tmpRetaliations.Clear();
			CompDissolutionEffect_Goodwill.tmpRetaliations.Add(QuestScriptDefOf.PollutionRetaliation);
			CompDissolutionEffect_Goodwill.tmpRetaliations.Add(QuestScriptDefOf.PollutionRaid);
			for (int i = CompDissolutionEffect_Goodwill.tmpRetaliations.Count - 1; i >= 0; i--)
			{
				if (!CompDissolutionEffect_Goodwill.tmpRetaliations[i].CanRun(slate, map))
				{
					CompDissolutionEffect_Goodwill.tmpRetaliations.RemoveAt(i);
				}
			}
			if (CompDissolutionEffect_Goodwill.tmpRetaliations.Count == 0)
			{
				Log.Warning("No pollution retaliations were possible");
				return;
			}
			QuestUtility.GenerateQuestAndMakeAvailable(CompDissolutionEffect_Goodwill.tmpRetaliations.RandomElement<QuestScriptDef>(), slate);
			CompDissolutionEffect_Goodwill.tmpRetaliations.Clear();
		}

		// Token: 0x04006246 RID: 25158
		private static readonly SimpleCurve GoodwillFactorOverDistanceCurvePerWastepack = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.9999f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.5f),
				true
			},
			{
				new CurvePoint(1.9999f, 0.5f),
				true
			},
			{
				new CurvePoint(2f, 0.2f),
				true
			},
			{
				new CurvePoint(4.9999f, 0.2f),
				true
			},
			{
				new CurvePoint(5f, 0.1f),
				true
			},
			{
				new CurvePoint(7.9999f, 0.1f),
				true
			},
			{
				new CurvePoint(8f, 0.066f),
				true
			},
			{
				new CurvePoint(16f, 0.066f),
				true
			},
			{
				new CurvePoint(16.0001f, 0.05f),
				true
			}
		};

		// Token: 0x04006247 RID: 25159
		private const float MaxDistanceFactor = 1.25f;

		// Token: 0x04006248 RID: 25160
		private static readonly List<CompDissolutionEffect_Goodwill.GoodwillPollutionEvent> pendingGoodwillEvents = new List<CompDissolutionEffect_Goodwill.GoodwillPollutionEvent>();

		// Token: 0x04006249 RID: 25161
		private static readonly Dictionary<Settlement, float> tmpAvailableSettlements = new Dictionary<Settlement, float>();

		// Token: 0x0400624A RID: 25162
		private static readonly List<QuestScriptDef> tmpRetaliations = new List<QuestScriptDef>();

		// Token: 0x02003727 RID: 14119
		private struct GoodwillPollutionEvent
		{
			// Token: 0x0400DC47 RID: 56391
			public PlanetTile tile;

			// Token: 0x0400DC48 RID: 56392
			public int amount;
		}
	}
}
