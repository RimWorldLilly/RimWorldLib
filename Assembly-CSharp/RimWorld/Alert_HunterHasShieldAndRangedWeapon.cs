using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF7 RID: 7159
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x17002116 RID: 8470
		// (get) Token: 0x0600B702 RID: 46850 RVA: 0x00407CB4 File Offset: 0x00405EB4
		private List<Pawn> BadHunters
		{
			get
			{
				this.badHuntersResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn))
					{
						this.badHuntersResult.Add(pawn);
					}
				}
				return this.badHuntersResult;
			}
		}

		// Token: 0x0600B703 RID: 46851 RVA: 0x00407D38 File Offset: 0x00405F38
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x0600B704 RID: 46852 RVA: 0x00407D75 File Offset: 0x00405F75
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}

		// Token: 0x040068FF RID: 26879
		private List<Pawn> badHuntersResult = new List<Pawn>();
	}
}
