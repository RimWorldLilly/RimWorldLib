using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002262 RID: 8802
	public class QuestNode_Root_AncientStructure : QuestNode
	{
		// Token: 0x0600D97E RID: 55678 RVA: 0x00509857 File Offset: 0x00507A57
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive;
		}

		// Token: 0x0600D97F RID: 55679 RVA: 0x00509864 File Offset: 0x00507A64
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			slate.Get<float>("points", 0f, false);
			PlanetTile tile = slate.Get<PlanetTile>("siteTile", default(PlanetTile), false);
			tile.Tile.AddMutator(this.structureMutator.GetValue(slate));
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(this.sitePartDef.GetValue(slate), new SitePartParams())
			}, tile, Faction.OfAncientsHostile, false, null, WorldObjectDefOf.ClaimableSite);
			quest.SpawnWorldObject(site, null, null);
			slate.Set<Site>("site", site, false);
			slate.Set<bool>("atLandmark", site.Tile.Tile.Landmark != null, false);
			Slate slate2 = slate;
			string name = "landmarkName";
			Landmark landmark = site.Tile.Tile.Landmark;
			slate2.Set<string>(name, ((landmark != null) ? landmark.name : null) ?? "", false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.AllEnemiesDefeated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x04008AFF RID: 35583
		public SlateRef<TileMutatorDef> structureMutator;

		// Token: 0x04008B00 RID: 35584
		public SlateRef<SitePartDef> sitePartDef;
	}
}
