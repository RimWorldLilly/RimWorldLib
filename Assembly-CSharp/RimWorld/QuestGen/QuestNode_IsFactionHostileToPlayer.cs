using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021EB RID: 8683
	public class QuestNode_IsFactionHostileToPlayer : QuestNode
	{
		// Token: 0x0600D7DF RID: 55263 RVA: 0x00501BDC File Offset: 0x004FFDDC
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsHostile(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7E0 RID: 55264 RVA: 0x00501C14 File Offset: 0x004FFE14
		protected override void RunInt()
		{
			if (this.IsHostile(QuestGen.slate))
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

		// Token: 0x0600D7E1 RID: 55265 RVA: 0x00501C4C File Offset: 0x004FFE4C
		private bool IsHostile(Slate slate)
		{
			Faction value = this.faction.GetValue(slate);
			if (value != null)
			{
				return value.HostileTo(Faction.OfPlayer);
			}
			Thing value2 = this.factionOf.GetValue(slate);
			return value2 != null && value2.Faction != null && value2.Faction.HostileTo(Faction.OfPlayer);
		}

		// Token: 0x0400894F RID: 35151
		public SlateRef<Faction> faction;

		// Token: 0x04008950 RID: 35152
		public SlateRef<Thing> factionOf;

		// Token: 0x04008951 RID: 35153
		public QuestNode node;

		// Token: 0x04008952 RID: 35154
		public QuestNode elseNode;
	}
}
