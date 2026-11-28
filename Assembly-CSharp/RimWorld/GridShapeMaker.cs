using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001301 RID: 4865
	public static class GridShapeMaker
	{
		// Token: 0x0600789B RID: 30875 RVA: 0x002AB218 File Offset: 0x002A9418
		public static List<IntVec3> IrregularLump(IntVec3 center, Map map, int numCells, Predicate<IntVec3> validator = null)
		{
			GridShapeMaker.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.lumpCells = new HashSet<IntVec3>();
			for (int i = 0; i < numCells * 2; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && (validator == null || validator(intVec)))
				{
					CS$<>8__locals1.lumpCells.Add(intVec);
				}
			}
			List<IntVec3> list = new List<IntVec3>();
			while (CS$<>8__locals1.lumpCells.Count > numCells)
			{
				int num = 99;
				foreach (IntVec3 sq in CS$<>8__locals1.lumpCells)
				{
					int num2 = GridShapeMaker.<IrregularLump>g__CountNeighbours|0_0(sq, ref CS$<>8__locals1);
					if (num2 < num)
					{
						num = num2;
					}
				}
				list.Clear();
				foreach (IntVec3 intVec2 in CS$<>8__locals1.lumpCells)
				{
					if (GridShapeMaker.<IrregularLump>g__CountNeighbours|0_0(intVec2, ref CS$<>8__locals1) == num)
					{
						list.Add(intVec2);
					}
				}
				CS$<>8__locals1.lumpCells.Remove(list.RandomElement<IntVec3>());
			}
			return CS$<>8__locals1.lumpCells.ToList<IntVec3>();
		}

		// Token: 0x0600789C RID: 30876 RVA: 0x002AB360 File Offset: 0x002A9560
		public static List<IntVec3> IrregularLumpRelative(int numCells)
		{
			GridShapeMaker.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.lumpCells = new HashSet<IntVec3>();
			for (int i = 0; i < numCells * 2; i++)
			{
				CS$<>8__locals1.lumpCells.Add(GenRadial.RadialPattern[i]);
			}
			List<IntVec3> list = new List<IntVec3>();
			while (CS$<>8__locals1.lumpCells.Count > numCells)
			{
				int num = 99;
				foreach (IntVec3 sq in CS$<>8__locals1.lumpCells)
				{
					int num2 = GridShapeMaker.<IrregularLumpRelative>g__CountNeighbours|1_0(sq, ref CS$<>8__locals1);
					if (num2 < num)
					{
						num = num2;
					}
				}
				list.Clear();
				foreach (IntVec3 intVec in CS$<>8__locals1.lumpCells)
				{
					if (GridShapeMaker.<IrregularLumpRelative>g__CountNeighbours|1_0(intVec, ref CS$<>8__locals1) == num)
					{
						list.Add(intVec);
					}
				}
				CS$<>8__locals1.lumpCells.Remove(list.RandomElement<IntVec3>());
			}
			return CS$<>8__locals1.lumpCells.ToList<IntVec3>();
		}

		// Token: 0x0600789D RID: 30877 RVA: 0x002AB484 File Offset: 0x002A9684
		public static List<IntVec3> UnnaturalShape(IntVec3 center, Map map, int approximateNumCells)
		{
			return (from c in GridShapeMaker.UnnaturalShapeRelative(approximateNumCells)
			select center + c).ToList<IntVec3>();
		}

		// Token: 0x0600789E RID: 30878 RVA: 0x002AB4BC File Offset: 0x002A96BC
		public static List<IntVec3> UnnaturalShapeRelative(int approximateNumCells)
		{
			GridShapeMaker.edgeCells.Clear();
			GridShapeMaker.shapeCells.Clear();
			int num = Mathf.Max(Mathf.FloorToInt(Mathf.Sqrt((float)approximateNumCells)), 1);
			int minInclusive = Mathf.Min(num, 4);
			while (GridShapeMaker.shapeCells.Count < approximateNumCells)
			{
				IntVec3 center = new IntVec3(0, 0, 0);
				if (GridShapeMaker.edgeCells.Count > 0)
				{
					center = GridShapeMaker.edgeCells.RandomElement<IntVec3>();
				}
				CellRect cellRect = CellRect.CenteredOn(center, Rand.Range(minInclusive, num), Rand.Range(minInclusive, num));
				GridShapeMaker.edgeCells.AddRange(cellRect.ExpandedBy(1).EdgeCells);
				GridShapeMaker.shapeCells.AddRange(cellRect.Cells);
				GridShapeMaker.edgeCells.RemoveWhere((IntVec3 c) => GridShapeMaker.shapeCells.Contains(c));
			}
			return GridShapeMaker.shapeCells.ToList<IntVec3>();
		}

		// Token: 0x060078A0 RID: 30880 RVA: 0x002AB5BC File Offset: 0x002A97BC
		[CompilerGenerated]
		internal static int <IrregularLump>g__CountNeighbours|0_0(IntVec3 sq, ref GridShapeMaker.<>c__DisplayClass0_0 A_1)
		{
			int num = 0;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				if (A_1.lumpCells.Contains(sq + b))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060078A1 RID: 30881 RVA: 0x002AB600 File Offset: 0x002A9800
		[CompilerGenerated]
		internal static int <IrregularLumpRelative>g__CountNeighbours|1_0(IntVec3 sq, ref GridShapeMaker.<>c__DisplayClass1_0 A_1)
		{
			int num = 0;
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				if (A_1.lumpCells.Contains(sq + b))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04004B99 RID: 19353
		private static HashSet<IntVec3> edgeCells = new HashSet<IntVec3>();

		// Token: 0x04004B9A RID: 19354
		private static HashSet<IntVec3> shapeCells = new HashSet<IntVec3>();
	}
}
