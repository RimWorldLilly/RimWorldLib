using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001370 RID: 4976
	public static class PowerNetMaker
	{
		// Token: 0x06007B56 RID: 31574 RVA: 0x002BE83C File Offset: 0x002BCA3C
		private static IEnumerable<CompPower> ContiguousPowerBuildings(Building root)
		{
			PowerNetMaker.closedSet.Clear();
			PowerNetMaker.openSet.Clear();
			PowerNetMaker.currentSet.Clear();
			PowerNetMaker.openSet.Add(root);
			do
			{
				foreach (Building item in PowerNetMaker.openSet)
				{
					PowerNetMaker.closedSet.Add(item);
				}
				HashSet<Building> hashSet = PowerNetMaker.currentSet;
				PowerNetMaker.currentSet = PowerNetMaker.openSet;
				PowerNetMaker.openSet = hashSet;
				PowerNetMaker.openSet.Clear();
				foreach (Building building in PowerNetMaker.currentSet)
				{
					foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(building))
					{
						if (c.InBounds(building.Map))
						{
							List<Thing> thingList = c.GetThingList(building.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								Building building2 = thingList[i] as Building;
								if (building2 != null && building2.TransmitsPowerNow && !PowerNetMaker.openSet.Contains(building2) && !PowerNetMaker.currentSet.Contains(building2) && !PowerNetMaker.closedSet.Contains(building2))
								{
									PowerNetMaker.openSet.Add(building2);
									break;
								}
							}
						}
					}
				}
			}
			while (PowerNetMaker.openSet.Count > 0);
			IEnumerable<CompPower> result = (from b in PowerNetMaker.closedSet
			select b.PowerComp).ToArray<CompPower>();
			PowerNetMaker.closedSet.Clear();
			PowerNetMaker.openSet.Clear();
			PowerNetMaker.currentSet.Clear();
			return result;
		}

		// Token: 0x06007B57 RID: 31575 RVA: 0x002BEA40 File Offset: 0x002BCC40
		public static PowerNet NewPowerNetStartingFrom(Building root)
		{
			return new PowerNet(PowerNetMaker.ContiguousPowerBuildings(root));
		}

		// Token: 0x06007B58 RID: 31576 RVA: 0x002BEA4D File Offset: 0x002BCC4D
		public static void UpdateVisualLinkagesFor(PowerNet net)
		{
		}

		// Token: 0x04004DE1 RID: 19937
		private static HashSet<Building> closedSet = new HashSet<Building>();

		// Token: 0x04004DE2 RID: 19938
		private static HashSet<Building> openSet = new HashSet<Building>();

		// Token: 0x04004DE3 RID: 19939
		private static HashSet<Building> currentSet = new HashSet<Building>();
	}
}
