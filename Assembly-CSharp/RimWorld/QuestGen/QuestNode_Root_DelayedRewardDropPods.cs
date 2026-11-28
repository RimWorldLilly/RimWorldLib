using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200226F RID: 8815
	public class QuestNode_Root_DelayedRewardDropPods : QuestNode
	{
		// Token: 0x0600D9BD RID: 55741 RVA: 0x0050C720 File Offset: 0x0050A920
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			List<Thing> rewards = slate.Get<List<Thing>>("rewards", null, false);
			Faction faction = slate.Get<Faction>("faction", null, false);
			Pawn pawn = slate.Get<Pawn>("giver", null, false);
			int delayTicks = slate.Get<int>("delayTicks", 0, false);
			string customerLetterLabel = slate.Get<string>("customLetterLabel", null, false);
			string customerLetterText = slate.Get<string>("customLetterText", null, false);
			if (pawn != null)
			{
				quest.ReservePawns(Gen.YieldSingle<Pawn>(pawn));
			}
			if (faction != null)
			{
				quest.ReserveFaction(faction);
			}
			quest.Delay(delayTicks, delegate
			{
				quest.DropPods(map.Parent, rewards, customerLetterLabel, null, customerLetterText, null, new bool?(true), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, false, false, true, null);
				quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, "RewardDelay", false, QuestPart.SignalListenMode.OngoingOnly, false);
		}

		// Token: 0x0600D9BE RID: 55742 RVA: 0x0050C80F File Offset: 0x0050AA0F
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<List<Thing>>("rewards", null, false) != null;
		}
	}
}
