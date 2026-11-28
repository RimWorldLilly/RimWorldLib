using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200216C RID: 8556
	public class QuestNode_MultiplyRange : QuestNode
	{
		// Token: 0x0600D615 RID: 54805 RVA: 0x004F8E9B File Offset: 0x004F709B
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600D616 RID: 54806 RVA: 0x004F8EB4 File Offset: 0x004F70B4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<FloatRange>(this.storeAs.GetValue(slate), this.range.GetValue(slate) * this.value.GetValue(slate), false);
		}

		// Token: 0x0400874F RID: 34639
		public SlateRef<FloatRange> range;

		// Token: 0x04008750 RID: 34640
		public SlateRef<float> value;

		// Token: 0x04008751 RID: 34641
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
