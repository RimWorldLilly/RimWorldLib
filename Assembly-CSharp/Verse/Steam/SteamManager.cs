using System;
using System.Text;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000746 RID: 1862
	public static class SteamManager
	{
		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06003B0C RID: 15116 RVA: 0x00178D66 File Offset: 0x00176F66
		public static bool Initialized
		{
			get
			{
				return SteamManager.initializedInt;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06003B0D RID: 15117 RVA: 0x00178D6D File Offset: 0x00176F6D
		public static bool Active
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x00178D70 File Offset: 0x00176F70
		public static void InitIfNeeded()
		{
			if (SteamManager.initializedInt)
			{
				return;
			}
			if (!Packsize.Test())
			{
				Log.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
			}
			if (!DllCheck.Test())
			{
				Log.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
			}
			try
			{
				if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
				{
					Application.Quit();
					return;
				}
			}
			catch (DllNotFoundException ex)
			{
				string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
				DllNotFoundException ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				Application.Quit();
				return;
			}
			SteamManager.initializedInt = SteamAPI.Init();
			if (!SteamManager.initializedInt)
			{
				Log.Warning("[Steamworks.NET] SteamAPI.Init() failed. Possible causes: Steam client not running, launched from outside Steam without steam_appid.txt in place, running with different privileges than Steam client (e.g. \"as administrator\")");
				return;
			}
			if (SteamManager.steamAPIWarningMessageHook == null)
			{
				SteamManager.steamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
				SteamClient.SetWarningMessageHook(SteamManager.steamAPIWarningMessageHook);
			}
			Workshop.Init();
			SteamDeck.Init();
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x00178E3C File Offset: 0x0017703C
		public static void Update()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamAPI.RunCallbacks();
			SteamDeck.Update();
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x00178E50 File Offset: 0x00177050
		public static void ShutdownSteam()
		{
			if (!SteamManager.initializedInt)
			{
				return;
			}
			SteamDeck.Shutdown();
			SteamAPI.Shutdown();
			SteamManager.initializedInt = false;
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x00178E6A File Offset: 0x0017706A
		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Log.Error(pchDebugText.ToString());
		}

		// Token: 0x040026C0 RID: 9920
		private static SteamAPIWarningMessageHook_t steamAPIWarningMessageHook;

		// Token: 0x040026C1 RID: 9921
		private static bool initializedInt;
	}
}
