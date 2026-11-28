using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021FF RID: 8703
	public class QuestNode_AllowDecreesForLodger : QuestNode
	{
		// Token: 0x0600D823 RID: 55331 RVA: 0x005028B3 File Offset: 0x00500AB3
		protected override void RunInt()
		{
			QuestGen.quest.AddPart(new QuestPart_AllowDecreesForLodger
			{
				lodger = this.lodger.GetValue(QuestGen.slate)
			});
		}

		// Token: 0x0600D824 RID: 55332 RVA: 0x005028DA File Offset: 0x00500ADA
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008982 RID: 35202
		public SlateRef<Pawn> lodger;
	}
}
