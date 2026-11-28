using System;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020022B9 RID: 8889
	public class QuestNode_ResolveTextNow : QuestNode
	{
		// Token: 0x0600DB6A RID: 56170 RVA: 0x0051BE3C File Offset: 0x0051A03C
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600DB6B RID: 56171 RVA: 0x0051BE40 File Offset: 0x0051A040
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string var = QuestGenUtility.ResolveLocalTextWithDescriptionRules(this.rules.GetValue(slate), this.root.GetValue(slate));
			slate.Set<string>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04008C6D RID: 35949
		[NoTranslate]
		public SlateRef<string> root;

		// Token: 0x04008C6E RID: 35950
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008C6F RID: 35951
		public SlateRef<RulePack> rules;
	}
}
