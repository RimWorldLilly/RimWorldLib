using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021EA RID: 8682
	public class QuestNode_HasRoyalTitleInCurrentFaction : QuestNode
	{
		// Token: 0x0600D7DB RID: 55259 RVA: 0x00501B23 File Offset: 0x004FFD23
		protected override bool TestRunInt(Slate slate)
		{
			if (this.HasRoyalTitleInCurrentFaction(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7DC RID: 55260 RVA: 0x00501B5B File Offset: 0x004FFD5B
		protected override void RunInt()
		{
			if (this.HasRoyalTitleInCurrentFaction(QuestGen.slate))
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

		// Token: 0x0600D7DD RID: 55261 RVA: 0x00501B94 File Offset: 0x004FFD94
		private bool HasRoyalTitleInCurrentFaction(Slate slate)
		{
			Pawn value = this.pawn.GetValue(slate);
			return value != null && value.Faction != null && value.royalty != null && value.royalty.HasAnyTitleIn(value.Faction);
		}

		// Token: 0x0400894C RID: 35148
		public SlateRef<Pawn> pawn;

		// Token: 0x0400894D RID: 35149
		public QuestNode node;

		// Token: 0x0400894E RID: 35150
		public QuestNode elseNode;
	}
}
