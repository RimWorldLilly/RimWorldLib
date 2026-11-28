using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200216A RID: 8554
	public class QuestNode_Divide : QuestNode
	{
		// Token: 0x0600D60F RID: 54799 RVA: 0x004F8DDB File Offset: 0x004F6FDB
		protected override bool TestRunInt(Slate slate)
		{
			return !this.storeAs.GetValue(slate).NullOrEmpty();
		}

		// Token: 0x0600D610 RID: 54800 RVA: 0x004F8DF4 File Offset: 0x004F6FF4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<double>(this.storeAs.GetValue(slate), this.value1.GetValue(slate) / this.value2.GetValue(slate), false);
		}

		// Token: 0x04008749 RID: 34633
		public SlateRef<double> value1;

		// Token: 0x0400874A RID: 34634
		public SlateRef<double> value2;

		// Token: 0x0400874B RID: 34635
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
