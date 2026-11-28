using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200139B RID: 5019
	public class StrengthWatcher
	{
		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x06007C6F RID: 31855 RVA: 0x002C731C File Offset: 0x002C551C
		public float StrengthRating
		{
			get
			{
				float num = 0f;
				foreach (Pawn pawn in this.map.mapPawns.FreeColonists)
				{
					float num2 = 1f;
					num2 *= pawn.health.summaryHealth.SummaryHealthPercent;
					if (pawn.Downed)
					{
						num2 *= 0.3f;
					}
					num += num2;
				}
				foreach (Building building in this.map.listerBuildings.allBuildingsColonistCombatTargets)
				{
					if (building.def.building != null && building.def.building.IsTurret)
					{
						num += 0.3f;
					}
				}
				return num;
			}
		}

		// Token: 0x06007C70 RID: 31856 RVA: 0x002C7418 File Offset: 0x002C5618
		public StrengthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x04004E4F RID: 20047
		private Map map;
	}
}
