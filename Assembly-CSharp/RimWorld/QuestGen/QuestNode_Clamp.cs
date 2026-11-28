using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002187 RID: 8583
	public class QuestNode_Clamp : QuestNode
	{
		// Token: 0x0600D664 RID: 54884 RVA: 0x004FA40B File Offset: 0x004F860B
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600D665 RID: 54885 RVA: 0x004FA415 File Offset: 0x004F8615
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D666 RID: 54886 RVA: 0x004FA424 File Offset: 0x004F8624
		private void DoWork(Slate slate)
		{
			double num = this.value.GetValue(slate);
			if (this.min.GetValue(slate) != null)
			{
				num = Math.Max(num, this.min.GetValue(slate).Value);
			}
			if (this.max.GetValue(slate) != null)
			{
				num = Math.Min(num, this.max.GetValue(slate).Value);
			}
			slate.Set<double>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x040087A8 RID: 34728
		public SlateRef<double> value;

		// Token: 0x040087A9 RID: 34729
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040087AA RID: 34730
		public SlateRef<double?> min;

		// Token: 0x040087AB RID: 34731
		public SlateRef<double?> max;
	}
}
