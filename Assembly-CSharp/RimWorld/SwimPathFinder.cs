using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7C RID: 2940
	public static class SwimPathFinder
	{
		// Token: 0x06005568 RID: 21864 RVA: 0x001FC0E0 File Offset: 0x001FA2E0
		public static bool TryFindSwimPath(Pawn pawn, IntVec3 root, out List<IntVec3> result)
		{
			List<IntVec3> list = new List<IntVec3>();
			list.Add(root);
			IntVec3 intVec = root;
			for (int i = 0; i < 12; i++)
			{
				IntVec3 intVec2 = IntVec3.Invalid;
				float num = -1f;
				for (int j = SwimPathFinder.StartRadialIndex; j > SwimPathFinder.EndRadialIndex; j -= SwimPathFinder.RadialIndexStride)
				{
					IntVec3 intVec3 = intVec + GenRadial.RadialPattern[j];
					if (intVec3.InBounds(pawn.Map) && intVec3.Standable(pawn.Map) && !intVec3.IsForbidden(pawn) && intVec3.GetTerrain(pawn.Map).toxicBuildupFactor == 0f && intVec3.GetTerrain(pawn.Map).IsWater && GenSight.LineOfSight(intVec, intVec3, pawn.Map) && !PawnUtility.KnownDangerAt(intVec3, pawn.Map, pawn))
					{
						float num2 = 10000f;
						float num3 = (float)(intVec3 - root).LengthManhattan;
						if (num3 > 40f)
						{
							num2 *= Mathf.InverseLerp(70f, 40f, num3);
						}
						if (list.Count >= 2)
						{
							float num4 = (list[list.Count - 1] - list[list.Count - 2]).AngleFlat;
							float angleFlat = (intVec3 - intVec).AngleFlat;
							float num5;
							if (angleFlat > num4)
							{
								num5 = angleFlat - num4;
							}
							else
							{
								num4 -= 360f;
								num5 = angleFlat - num4;
							}
							if (num5 > 110f)
							{
								num2 *= 0.01f;
							}
						}
						if (list.Count >= 6 && (intVec - root).LengthManhattan < (intVec3 - root).LengthManhattan)
						{
							num2 *= 1.5f;
						}
						if (num2 > num)
						{
							intVec2 = intVec3;
							num = num2;
						}
					}
				}
				if (num < 0f)
				{
					result = null;
					return false;
				}
				list.Add(intVec2);
				intVec = intVec2;
			}
			list.Add(root);
			result = list;
			return true;
		}

		// Token: 0x06005569 RID: 21865 RVA: 0x001FC2F8 File Offset: 0x001FA4F8
		public static void DebugFlashSwimPath(IntVec3 root, int numEntries = 12)
		{
			Map currentMap = Find.CurrentMap;
			List<IntVec3> list;
			if (!SwimPathFinder.TryFindSwimPath(currentMap.mapPawns.FreeColonistsSpawned.First<Pawn>(), root, out list))
			{
				currentMap.debugDrawer.FlashCell(root, 0.2f, "NOPATH", 50);
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				currentMap.debugDrawer.FlashCell(list[i], (float)i / (float)numEntries, i.ToString(), 50);
				if (i > 0)
				{
					currentMap.debugDrawer.FlashLine(list[i], list[i - 1], 50, SimpleColor.White);
				}
			}
		}

		// Token: 0x040032D7 RID: 13015
		private const int NumPathNodes = 12;

		// Token: 0x040032D8 RID: 13016
		private const float StepDistMin = 1f;

		// Token: 0x040032D9 RID: 13017
		private const float StepDistMax = 3f;

		// Token: 0x040032DA RID: 13018
		private static readonly int StartRadialIndex = GenRadial.NumCellsInRadius(3f);

		// Token: 0x040032DB RID: 13019
		private static readonly int EndRadialIndex = GenRadial.NumCellsInRadius(1f);

		// Token: 0x040032DC RID: 13020
		private static readonly int RadialIndexStride = 1;
	}
}
