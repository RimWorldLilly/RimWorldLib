using System;

namespace Verse
{
	// Token: 0x0200008C RID: 140
	public static class UnityDataInitializer
	{
		// Token: 0x0600065D RID: 1629 RVA: 0x0002570C File Offset: 0x0002390C
		public static void CopyUnityData()
		{
			UnityDataInitializer.initializing = true;
			try
			{
				UnityData.CopyUnityData();
			}
			finally
			{
				UnityDataInitializer.initializing = false;
			}
		}

		// Token: 0x04000218 RID: 536
		public static bool initializing;
	}
}
