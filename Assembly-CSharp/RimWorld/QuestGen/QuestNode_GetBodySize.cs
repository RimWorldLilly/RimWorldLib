using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B8 RID: 8632
	public class QuestNode_GetBodySize : QuestNode
	{
		// Token: 0x0600D706 RID: 55046 RVA: 0x004FCF0A File Offset: 0x004FB10A
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
			return true;
		}

		// Token: 0x0600D707 RID: 55047 RVA: 0x004FCF38 File Offset: 0x004FB138
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Set<float>(this.storeAs.GetValue(slate), this.pawnKind.GetValue(slate).RaceProps.baseBodySize, false);
		}

		// Token: 0x04008863 RID: 34915
		public SlateRef<PawnKindDef> pawnKind;

		// Token: 0x04008864 RID: 34916
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
