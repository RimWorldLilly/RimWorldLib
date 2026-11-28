using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021EC RID: 8684
	public class QuestNode_IsFactionLeader : QuestNode
	{
		// Token: 0x0600D7E3 RID: 55267 RVA: 0x00501CA7 File Offset: 0x004FFEA7
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFactionLeader(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7E4 RID: 55268 RVA: 0x00501CDF File Offset: 0x004FFEDF
		protected override void RunInt()
		{
			if (this.IsFactionLeader(QuestGen.slate))
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

		// Token: 0x0600D7E5 RID: 55269 RVA: 0x00501D18 File Offset: 0x004FFF18
		private bool IsFactionLeader(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && this.pawn.GetValue(slate).Faction != null && this.pawn.GetValue(slate).Faction.leader == this.pawn.GetValue(slate);
		}

		// Token: 0x04008953 RID: 35155
		public SlateRef<Pawn> pawn;

		// Token: 0x04008954 RID: 35156
		public QuestNode node;

		// Token: 0x04008955 RID: 35157
		public QuestNode elseNode;
	}
}
