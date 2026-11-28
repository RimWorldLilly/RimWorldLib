using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021DD RID: 8669
	public class QuestNode_GetSameQuestsCount : QuestNode
	{
		// Token: 0x0600D7A6 RID: 55206 RVA: 0x00500684 File Offset: 0x004FE884
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D7A7 RID: 55207 RVA: 0x0050068E File Offset: 0x004FE88E
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D7A8 RID: 55208 RVA: 0x0050069C File Offset: 0x004FE89C
		private void SetVars(Slate slate)
		{
			int var = Find.QuestManager.QuestsListForReading.Count((Quest x) => x.root == QuestGen.Root);
			slate.Set<int>("sameQuestsCount", var, false);
		}

		// Token: 0x04008906 RID: 35078
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
