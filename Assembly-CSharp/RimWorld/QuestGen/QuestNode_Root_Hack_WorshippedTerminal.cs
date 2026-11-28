using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B1 RID: 8881
	public class QuestNode_Root_Hack_WorshippedTerminal : QuestNode
	{
		// Token: 0x0600DB4F RID: 56143 RVA: 0x0051B484 File Offset: 0x00519684
		protected override void RunInt()
		{
			if (!ModLister.CheckIdeology("Worshipped terminal"))
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, true);
			QuestGenUtility.RunAdjustPointsForDistantFight();
			float num = slate.Get<float>("points", 0f, false);
			Precept_Relic precept_Relic = slate.Get<Precept_Relic>("relic", null, false);
			slate.Set<Faction>("playerFaction", Faction.OfPlayer, false);
			slate.Set<bool>("allowViolentQuests", Find.Storyteller.difficulty.allowViolentQuests, false);
			PlanetTile tile;
			this.TryFindSiteTile(out tile);
			List<FactionRelation> list = new List<FactionRelation>();
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction.def.PermanentlyHostileTo(FactionDefOf.TribeCivil))
				{
					list.Add(new FactionRelation
					{
						other = faction,
						kind = FactionRelationKind.Neutral
					});
				}
			}
			FactionDef tribeCivil = FactionDefOf.TribeCivil;
			bool? hidden = new bool?(true);
			FactionGeneratorParms factionGeneratorParms = new FactionGeneratorParms(tribeCivil, default(IdeoGenerationParms), hidden);
			factionGeneratorParms.ideoGenerationParms = new IdeoGenerationParms(factionGeneratorParms.factionDef, false, null, null, null, false, false, false, false, "", null, null, false, "", false);
			Faction tribalFaction = FactionGenerator.NewGeneratedFactionWithRelations(factionGeneratorParms, list);
			tribalFaction.temporary = true;
			tribalFaction.factionHostileOnHarmByPlayer = Find.Storyteller.difficulty.allowViolentQuests;
			tribalFaction.neverFlee = true;
			Find.FactionManager.Add(tribalFaction);
			quest.ReserveFaction(tribalFaction);
			if (precept_Relic == null)
			{
				precept_Relic = Faction.OfPlayer.ideos.PrimaryIdeo.GetAllPreceptsOfType<Precept_Relic>().RandomElement<Precept_Relic>();
				Log.Warning("Worshipped terminal quest requires relic from parent quest. None found so picking random player relic");
			}
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("playerFaction.BuiltBuilding");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("playerFaction.PlacedBlueprint");
			num = Mathf.Max(num, tribalFaction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Settlement_RangedOnly, null));
			SitePartParams parms = new SitePartParams
			{
				points = num,
				threatPoints = num,
				relic = precept_Relic
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.WorshippedTerminal, parms)), tile, tribalFaction, false, null, null);
			site.doorsAlwaysOpenForPlayerPawns = true;
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			int num2 = 25000;
			site.GetComponent<TimedMakeFactionHostile>().SetupTimer(num2, "WorshippedTerminalFactionBecameHostileTimed".Translate(tribalFaction.Named("FACTION")), null);
			Thing thing = site.parts[0].things.First((Thing t) => t.def == ThingDefOf.AncientTerminal_Worshipful);
			slate.Set<Thing>("terminal", thing, false);
			string terminalDestroyedSignal = QuestGenUtility.HardcodedSignalWithQuestID("terminal.Destroyed");
			string text = QuestGenUtility.HardcodedSignalWithQuestID("terminal.Hacked");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID("terminal.HackingStarted");
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string inSignalEnable = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("tribalFaction.FactionMemberArrested");
			CompHackable compHackable = thing.TryGetComp<CompHackable>();
			compHackable.hackingStartedSignal = text2;
			compHackable.defence = (float)QuestNode_Root_Hack_WorshippedTerminal.HackDefenceRange.RandomInRange;
			quest.Message("[terminalHackedMessage]", null, true, null, null, text);
			quest.SetFactionHidden(tribalFaction, false, null);
			if (Find.Storyteller.difficulty.allowViolentQuests)
			{
				quest.FactionRelationToPlayerChange(tribalFaction, FactionRelationKind.Hostile, false, text2);
				quest.StartRecurringRaids(site, new FloatRange?(new FloatRange(24f, 24f)), new int?(2500), text2);
				quest.BuiltNearSettlement(tribalFaction, site, delegate
				{
					quest.FactionRelationToPlayerChange(tribalFaction, FactionRelationKind.Hostile, true, null);
				}, null, inSignal, null, null, QuestPart.SignalListenMode.OngoingOnly);
				quest.BuiltNearSettlement(tribalFaction, site, delegate
				{
					quest.Message("WarningBuildingCausesHostility".Translate(tribalFaction.Named("FACTION")), MessageTypeDefOf.CautionInput, false, null, null, null);
				}, null, inSignal2, null, null, QuestPart.SignalListenMode.OngoingOnly);
				quest.FactionRelationToPlayerChange(tribalFaction, FactionRelationKind.Hostile, true, inSignal3);
			}
			Reward_RelicInfo reward_RelicInfo = new Reward_RelicInfo();
			reward_RelicInfo.relic = precept_Relic;
			reward_RelicInfo.quest = quest;
			QuestPart_Choice questPart_Choice = quest.RewardChoice(null, null);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(reward_RelicInfo);
			questPart_Choice.choices.Add(choice);
			quest.SignalPassActivable(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, terminalDestroyedSignal, QuestPart.SignalListenMode.OngoingOnly, true, false);
			}, null, null, null, null, text, false);
			quest.SignalPassActivable(delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			}, inSignalEnable, text3, null, null, text, false);
			quest.SignalPassAll(delegate
			{
				quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, true, false);
			}, new List<string>
			{
				text,
				text3
			}, null);
			slate.Set<Map>("map", map, false);
			slate.Set<Precept_Relic>("relic", precept_Relic, false);
			slate.Set<int>("timer", num2, false);
			slate.Set<Faction>("tribalFaction", tribalFaction, false);
		}

		// Token: 0x0600DB50 RID: 56144 RVA: 0x0051B9CC File Offset: 0x00519BCC
		private bool TryFindSiteTile(out PlanetTile tile)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 10, false, null, 0.5f, true, TileFinderMode.Near, false, false, null, null);
		}

		// Token: 0x0600DB51 RID: 56145 RVA: 0x0051B9F0 File Offset: 0x00519BF0
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			return this.TryFindSiteTile(out planetTile);
		}

		// Token: 0x04008C58 RID: 35928
		private const int MinDistanceFromColony = 2;

		// Token: 0x04008C59 RID: 35929
		private const int MaxDistanceFromColony = 10;

		// Token: 0x04008C5A RID: 35930
		private static IntRange HackDefenceRange = new IntRange(10, 100);

		// Token: 0x04008C5B RID: 35931
		private const int FactionBecomesHostileAfterHours = 10;

		// Token: 0x04008C5C RID: 35932
		private const float PointsMultiplierRaid = 0.2f;

		// Token: 0x04008C5D RID: 35933
		private const float MinPointsRaid = 45f;
	}
}
