using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002221 RID: 8737
	public class QuestNode_MakeMinified : QuestNode
	{
		// Token: 0x0600D893 RID: 55443 RVA: 0x0050477C File Offset: 0x0050297C
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D894 RID: 55444 RVA: 0x00504780 File Offset: 0x00502980
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MinifiedThing var = this.thing.GetValue(slate).MakeMinified(DestroyMode.Vanish);
			QuestGen.slate.Set<MinifiedThing>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04008A2F RID: 35375
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008A30 RID: 35376
		public SlateRef<Thing> thing;
	}
}
