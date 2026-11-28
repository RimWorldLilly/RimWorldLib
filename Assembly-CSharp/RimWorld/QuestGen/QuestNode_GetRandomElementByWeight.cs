using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D6 RID: 8662
	public class QuestNode_GetRandomElementByWeight : QuestNode
	{
		// Token: 0x0600D789 RID: 55177 RVA: 0x004FFEAF File Offset: 0x004FE0AF
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D78A RID: 55178 RVA: 0x004FFEB9 File Offset: 0x004FE0B9
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D78B RID: 55179 RVA: 0x004FFEC8 File Offset: 0x004FE0C8
		private void SetVars(Slate slate)
		{
			QuestNode_GetRandomElementByWeight.Option option;
			if (this.options.TryRandomElementByWeight((QuestNode_GetRandomElementByWeight.Option x) => x.weight, out option))
			{
				slate.Set<object>(this.storeAs.GetValue(slate), option.element.GetValue(slate), false);
			}
		}

		// Token: 0x040088EE RID: 35054
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088EF RID: 35055
		public List<QuestNode_GetRandomElementByWeight.Option> options = new List<QuestNode_GetRandomElementByWeight.Option>();

		// Token: 0x02003D2D RID: 15661
		public class Option
		{
			// Token: 0x0400F187 RID: 61831
			public SlateRef<object> element;

			// Token: 0x0400F188 RID: 61832
			public float weight;
		}
	}
}
