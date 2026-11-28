using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020021E9 RID: 8681
	public class QuestNode_FactionExists : QuestNode
	{
		// Token: 0x0600D7D7 RID: 55255 RVA: 0x00501A9C File Offset: 0x004FFC9C
		protected override bool TestRunInt(Slate slate)
		{
			if (this.FactionExists(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7D8 RID: 55256 RVA: 0x00501AD4 File Offset: 0x004FFCD4
		protected override void RunInt()
		{
			if (this.FactionExists(QuestGen.slate))
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		// Token: 0x0600D7D9 RID: 55257 RVA: 0x00501B0A File Offset: 0x004FFD0A
		private bool FactionExists(Slate slate)
		{
			return this.faction.GetValue(slate) != null;
		}

		// Token: 0x04008949 RID: 35145
		public SlateRef<Faction> faction;

		// Token: 0x0400894A RID: 35146
		public QuestNode node;

		// Token: 0x0400894B RID: 35147
		public QuestNode elseNode;
	}
}
