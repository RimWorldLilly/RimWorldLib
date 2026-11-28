using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F2 RID: 8690
	public class QuestNode_RequireRoyalFavorFromFaction : QuestNode
	{
		// Token: 0x0600D7FA RID: 55290 RVA: 0x005020B5 File Offset: 0x005002B5
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate) != null && this.faction.GetValue(slate).allowRoyalFavorRewards;
		}

		// Token: 0x0600D7FB RID: 55291 RVA: 0x005020D8 File Offset: 0x005002D8
		protected override void RunInt()
		{
		}

		// Token: 0x04008966 RID: 35174
		public SlateRef<Faction> faction;
	}
}
