using System;

namespace Verse
{
	// Token: 0x02000310 RID: 784
	public static class MapExposeUtility
	{
		// Token: 0x06001817 RID: 6167 RVA: 0x00090A6C File Offset: 0x0008EC6C
		public static void ExposeUshort(Map map, Func<IntVec3, ushort> shortReader, Action<IntVec3, ushort> shortWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeUshort(map, shortReader);
			}
			DataExposeUtility.LookByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadUshort(arr, map, shortWriter);
			}
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x00090AA4 File Offset: 0x0008ECA4
		public static void ExposeInt(Map map, Func<IntVec3, int> intReader, Action<IntVec3, int> intWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeInt(map, intReader);
			}
			DataExposeUtility.LookByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadInt(arr, map, intWriter);
			}
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00090ADC File Offset: 0x0008ECDC
		public static void ExposeUint(Map map, Func<IntVec3, uint> uintReader, Action<IntVec3, uint> uintWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeUint(map, uintReader);
			}
			DataExposeUtility.LookByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadUint(arr, map, uintWriter);
			}
		}
	}
}
