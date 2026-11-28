using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF0 RID: 7152
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		// Token: 0x1700210E RID: 8462
		// (get) Token: 0x0600B6EC RID: 46828 RVA: 0x004074F8 File Offset: 0x004056F8
		private List<Pawn> ShieldUsersWithRangedWeapon
		{
			get
			{
				this.shieldUsersWithRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					ThingWithComps primary = pawn.equipment.Primary;
					if (primary != null && primary.def.IsRangedWeapon)
					{
						bool flag = false;
						List<VerbProperties> verbs = primary.def.Verbs;
						for (int i = 0; i < verbs.Count; i++)
						{
							if (typeof(Verb_LaunchProjectile).IsAssignableFrom(verbs[i].verbClass))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							List<Apparel> wornApparel = pawn.apparel.WornApparel;
							for (int j = 0; j < wornApparel.Count; j++)
							{
								if (wornApparel[j].def.IsShieldThatBlocksRanged)
								{
									this.shieldUsersWithRangedWeaponResult.Add(pawn);
									break;
								}
							}
						}
					}
				}
				return this.shieldUsersWithRangedWeaponResult;
			}
		}

		// Token: 0x0600B6ED RID: 46829 RVA: 0x00407610 File Offset: 0x00405810
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		// Token: 0x0600B6EE RID: 46830 RVA: 0x0040764D File Offset: 0x0040584D
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}

		// Token: 0x040068F8 RID: 26872
		private List<Pawn> shieldUsersWithRangedWeaponResult = new List<Pawn>();
	}
}
