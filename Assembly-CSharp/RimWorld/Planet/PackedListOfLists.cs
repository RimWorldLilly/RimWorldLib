using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002342 RID: 9026
	public static class PackedListOfLists
	{
		// Token: 0x0600DF18 RID: 57112 RVA: 0x0052FE7F File Offset: 0x0052E07F
		public static void AddList<T>(List<int> offsets, List<T> values, List<T> listToAdd)
		{
			offsets.Add(values.Count);
			values.AddRange(listToAdd);
		}

		// Token: 0x0600DF19 RID: 57113 RVA: 0x0052FE94 File Offset: 0x0052E094
		public static void GetList<T>(List<int> offsets, List<T> values, int listIndex, List<T> outList)
		{
			outList.Clear();
			int num = offsets[listIndex];
			int num2 = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num2 = offsets[listIndex + 1];
			}
			for (int i = num; i < num2; i++)
			{
				outList.Add(values[i]);
			}
		}

		// Token: 0x0600DF1A RID: 57114 RVA: 0x0052FEE4 File Offset: 0x0052E0E4
		[return: TupleElementNames(new string[]
		{
			"start",
			"end"
		})]
		public static ValueTuple<int, int> GetListIndexes<[IsUnmanaged] T>(NativeArray<int> offsets, NativeArray<T> values, int listIndex) where T : struct, ValueType
		{
			if (!offsets.IsCreated || !values.IsCreated)
			{
				return new ValueTuple<int, int>(0, 0);
			}
			int item = offsets[listIndex];
			int item2 = values.Length;
			if (listIndex + 1 < offsets.Length)
			{
				item2 = offsets[listIndex + 1];
			}
			return new ValueTuple<int, int>(item, item2);
		}

		// Token: 0x0600DF1B RID: 57115 RVA: 0x0052FF38 File Offset: 0x0052E138
		public static void GetList<[IsUnmanaged] T>(NativeArray<int> offsets, NativeArray<T> values, int listIndex, List<T> outList) where T : struct, ValueType
		{
			outList.Clear();
			if (!offsets.IsCreated || !values.IsCreated)
			{
				return;
			}
			ValueTuple<int, int> listIndexes = PackedListOfLists.GetListIndexes<T>(offsets, values, listIndex);
			int item = listIndexes.Item1;
			int item2 = listIndexes.Item2;
			for (int i = item; i < item2; i++)
			{
				outList.Add(values[i]);
			}
		}

		// Token: 0x0600DF1C RID: 57116 RVA: 0x0052FF90 File Offset: 0x0052E190
		public static void GetListValuesIndices<T>(List<int> offsets, List<T> values, int listIndex, List<int> outList)
		{
			outList.Clear();
			int num = offsets[listIndex];
			int num2 = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num2 = offsets[listIndex + 1];
			}
			for (int i = num; i < num2; i++)
			{
				outList.Add(i);
			}
		}

		// Token: 0x0600DF1D RID: 57117 RVA: 0x0052FFD8 File Offset: 0x0052E1D8
		public static void GetListValuesIndices<[IsUnmanaged] T>(NativeArray<int> offsets, NativeArray<T> values, int listIndex, List<int> outList) where T : struct, ValueType
		{
			outList.Clear();
			if (!offsets.IsCreated || !values.IsCreated)
			{
				return;
			}
			int num = offsets[listIndex];
			int num2 = values.Length;
			if (listIndex + 1 < offsets.Length)
			{
				num2 = offsets[listIndex + 1];
			}
			for (int i = num; i < num2; i++)
			{
				outList.Add(i);
			}
		}

		// Token: 0x0600DF1E RID: 57118 RVA: 0x00530038 File Offset: 0x0052E238
		public static int GetListCount<T>(List<int> offsets, List<T> values, int listIndex)
		{
			int num = offsets[listIndex];
			int num2 = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num2 = offsets[listIndex + 1];
			}
			return num2 - num;
		}

		// Token: 0x0600DF1F RID: 57119 RVA: 0x0053006C File Offset: 0x0052E26C
		public static int GetListCount<[IsUnmanaged] T>(NativeArray<int> offsets, NativeArray<T> values, int listIndex) where T : struct, ValueType
		{
			int num = offsets[listIndex];
			if (!offsets.IsCreated || !values.IsCreated)
			{
				return 0;
			}
			int num2 = values.Length;
			if (listIndex + 1 < offsets.Length)
			{
				num2 = offsets[listIndex + 1];
			}
			return num2 - num;
		}

		// Token: 0x0600DF20 RID: 57120 RVA: 0x005300B8 File Offset: 0x0052E2B8
		public static void GenerateVertToTrisPackedList(List<Vector3> verts, List<TriangleIndices> tris, List<int> outOffsets, List<int> outValues)
		{
			outOffsets.Clear();
			outValues.Clear();
			PackedListOfLists.vertAdjacentTrisCount.Clear();
			int i = 0;
			int count = verts.Count;
			while (i < count)
			{
				PackedListOfLists.vertAdjacentTrisCount.Add(0);
				i++;
			}
			int j = 0;
			int count2 = tris.Count;
			while (j < count2)
			{
				TriangleIndices triangleIndices = tris[j];
				List<int> list = PackedListOfLists.vertAdjacentTrisCount;
				int num = triangleIndices.v1;
				int num2 = list[num];
				list[num] = num2 + 1;
				List<int> list2 = PackedListOfLists.vertAdjacentTrisCount;
				num2 = triangleIndices.v2;
				num = list2[num2];
				list2[num2] = num + 1;
				List<int> list3 = PackedListOfLists.vertAdjacentTrisCount;
				num = triangleIndices.v3;
				num2 = list3[num];
				list3[num] = num2 + 1;
				j++;
			}
			int num3 = 0;
			int k = 0;
			int count3 = verts.Count;
			while (k < count3)
			{
				outOffsets.Add(num3);
				int num4 = PackedListOfLists.vertAdjacentTrisCount[k];
				PackedListOfLists.vertAdjacentTrisCount[k] = 0;
				for (int l = 0; l < num4; l++)
				{
					outValues.Add(-1);
				}
				num3 += num4;
				k++;
			}
			int m = 0;
			int count4 = tris.Count;
			while (m < count4)
			{
				TriangleIndices triangleIndices2 = tris[m];
				outValues[outOffsets[triangleIndices2.v1] + PackedListOfLists.vertAdjacentTrisCount[triangleIndices2.v1]] = m;
				outValues[outOffsets[triangleIndices2.v2] + PackedListOfLists.vertAdjacentTrisCount[triangleIndices2.v2]] = m;
				outValues[outOffsets[triangleIndices2.v3] + PackedListOfLists.vertAdjacentTrisCount[triangleIndices2.v3]] = m;
				List<int> list4 = PackedListOfLists.vertAdjacentTrisCount;
				int num2 = triangleIndices2.v1;
				int num = list4[num2];
				list4[num2] = num + 1;
				List<int> list5 = PackedListOfLists.vertAdjacentTrisCount;
				num = triangleIndices2.v2;
				num2 = list5[num];
				list5[num] = num2 + 1;
				List<int> list6 = PackedListOfLists.vertAdjacentTrisCount;
				num2 = triangleIndices2.v3;
				num = list6[num2];
				list6[num2] = num + 1;
				m++;
			}
		}

		// Token: 0x04008F47 RID: 36679
		private static readonly List<int> vertAdjacentTrisCount = new List<int>();
	}
}
