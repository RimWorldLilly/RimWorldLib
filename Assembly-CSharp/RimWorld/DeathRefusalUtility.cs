using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E60 RID: 7776
	public static class DeathRefusalUtility
	{
		// Token: 0x0600CA18 RID: 51736 RVA: 0x004A57A0 File Offset: 0x004A39A0
		public static bool PlayerHasCorpseWithDeathRefusal()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
				for (int j = 0; j < list.Count; j++)
				{
					Corpse corpse = list[j] as Corpse;
					if (corpse != null && DeathRefusalUtility.HasPlayerControlledDeathRefusal(corpse.InnerPawn))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600CA19 RID: 51737 RVA: 0x004A580C File Offset: 0x004A3A0C
		public static bool HasPlayerControlledDeathRefusal(Pawn pawn)
		{
			Hediff_DeathRefusal firstHediff = pawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			return firstHediff != null && firstHediff.PlayerControlled;
		}
	}
}
