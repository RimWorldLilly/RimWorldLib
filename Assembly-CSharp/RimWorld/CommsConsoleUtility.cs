using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA5 RID: 6821
	public static class CommsConsoleUtility
	{
		// Token: 0x0600AC4C RID: 44108 RVA: 0x003CCE3C File Offset: 0x003CB03C
		public static bool PlayerHasPoweredCommsConsole(Map map)
		{
			foreach (Building_CommsConsole building_CommsConsole in map.listerBuildings.AllBuildingsColonistOfClass<Building_CommsConsole>())
			{
				if (building_CommsConsole.Faction == Faction.OfPlayer)
				{
					CompPowerTrader compPowerTrader = building_CommsConsole.TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600AC4D RID: 44109 RVA: 0x003CCEB0 File Offset: 0x003CB0B0
		public static bool PlayerHasPoweredCommsConsole()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (CommsConsoleUtility.PlayerHasPoweredCommsConsole(maps[i]))
				{
					return true;
				}
			}
			return false;
		}
	}
}
