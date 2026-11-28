using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02002279 RID: 8825
	public class QuestNode_Root_Loot_AncientComplex_Mechanitor : QuestNode_Root_Loot_AncientComplex
	{
		// Token: 0x17002618 RID: 9752
		// (get) Token: 0x0600D9F0 RID: 55792 RVA: 0x0050F17A File Offset: 0x0050D37A
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientComplex_Mechanitor_Loot;
			}
		}

		// Token: 0x17002619 RID: 9753
		// (get) Token: 0x0600D9F1 RID: 55793 RVA: 0x0050F181 File Offset: 0x0050D381
		protected override SitePartDef SitePartDef
		{
			get
			{
				return SitePartDefOf.AncientComplex_Mechanitor;
			}
		}

		// Token: 0x0600D9F2 RID: 55794 RVA: 0x0050F188 File Offset: 0x0050D388
		protected override bool BeforeRunInt()
		{
			return ModLister.CheckBiotech("Ancient mechanitor complex");
		}

		// Token: 0x0600D9F3 RID: 55795 RVA: 0x0050F19C File Offset: 0x0050D39C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			bool flag;
			if (!slate.TryGet<bool>("discovered", out flag, false))
			{
				slate.Set<bool>("discovered", false, false);
			}
			base.RunInt();
		}
	}
}
