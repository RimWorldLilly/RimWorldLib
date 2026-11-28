using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F10 RID: 7952
	public static class DevModePermanentlyDisabledUtility
	{
		// Token: 0x17002523 RID: 9507
		// (get) Token: 0x0600CAC8 RID: 51912 RVA: 0x004A660A File Offset: 0x004A480A
		public static bool Disabled
		{
			get
			{
				if (!DevModePermanentlyDisabledUtility.initialized)
				{
					DevModePermanentlyDisabledUtility.initialized = true;
					DevModePermanentlyDisabledUtility.disabled = File.Exists(GenFilePaths.DevModePermanentlyDisabledFilePath);
				}
				return DevModePermanentlyDisabledUtility.disabled;
			}
		}

		// Token: 0x0600CAC9 RID: 51913 RVA: 0x004A6630 File Offset: 0x004A4830
		public static void Disable()
		{
			try
			{
				File.Create(GenFilePaths.DevModePermanentlyDisabledFilePath).Dispose();
			}
			catch (Exception ex)
			{
				string str = "Could not permanently disable dev mode: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				return;
			}
			DevModePermanentlyDisabledUtility.disabled = true;
			Prefs.DevMode = false;
		}

		// Token: 0x04008163 RID: 33123
		private static bool initialized;

		// Token: 0x04008164 RID: 33124
		private static bool disabled;
	}
}
