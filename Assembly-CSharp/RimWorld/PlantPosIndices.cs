using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3B RID: 6971
	public static class PlantPosIndices
	{
		// Token: 0x0600B31B RID: 45851 RVA: 0x003F1968 File Offset: 0x003EFB68
		static PlantPosIndices()
		{
			for (int i = 0; i < 25; i++)
			{
				PlantPosIndices.rootList[i] = new int[8][];
				for (int j = 0; j < 8; j++)
				{
					int[] array = new int[i + 1];
					for (int k = 0; k < i; k++)
					{
						array[k] = k;
					}
					array.Shuffle<int>();
					PlantPosIndices.rootList[i][j] = array;
				}
			}
		}

		// Token: 0x0600B31C RID: 45852 RVA: 0x003F19D0 File Offset: 0x003EFBD0
		public static int[] GetPositionIndices(Plant p)
		{
			int maxMeshCount = p.def.plant.maxMeshCount;
			int num = (p.thingIDNumber ^ 42348528) % 8;
			return PlantPosIndices.rootList[maxMeshCount - 1][num];
		}

		// Token: 0x04006752 RID: 26450
		private static int[][][] rootList = new int[25][][];

		// Token: 0x04006753 RID: 26451
		private const int ListCount = 8;
	}
}
