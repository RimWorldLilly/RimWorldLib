using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C1 RID: 8641
	public class QuestNode_GetHediff : QuestNode
	{
		// Token: 0x0600D72C RID: 55084 RVA: 0x004FD86E File Offset: 0x004FBA6E
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D72D RID: 55085 RVA: 0x004FD878 File Offset: 0x004FBA78
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D72E RID: 55086 RVA: 0x004FD888 File Offset: 0x004FBA88
		private void SetVars(Slate slate)
		{
			QuestNode_GetHediff.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetHediff.Option x) => x.weight);
			slate.Set<HediffDef>(this.storeHediffAs.GetValue(slate), option.def, false);
			if (this.storePartsToAffectAs.GetValue(slate) != null)
			{
				slate.Set<List<BodyPartDef>>(this.storePartsToAffectAs.GetValue(slate), option.partsToAffect, false);
			}
		}

		// Token: 0x0400888A RID: 34954
		[NoTranslate]
		public SlateRef<string> storeHediffAs;

		// Token: 0x0400888B RID: 34955
		[NoTranslate]
		public SlateRef<string> storePartsToAffectAs;

		// Token: 0x0400888C RID: 34956
		public SlateRef<List<QuestNode_GetHediff.Option>> options;

		// Token: 0x02003D17 RID: 15639
		public class Option
		{
			// Token: 0x0400F14D RID: 61773
			public HediffDef def;

			// Token: 0x0400F14E RID: 61774
			public List<BodyPartDef> partsToAffect;

			// Token: 0x0400F14F RID: 61775
			public float weight = 1f;
		}
	}
}
