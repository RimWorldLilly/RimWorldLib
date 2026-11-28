using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C9 RID: 8649
	public class QuestNode_GetMonumentSize : QuestNode
	{
		// Token: 0x0600D750 RID: 55120 RVA: 0x004FE704 File Offset: 0x004FC904
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600D751 RID: 55121 RVA: 0x004FE70E File Offset: 0x004FC90E
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D752 RID: 55122 RVA: 0x004FE71B File Offset: 0x004FC91B
		private void DoWork(Slate slate)
		{
			if (this.monumentMarker.GetValue(slate) == null)
			{
				return;
			}
			slate.Set<IntVec2>(this.storeAs.GetValue(slate), this.monumentMarker.GetValue(slate).Size, false);
		}

		// Token: 0x040088A1 RID: 34977
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088A2 RID: 34978
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
