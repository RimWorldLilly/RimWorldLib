using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001649 RID: 5705
	public class PreceptWorker_Apparel : PreceptWorker
	{
		// Token: 0x06008F2E RID: 36654 RVA: 0x0034A250 File Offset: 0x00348450
		public override bool ShouldSkipThing(Ideo ideo, ThingDef thingDef)
		{
			foreach (Precept precept in ideo.PreceptsListForReading)
			{
				Precept_Apparel precept_Apparel = precept as Precept_Apparel;
				if (precept_Apparel != null && precept_Apparel.apparelDef == thingDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008F2F RID: 36655 RVA: 0x0034A2B4 File Offset: 0x003484B4
		public override float GetThingOrder(PreceptThingChance thingChance)
		{
			if (thingChance.def.IsApparel)
			{
				return (float)((thingChance.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || thingChance.def.apparel.LastLayer == ApparelLayerDefOf.EyeCover) ? 1 : 2);
			}
			return 0f;
		}

		// Token: 0x06008F30 RID: 36656 RVA: 0x0034A308 File Offset: 0x00348508
		private bool IsValidApparel(ThingDef td)
		{
			if (!td.IsApparel || !td.MadeFromStuff)
			{
				return false;
			}
			if (!td.apparel.canBeDesiredForIdeo)
			{
				return false;
			}
			if (td.thingCategories != null && (td.thingCategories.Contains(ThingCategoryDefOf.ArmorHeadgear) || td.thingCategories.Contains(ThingCategoryDefOf.ApparelArmor)))
			{
				return false;
			}
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				PreceptComp_Apparel preceptComp_Apparel = this.def.comps[i] as PreceptComp_Apparel;
				if (preceptComp_Apparel != null && !preceptComp_Apparel.CanApplyToApparel(td))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008F31 RID: 36657 RVA: 0x0034A3A8 File Offset: 0x003485A8
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			PreceptWorker_Apparel.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.def = def;
			if (this.IsValidApparel(CS$<>8__locals1.def))
			{
				if (generatingFor != null)
				{
					AcceptanceReport result = PreceptWorker_Apparel.<CanUse>g__CheckFaction|3_0(generatingFor, ref CS$<>8__locals1);
					if (!result.Accepted)
					{
						return result;
					}
				}
				if (Find.World != null && Find.FactionManager != null)
				{
					foreach (Faction faction in Find.FactionManager.AllFactions)
					{
						if (faction.def != generatingFor && faction.ideos != null && faction.ideos.PrimaryIdeo == ideo)
						{
							AcceptanceReport acceptanceReport = PreceptWorker_Apparel.<CanUse>g__CheckFaction|3_0(faction.def, ref CS$<>8__locals1);
							if (!acceptanceReport)
							{
								return acceptanceReport;
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x170018BC RID: 6332
		// (get) Token: 0x06008F32 RID: 36658 RVA: 0x0034A480 File Offset: 0x00348680
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				PreceptWorker_Apparel.<get_ThingDefs>d__5 <get_ThingDefs>d__ = new PreceptWorker_Apparel.<get_ThingDefs>d__5(-2);
				<get_ThingDefs>d__.<>4__this = this;
				return <get_ThingDefs>d__;
			}
		}

		// Token: 0x06008F34 RID: 36660 RVA: 0x0034A498 File Offset: 0x00348698
		[CompilerGenerated]
		internal static AcceptanceReport <CanUse>g__CheckFaction|3_0(FactionDef faction, ref PreceptWorker_Apparel.<>c__DisplayClass3_0 A_1)
		{
			if (A_1.def.apparel.ideoDesireAllowedFactionCategoryTags != null && !A_1.def.apparel.ideoDesireAllowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle((from t in A_1.def.apparel.ideoDesireAllowedFactionCategoryTags
				select ("RoleApparelRequirementIncompatibleFaction_Allowed_" + t).Translate().Resolve()).ToCommaListOr(false), false, false)));
			}
			if (A_1.def.apparel.ideoDesireDisallowedFactionCategoryTags != null && A_1.def.apparel.ideoDesireDisallowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle((from t in A_1.def.apparel.ideoDesireDisallowedFactionCategoryTags
				select ("RoleApparelRequirementIncompatibleFaction_Disallowed_" + t).Translate().Resolve()).ToCommaListOr(false), false, false)));
			}
			return true;
		}
	}
}
