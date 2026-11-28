using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200223A RID: 8762
	public class QuestNode_SetRoyalTitle : QuestNode
	{
		// Token: 0x0600D8E2 RID: 55522 RVA: 0x005058E5 File Offset: 0x00503AE5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D8E3 RID: 55523 RVA: 0x005058E8 File Offset: 0x00503AE8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value.royalty != null)
			{
				value.royalty.SetTitle(this.faction.GetValue(slate), this.royalTitle.GetValue(slate), false, false, true);
			}
		}

		// Token: 0x04008A83 RID: 35459
		public SlateRef<Pawn> pawn;

		// Token: 0x04008A84 RID: 35460
		public SlateRef<RoyalTitleDef> royalTitle;

		// Token: 0x04008A85 RID: 35461
		public SlateRef<Faction> faction;
	}
}
