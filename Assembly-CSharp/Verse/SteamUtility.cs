using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200072C RID: 1836
	public static class SteamUtility
	{
		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x00171BB3 File Offset: 0x0016FDB3
		public static string SteamPersonaName
		{
			get
			{
				if (SteamManager.Initialized && SteamUtility.cachedPersonaName == null)
				{
					SteamUtility.cachedPersonaName = SteamFriends.GetPersonaName();
				}
				if (SteamUtility.cachedPersonaName == null)
				{
					return "???";
				}
				return SteamUtility.cachedPersonaName;
			}
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x00171BDF File Offset: 0x0016FDDF
		public static void OpenUrl(string url)
		{
			if (SteamManager.Initialized)
			{
				SteamFriends.ActivateGameOverlayToWebPage(url, EActivateGameOverlayToWebPageMode.k_EActivateGameOverlayToWebPageMode_Default);
				return;
			}
			Application.OpenURL(url);
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x00171BF6 File Offset: 0x0016FDF6
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x00171C04 File Offset: 0x0016FE04
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("steam://url/SteamWorkshopPage/" + SteamUtils.GetAppID().ToString());
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x00171C34 File Offset: 0x0016FE34
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			string str = "steam://url/CommunityFilePage/";
			PublishedFileId_t publishedFileId_t = pfid;
			return str + publishedFileId_t.ToString();
		}

		// Token: 0x04002649 RID: 9801
		private static string cachedPersonaName;
	}
}
