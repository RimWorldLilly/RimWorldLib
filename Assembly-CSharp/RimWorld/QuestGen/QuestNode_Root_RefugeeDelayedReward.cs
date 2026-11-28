using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002275 RID: 8821
	public class QuestNode_Root_RefugeeDelayedReward : QuestNode
	{
		// Token: 0x0600D9DE RID: 55774 RVA: 0x0050DC38 File Offset: 0x0050BE38
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			Faction faction = slate.Get<Faction>("faction", null, false);
			FloatRange marketValueRange = slate.Get<FloatRange>("marketValueRange", default(FloatRange), false);
			Pawn val = slate.Get<Pawn>("rewardGiver", null, false);
			quest.ReservePawns(Gen.YieldSingle<Pawn>(val));
			quest.ReserveFaction(faction);
			int num = Rand.Range(5, 20) * 60000;
			slate.Set<int>("rewardDelayTicks", num, false);
			quest.Delay(num, delegate
			{
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.totalMarketValueRange = new FloatRange?(marketValueRange);
				parms.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
				parms.makingFaction = faction;
				List<Thing> list = ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms);
				slate.Set<string>("listOfRewards", GenLabel.ThingsLabel(list, "  - "), false);
				quest.DropPods(map.Parent, list, null, null, "[rewardLetterText]", null, new bool?(true), true, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, false, false, true, null);
				quest.End(QuestEndOutcome.Unknown, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, "RewardDelay", false, QuestPart.SignalListenMode.OngoingOnly, true);
		}

		// Token: 0x0600D9DF RID: 55775 RVA: 0x0050DD34 File Offset: 0x0050BF34
		protected override bool TestRunInt(Slate slate)
		{
			FloatRange floatRange;
			return slate.Get<Pawn>("rewardGiver", null, false) != null && slate.TryGet<FloatRange>("marketValueRange", out floatRange, false) && slate.Get<Faction>("faction", null, false) != null && QuestGen_Get.GetMap(false, null, false) != null;
		}
	}
}
