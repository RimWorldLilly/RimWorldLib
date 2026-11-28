using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000076 RID: 118
	public static class GenAdjFast
	{
		// Token: 0x06000494 RID: 1172 RVA: 0x0001B4B1 File Offset: 0x000196B1
		public static List<IntVec3> AdjacentCells8Way(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCells8Way((Thing)pack);
			}
			return GenAdjFast.AdjacentCells8Way((IntVec3)pack);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0001B4D4 File Offset: 0x000196D4
		public static List<IntVec3> AdjacentCells8Way(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 8; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.AdjacentCells[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001B535 File Offset: 0x00019735
		private static List<IntVec3> AdjacentCells8Way(Thing t)
		{
			return GenAdjFast.AdjacentCells8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001B554 File Offset: 0x00019754
		public static List<IntVec3> AdjacentCells8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCells8Way(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num - 1, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2);
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4);
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num);
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001B685 File Offset: 0x00019885
		public static List<IntVec3> AdjacentCellsCardinal(LocalTargetInfo pack)
		{
			if (pack.HasThing)
			{
				return GenAdjFast.AdjacentCellsCardinal((Thing)pack);
			}
			return GenAdjFast.AdjacentCellsCardinal((IntVec3)pack);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001B6A8 File Offset: 0x000198A8
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 4; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.CardinalDirections[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001B709 File Offset: 0x00019909
		private static List<IntVec3> AdjacentCellsCardinal(Thing t)
		{
			return GenAdjFast.AdjacentCellsCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001B728 File Offset: 0x00019928
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				return GenAdjFast.AdjacentCellsCardinal(thingCenter);
			}
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
			int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
			int num2 = num + thingSize.x + 1;
			int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
			int num4 = num3 + thingSize.z + 1;
			IntVec3 intVec = new IntVec3(num, 0, num3);
			do
			{
				intVec.x++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x < num2 - 1);
			intVec.x++;
			do
			{
				intVec.z++;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z < num4 - 1);
			intVec.z++;
			do
			{
				intVec.x--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.x > num + 1);
			intVec.x--;
			do
			{
				intVec.z--;
				GenAdjFast.resultList.Add(intVec);
			}
			while (intVec.z > num3 + 1);
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001B884 File Offset: 0x00019A84
		public static void AdjacentThings8Way(Thing thing, List<Thing> outThings)
		{
			outThings.Clear();
			if (!thing.Spawned)
			{
				return;
			}
			Map map = thing.Map;
			List<IntVec3> list = GenAdjFast.AdjacentCells8Way(thing);
			for (int i = 0; i < list.Count; i++)
			{
				List<Thing> thingList = list[i].GetThingList(map);
				for (int j = 0; j < thingList.Count; j++)
				{
					if (!outThings.Contains(thingList[j]))
					{
						outThings.Add(thingList[j]);
					}
				}
			}
		}

		// Token: 0x040001B7 RID: 439
		private static List<IntVec3> resultList = new List<IntVec3>();

		// Token: 0x040001B8 RID: 440
		private static bool working = false;
	}
}
