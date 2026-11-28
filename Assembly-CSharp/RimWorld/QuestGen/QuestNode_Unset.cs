using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002195 RID: 8597
	public class QuestNode_Unset : QuestNode
	{
		// Token: 0x0600D692 RID: 54930 RVA: 0x004FB344 File Offset: 0x004F9544
		protected override bool TestRunInt(Slate slate)
		{
			slate.Remove(this.name.GetValue(slate), false);
			return true;
		}

		// Token: 0x0600D693 RID: 54931 RVA: 0x004FB35C File Offset: 0x004F955C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Remove(this.name.GetValue(slate), false);
		}

		// Token: 0x040087E7 RID: 34791
		[NoTranslate]
		public SlateRef<string> name;
	}
}
