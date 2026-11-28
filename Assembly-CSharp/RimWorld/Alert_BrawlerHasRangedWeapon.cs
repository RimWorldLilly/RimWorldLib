using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BEF RID: 7151
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x1700210D RID: 8461
		// (get) Token: 0x0600B6E9 RID: 46825 RVA: 0x00407408 File Offset: 0x00405608
		private List<Pawn> BrawlersWithRangedWeapon
		{
			get
			{
				this.brawlersWithRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.story.traits.HasTrait(TraitDefOf.Brawler) && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
					{
						this.brawlersWithRangedWeaponResult.Add(pawn);
					}
				}
				return this.brawlersWithRangedWeaponResult;
			}
		}

		// Token: 0x0600B6EA RID: 46826 RVA: 0x004074AC File Offset: 0x004056AC
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x0600B6EB RID: 46827 RVA: 0x004074E9 File Offset: 0x004056E9
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}

		// Token: 0x040068F7 RID: 26871
		private List<Pawn> brawlersWithRangedWeaponResult = new List<Pawn>();
	}
}
