using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021EE RID: 8686
	public class QuestNode_IsOfFaction : QuestNode
	{
		// Token: 0x0600D7EB RID: 55275 RVA: 0x00501E15 File Offset: 0x00500015
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7EC RID: 55276 RVA: 0x00501E4D File Offset: 0x0050004D
		protected override void RunInt()
		{
			if (this.IsOfFaction(QuestGen.slate))
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

		// Token: 0x0600D7ED RID: 55277 RVA: 0x00501E83 File Offset: 0x00500083
		private bool IsOfFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction == this.faction.GetValue(slate);
		}

		// Token: 0x04008959 RID: 35161
		public SlateRef<Thing> thing;

		// Token: 0x0400895A RID: 35162
		public SlateRef<Faction> faction;

		// Token: 0x0400895B RID: 35163
		public QuestNode node;

		// Token: 0x0400895C RID: 35164
		public QuestNode elseNode;
	}
}
