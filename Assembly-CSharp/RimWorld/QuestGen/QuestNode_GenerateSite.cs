using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B1 RID: 8625
	public class QuestNode_GenerateSite : QuestNode
	{
		// Token: 0x0600D6ED RID: 55021 RVA: 0x004FC914 File Offset: 0x004FAB14
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests && this.sitePartsParams.GetValue(slate) != null)
			{
				using (IEnumerator<SitePartDefWithParams> enumerator = this.sitePartsParams.GetValue(slate).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.wantsThreatPoints)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600D6EE RID: 55022 RVA: 0x004FC994 File Offset: 0x004FAB94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Site var = QuestGen_Sites.GenerateSite(this.sitePartsParams.GetValue(slate), this.tile.GetValue(slate), this.faction.GetValue(slate), this.hiddenSitePartsPossible.GetValue(slate), this.singleSitePartRules.GetValue(slate), this.worldObjectDef.GetValue(slate));
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Site>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x04008844 RID: 34884
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;

		// Token: 0x04008845 RID: 34885
		public SlateRef<Faction> faction;

		// Token: 0x04008846 RID: 34886
		public SlateRef<PlanetTile> tile;

		// Token: 0x04008847 RID: 34887
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008848 RID: 34888
		public SlateRef<RulePack> singleSitePartRules;

		// Token: 0x04008849 RID: 34889
		public SlateRef<bool> hiddenSitePartsPossible;

		// Token: 0x0400884A RID: 34890
		public SlateRef<WorldObjectDef> worldObjectDef;

		// Token: 0x0400884B RID: 34891
		private const string RootSymbol = "root";
	}
}
