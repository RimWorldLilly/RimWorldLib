using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002278 RID: 8824
	public class QuestNode_Root_Loot_AncientComplex : QuestNode_Root_AncientComplex
	{
		// Token: 0x17002617 RID: 9751
		// (get) Token: 0x0600D9E8 RID: 55784 RVA: 0x0050EF98 File Offset: 0x0050D198
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientComplex_Loot;
			}
		}

		// Token: 0x0600D9E9 RID: 55785 RVA: 0x0050EF9F File Offset: 0x0050D19F
		protected virtual bool BeforeRunInt()
		{
			return ModLister.CheckIdeology("Ancient complex rescue");
		}

		// Token: 0x0600D9EA RID: 55786 RVA: 0x0050EFB0 File Offset: 0x0050D1B0
		protected override void RunInt()
		{
			if (!this.BeforeRunInt())
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			Map map = QuestGen_Get.GetMap(false, null, false);
			float num = slate.Get<float>("points", 0f, false);
			PlanetTile tile;
			if (!slate.TryGet<PlanetTile>("siteTile", out tile, false))
			{
				this.TryFindSiteTile(out tile, false);
			}
			Faction faction;
			this.TryFindEnemyFaction(out faction);
			QuestGen.GenerateNewSignal("RaidArrives", true);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.NoActiveThreats");
			LayoutStructureSketch ancientLayoutStructureSketch = this.QuestSetupComplex(quest, num);
			SitePartParams parms = new SitePartParams
			{
				threatPoints = (Find.Storyteller.difficulty.allowViolentQuests ? QuestNode_Root_AncientComplex.ThreatPointsOverPointsCurve.Evaluate(num) : 0f),
				ancientLayoutStructureSketch = ancientLayoutStructureSketch,
				ancientComplexRewardMaker = ThingSetMakerDefOf.MapGen_AncientComplexRoomLoot_Better
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(this.SitePartDef, parms)), tile, Faction.OfAncients, false, null, null);
			quest.SpawnWorldObject(site, null, null);
			TimedDetectionRaids component = site.GetComponent<TimedDetectionRaids>();
			if (component != null)
			{
				component.alertRaidsArrivingIn = true;
			}
			quest.End(QuestEndOutcome.Success, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			slate.Set<Map>("map", map, false);
			slate.Set<Site>("site", site, false);
		}

		// Token: 0x0600D9EB RID: 55787 RVA: 0x0050F103 File Offset: 0x0050D303
		public override LayoutStructureSketch QuestSetupComplex(Quest quest, float points)
		{
			return this.GenerateStructureSketch(points, false);
		}

		// Token: 0x0600D9EC RID: 55788 RVA: 0x0050F110 File Offset: 0x0050D310
		private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 10, false, QuestNode_Root_AncientComplex.AllowedLandmarks, 0.5f, true, TileFinderMode.Near, exitOnFirstTileFound, false, null, null);
		}

		// Token: 0x0600D9ED RID: 55789 RVA: 0x0050F137 File Offset: 0x0050D337
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			enemyFaction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			return enemyFaction != null;
		}

		// Token: 0x0600D9EE RID: 55790 RVA: 0x0050F150 File Offset: 0x0050D350
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			Faction faction;
			return this.TryFindSiteTile(out planetTile, true) && this.TryFindEnemyFaction(out faction);
		}

		// Token: 0x04008B57 RID: 35671
		private const int MinDistanceFromColony = 2;

		// Token: 0x04008B58 RID: 35672
		private const int MaxDistanceFromColony = 10;
	}
}
