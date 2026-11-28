using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021DA RID: 8666
	public class QuestNode_GetRandomInRangeInt : QuestNode
	{
		// Token: 0x0600D798 RID: 55192 RVA: 0x00500150 File Offset: 0x004FE350
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
			return true;
		}

		// Token: 0x0600D799 RID: 55193 RVA: 0x00500188 File Offset: 0x004FE388
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<int>(this.storeAs.GetValue(slate), this.range.GetValue(slate).RandomInRange, false);
		}

		// Token: 0x040088FA RID: 35066
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088FB RID: 35067
		public SlateRef<IntRange> range;
	}
}
