using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002190 RID: 8592
	public class QuestNode_Set : QuestNode
	{
		// Token: 0x0600D681 RID: 54913 RVA: 0x004FAD11 File Offset: 0x004F8F11
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D682 RID: 54914 RVA: 0x004FAD1B File Offset: 0x004F8F1B
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D683 RID: 54915 RVA: 0x004FAD28 File Offset: 0x004F8F28
		private void SetVars(Slate slate)
		{
			object obj = this.value.GetValue(slate);
			if (this.convertTo.GetValue(slate) != null)
			{
				obj = ConvertHelper.Convert(obj, this.convertTo.GetValue(slate));
			}
			slate.Set<object>(this.name.GetValue(slate), obj, false);
		}

		// Token: 0x040087D0 RID: 34768
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x040087D1 RID: 34769
		public SlateRef<object> value;

		// Token: 0x040087D2 RID: 34770
		public SlateRef<Type> convertTo;
	}
}
