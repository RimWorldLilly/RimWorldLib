using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002188 RID: 8584
	public class QuestNode_Delay : QuestNode
	{
		// Token: 0x0600D668 RID: 54888 RVA: 0x004FA4BC File Offset: 0x004F86BC
		protected override bool TestRunInt(Slate slate)
		{
			return this.node == null || this.node.TestRun(slate);
		}

		// Token: 0x0600D669 RID: 54889 RVA: 0x004FA4D4 File Offset: 0x004F86D4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_Delay questPart_Delay;
			if (this.useAcceptanceExpiry.GetValue(slate))
			{
				questPart_Delay = this.MakeDelayQuestPart();
				questPart_Delay.delayTicks = QuestGen.quest.TicksUntilExpiry;
			}
			else if (this.delayTicksRange.GetValue(slate) != null)
			{
				questPart_Delay = new QuestPart_DelayRandom();
				((QuestPart_DelayRandom)questPart_Delay).delayTicksRange = this.delayTicksRange.GetValue(slate).Value;
			}
			else
			{
				questPart_Delay = this.MakeDelayQuestPart();
				questPart_Delay.delayTicks = this.delayTicks.GetValue(slate);
			}
			questPart_Delay.inSignalEnable = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalEnable.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_Delay.inSignalDisable = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			questPart_Delay.reactivatable = this.reactivatable.GetValue(slate);
			questPart_Delay.waitUntilPlayerHasHomeMap = this.waitUntilPlayerHasHomeMap.GetValue(slate);
			if (!this.inspectStringTargets.GetValue(slate).EnumerableNullOrEmpty<ISelectable>())
			{
				questPart_Delay.inspectString = this.inspectString.GetValue(slate);
				questPart_Delay.inspectStringTargets = new List<ISelectable>();
				questPart_Delay.inspectStringTargets.AddRange(this.inspectStringTargets.GetValue(slate));
			}
			if (this.isQuestTimeout.GetValue(slate))
			{
				questPart_Delay.isBad = true;
				questPart_Delay.expiryInfoPart = "QuestExpiresIn".Translate();
				questPart_Delay.expiryInfoPartTip = "QuestExpiresOn".Translate();
			}
			else
			{
				questPart_Delay.expiryInfoPart = this.expiryInfoPart.GetValue(slate);
				questPart_Delay.expiryInfoPartTip = this.expiryInfoPartTip.GetValue(slate);
			}
			if (this.node != null)
			{
				QuestGenUtility.RunInnerNode(this.node, questPart_Delay);
			}
			if (!this.outSignalComplete.GetValue(slate).NullOrEmpty())
			{
				questPart_Delay.outSignalsCompleted.Add(QuestGenUtility.HardcodedSignalWithQuestID(this.outSignalComplete.GetValue(slate)));
			}
			QuestGen.quest.AddPart(questPart_Delay);
		}

		// Token: 0x0600D66A RID: 54890 RVA: 0x004FA6C5 File Offset: 0x004F88C5
		protected virtual QuestPart_Delay MakeDelayQuestPart()
		{
			return new QuestPart_Delay();
		}

		// Token: 0x040087AC RID: 34732
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040087AD RID: 34733
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040087AE RID: 34734
		[NoTranslate]
		public SlateRef<string> outSignalComplete;

		// Token: 0x040087AF RID: 34735
		public SlateRef<string> expiryInfoPart;

		// Token: 0x040087B0 RID: 34736
		public SlateRef<string> expiryInfoPartTip;

		// Token: 0x040087B1 RID: 34737
		public SlateRef<string> inspectString;

		// Token: 0x040087B2 RID: 34738
		public SlateRef<IEnumerable<ISelectable>> inspectStringTargets;

		// Token: 0x040087B3 RID: 34739
		public SlateRef<int> delayTicks;

		// Token: 0x040087B4 RID: 34740
		public SlateRef<IntRange?> delayTicksRange;

		// Token: 0x040087B5 RID: 34741
		public SlateRef<bool> isQuestTimeout;

		// Token: 0x040087B6 RID: 34742
		public SlateRef<bool> reactivatable;

		// Token: 0x040087B7 RID: 34743
		public SlateRef<bool> waitUntilPlayerHasHomeMap;

		// Token: 0x040087B8 RID: 34744
		public SlateRef<bool> useAcceptanceExpiry;

		// Token: 0x040087B9 RID: 34745
		public QuestNode node;
	}
}
