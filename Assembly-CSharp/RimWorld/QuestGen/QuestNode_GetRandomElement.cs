using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D5 RID: 8661
	public class QuestNode_GetRandomElement : QuestNode
	{
		// Token: 0x0600D785 RID: 55173 RVA: 0x004FFE57 File Offset: 0x004FE057
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D786 RID: 55174 RVA: 0x004FFE61 File Offset: 0x004FE061
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D787 RID: 55175 RVA: 0x004FFE70 File Offset: 0x004FE070
		private void SetVars(Slate slate)
		{
			SlateRef<object> slateRef;
			if (this.options.TryRandomElement(out slateRef))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), slateRef.GetValue(slate), false);
			}
		}

		// Token: 0x040088EC RID: 35052
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088ED RID: 35053
		public List<SlateRef<object>> options;
	}
}
