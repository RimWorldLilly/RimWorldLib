using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C6 RID: 8646
	public class QuestNode_GetMapWealth : QuestNode
	{
		// Token: 0x0600D742 RID: 55106 RVA: 0x004FDDF9 File Offset: 0x004FBFF9
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
			return true;
		}

		// Token: 0x0600D743 RID: 55107 RVA: 0x004FDE28 File Offset: 0x004FC028
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.map.GetValue(slate).wealthWatcher.WealthTotal, false);
		}

		// Token: 0x0400889A RID: 34970
		public SlateRef<Map> map;

		// Token: 0x0400889B RID: 34971
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
