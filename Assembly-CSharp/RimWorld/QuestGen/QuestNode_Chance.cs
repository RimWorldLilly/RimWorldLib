using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002186 RID: 8582
	public class QuestNode_Chance : QuestNode
	{
		// Token: 0x0600D661 RID: 54881 RVA: 0x004FA2CC File Offset: 0x004F84CC
		protected override bool TestRunInt(Slate slate)
		{
			if (this.node == null || this.elseNode == null)
			{
				return true;
			}
			if (this.node.TestRun(slate.DeepCopy()))
			{
				this.node.TestRun(slate);
				return true;
			}
			if (this.elseNode.TestRun(slate.DeepCopy()))
			{
				this.elseNode.TestRun(slate);
				return true;
			}
			return false;
		}

		// Token: 0x0600D662 RID: 54882 RVA: 0x004FA330 File Offset: 0x004F8530
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (Rand.Chance(this.chance.GetValue(slate)))
			{
				if (this.node == null)
				{
					return;
				}
				if (this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
					return;
				}
				if (this.elseNode != null && this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
			}
			else
			{
				if (this.elseNode == null)
				{
					return;
				}
				if (this.elseNode.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.elseNode.Run();
					return;
				}
				if (this.node != null && this.node.TestRun(QuestGen.slate.DeepCopy()))
				{
					this.node.Run();
				}
			}
		}

		// Token: 0x040087A5 RID: 34725
		public SlateRef<float> chance;

		// Token: 0x040087A6 RID: 34726
		public QuestNode node;

		// Token: 0x040087A7 RID: 34727
		public QuestNode elseNode;
	}
}
