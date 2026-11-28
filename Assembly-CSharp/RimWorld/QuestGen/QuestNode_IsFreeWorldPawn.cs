using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021ED RID: 8685
	public class QuestNode_IsFreeWorldPawn : QuestNode
	{
		// Token: 0x0600D7E7 RID: 55271 RVA: 0x00501D74 File Offset: 0x004FFF74
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsFreeWorldPawn(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7E8 RID: 55272 RVA: 0x00501DAC File Offset: 0x004FFFAC
		protected override void RunInt()
		{
			if (this.IsFreeWorldPawn(QuestGen.slate))
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

		// Token: 0x0600D7E9 RID: 55273 RVA: 0x00501DE2 File Offset: 0x004FFFE2
		private bool IsFreeWorldPawn(Slate slate)
		{
			return this.pawn.GetValue(slate) != null && Find.WorldPawns.GetSituation(this.pawn.GetValue(slate)) == WorldPawnSituation.Free;
		}

		// Token: 0x04008956 RID: 35158
		public SlateRef<Pawn> pawn;

		// Token: 0x04008957 RID: 35159
		public QuestNode node;

		// Token: 0x04008958 RID: 35160
		public QuestNode elseNode;
	}
}
