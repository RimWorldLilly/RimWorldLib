using System;

namespace Verse
{
	// Token: 0x0200052A RID: 1322
	public static class Scribe
	{
		// Token: 0x0600270E RID: 9998 RVA: 0x000F3237 File Offset: 0x000F1437
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000F3254 File Offset: 0x000F1454
		public static bool EnterNode(string nodeName)
		{
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				return false;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return Scribe.saver.EnterNode(nodeName);
			}
			return (Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit) || Scribe.loader.EnterNode(nodeName);
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x000F32A3 File Offset: 0x000F14A3
		public static void ExitNode()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.saver.ExitNode();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.loader.ExitNode();
			}
		}

		// Token: 0x04001C28 RID: 7208
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x04001C29 RID: 7209
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x04001C2A RID: 7210
		public static LoadSaveMode mode = LoadSaveMode.Inactive;
	}
}
