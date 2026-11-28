using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002239 RID: 8761
	public class QuestNode_SetItemStashContents : QuestNode
	{
		// Token: 0x0600D8DD RID: 55517 RVA: 0x00505891 File Offset: 0x00503A91
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8DE RID: 55518 RVA: 0x00505894 File Offset: 0x00503A94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<IEnumerable<ThingDef>>("itemStashThings", this.GetContents(slate), false);
		}

		// Token: 0x0600D8DF RID: 55519 RVA: 0x005058BA File Offset: 0x00503ABA
		private IEnumerable<ThingDef> GetContents(Slate slate)
		{
			QuestNode_SetItemStashContents.<GetContents>d__6 <GetContents>d__ = new QuestNode_SetItemStashContents.<GetContents>d__6(-2);
			<GetContents>d__.<>4__this = this;
			<GetContents>d__.<>3__slate = slate;
			return <GetContents>d__;
		}

		// Token: 0x04008A80 RID: 35456
		public SlateRef<IEnumerable<ThingDef>> items;

		// Token: 0x04008A81 RID: 35457
		public SlateRef<List<QuestNode_SetItemStashContents.ThingCategoryCount>> categories;

		// Token: 0x04008A82 RID: 35458
		private static List<ThingDef> tmpItems = new List<ThingDef>();

		// Token: 0x02003D4C RID: 15692
		public class ThingCategoryCount
		{
			// Token: 0x0400F1C3 RID: 61891
			public ThingCategoryDef category;

			// Token: 0x0400F1C4 RID: 61892
			public IntRange amount;

			// Token: 0x0400F1C5 RID: 61893
			public bool allowDuplicates = true;
		}
	}
}
