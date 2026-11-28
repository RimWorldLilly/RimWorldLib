using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002297 RID: 8855
	public class QuestNode_Root_SurveyScanner : QuestNode
	{
		// Token: 0x0600DAA8 RID: 55976 RVA: 0x005166E0 File Offset: 0x005148E0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			float num = QuestGen.slate.Get<float>("points", 0f, false);
			string text = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("scanner");
			string inSignalEnable = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string text2 = QuestGenUtility.HardcodedSignalWithQuestID(text + ".Destroyed");
			string completedSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.SurveyCompleted");
			string text3 = QuestGenUtility.HardcodedSignalWithQuestID("site.Raid");
			int value = this.duration.GetValue(slate);
			Site site = (Site)this.site.GetValue(slate);
			int fireRaidRemainingTicks = (int)(this.raidAttackRemainingHoursRange.GetValue(slate).RandomInRange * 2500f);
			Faction faction;
			if (Rand.Chance(this.raidChance.GetValue(slate)) && Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, true, false, TechLevel.Undefined, TechLevel.Undefined, false, true))
			{
				slate.Set<Faction>("enemyFaction", faction, false);
				Quest quest2 = quest;
				MapParent parent = site;
				float points = QuestNode_Root_SurveyScanner.RaidPointsRandomFactor.RandomInRange * num;
				Faction faction2 = faction;
				string inSignalLeave = null;
				string inSignal2 = text3;
				RaidStrategyDef immediateAttack = RaidStrategyDefOf.ImmediateAttack;
				quest2.Raid(parent, points, faction2, inSignalLeave, this.raidLetterLabel, this.raidLetterText, null, null, null, null, inSignal2, "root", null, immediateAttack, null, false, true, true, true);
			}
			quest.AddPart(new QuestPart_SurveyScanner(site, inSignalEnable, text, value));
			quest.AddPart(new QuestPart_ScannerDurationRemainingAlert(site, inSignalEnable, text2, completedSignal, text3, value, fireRaidRemainingTicks));
			quest.End(QuestEndOutcome.Fail, 0, null, text2, QuestPart.SignalListenMode.OngoingOnly, true, false);
			quest.End(QuestEndOutcome.Fail, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, true, false);
		}

		// Token: 0x0600DAA9 RID: 55977 RVA: 0x00516867 File Offset: 0x00514A67
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive;
		}

		// Token: 0x04008BD7 RID: 35799
		private SlateRef<WorldObject> site;

		// Token: 0x04008BD8 RID: 35800
		private SlateRef<int> duration;

		// Token: 0x04008BD9 RID: 35801
		private SlateRef<float> raidChance;

		// Token: 0x04008BDA RID: 35802
		private SlateRef<FloatRange> raidAttackRemainingHoursRange;

		// Token: 0x04008BDB RID: 35803
		private string raidLetterLabel;

		// Token: 0x04008BDC RID: 35804
		private string raidLetterText;

		// Token: 0x04008BDD RID: 35805
		private const string ReasonSymbol = "attackReason";

		// Token: 0x04008BDE RID: 35806
		private const string SiteAlias = "site";

		// Token: 0x04008BDF RID: 35807
		private const string SurveyCompleted = "SurveyCompleted";

		// Token: 0x04008BE0 RID: 35808
		private const string SiteRaid = "Raid";

		// Token: 0x04008BE1 RID: 35809
		private static readonly FloatRange RaidPointsRandomFactor = new FloatRange(0.9f, 1.1f);
	}
}
