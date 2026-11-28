using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002270 RID: 8816
	public class QuestNode_Root_DistressCall : QuestNode
	{
		// Token: 0x17002616 RID: 9750
		// (get) Token: 0x0600D9C0 RID: 55744 RVA: 0x0050C82C File Offset: 0x0050AA2C
		protected static List<LandmarkDef> AllowedLandmarks
		{
			get
			{
				if (ModsConfig.OdysseyActive && QuestNode_Root_DistressCall.allowedLandmarksCached == null)
				{
					QuestNode_Root_DistressCall.allowedLandmarksCached = new List<LandmarkDef>
					{
						LandmarkDefOf.Oasis,
						LandmarkDefOf.Lake,
						LandmarkDefOf.LakeWithIsland,
						LandmarkDefOf.LakeWithIslands,
						LandmarkDefOf.Pond,
						LandmarkDefOf.DryLake,
						LandmarkDefOf.ToxicLake,
						LandmarkDefOf.Wetland,
						LandmarkDefOf.HotSprings,
						LandmarkDefOf.CoastalIsland,
						LandmarkDefOf.Peninsula,
						LandmarkDefOf.Valley,
						LandmarkDefOf.Cavern,
						LandmarkDefOf.Chasm,
						LandmarkDefOf.Cliffs,
						LandmarkDefOf.Hollow,
						LandmarkDefOf.TerraformingScar,
						LandmarkDefOf.Dunes
					};
				}
				return QuestNode_Root_DistressCall.allowedLandmarksCached;
			}
		}

		// Token: 0x0600D9C1 RID: 55745 RVA: 0x0050C924 File Offset: 0x0050AB24
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests)
			{
				return false;
			}
			QuestGenUtility.TestRunAdjustPointsForDistantFight(slate);
			float num = slate.Get<float>("points", 0f, false);
			PlanetTile planetTile;
			Faction faction;
			return num >= 100f && this.TryFindSiteTile(out planetTile, true) && this.TryFindFaction(num, out faction);
		}

		// Token: 0x0600D9C2 RID: 55746 RVA: 0x0050C97C File Offset: 0x0050AB7C
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			float num = slate.Get<float>("points", 0f, false);
			if (num < 100f)
			{
				num = 100f;
			}
			PlanetTile tile;
			this.TryFindSiteTile(out tile, false);
			Faction faction;
			this.TryFindFaction(num, out faction);
			slate.Set<Faction>("faction", faction, false);
			IEnumerable<SitePartDef> source = from def in DefDatabase<SitePartDef>.AllDefs
			where def.tags != null && def.tags.Contains("DistressCall") && typeof(SitePartWorker_DistressCall).IsAssignableFrom(def.workerClass)
			select def;
			SitePartDefWithParams[] array = new SitePartDefWithParams[1];
			array[0] = new SitePartDefWithParams(source.RandomElementByWeight((SitePartDef sp) => sp.selectionWeight), new SitePartParams
			{
				threatPoints = num
			});
			Site site = QuestGen_Sites.GenerateSite(array, tile, faction, false, null, null);
			quest.SpawnWorldObject(site, null, null);
			slate.Set<Site>("site", site, false);
			string inSignalEnable = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.NoActiveThreats");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string signalAmbush = QuestGenUtility.HardcodedSignalWithQuestID("ambush");
			Quest quest2 = quest;
			LetterDef neutralEvent = LetterDefOf.NeutralEvent;
			string inSignal3 = null;
			string chosenPawnSignal = null;
			string label = "DistressSignalLabel".Translate();
			string text = "DistressSignalText".Translate(site.Faction.Named("FACTION")).Resolve();
			IEnumerable<object> lookTargets = Gen.YieldSingle<Site>(site);
			quest2.Letter(neutralEvent, inSignal3, chosenPawnSignal, site.Faction, null, false, QuestPart.SignalListenMode.OngoingOnly, lookTargets, false, text, null, label, null, null);
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_CampLoot());
			questPart_Choice.choices.Add(choice);
			if (Rand.Chance(0.75f))
			{
				quest.Delay(QuestNode_Root_DistressCall.AmbushDelayTicks.RandomInRange, delegate
				{
					quest.SignalPass(null, null, signalAmbush);
				}, inSignalEnable, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
				quest.AddPart(new QuestPart_DistressCallAmbush(signalAmbush, site, QuestNode_Root_DistressCall.AmbushPointsCurve.Evaluate(num)));
			}
			quest.WorldObjectTimeout(site, 900000, null, null, false, null, true);
			quest.Delay(900000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x0600D9C3 RID: 55747 RVA: 0x0050CC14 File Offset: 0x0050AE14
		private bool TryFindFaction(float points, out Faction faction)
		{
			return (from f in Find.FactionManager.AllFactionsListForReading
			where this.FactionUsable(f, points)
			select f).TryRandomElement(out faction);
		}

		// Token: 0x0600D9C4 RID: 55748 RVA: 0x0050CC58 File Offset: 0x0050AE58
		private bool FactionUsable(Faction f, float points)
		{
			return (!ModsConfig.RoyaltyActive || points >= 2000f || f != Faction.OfEmpire) && f.def.canGenerateQuestSites && (f.def.humanlikeFaction && !f.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()) && !f.def.permanentEnemy;
		}

		// Token: 0x0600D9C5 RID: 55749 RVA: 0x0050CCBC File Offset: 0x0050AEBC
		private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 3, 9, false, QuestNode_Root_DistressCall.AllowedLandmarks, 0.5f, true, TileFinderMode.Near, exitOnFirstTileFound, false, null, null);
		}

		// Token: 0x04008B2F RID: 35631
		private static List<LandmarkDef> allowedLandmarksCached;

		// Token: 0x04008B30 RID: 35632
		private const string SitePartTag = "DistressCall";

		// Token: 0x04008B31 RID: 35633
		private const int MaxDistanceFromColony = 9;

		// Token: 0x04008B32 RID: 35634
		private const int MinDistanceFromColony = 3;

		// Token: 0x04008B33 RID: 35635
		private const float MinPoints = 100f;

		// Token: 0x04008B34 RID: 35636
		private const int TimeoutTicks = 900000;

		// Token: 0x04008B35 RID: 35637
		private const float EmpireSitePointsThreshold = 2000f;

		// Token: 0x04008B36 RID: 35638
		private const float AmbushChance = 0.75f;

		// Token: 0x04008B37 RID: 35639
		private static readonly IntRange AmbushDelayTicks = new IntRange(2400, 4800);

		// Token: 0x04008B38 RID: 35640
		private static readonly SimpleCurve AmbushPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 100f),
				true
			},
			{
				new CurvePoint(1000f, 400f),
				true
			},
			{
				new CurvePoint(5000f, 1000f),
				true
			}
		};
	}
}
