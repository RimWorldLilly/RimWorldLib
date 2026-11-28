using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002191 RID: 8593
	public class QuestNode_SetAndRestore : QuestNode
	{
		// Token: 0x0600D685 RID: 54917 RVA: 0x004FAD88 File Offset: 0x004F8F88
		protected override bool TestRunInt(Slate slate)
		{
			Slate.VarRestoreInfo restoreInfo = slate.GetRestoreInfo(this.name.GetValue(slate));
			slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			bool result;
			try
			{
				result = this.node.TestRun(slate);
			}
			finally
			{
				slate.Restore(restoreInfo);
			}
			return result;
		}

		// Token: 0x0600D686 RID: 54918 RVA: 0x004FADF0 File Offset: 0x004F8FF0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Slate.VarRestoreInfo restoreInfo = QuestGen.slate.GetRestoreInfo(this.name.GetValue(slate));
			QuestGen.slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			try
			{
				this.node.Run();
			}
			finally
			{
				QuestGen.slate.Restore(restoreInfo);
			}
		}

		// Token: 0x040087D3 RID: 34771
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x040087D4 RID: 34772
		public SlateRef<object> value;

		// Token: 0x040087D5 RID: 34773
		public QuestNode node;
	}
}
