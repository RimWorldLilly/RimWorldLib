using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002267 RID: 8807
	public class QuestNode_Root_ArchonexusVictory_ThirdCycle : QuestNode_Root_ArchonexusVictory_Cycle
	{
		// Token: 0x17002614 RID: 9748
		// (get) Token: 0x0600D993 RID: 55699 RVA: 0x00509FDC File Offset: 0x005081DC
		protected override int ArchonexusCycle
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0600D994 RID: 55700 RVA: 0x00509FE0 File Offset: 0x005081E0
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			float num = slate.Get<float>("points", 0f, false);
			Faction faction = slate.Get<Faction>("roughTribe", null, false);
			PlanetTile tile;
			this.TryFindSiteTile(out tile, false);
			if (faction != null)
			{
				quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally, true);
			}
			base.TryAddStudyRequirement(quest, slate, ThingDefOf.GrandArchotechStructure);
			quest.DialogWithCloseBehavior("[questDescriptionBeforeAccepted]", null, quest.AddedSignal, null, null, QuestPart.SignalListenMode.NotYetAcceptedOnly, QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound3rd);
			quest.DescriptionPart("[questDescriptionBeforeAccepted]", quest.AddedSignal, quest.InitiateSignal, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, null);
			quest.DescriptionPart("[questDescriptionAfterAccepted]", quest.InitiateSignal, null, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, null);
			quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[questAcceptedLetterText]", null, "[questAcceptedLetterLabel]", null, null);
			float threatPoints = Find.Storyteller.difficulty.allowViolentQuests ? (num * QuestNode_Root_ArchonexusVictory_ThirdCycle.ThreatPointsFactor) : 0f;
			SitePartParams parms = new SitePartParams
			{
				threatPoints = threatPoints
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.Archonexus, parms)), tile, Faction.OfAncients, false, null, null);
			if (num <= 0f && Find.Storyteller.difficulty.allowViolentQuests)
			{
				quest.SetSitePartThreatPointsToCurrent(site, SitePartDefOf.Archonexus, this.map.Parent, null, QuestNode_Root_ArchonexusVictory_ThirdCycle.ThreatPointsFactor);
			}
			quest.SpawnWorldObject(site, null, null);
			slate.Set<bool>("factionless", faction == null, false);
			slate.Set<bool>("threatsEnabled", Find.Storyteller.difficulty.allowViolentQuests, false);
		}

		// Token: 0x0600D995 RID: 55701 RVA: 0x0050A16C File Offset: 0x0050836C
		private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 10, 40, false, null, 0f, true, TileFinderMode.Near, exitOnFirstTileFound, false, null, (PlanetTile p) => !ModsConfig.OdysseyActive || Find.World.landmarks[p] == null);
		}

		// Token: 0x0600D996 RID: 55702 RVA: 0x0050A1B0 File Offset: 0x005083B0
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			return base.TestRunInt(slate) && this.TryFindSiteTile(out planetTile, true);
		}

		// Token: 0x04008B07 RID: 35591
		private const int MinDistanceFromColony = 10;

		// Token: 0x04008B08 RID: 35592
		private const int MaxDistanceFromColony = 40;

		// Token: 0x04008B09 RID: 35593
		private static float ThreatPointsFactor = 0.6f;
	}
}
