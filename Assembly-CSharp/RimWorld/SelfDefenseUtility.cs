using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABC RID: 2748
	public static class SelfDefenseUtility
	{
		// Token: 0x0600529B RID: 21147 RVA: 0x001EC7AC File Offset: 0x001EA9AC
		public static bool ShouldStartFleeing(Pawn pawn)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AlwaysFlee);
			for (int i = 0; i < list.Count; i++)
			{
				if (FleeUtility.ShouldFleeFrom(list[i], pawn, true, false))
				{
					return true;
				}
			}
			bool foundThreat = false;
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return false;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region reg) => reg.door == null || reg.door.Open, delegate(Region reg)
			{
				List<Thing> list2 = reg.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int j = 0; j < list2.Count; j++)
				{
					if (FleeUtility.ShouldFleeFrom(list2[j], pawn, true, true))
					{
						foundThreat = true;
						break;
					}
				}
				return foundThreat;
			}, 9, RegionType.Set_Passable);
			return foundThreat;
		}
	}
}
