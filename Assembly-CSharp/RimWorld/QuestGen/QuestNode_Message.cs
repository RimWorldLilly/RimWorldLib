using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02002222 RID: 8738
	public class QuestNode_Message : QuestNode
	{
		// Token: 0x0600D896 RID: 55446 RVA: 0x005047C6 File Offset: 0x005029C6
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D897 RID: 55447 RVA: 0x005047CC File Offset: 0x005029CC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Message message = new QuestPart_Message();
			message.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? slate.Get<string>("inSignal", null, false));
			message.messageType = (this.messageType.GetValue(slate) ?? MessageTypeDefOf.NeutralEvent);
			message.lookTargets = QuestGenUtility.ToLookTargets(this.lookTargets, slate);
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				message.message = x;
			}, QuestGenUtility.MergeRules(this.rules.GetValue(slate), this.text.GetValue(slate), "root"));
			QuestGen.quest.AddPart(message);
		}

		// Token: 0x04008A31 RID: 35377
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A32 RID: 35378
		public SlateRef<MessageTypeDef> messageType;

		// Token: 0x04008A33 RID: 35379
		public SlateRef<string> text;

		// Token: 0x04008A34 RID: 35380
		public SlateRef<RulePack> rules;

		// Token: 0x04008A35 RID: 35381
		[NoTranslate]
		public SlateRef<IEnumerable<object>> lookTargets;

		// Token: 0x04008A36 RID: 35382
		private const string RootSymbol = "root";
	}
}
