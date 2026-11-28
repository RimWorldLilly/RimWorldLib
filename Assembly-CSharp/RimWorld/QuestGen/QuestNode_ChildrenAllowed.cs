using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021E8 RID: 8680
	public class QuestNode_ChildrenAllowed : QuestNode
	{
		// Token: 0x0600D7D3 RID: 55251 RVA: 0x005019DC File Offset: 0x004FFBDC
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate, (QuestNode n) => n.TestRun(slate));
		}

		// Token: 0x0600D7D4 RID: 55252 RVA: 0x00501A0E File Offset: 0x004FFC0E
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate, delegate(QuestNode n)
			{
				n.Run();
				return true;
			});
		}

		// Token: 0x0600D7D5 RID: 55253 RVA: 0x00501A3C File Offset: 0x004FFC3C
		private bool DoWork(Slate slate, Func<QuestNode, bool> func)
		{
			bool childrenAllowed = Find.Storyteller.difficulty.ChildrenAllowed;
			slate.Set<bool>("allowChildren", childrenAllowed, false);
			if (childrenAllowed)
			{
				if (this.node != null)
				{
					return func(this.node);
				}
			}
			else if (this.elseNode != null)
			{
				return func(this.elseNode);
			}
			return true;
		}

		// Token: 0x04008947 RID: 35143
		public QuestNode node;

		// Token: 0x04008948 RID: 35144
		public QuestNode elseNode;
	}
}
