using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002180 RID: 8576
	public class QuestNode_AddToList : QuestNode
	{
		// Token: 0x0600D64F RID: 54863 RVA: 0x004F9AA8 File Offset: 0x004F7CA8
		protected override bool TestRunInt(Slate slate)
		{
			QuestGenUtility.AddToOrMakeList(slate, this.name.GetValue(slate), this.value.GetValue(slate));
			return true;
		}

		// Token: 0x0600D650 RID: 54864 RVA: 0x004F9ACC File Offset: 0x004F7CCC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.name.GetValue(slate), this.value.GetValue(slate));
		}

		// Token: 0x0400878D RID: 34701
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400878E RID: 34702
		public SlateRef<object> value;
	}
}
