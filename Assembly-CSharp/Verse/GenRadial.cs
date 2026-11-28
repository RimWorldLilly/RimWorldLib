using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000080 RID: 128
	public static class GenRadial
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0001FAB3 File Offset: 0x0001DCB3
		public static float MaxRadialPatternRadius
		{
			get
			{
				float[] radialPatternRadii = GenRadial.RadialPatternRadii;
				return radialPatternRadii[radialPatternRadii.Length - 1];
			}
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001FAC0 File Offset: 0x0001DCC0
		static GenRadial()
		{
			GenRadial.SetupManualRadialPattern();
			GenRadial.SetupRadialPattern();
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001FB00 File Offset: 0x0001DD00
		private static void SetupManualRadialPattern()
		{
			GenRadial.ManualRadialPattern[0] = new IntVec3(0, 0, 0);
			GenRadial.ManualRadialPattern[1] = new IntVec3(0, 0, -1);
			GenRadial.ManualRadialPattern[2] = new IntVec3(1, 0, 0);
			GenRadial.ManualRadialPattern[3] = new IntVec3(0, 0, 1);
			GenRadial.ManualRadialPattern[4] = new IntVec3(-1, 0, 0);
			GenRadial.ManualRadialPattern[5] = new IntVec3(1, 0, -1);
			GenRadial.ManualRadialPattern[6] = new IntVec3(1, 0, 1);
			GenRadial.ManualRadialPattern[7] = new IntVec3(-1, 0, 1);
			GenRadial.ManualRadialPattern[8] = new IntVec3(-1, 0, -1);
			GenRadial.ManualRadialPattern[9] = new IntVec3(2, 0, 0);
			GenRadial.ManualRadialPattern[10] = new IntVec3(-2, 0, 0);
			GenRadial.ManualRadialPattern[11] = new IntVec3(0, 0, 2);
			GenRadial.ManualRadialPattern[12] = new IntVec3(0, 0, -2);
			GenRadial.ManualRadialPattern[13] = new IntVec3(2, 0, 1);
			GenRadial.ManualRadialPattern[14] = new IntVec3(2, 0, -1);
			GenRadial.ManualRadialPattern[15] = new IntVec3(-2, 0, 1);
			GenRadial.ManualRadialPattern[16] = new IntVec3(-2, 0, -1);
			GenRadial.ManualRadialPattern[17] = new IntVec3(-1, 0, 2);
			GenRadial.ManualRadialPattern[18] = new IntVec3(1, 0, 2);
			GenRadial.ManualRadialPattern[19] = new IntVec3(-1, 0, -2);
			GenRadial.ManualRadialPattern[20] = new IntVec3(1, 0, -2);
			GenRadial.ManualRadialPattern[21] = new IntVec3(2, 0, 2);
			GenRadial.ManualRadialPattern[22] = new IntVec3(-2, 0, -2);
			GenRadial.ManualRadialPattern[23] = new IntVec3(2, 0, -2);
			GenRadial.ManualRadialPattern[24] = new IntVec3(-2, 0, 2);
			GenRadial.ManualRadialPattern[25] = new IntVec3(3, 0, 0);
			GenRadial.ManualRadialPattern[26] = new IntVec3(0, 0, 3);
			GenRadial.ManualRadialPattern[27] = new IntVec3(-3, 0, 0);
			GenRadial.ManualRadialPattern[28] = new IntVec3(0, 0, -3);
			GenRadial.ManualRadialPattern[29] = new IntVec3(3, 0, 1);
			GenRadial.ManualRadialPattern[30] = new IntVec3(-3, 0, -1);
			GenRadial.ManualRadialPattern[31] = new IntVec3(1, 0, 3);
			GenRadial.ManualRadialPattern[32] = new IntVec3(-1, 0, -3);
			GenRadial.ManualRadialPattern[33] = new IntVec3(-3, 0, 1);
			GenRadial.ManualRadialPattern[34] = new IntVec3(3, 0, -1);
			GenRadial.ManualRadialPattern[35] = new IntVec3(-1, 0, 3);
			GenRadial.ManualRadialPattern[36] = new IntVec3(1, 0, -3);
			GenRadial.ManualRadialPattern[37] = new IntVec3(3, 0, 2);
			GenRadial.ManualRadialPattern[38] = new IntVec3(-3, 0, -2);
			GenRadial.ManualRadialPattern[39] = new IntVec3(2, 0, 3);
			GenRadial.ManualRadialPattern[40] = new IntVec3(-2, 0, -3);
			GenRadial.ManualRadialPattern[41] = new IntVec3(-3, 0, 2);
			GenRadial.ManualRadialPattern[42] = new IntVec3(3, 0, -2);
			GenRadial.ManualRadialPattern[43] = new IntVec3(-2, 0, 3);
			GenRadial.ManualRadialPattern[44] = new IntVec3(2, 0, -3);
			GenRadial.ManualRadialPattern[45] = new IntVec3(3, 0, 3);
			GenRadial.ManualRadialPattern[46] = new IntVec3(3, 0, -3);
			GenRadial.ManualRadialPattern[47] = new IntVec3(-3, 0, 3);
			GenRadial.ManualRadialPattern[48] = new IntVec3(-3, 0, -3);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001FEF4 File Offset: 0x0001E0F4
		private static void SetupRadialPattern()
		{
			List<IntVec3> list = new List<IntVec3>();
			for (int i = -80; i < 80; i++)
			{
				for (int j = -80; j < 80; j++)
				{
					list.Add(new IntVec3(i, 0, j));
				}
			}
			list.Sort(delegate(IntVec3 a, IntVec3 b)
			{
				float num = (float)a.LengthHorizontalSquared;
				float num2 = (float)b.LengthHorizontalSquared;
				if (num < num2)
				{
					return -1;
				}
				if (num == num2)
				{
					return 0;
				}
				return 1;
			});
			for (int k = 0; k < 20000; k++)
			{
				GenRadial.RadialPattern[k] = list[k];
				GenRadial.RadialPatternRadii[k] = list[k].LengthHorizontal;
			}
			GenRadial.BuildLengthSquaredIndex();
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001FF94 File Offset: 0x0001E194
		private static void BuildLengthSquaredIndex()
		{
			int num = 6400;
			GenRadial.LengthSquaredToIndexArray = new int[num + 1];
			for (int i = 0; i <= num; i++)
			{
				GenRadial.LengthSquaredToIndexArray[i] = -1;
			}
			for (int j = 0; j < 20000; j++)
			{
				int lengthHorizontalSquared = GenRadial.RadialPattern[j].LengthHorizontalSquared;
				if (GenRadial.LengthSquaredToIndexArray[lengthHorizontalSquared] == -1)
				{
					GenRadial.LengthSquaredToIndexArray[lengthHorizontalSquared] = j;
				}
			}
			int num2 = 0;
			for (int k = 0; k <= num; k++)
			{
				if (GenRadial.LengthSquaredToIndexArray[k] != -1)
				{
					num2 = GenRadial.LengthSquaredToIndexArray[k];
				}
				else
				{
					GenRadial.LengthSquaredToIndexArray[k] = num2;
				}
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0002002F File Offset: 0x0001E22F
		public static int NumCellsToFillForRadius_ManualRadialPattern(int radius)
		{
			if (radius == 0)
			{
				return 1;
			}
			if (radius == 1)
			{
				return 9;
			}
			if (radius == 2)
			{
				return 21;
			}
			if (radius == 3)
			{
				return 37;
			}
			Log.Error("NumSquares radius error");
			return 0;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00020058 File Offset: 0x0001E258
		public static int NumCellsInRadius(float radius)
		{
			if (radius >= GenRadial.MaxRadialPatternRadius)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Not enough squares to get to radius ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(radius);
				defaultInterpolatedStringHandler.AppendLiteral(". Max is ");
				defaultInterpolatedStringHandler.AppendFormatted<float>(GenRadial.MaxRadialPatternRadius);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return 20000;
			}
			float num = radius + float.Epsilon;
			int num2 = (int)Math.Floor((double)(num * num));
			int num3 = 6400;
			if (num2 > num3)
			{
				num2 = num3;
			}
			for (int i = GenRadial.LengthSquaredToIndexArray[num2]; i < 20000; i++)
			{
				if (GenRadial.RadialPatternRadii[i] > num)
				{
					return i;
				}
			}
			return 20000;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00020101 File Offset: 0x0001E301
		public static float RadiusOfNumCells(int numCells)
		{
			return GenRadial.RadialPatternRadii[numCells];
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0002010A File Offset: 0x0001E30A
		public static IEnumerable<IntVec3> RadialPatternInRadius(float radius)
		{
			GenRadial.<RadialPatternInRadius>d__15 <RadialPatternInRadius>d__ = new GenRadial.<RadialPatternInRadius>d__15(-2);
			<RadialPatternInRadius>d__.<>3__radius = radius;
			return <RadialPatternInRadius>d__;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0002011A File Offset: 0x0001E31A
		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float radius, bool useCenter)
		{
			GenRadial.<RadialCellsAround>d__16 <RadialCellsAround>d__ = new GenRadial.<RadialCellsAround>d__16(-2);
			<RadialCellsAround>d__.<>3__center = center;
			<RadialCellsAround>d__.<>3__radius = radius;
			<RadialCellsAround>d__.<>3__useCenter = useCenter;
			return <RadialCellsAround>d__;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00020138 File Offset: 0x0001E338
		public static IEnumerable<IntVec3> RadialCellsAround(IntVec3 center, float minRadius, float maxRadius)
		{
			GenRadial.<RadialCellsAround>d__17 <RadialCellsAround>d__ = new GenRadial.<RadialCellsAround>d__17(-2);
			<RadialCellsAround>d__.<>3__center = center;
			<RadialCellsAround>d__.<>3__minRadius = minRadius;
			<RadialCellsAround>d__.<>3__maxRadius = maxRadius;
			return <RadialCellsAround>d__;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00020156 File Offset: 0x0001E356
		public static IEnumerable<Thing> RadialDistinctThingsAround(IntVec3 center, Map map, float radius, bool useCenter)
		{
			GenRadial.<RadialDistinctThingsAround>d__18 <RadialDistinctThingsAround>d__ = new GenRadial.<RadialDistinctThingsAround>d__18(-2);
			<RadialDistinctThingsAround>d__.<>3__center = center;
			<RadialDistinctThingsAround>d__.<>3__map = map;
			<RadialDistinctThingsAround>d__.<>3__radius = radius;
			<RadialDistinctThingsAround>d__.<>3__useCenter = useCenter;
			return <RadialDistinctThingsAround>d__;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0002017C File Offset: 0x0001E37C
		public static void ProcessEquidistantCells(IntVec3 center, float radius, Func<List<IntVec3>, bool> processor, Map map = null)
		{
			if (GenRadial.working)
			{
				Log.Error("Nested calls to ProcessEquidistantCells() are not allowed.");
				return;
			}
			GenRadial.tmpCells.Clear();
			GenRadial.working = true;
			try
			{
				float num = -1f;
				int num2 = GenRadial.NumCellsInRadius(radius);
				for (int i = 0; i < num2; i++)
				{
					IntVec3 intVec = center + GenRadial.RadialPattern[i];
					if (map == null || intVec.InBounds(map))
					{
						float num3 = (float)intVec.DistanceToSquared(center);
						if (Mathf.Abs(num3 - num) > 0.0001f)
						{
							if (GenRadial.tmpCells.Any<IntVec3>() && processor(GenRadial.tmpCells))
							{
								return;
							}
							num = num3;
							GenRadial.tmpCells.Clear();
						}
						GenRadial.tmpCells.Add(intVec);
					}
				}
				if (GenRadial.tmpCells.Any<IntVec3>())
				{
					processor(GenRadial.tmpCells);
				}
			}
			finally
			{
				GenRadial.tmpCells.Clear();
				GenRadial.working = false;
			}
		}

		// Token: 0x040001CD RID: 461
		public static IntVec3[] ManualRadialPattern = new IntVec3[49];

		// Token: 0x040001CE RID: 462
		public static IntVec3[] RadialPattern = new IntVec3[20000];

		// Token: 0x040001CF RID: 463
		public static float[] RadialPatternRadii = new float[20000];

		// Token: 0x040001D0 RID: 464
		private static int[] LengthSquaredToIndexArray;

		// Token: 0x040001D1 RID: 465
		private const int RadialPatternCount = 20000;

		// Token: 0x040001D2 RID: 466
		private const int MAX_RADIUS = 80;

		// Token: 0x040001D3 RID: 467
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();

		// Token: 0x040001D4 RID: 468
		private static bool working;
	}
}
