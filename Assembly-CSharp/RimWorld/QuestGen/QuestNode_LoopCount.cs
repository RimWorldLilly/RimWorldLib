using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002220 RID: 8736
	public class QuestNode_LoopCount : QuestNode
	{
		// Token: 0x0600D890 RID: 55440 RVA: 0x00504680 File Offset: 0x00502880
		protected override bool TestRunInt(Slate slate)
		{
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					if (!this.node.TestRun(slate))
					{
						return false;
					}
				}
				finally
				{
					slate.PopPrefix();
				}
			}
			return true;
		}

		// Token: 0x0600D891 RID: 55441 RVA: 0x005046F8 File Offset: 0x005028F8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			for (int i = 0; i < this.loopCount.GetValue(slate); i++)
			{
				if (this.storeLoopCounterAs.GetValue(slate) != null)
				{
					QuestGen.slate.Set<int>(this.storeLoopCounterAs.GetValue(slate), i, false);
				}
				try
				{
					this.node.Run();
				}
				finally
				{
					QuestGen.slate.PopPrefix();
				}
			}
		}

		// Token: 0x04008A2C RID: 35372
		public QuestNode node;

		// Token: 0x04008A2D RID: 35373
		public SlateRef<int> loopCount;

		// Token: 0x04008A2E RID: 35374
		[NoTranslate]
		public SlateRef<string> storeLoopCounterAs;
	}
}
