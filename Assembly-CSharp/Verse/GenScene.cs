using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x020001FE RID: 510
	public static class GenScene
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x0004BE98 File Offset: 0x0004A098
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0004BEBC File Offset: 0x0004A0BC
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0004BEE0 File Offset: 0x0004A0E0
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			Game game = Current.Game;
			if (game != null)
			{
				game.Dispose();
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null, false, false);
		}

		// Token: 0x04000E66 RID: 3686
		public const string EntrySceneName = "Entry";

		// Token: 0x04000E67 RID: 3687
		public const string PlaySceneName = "Play";
	}
}
