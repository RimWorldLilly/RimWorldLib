using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F0 RID: 8688
	public class QuestNode_IsPermanentEnemy : QuestNode
	{
		// Token: 0x0600D7F3 RID: 55283 RVA: 0x00501F72 File Offset: 0x00500172
		protected override bool TestRunInt(Slate slate)
		{
			if (this.IsPermanentEnemy(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7F4 RID: 55284 RVA: 0x00501FAA File Offset: 0x005001AA
		protected override void RunInt()
		{
			if (this.IsPermanentEnemy(QuestGen.slate))
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

		// Token: 0x0600D7F5 RID: 55285 RVA: 0x00501FE0 File Offset: 0x005001E0
		private bool IsPermanentEnemy(Slate slate)
		{
			Thing value = this.thing.GetValue(slate);
			return value != null && value.Faction != null && value.Faction.def.permanentEnemy;
		}

		// Token: 0x04008960 RID: 35168
		public SlateRef<Thing> thing;

		// Token: 0x04008961 RID: 35169
		public QuestNode node;

		// Token: 0x04008962 RID: 35170
		public QuestNode elseNode;
	}
}
