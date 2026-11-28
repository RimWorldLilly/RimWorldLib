using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021BB RID: 8635
	public class QuestNode_GetDefaultSitePartsParams : QuestNode
	{
		// Token: 0x0600D712 RID: 55058 RVA: 0x004FD113 File Offset: 0x004FB313
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D713 RID: 55059 RVA: 0x004FD11D File Offset: 0x004FB31D
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D714 RID: 55060 RVA: 0x004FD12C File Offset: 0x004FB32C
		private void SetVars(Slate slate)
		{
			List<SitePartDefWithParams> list;
			SiteMakerHelper.GenerateDefaultParams(slate.Get<float>("points", 0f, false), this.tile.GetValue(slate), this.faction.GetValue(slate), this.sitePartDefs.GetValue(slate), out list);
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def == SitePartDefOf.PreciousLump)
					{
						list[i].parms.preciousLumpResources = slate.Get<ThingDef>("targetMineable", null, false);
					}
				}
			}
			slate.Set<List<SitePartDefWithParams>>(this.storeSitePartsParamsAs.GetValue(slate), list, false);
		}

		// Token: 0x0400886B RID: 34923
		public SlateRef<PlanetTile> tile;

		// Token: 0x0400886C RID: 34924
		public SlateRef<Faction> faction;

		// Token: 0x0400886D RID: 34925
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x0400886E RID: 34926
		[NoTranslate]
		public SlateRef<string> storeSitePartsParamsAs;
	}
}
