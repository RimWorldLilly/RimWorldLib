using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021EF RID: 8687
	public class QuestNode_IsOfRoyalFaction : QuestNode
	{
		// Token: 0x0600D7EF RID: 55279 RVA: 0x00501EBC File Offset: 0x005000BC
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsOfRoyalFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7F0 RID: 55280 RVA: 0x00501EF4 File Offset: 0x005000F4
		protected override void RunInt()
		{
			if (this.IsOfRoyalFaction(QuestGen.slate))
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

		// Token: 0x0600D7F1 RID: 55281 RVA: 0x00501F2A File Offset: 0x0050012A
		private bool IsOfRoyalFaction(Slate slate)
		{
			return this.thing.GetValue(slate) != null && this.thing.GetValue(slate).Faction != null && this.thing.GetValue(slate).Faction.def.HasRoyalTitles;
		}

		// Token: 0x0400895D RID: 35165
		public SlateRef<Thing> thing;

		// Token: 0x0400895E RID: 35166
		public QuestNode node;

		// Token: 0x0400895F RID: 35167
		public QuestNode elseNode;
	}
}
