using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200216D RID: 8557
	public class QuestNode_Subtract : QuestNode
	{
		// Token: 0x0600D618 RID: 54808 RVA: 0x004F8EFF File Offset: 0x004F70FF
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600D619 RID: 54809 RVA: 0x004F8F18 File Offset: 0x004F7118
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) - this.value2.GetValue(slate), false);
		}

		// Token: 0x04008752 RID: 34642
		public SlateRef<double> value1;

		// Token: 0x04008753 RID: 34643
		public SlateRef<double> value2;

		// Token: 0x04008754 RID: 34644
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
