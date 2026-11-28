using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002243 RID: 8771
	public class QuestNode_TrackWhenExitMentalState : QuestNode
	{
		// Token: 0x0600D8FE RID: 55550 RVA: 0x0050622D File Offset: 0x0050442D
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Get<Map>("map", null, false) != null;
		}

		// Token: 0x0600D8FF RID: 55551 RVA: 0x00506244 File Offset: 0x00504444
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_TrackWhenExitMentalState questPart_TrackWhenExitMentalState = new QuestPart_TrackWhenExitMentalState();
			questPart_TrackWhenExitMentalState.mapParent = slate.Get<Map>("map", null, false).Parent;
			questPart_TrackWhenExitMentalState.tag = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			questPart_TrackWhenExitMentalState.mentalStateDef = this.mentalStateDef.GetValue(slate);
			questPart_TrackWhenExitMentalState.outSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.outSignal.GetValue(slate));
			questPart_TrackWhenExitMentalState.inSignals = new List<string>();
			foreach (string signal in this.inSignals.GetValue(slate))
			{
				questPart_TrackWhenExitMentalState.inSignals.Add(QuestGenUtility.HardcodedSignalWithQuestID(signal));
			}
			QuestGen.quest.AddPart(questPart_TrackWhenExitMentalState);
		}

		// Token: 0x04008AAC RID: 35500
		[NoTranslate]
		public SlateRef<string> tag;

		// Token: 0x04008AAD RID: 35501
		public SlateRef<MentalStateDef> mentalStateDef;

		// Token: 0x04008AAE RID: 35502
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignals;

		// Token: 0x04008AAF RID: 35503
		[NoTranslate]
		public SlateRef<string> outSignal;
	}
}
