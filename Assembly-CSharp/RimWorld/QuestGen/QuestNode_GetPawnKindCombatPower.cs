using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002212 RID: 8722
	public class QuestNode_GetPawnKindCombatPower : QuestNode
	{
		// Token: 0x0600D85D RID: 55389 RVA: 0x0050373D File Offset: 0x0050193D
		protected override bool TestRunInt(Slate slate)
		{
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
			return true;
		}

		// Token: 0x0600D85E RID: 55390 RVA: 0x00503764 File Offset: 0x00501964
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Set<float>(this.storeAs.GetValue(slate), this.kindDef.GetValue(slate).combatPower, false);
		}

		// Token: 0x040089D4 RID: 35284
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x040089D5 RID: 35285
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
