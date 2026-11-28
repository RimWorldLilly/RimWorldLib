using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021E0 RID: 8672
	public class QuestNode_GetSiteThreatPoints : QuestNode
	{
		// Token: 0x0600D7B3 RID: 55219 RVA: 0x005009C8 File Offset: 0x004FEBC8
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600D7B4 RID: 55220 RVA: 0x005009CC File Offset: 0x004FEBCC
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.site.GetValue(slate) != null)
			{
				slate.Set<float>(this.storeAs.GetValue(slate), this.site.GetValue(slate).ActualThreatPoints, false);
				return;
			}
			float num = 0f;
			IEnumerable<SitePartDefWithParams> value = this.sitePartsParams.GetValue(slate);
			if (value != null)
			{
				foreach (SitePartDefWithParams sitePartDefWithParams in value)
				{
					num += sitePartDefWithParams.parms.threatPoints;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x0400890E RID: 35086
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400890F RID: 35087
		public SlateRef<Site> site;

		// Token: 0x04008910 RID: 35088
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;
	}
}
