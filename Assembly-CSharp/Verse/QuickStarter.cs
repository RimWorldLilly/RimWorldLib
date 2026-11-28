using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x020001FF RID: 511
	public static class QuickStarter
	{
		// Token: 0x06000EB6 RID: 3766 RVA: 0x0004BF34 File Offset: 0x0004A134
		public static bool CheckQuickStart()
		{
			if (GenCommandLine.CommandLineArgPassed("quicktest") && !QuickStarter.quickStarted && GenScene.InEntryScene)
			{
				QuickStarter.quickStarted = true;
				SceneManager.LoadScene("Play");
				return true;
			}
			return false;
		}

		// Token: 0x04000E68 RID: 3688
		private static bool quickStarted;
	}
}
