using System;
using System.Reflection;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C0 RID: 8640
	public class QuestNode_GetFieldValue : QuestNode
	{
		// Token: 0x0600D728 RID: 55080 RVA: 0x004FD796 File Offset: 0x004FB996
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D729 RID: 55081 RVA: 0x004FD7A0 File Offset: 0x004FB9A0
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D72A RID: 55082 RVA: 0x004FD7B0 File Offset: 0x004FB9B0
		private void SetVars(Slate slate)
		{
			object obj = (this.type.GetValue(slate) != null) ? ConvertHelper.Convert(this.obj.GetValue(slate), this.type.GetValue(slate)) : this.obj.GetValue(slate);
			FieldInfo fieldInfo = obj.GetType().GetField(this.field.GetValue(slate), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fieldInfo == null)
			{
				Log.Error("QuestNode error: " + obj.GetType().Name + " doesn't have a field named " + this.field.GetValue(slate));
				return;
			}
			slate.Set<object>(this.storeAs.GetValue(slate), fieldInfo.GetValue(obj), false);
		}

		// Token: 0x04008886 RID: 34950
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008887 RID: 34951
		[NoTranslate]
		public SlateRef<object> obj;

		// Token: 0x04008888 RID: 34952
		[NoTranslate]
		public SlateRef<string> field;

		// Token: 0x04008889 RID: 34953
		public SlateRef<Type> type;
	}
}
