using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200226D RID: 8813
	public class QuestNode_SetupCreepjoiner : QuestNode
	{
		// Token: 0x0600D9B4 RID: 55732 RVA: 0x0050C2D0 File Offset: 0x0050A4D0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen_Get.GetMap(false, null, false);
			CreepJoinerFormKindDef value = this.form.GetValue(slate);
			CreepJoinerBenefitDef value2 = this.benefit.GetValue(slate);
			CreepJoinerDownsideDef value3 = this.downside.GetValue(slate);
			CreepJoinerAggressiveDef value4 = this.aggressive.GetValue(slate);
			CreepJoinerRejectionDef value5 = this.rejection.GetValue(slate);
			CreepJoinerUtility.GetCreepjoinerSpecifics(map, ref value, ref value2, ref value3, ref value4, ref value5);
			if (this.storeFormAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerFormKindDef>(this.storeFormAs.GetValue(slate), value, false);
			}
			if (this.storeBenefitAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerBenefitDef>(this.storeBenefitAs.GetValue(slate), value2, false);
			}
			if (this.storeDownsideAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerDownsideDef>(this.storeDownsideAs.GetValue(slate), value3, false);
			}
			if (this.storeAggressiveAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerAggressiveDef>(this.storeAggressiveAs.GetValue(slate), value4, false);
			}
			if (this.storeRejectionAs.GetValue(slate) != null)
			{
				slate.Set<CreepJoinerRejectionDef>(this.storeRejectionAs.GetValue(slate), value5, false);
			}
		}

		// Token: 0x0600D9B5 RID: 55733 RVA: 0x0050C3F2 File Offset: 0x0050A5F2
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008B1C RID: 35612
		public SlateRef<CreepJoinerFormKindDef> form;

		// Token: 0x04008B1D RID: 35613
		public SlateRef<CreepJoinerBenefitDef> benefit;

		// Token: 0x04008B1E RID: 35614
		public SlateRef<CreepJoinerDownsideDef> downside;

		// Token: 0x04008B1F RID: 35615
		public SlateRef<CreepJoinerAggressiveDef> aggressive;

		// Token: 0x04008B20 RID: 35616
		public SlateRef<CreepJoinerRejectionDef> rejection;

		// Token: 0x04008B21 RID: 35617
		[NoTranslate]
		public SlateRef<string> storeFormAs;

		// Token: 0x04008B22 RID: 35618
		[NoTranslate]
		public SlateRef<string> storeBenefitAs;

		// Token: 0x04008B23 RID: 35619
		[NoTranslate]
		public SlateRef<string> storeDownsideAs;

		// Token: 0x04008B24 RID: 35620
		[NoTranslate]
		public SlateRef<string> storeAggressiveAs;

		// Token: 0x04008B25 RID: 35621
		[NoTranslate]
		public SlateRef<string> storeRejectionAs;
	}
}
