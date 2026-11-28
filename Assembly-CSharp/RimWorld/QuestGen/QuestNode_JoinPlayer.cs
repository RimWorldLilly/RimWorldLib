using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200221B RID: 8731
	public class QuestNode_JoinPlayer : QuestNode
	{
		// Token: 0x0600D881 RID: 55425 RVA: 0x005040FD File Offset: 0x005022FD
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D882 RID: 55426 RVA: 0x00504100 File Offset: 0x00502300
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) == null)
			{
				return;
			}
			QuestPart_JoinPlayer questPart_JoinPlayer = new QuestPart_JoinPlayer();
			questPart_JoinPlayer.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_JoinPlayer.joinPlayer = this.joinPlayer.GetValue(slate);
			questPart_JoinPlayer.makePrisoners = this.makePrisoners.GetValue(slate);
			questPart_JoinPlayer.mapParent = QuestGen.slate.Get<Map>("map", null, false).Parent;
			questPart_JoinPlayer.pawns.AddRange(this.pawns.GetValue(slate));
			QuestGen.quest.AddPart(questPart_JoinPlayer);
		}

		// Token: 0x04008A0A RID: 35338
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x04008A0B RID: 35339
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x04008A0C RID: 35340
		public SlateRef<bool> joinPlayer;

		// Token: 0x04008A0D RID: 35341
		public SlateRef<bool> makePrisoners;
	}
}
