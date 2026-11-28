using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021DE RID: 8670
	public class QuestNode_GetSiteDisturbanceFactor : QuestNode
	{
		// Token: 0x0600D7AA RID: 55210 RVA: 0x005006ED File Offset: 0x004FE8ED
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D7AB RID: 55211 RVA: 0x005006F7 File Offset: 0x004FE8F7
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D7AC RID: 55212 RVA: 0x00500704 File Offset: 0x004FE904
		private void SetVars(Slate slate)
		{
			float num = 1f;
			IEnumerable<SitePartDef> value = this.sitePartDefs.GetValue(slate);
			if (value != null)
			{
				foreach (SitePartDef sitePartDef in value)
				{
					num *= sitePartDef.activeThreatDisturbanceFactor;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x04008907 RID: 35079
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008908 RID: 35080
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;
	}
}
