using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022BE RID: 8894
	public class QuestNode_GameCondition : QuestNode
	{
		// Token: 0x0600DB78 RID: 56184 RVA: 0x0051C294 File Offset: 0x0051A494
		private static Map GetMap(Slate slate)
		{
			Map randomPlayerHomeMap;
			if (!slate.TryGet<Map>("map", out randomPlayerHomeMap, false))
			{
				randomPlayerHomeMap = Find.RandomPlayerHomeMap;
			}
			return randomPlayerHomeMap;
		}

		// Token: 0x0600DB79 RID: 56185 RVA: 0x0051C2B8 File Offset: 0x0051A4B8
		protected override bool TestRunInt(Slate slate)
		{
			return this.targetWorld.GetValue(slate) || QuestNode_GameCondition.GetMap(slate) != null;
		}

		// Token: 0x0600DB7A RID: 56186 RVA: 0x0051C2D4 File Offset: 0x0051A4D4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			float points = QuestGen.slate.Get<float>("points", 0f, false);
			GameCondition gameCondition = GameConditionMaker.MakeCondition(this.gameCondition.GetValue(slate), this.duration.GetValue(slate));
			QuestPart_GameCondition questPart_GameCondition = new QuestPart_GameCondition();
			questPart_GameCondition.gameCondition = gameCondition;
			List<Rule> list = new List<Rule>();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (this.targetWorld.GetValue(slate))
			{
				questPart_GameCondition.targetWorld = true;
				gameCondition.RandomizeSettings(points, null, list, dictionary);
			}
			else
			{
				Map map = QuestNode_GameCondition.GetMap(QuestGen.slate);
				questPart_GameCondition.mapParent = map.Parent;
				gameCondition.RandomizeSettings(points, map, list, dictionary);
			}
			questPart_GameCondition.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			QuestGen.quest.AddPart(questPart_GameCondition);
			if (!this.storeGameConditionDescriptionFutureAs.GetValue(slate).NullOrEmpty())
			{
				slate.Set<string>(this.storeGameConditionDescriptionFutureAs.GetValue(slate), gameCondition.def.descriptionFuture, false);
			}
			QuestGen.AddQuestNameRules(list);
			QuestGen.AddQuestDescriptionRules(list);
			QuestGen.AddQuestDescriptionConstants(dictionary);
		}

		// Token: 0x04008C80 RID: 35968
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008C81 RID: 35969
		public SlateRef<GameConditionDef> gameCondition;

		// Token: 0x04008C82 RID: 35970
		public SlateRef<bool> targetWorld;

		// Token: 0x04008C83 RID: 35971
		public SlateRef<int> duration;

		// Token: 0x04008C84 RID: 35972
		[NoTranslate]
		public SlateRef<string> storeGameConditionDescriptionFutureAs;
	}
}
