using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002169 RID: 8553
	public class QuestNode_Add : QuestNode
	{
		// Token: 0x0600D60C RID: 54796 RVA: 0x004F8D7C File Offset: 0x004F6F7C
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600D60D RID: 54797 RVA: 0x004F8D94 File Offset: 0x004F6F94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) + this.value2.GetValue(slate), false);
		}

		// Token: 0x04008746 RID: 34630
		public SlateRef<double> value1;

		// Token: 0x04008747 RID: 34631
		public SlateRef<double> value2;

		// Token: 0x04008748 RID: 34632
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
