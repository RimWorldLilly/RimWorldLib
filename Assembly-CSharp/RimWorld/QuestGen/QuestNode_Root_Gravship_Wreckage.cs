using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200225C RID: 8796
	public class QuestNode_Root_Gravship_Wreckage : QuestNode_Root_Gravcore
	{
		// Token: 0x0600D961 RID: 55649 RVA: 0x00508BAC File Offset: 0x00506DAC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(out tile))
			{
				Log.Error("Could not find valid site tile for gravengine wreckage quest.");
				return;
			}
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.GravshipWreckage, new SitePartParams
				{
					points = slate.Get<float>("points", 0f, false),
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, null, false, null, WorldObjectDefOf.ClaimableSite);
			site.questTags = new List<string>
			{
				QuestGen.slate.CurrentPrefix
			};
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			Pawn pawn = this.FindAsker();
			slate.Set<Pawn>("asker", pawn, false);
			slate.Set<bool>("askerIsNull", pawn == null, false);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.GravEngine));
			quest.RewardChoice(null, null).choices.Add(choice);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.AddPart<QuestPart_EngineClaimed>();
		}

		// Token: 0x17002608 RID: 9736
		// (get) Token: 0x0600D962 RID: 55650 RVA: 0x00508CF8 File Offset: 0x00506EF8
		private bool ScenarioMechSignalDisabled
		{
			get
			{
				if (this.scenarioMechSignalDisabled == null)
				{
					this.scenarioMechSignalDisabled = new bool?(false);
					Scenario scenario = Find.Scenario;
					for (int i = 0; i < scenario.parts.Count; i++)
					{
						ScenPart_DisableQuest scenPart_DisableQuest = scenario.parts[i] as ScenPart_DisableQuest;
						if (scenPart_DisableQuest != null && scenPart_DisableQuest.questDef == QuestScriptDefOf.MechanoidSignal)
						{
							this.scenarioMechSignalDisabled = new bool?(true);
						}
					}
				}
				return this.scenarioMechSignalDisabled.Value;
			}
		}

		// Token: 0x0600D963 RID: 55651 RVA: 0x00508D74 File Offset: 0x00506F74
		protected override bool TestRunInt(Slate slate)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (!ResearchProjectDefOf.BasicGravtech.IsFinished && !this.ScenarioMechSignalDisabled)
			{
				return false;
			}
			if (GravshipUtility.PlayerHasGravEngine())
			{
				return false;
			}
			foreach (Quest quest in Find.QuestManager.questsInDisplayOrder)
			{
				if (quest.State == QuestState.Ongoing && quest.root == QuestScriptDefOf.GravshipWreckage)
				{
					return false;
				}
			}
			return base.TestRunInt(slate);
		}

		// Token: 0x0600D964 RID: 55652 RVA: 0x00508E10 File Offset: 0x00507010
		private Pawn FindAsker()
		{
			Faction faction;
			if ((from f in Find.FactionManager.AllFactionsVisible
			where f.def.humanlikeFaction && !f.IsPlayer && !f.HostileTo(Faction.OfPlayer) && f.def.techLevel > TechLevel.Neolithic && f.leader != null && !f.temporary && !f.Hidden
			select f).TryRandomElement(out faction))
			{
				return faction.leader;
			}
			return null;
		}

		// Token: 0x04008AF5 RID: 35573
		private bool? scenarioMechSignalDisabled;
	}
}
