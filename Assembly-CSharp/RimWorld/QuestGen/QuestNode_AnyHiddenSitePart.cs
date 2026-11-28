using System;
using System.Collections.Generic;
using System.Linq;

namespace RimWorld.QuestGen
{
	// Token: 0x020021E7 RID: 8679
	public class QuestNode_AnyHiddenSitePart : QuestNode
	{
		// Token: 0x0600D7CF RID: 55247 RVA: 0x0050191F File Offset: 0x004FFB1F
		protected override bool TestRunInt(Slate slate)
		{
			if (this.AnyHiddenSitePart(slate))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600D7D0 RID: 55248 RVA: 0x00501957 File Offset: 0x004FFB57
		protected override void RunInt()
		{
			if (this.AnyHiddenSitePart(QuestGen.slate))
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

		// Token: 0x0600D7D1 RID: 55249 RVA: 0x00501990 File Offset: 0x004FFB90
		private bool AnyHiddenSitePart(Slate slate)
		{
			IEnumerable<SitePartDef> value = this.sitePartDefs.GetValue(slate);
			if (value != null)
			{
				return value.Any((SitePartDef x) => x.defaultHidden);
			}
			return false;
		}

		// Token: 0x04008944 RID: 35140
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x04008945 RID: 35141
		public QuestNode node;

		// Token: 0x04008946 RID: 35142
		public QuestNode elseNode;
	}
}
