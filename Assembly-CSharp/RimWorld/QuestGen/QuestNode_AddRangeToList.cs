using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200217E RID: 8574
	public class QuestNode_AddRangeToList : QuestNode
	{
		// Token: 0x0600D649 RID: 54857 RVA: 0x004F9964 File Offset: 0x004F7B64
		protected override bool TestRunInt(Slate slate)
		{
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
			return true;
		}

		// Token: 0x0600D64A RID: 54858 RVA: 0x004F9998 File Offset: 0x004F7B98
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			List<object> list = this.value.GetValue(slate);
			if (list != null)
			{
				QuestGenUtility.AddRangeToOrMakeList(slate, this.name.GetValue(slate), list);
			}
		}

		// Token: 0x04008789 RID: 34697
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400878A RID: 34698
		public SlateRef<List<object>> value;
	}
}
