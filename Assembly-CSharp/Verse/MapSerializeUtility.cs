using System;

namespace Verse
{
	// Token: 0x02000311 RID: 785
	public static class MapSerializeUtility
	{
		// Token: 0x0600181A RID: 6170 RVA: 0x00090B14 File Offset: 0x0008ED14
		public static byte[] SerializeByte(Map map, Func<IntVec3, byte> shortReader)
		{
			return DataSerializeUtility.SerializeByte(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00090B58 File Offset: 0x0008ED58
		public static void LoadByte(byte[] arr, Map map, Action<IntVec3, byte> shortWriter)
		{
			DataSerializeUtility.LoadByte(arr, map.info.NumCells, delegate(int idx, byte data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x00090B9C File Offset: 0x0008ED9C
		public static byte[] SerializeUshort(Map map, Func<IntVec3, ushort> shortReader)
		{
			return DataSerializeUtility.SerializeUshort(map.info.NumCells, (int idx) => shortReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00090BE0 File Offset: 0x0008EDE0
		public static void LoadUshort(byte[] arr, Map map, Action<IntVec3, ushort> shortWriter)
		{
			DataSerializeUtility.LoadUshort(arr, map.info.NumCells, delegate(int idx, ushort data)
			{
				shortWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x00090C24 File Offset: 0x0008EE24
		public static byte[] SerializeInt(Map map, Func<IntVec3, int> intReader)
		{
			return DataSerializeUtility.SerializeInt(map.info.NumCells, (int idx) => intReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x00090C68 File Offset: 0x0008EE68
		public static void LoadInt(byte[] arr, Map map, Action<IntVec3, int> intWriter)
		{
			DataSerializeUtility.LoadInt(arr, map.info.NumCells, delegate(int idx, int data)
			{
				intWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00090CAC File Offset: 0x0008EEAC
		public static byte[] SerializeUint(Map map, Func<IntVec3, uint> uintReader)
		{
			return DataSerializeUtility.SerializeUint(map.info.NumCells, (int idx) => uintReader(map.cellIndices.IndexToCell(idx)));
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x00090CF0 File Offset: 0x0008EEF0
		public static void LoadUint(byte[] arr, Map map, Action<IntVec3, uint> uintWriter)
		{
			DataSerializeUtility.LoadUint(arr, map.info.NumCells, delegate(int idx, uint data)
			{
				uintWriter(map.cellIndices.IndexToCell(idx), data);
			});
		}
	}
}
