using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002194 RID: 8596
	public class QuestNode_SubScript : QuestNode
	{
		// Token: 0x0600D68E RID: 54926 RVA: 0x004FB1B8 File Offset: 0x004F93B8
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
				result = this.def.GetValue(slate).root.TestRun(slate);
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

		// Token: 0x0600D68F RID: 54927 RVA: 0x004FB24C File Offset: 0x004F944C
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
				this.def.GetValue(slate).Run();
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

		// Token: 0x0600D690 RID: 54928 RVA: 0x004FB2F0 File Offset: 0x004F94F0
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " (";
			SlateRef<QuestScriptDef> slateRef = this.def;
			return str + str2 + slateRef.ToString() + ")";
		}

		// Token: 0x040087E2 RID: 34786
		[TranslationHandle(Priority = 100)]
		public SlateRef<QuestScriptDef> def;

		// Token: 0x040087E3 RID: 34787
		[NoTranslate]
		public SlateRef<string> prefix;

		// Token: 0x040087E4 RID: 34788
		public SlateRef<bool> allowNonPrefixedLookup;

		// Token: 0x040087E5 RID: 34789
		public List<PrefixCapturedVar> parms = new List<PrefixCapturedVar>();

		// Token: 0x040087E6 RID: 34790
		[NoTranslate]
		public List<SlateRef<string>> returnVarNames = new List<SlateRef<string>>();
	}
}
