using System;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x0200228A RID: 8842
	public class QuestNode_Root_PollutionRaid : QuestNode
	{
		// Token: 0x0600DA59 RID: 55897 RVA: 0x005122C8 File Offset: 0x005104C8
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			return QuestGen_Get.GetMap(false, null, true) != null && slate.TryGet<Faction>("enemyFaction", out faction, false) && QuestNode_Root_PollutionRaid.IsValidFaction(faction);
		}

		// Token: 0x0600DA5A RID: 55898 RVA: 0x00512300 File Offset: 0x00510500
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = slate.Get<Map>("map", null, false);
			Faction faction = slate.Get<Faction>("enemyFaction", null, false);
			float num = slate.Get<float>("points", 0f, false) * 1.5f;
			string completeSignal = QuestGen.GenerateNewSignal("DelayCompleted", true);
			quest.Delay(QuestNode_Root_PollutionRaid.MaxDelayTicksRange.RandomInRange, delegate
			{
				quest.SignalPass(null, null, completeSignal);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, true).debugLabel = "Arrival delay";
			Quest quest2 = quest;
			MapParent parent = map.Parent;
			float points = num;
			Faction faction2 = faction;
			string inSignalLeave = null;
			string customLetterLabel = "[retaliationLetterLabel]";
			string customLetterText = "[retaliationLetterText]";
			RulePack customLetterLabelRules = null;
			RulePack customLetterTextRules = null;
			string completeSignal2 = completeSignal;
			quest2.Raid(parent, points, faction2, inSignalLeave, customLetterLabel, customLetterText, customLetterLabelRules, customLetterTextRules, null, null, completeSignal2, "root", null, null, null, false, true, true, true);
			quest.End(QuestEndOutcome.Unknown, 0, null, completeSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			slate.Set<Faction>("enemyFaction", faction, false);
		}

		// Token: 0x0600DA5B RID: 55899 RVA: 0x00512409 File Offset: 0x00510609
		private static bool IsValidFaction(Faction faction)
		{
			return !faction.IsPlayer && faction.HostileTo(Faction.OfPlayer);
		}

		// Token: 0x04008B88 RID: 35720
		private static readonly IntRange MaxDelayTicksRange = new IntRange(60000, 180000);

		// Token: 0x04008B89 RID: 35721
		private const float PointModifier = 1.5f;
	}
}
