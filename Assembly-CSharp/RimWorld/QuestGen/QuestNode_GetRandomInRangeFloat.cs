using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D8 RID: 8664
	public class QuestNode_GetRandomInRangeFloat : QuestNode
	{
		// Token: 0x0600D791 RID: 55185 RVA: 0x00500014 File Offset: 0x004FE214
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x0600D792 RID: 55186 RVA: 0x0050004C File Offset: 0x004FE24C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x040088F3 RID: 35059
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088F4 RID: 35060
		public SlateRef<FloatRange> range;
	}
}
