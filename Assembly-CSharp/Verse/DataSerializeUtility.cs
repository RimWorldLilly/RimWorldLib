using System;

namespace Verse
{
	// Token: 0x0200069E RID: 1694
	public static class DataSerializeUtility
	{
		// Token: 0x0600354C RID: 13644 RVA: 0x0015AC84 File Offset: 0x00158E84
		public static byte[] SerializeByte(int elements, Func<int, byte> reader)
		{
			byte[] array = new byte[elements];
			for (int i = 0; i < elements; i++)
			{
				array[i] = reader(i);
			}
			return array;
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x0015ACAF File Offset: 0x00158EAF
		public static byte[] SerializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x0015ACB2 File Offset: 0x00158EB2
		public static byte[] DeserializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x0015ACB8 File Offset: 0x00158EB8
		public static void LoadByte(byte[] arr, int elements, Action<int, byte> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, arr[i]);
			}
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x0015ACE4 File Offset: 0x00158EE4
		public static byte[] SerializeUshort(int elements, Func<int, ushort> reader)
		{
			byte[] array = new byte[elements * 2];
			for (int i = 0; i < elements; i++)
			{
				ushort num = reader(i);
				array[i * 2] = (byte)(num & 255);
				array[i * 2 + 1] = (byte)(num >> 8 & 255);
			}
			return array;
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x0015AD30 File Offset: 0x00158F30
		public static byte[] SerializeUshort(ushort[] data)
		{
			return DataSerializeUtility.SerializeUshort(data.Length, (int i) => data[i]);
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x0015AD64 File Offset: 0x00158F64
		public static ushort[] DeserializeUshort(byte[] data)
		{
			ushort[] result = new ushort[data.Length / 2];
			DataSerializeUtility.LoadUshort(data, result.Length, delegate(int i, ushort dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x0015ADA8 File Offset: 0x00158FA8
		public static void LoadUshort(byte[] arr, int elements, Action<int, ushort> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (ushort)((int)arr[i * 2] | (int)arr[i * 2 + 1] << 8));
			}
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x0015ADE0 File Offset: 0x00158FE0
		public static byte[] SerializeInt(int elements, Func<int, int> reader)
		{
			byte[] array = new byte[elements * 4];
			for (int i = 0; i < elements; i++)
			{
				int num = reader(i);
				array[i * 4] = (byte)(num & 255);
				array[i * 4 + 1] = (byte)(num >> 8 & 255);
				array[i * 4 + 2] = (byte)(num >> 16 & 255);
				array[i * 4 + 3] = (byte)(num >> 24 & 255);
			}
			return array;
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x0015AE50 File Offset: 0x00159050
		public static byte[] SerializeInt(int[] data)
		{
			return DataSerializeUtility.SerializeInt(data.Length, (int i) => data[i]);
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x0015AE84 File Offset: 0x00159084
		public static int[] DeserializeInt(byte[] data)
		{
			int[] result = new int[data.Length / 4];
			DataSerializeUtility.LoadInt(data, result.Length, delegate(int i, int dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x0015AEC8 File Offset: 0x001590C8
		public static void LoadInt(byte[] arr, int elements, Action<int, int> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (int)arr[i * 4] | (int)arr[i * 4 + 1] << 8 | (int)arr[i * 4 + 2] << 16 | (int)arr[i * 4 + 3] << 24);
			}
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x0015AF18 File Offset: 0x00159118
		public static byte[] SerializeUint(int elements, Func<int, uint> reader)
		{
			byte[] array = new byte[elements * 4];
			for (int i = 0; i < elements; i++)
			{
				uint num = reader(i);
				array[i * 4] = (byte)(num & 255U);
				array[i * 4 + 1] = (byte)(num >> 8 & 255U);
				array[i * 4 + 2] = (byte)(num >> 16 & 255U);
				array[i * 4 + 3] = (byte)(num >> 24 & 255U);
			}
			return array;
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x0015AF88 File Offset: 0x00159188
		public static void LoadUint(byte[] arr, int elements, Action<int, uint> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (uint)((int)arr[i * 4] | (int)arr[i * 4 + 1] << 8 | (int)arr[i * 4 + 2] << 16 | (int)arr[i * 4 + 3] << 24));
			}
		}
	}
}
