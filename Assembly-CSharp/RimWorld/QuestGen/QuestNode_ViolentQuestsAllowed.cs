using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021F3 RID: 8691
	public class QuestNode_ViolentQuestsAllowed : QuestNode
	{
		// Token: 0x0600D7FD RID: 55293 RVA: 0x005020E4 File Offset: 0x005002E4
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate, (QuestNode n) => n.TestRun(slate));
		}

		// Token: 0x0600D7FE RID: 55294 RVA: 0x00502116 File Offset: 0x00500316
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate, delegate(QuestNode n)
			{
				n.Run();
				return true;
			});
		}

		// Token: 0x0600D7FF RID: 55295 RVA: 0x00502144 File Offset: 0x00500344
		private bool DoWork(Slate slate, Func<QuestNode, bool> func)
		{
			bool allowViolentQuests = Find.Storyteller.difficulty.allowViolentQuests;
			slate.Set<bool>("allowViolentQuests", allowViolentQuests, false);
			if (allowViolentQuests)
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

		// Token: 0x04008967 RID: 35175
		public QuestNode node;

		// Token: 0x04008968 RID: 35176
		public QuestNode elseNode;
	}
}
