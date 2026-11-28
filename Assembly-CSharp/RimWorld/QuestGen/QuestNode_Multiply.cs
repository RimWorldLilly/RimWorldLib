using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200216B RID: 8555
	public class QuestNode_Multiply : QuestNode
	{
		// Token: 0x0600D612 RID: 54802 RVA: 0x004F8E3B File Offset: 0x004F703B
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600D613 RID: 54803 RVA: 0x004F8E54 File Offset: 0x004F7054
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) * this.value2.GetValue(slate), false);
		}

		// Token: 0x0400874C RID: 34636
		public SlateRef<double> value1;

		// Token: 0x0400874D RID: 34637
		public SlateRef<double> value2;

		// Token: 0x0400874E RID: 34638
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
