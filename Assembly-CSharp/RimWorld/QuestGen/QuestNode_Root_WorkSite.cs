using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B0 RID: 8880
	public class QuestNode_Root_WorkSite : QuestNode
	{
		// Token: 0x0600DB44 RID: 56132 RVA: 0x0051A98C File Offset: 0x00518B8C
		private static bool AnySpawnCandidate(PlanetTile aroundTile)
		{
			foreach (QuestNode_Root_WorkSite.SiteSpawnCandidate siteSpawnCandidate in QuestNode_Root_WorkSite.GetCandidates(aroundTile))
			{
				if (Find.WorldGrid[siteSpawnCandidate.tile].PrimaryBiome.campSelectionWeight > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600DB45 RID: 56133 RVA: 0x0051A9FC File Offset: 0x00518BFC
		private static QuestNode_Root_WorkSite.SiteSpawnCandidate GetSpawnCandidate(PlanetTile aroundTile)
		{
			List<QuestNode_Root_WorkSite.SiteSpawnCandidate> list = QuestNode_Root_WorkSite.GetCandidates(aroundTile).ToList<QuestNode_Root_WorkSite.SiteSpawnCandidate>();
			if (list.Count == 0)
			{
				return default(QuestNode_Root_WorkSite.SiteSpawnCandidate);
			}
			SitePartDef campTypeToGenerate = (from tct in list
			select tct.sitePart).Distinct<SitePartDef>().RandomElement<SitePartDef>();
			return (from tct in list
			where tct.sitePart == campTypeToGenerate
			select tct).RandomElementByWeightWithFallback((QuestNode_Root_WorkSite.SiteSpawnCandidate t) => Find.WorldGrid[t.tile].PrimaryBiome.campSelectionWeight, default(QuestNode_Root_WorkSite.SiteSpawnCandidate));
		}

		// Token: 0x0600DB46 RID: 56134 RVA: 0x0051AAA4 File Offset: 0x00518CA4
		private static IEnumerable<QuestNode_Root_WorkSite.SiteSpawnCandidate> GetCandidates(PlanetTile aroundTile)
		{
			IEnumerable<SitePartDef> source = from def in DefDatabase<SitePartDef>.AllDefs
			where def.tags != null && def.tags.Contains("WorkSite") && typeof(SitePartWorker_WorkSite).IsAssignableFrom(def.workerClass)
			select def;
			List<PlanetTile> potentialTiles = QuestNode_Root_WorkSite.PotentialSiteTiles(aroundTile);
			return source.SelectMany(delegate(SitePartDef sitePart)
			{
				SitePartWorker_WorkSite worker = (SitePartWorker_WorkSite)sitePart.Worker;
				return from tile in potentialTiles
				where worker.CanSpawnOn(tile)
				select tile into t
				select new QuestNode_Root_WorkSite.SiteSpawnCandidate
				{
					tile = t,
					sitePart = sitePart
				};
			});
		}

		// Token: 0x0600DB47 RID: 56135 RVA: 0x0051AB00 File Offset: 0x00518D00
		public static Site GenerateSite(float points, PlanetTile aroundTile)
		{
			QuestNode_Root_WorkSite.<>c__DisplayClass16_0 CS$<>8__locals1 = new QuestNode_Root_WorkSite.<>c__DisplayClass16_0();
			QuestNode_Root_WorkSite.SiteSpawnCandidate spawnCandidate = QuestNode_Root_WorkSite.GetSpawnCandidate(aroundTile);
			CS$<>8__locals1.sitePartWorker = (SitePartWorker_WorkSite)spawnCandidate.sitePart.Worker;
			Faction faction = null;
			QuestNode_Root_WorkSite.FactionType item = QuestNode_Root_WorkSite.FactionChances.RandomElementByWeight((Tuple<float, QuestNode_Root_WorkSite.FactionType> e) => e.Item1).Item2;
			QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
			QuestGen.quest.AddPart(questPart_Hyperlinks);
			CS$<>8__locals1.factionValidator = null;
			if (item != QuestNode_Root_WorkSite.FactionType.Enemy)
			{
				if (item == QuestNode_Root_WorkSite.FactionType.AllyOrNeutral)
				{
					CS$<>8__locals1.factionValidator = ((Faction f) => f.AllyOrNeutralTo(Faction.OfPlayer));
				}
			}
			else
			{
				CS$<>8__locals1.factionValidator = ((Faction f) => f.HostileTo(Faction.OfPlayer));
			}
			if (CS$<>8__locals1.factionValidator != null)
			{
				faction = (from f in Find.FactionManager.AllFactionsListForReading
				where base.<GenerateSite>g__FactionUseable|2(f) && CS$<>8__locals1.factionValidator(f)
				select f).RandomElementWithFallback(null);
			}
			if (faction != null)
			{
				questPart_Hyperlinks.factions.Add(faction);
			}
			else
			{
				FactionDef factionDef = DefDatabase<FactionDef>.AllDefsListForReading.Where(new Func<FactionDef, bool>(CS$<>8__locals1.<GenerateSite>g__FactionDefUseable|1)).RandomElement<FactionDef>();
				bool? hidden = new bool?(true);
				FactionGeneratorParms factionGeneratorParms = new FactionGeneratorParms(factionDef, default(IdeoGenerationParms), hidden);
				factionGeneratorParms.ideoGenerationParms = new IdeoGenerationParms(factionGeneratorParms.factionDef, false, CS$<>8__locals1.sitePartWorker.DisallowedPrecepts.ToList<PreceptDef>(), null, null, false, false, false, false, "", null, null, false, "", false);
				List<FactionRelation> list = new List<FactionRelation>();
				foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
				{
					if (!faction2.def.PermanentlyHostileTo(factionGeneratorParms.factionDef))
					{
						if (faction2 == Faction.OfPlayer)
						{
							list.Add(new FactionRelation
							{
								other = faction2,
								kind = FactionRelationKind.Hostile
							});
						}
						else
						{
							list.Add(new FactionRelation
							{
								other = faction2,
								kind = FactionRelationKind.Neutral
							});
						}
					}
				}
				faction = FactionGenerator.NewGeneratedFactionWithRelations(factionGeneratorParms, list);
				faction.temporary = true;
				Find.FactionManager.Add(faction);
			}
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(spawnCandidate.sitePart, new SitePartParams
				{
					threatPoints = points
				})
			}, spawnCandidate.tile, faction, false, null, null);
			if (site.parts.Any<SitePart>() && site.parts[0].lootThings.Any<ThingDefCount>())
			{
				questPart_Hyperlinks.thingDefs.Add(site.parts[0].lootThings[0].ThingDef);
			}
			site.desiredThreatPoints = site.ActualThreatPoints;
			return site;
		}

		// Token: 0x0600DB48 RID: 56136 RVA: 0x0051ADD4 File Offset: 0x00518FD4
		public static List<PlanetTile> PotentialSiteTiles(PlanetTile root)
		{
			List<PlanetTile> tiles = new List<PlanetTile>();
			root.Layer.Filler.FloodFill(root, (PlanetTile p) => !Find.World.Impassable(p) && Find.WorldGrid.ApproxDistanceInTiles(p, root) <= 9f, delegate(PlanetTile p)
			{
				if (Find.WorldGrid.ApproxDistanceInTiles(p, root) >= 3f)
				{
					WorldLandmarks landmarks = Find.World.landmarks;
					if (((landmarks != null) ? landmarks[p] : null) == null)
					{
						tiles.Add(p);
					}
				}
			}, int.MaxValue, null);
			return tiles;
		}

		// Token: 0x0600DB49 RID: 56137 RVA: 0x0051AE38 File Offset: 0x00519038
		public static float AppearanceFrequency(Map map)
		{
			float num = 1f;
			float num2 = 0f;
			List<PlanetTile> list = QuestNode_Root_WorkSite.PotentialSiteTiles(map.Tile);
			if (list.Count == 0)
			{
				return 0f;
			}
			if (!QuestNode_Root_WorkSite.AnySpawnCandidate(map.Tile))
			{
				return 0f;
			}
			foreach (PlanetTile tile in list)
			{
				num2 += Find.WorldGrid[tile].PrimaryBiome.campSelectionWeight;
			}
			num2 /= (float)list.Count;
			num *= num2;
			int num3 = 0;
			foreach (Site site in Find.WorldObjects.Sites)
			{
				if (site.MainSitePartDef.tags != null && site.MainSitePartDef.tags.Contains("WorkSite"))
				{
					num3++;
				}
			}
			num *= QuestNode_Root_WorkSite.ExistingCampsAppearanceFrequencyMultiplier.Evaluate((float)num3);
			int num4 = map.mapPawns.FreeColonists.Count<Pawn>();
			if (num4 <= 1)
			{
				return 0f;
			}
			if (num4 == 2)
			{
				return num / 2f;
			}
			return num;
		}

		// Token: 0x0600DB4A RID: 56138 RVA: 0x0051AF88 File Offset: 0x00519188
		public static float BestAppearanceFrequency()
		{
			float num = 0f;
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					num = Mathf.Max(num, QuestNode_Root_WorkSite.AppearanceFrequency(map));
				}
			}
			return num;
		}

		// Token: 0x0600DB4B RID: 56139 RVA: 0x0051AFF0 File Offset: 0x005191F0
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Work site"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			QuestGenUtility.RunAdjustPointsForDistantFight();
			float num = slate.Get<float>("points", 0f, false);
			if (num < 40f)
			{
				num = 40f;
			}
			Map map = (from m in Find.Maps
			where m.IsPlayerHome && QuestNode_Root_WorkSite.GetCandidates(m.Tile).Any<QuestNode_Root_WorkSite.SiteSpawnCandidate>()
			select m).RandomElementByWeight((Map m) => QuestNode_Root_WorkSite.AppearanceFrequency(m));
			slate.Set<Map>("map", map, false);
			Site site = QuestNode_Root_WorkSite.GenerateSite(num, map.Tile);
			quest.SpawnWorldObject(site, null, null);
			quest.ReserveFaction(site.Faction);
			int num2 = 1800000;
			quest.WorldObjectTimeout(site, num2, null, null, false, null, true);
			quest.Delay(num2, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.Message("MessageCampDetected".Translate(site.Named("CAMP"), site.Faction.Named("FACTION")), MessageTypeDefOf.NeutralEvent, false, null, new LookTargets(site), null);
			SitePart sitePart = site.parts[0];
			if (!sitePart.things.NullOrEmpty<Thing>())
			{
				ThingDef def = sitePart.things.First<Thing>().def;
				int num3 = 0;
				foreach (Thing thing in ((IEnumerable<Thing>)sitePart.things))
				{
					if (thing.def == def)
					{
						num3 += thing.stackCount;
					}
				}
				QuestGen.AddQuestDescriptionRules(new List<Rule>
				{
					new Rule_String("loot", def.label + " x" + num3.ToString())
				});
			}
			slate.Set<Site>("campSite", site, false);
			slate.Set<Faction>("faction", site.Faction, false);
			slate.Set<int>("timeout", num2, false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("campSite.AllEnemiesDefeated");
			string inSignalEnabled = QuestGenUtility.HardcodedSignalWithQuestID("campSite.MapGenerated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("campSite.MapRemoved");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("campSite.NoActiveThreats");
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_CampLoot());
			questPart_Choice.choices.Add(choice);
			if (num >= 400f)
			{
				quest.SurpriseReinforcements(inSignalEnabled, site, site.Faction, 0.35f);
			}
			quest.Notify_PlayerRaidedSomeone(null, site, inSignal);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			QuestGen.AddQuestDescriptionRules(new List<Rule>
			{
				new Rule_String("siteLabel", site.Label)
			});
		}

		// Token: 0x0600DB4C RID: 56140 RVA: 0x0051B32C File Offset: 0x0051952C
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			QuestGenUtility.TestRunAdjustPointsForDistantFight(slate);
			if (slate.Get<float>("points", 0f, false) < 40f)
			{
				return false;
			}
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome && QuestNode_Root_WorkSite.AppearanceFrequency(map) > 0f && QuestNode_Root_WorkSite.GetCandidates(map.Tile).Any<QuestNode_Root_WorkSite.SiteSpawnCandidate>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008C4D RID: 35917
		private const int SpawnRange = 9;

		// Token: 0x04008C4E RID: 35918
		private const int MinSpawnDist = 3;

		// Token: 0x04008C4F RID: 35919
		private const float MinPointsForSurpriseReinforcements = 400f;

		// Token: 0x04008C50 RID: 35920
		private const float SurpriseReinforcementChance = 0.35f;

		// Token: 0x04008C51 RID: 35921
		private static readonly SimpleCurve ExistingCampsAppearanceFrequencyMultiplier = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1f),
				true
			},
			{
				new CurvePoint(3f, 0.3f),
				true
			},
			{
				new CurvePoint(4f, 0.1f),
				true
			}
		};

		// Token: 0x04008C52 RID: 35922
		private const float MinPoints = 40f;

		// Token: 0x04008C53 RID: 35923
		private const string SitePartTag = "WorkSite";

		// Token: 0x04008C54 RID: 35924
		private const float TemporaryFactionChance = 0.6f;

		// Token: 0x04008C55 RID: 35925
		private const float EnemyFactionChance = 0.25f;

		// Token: 0x04008C56 RID: 35926
		private const float AllyFactionChance = 0.15f;

		// Token: 0x04008C57 RID: 35927
		private static readonly Tuple<float, QuestNode_Root_WorkSite.FactionType>[] FactionChances = new Tuple<float, QuestNode_Root_WorkSite.FactionType>[]
		{
			new Tuple<float, QuestNode_Root_WorkSite.FactionType>(0.6f, QuestNode_Root_WorkSite.FactionType.Temporary),
			new Tuple<float, QuestNode_Root_WorkSite.FactionType>(0.25f, QuestNode_Root_WorkSite.FactionType.Enemy),
			new Tuple<float, QuestNode_Root_WorkSite.FactionType>(0.15f, QuestNode_Root_WorkSite.FactionType.AllyOrNeutral)
		};

		// Token: 0x02003DB1 RID: 15793
		private struct SiteSpawnCandidate
		{
			// Token: 0x0400F34E RID: 62286
			public PlanetTile tile;

			// Token: 0x0400F34F RID: 62287
			public SitePartDef sitePart;
		}

		// Token: 0x02003DB2 RID: 15794
		private enum FactionType
		{
			// Token: 0x0400F351 RID: 62289
			Temporary,
			// Token: 0x0400F352 RID: 62290
			Enemy,
			// Token: 0x0400F353 RID: 62291
			AllyOrNeutral
		}
	}
}
