using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200218C RID: 8588
	public class QuestNode_Prefix : QuestNode
	{
		// Token: 0x0600D674 RID: 54900 RVA: 0x004FA940 File Offset: 0x004F8B40
		protected override bool TestRunInt(Slate slate)
		{
			string value = this.prefix.GetValue(slate);
			List<Slate.VarRestoreInfo> varsRestoreInfo = QuestGenUtility.SetVarsForPrefix(this.parms, value, slate);
			if (!value.NullOrEmpty())
			{
				slate.PushPrefix(value, this.allowNonPrefixedLookup.GetValue(slate));
			}
			bool result;
			try
			{
				result = this.node.TestRun(slate);
			}
			finally
			{
				if (!value.NullOrEmpty())
				{
					slate.PopPrefix();
				}
				QuestGenUtility.GetReturnedVars(this.returnVarNames, value, slate);
				QuestGenUtility.RestoreVarsForPrefix(varsRestoreInfo, slate);
			}
			return result;
		}

		// Token: 0x0600D675 RID: 54901 RVA: 0x004FA9C8 File Offset: 0x004F8BC8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string value = this.prefix.GetValue(slate);
			List<Slate.VarRestoreInfo> varsRestoreInfo = QuestGenUtility.SetVarsForPrefix(this.parms, value, QuestGen.slate);
			if (!value.NullOrEmpty())
			{
				QuestGen.slate.PushPrefix(value, this.allowNonPrefixedLookup.GetValue(slate));
			}
			try
			{
				this.node.Run();
			}
			finally
			{
				if (!value.NullOrEmpty())
				{
					QuestGen.slate.PopPrefix();
				}
				QuestGenUtility.GetReturnedVars(this.returnVarNames, value, QuestGen.slate);
				QuestGenUtility.RestoreVarsForPrefix(varsRestoreInfo, QuestGen.slate);
			}
		}

		// Token: 0x040087C6 RID: 34758
		[NoTranslate]
		public SlateRef<string> prefix;

		// Token: 0x040087C7 RID: 34759
		public SlateRef<bool> allowNonPrefixedLookup;

		// Token: 0x040087C8 RID: 34760
		public List<PrefixCapturedVar> parms = new List<PrefixCapturedVar>();

		// Token: 0x040087C9 RID: 34761
		[NoTranslate]
		public List<SlateRef<string>> returnVarNames = new List<SlateRef<string>>();

		// Token: 0x040087CA RID: 34762
		public QuestNode node;
	}
}
